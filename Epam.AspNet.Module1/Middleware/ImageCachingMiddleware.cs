using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc.Controllers;
using SixLabors.Fonts;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;
using SixLabors.Primitives;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mime;
using System.Threading.Tasks;

namespace Epam.AspNet.Module1.Middleware
{
    public class ImageCachingMiddleware
    {
        private DateTime lastCacheHit;
        public ImageCachingMiddleware(RequestDelegate next, ImageCachingOptions imageCachingOptions)
        {
            Next = next;
            ImageCachingOptions = imageCachingOptions;
            if (!Directory.Exists(ImageCachingOptions.CacheDirectoryPath))
                Directory.CreateDirectory(ImageCachingOptions.CacheDirectoryPath);
        }

        public RequestDelegate Next { get; }
        public ImageCachingOptions ImageCachingOptions { get; }

        public async Task Invoke(HttpContext httpContext)
        {
            ClearCacheIfTimeout();

            Stream oldStream = httpContext.Response.Body;
            Stream memStream = null;
            int imageId=-1;
            try
            {
                // (other middleware) => (we're here) => MVC
                var endpointFeature = httpContext.Features[typeof(IEndpointFeature)] as IEndpointFeature;
                Endpoint endpoint = endpointFeature?.Endpoint;

                if (endpoint != null)
                {
                    var descriptor = endpoint.Metadata.GetMetadata<ControllerActionDescriptor>();
                    if(descriptor?.ControllerName == ImageCachingOptions.ControllerName && descriptor?.ActionName == ImageCachingOptions.ActionName)
                    {
                        
                        imageId = Convert.ToInt32(httpContext.Request.RouteValues["id"]);
                        var dirInfo = new DirectoryInfo(ImageCachingOptions.CacheDirectoryPath);
                        FileInfo file = dirInfo.GetFiles(imageId + "_*.bmp").FirstOrDefault();
                        if (file != null)
                        {
                            int underscoreIndex = file.Name.IndexOf('_');
                            var cd = new System.Net.Mime.ContentDisposition
                            {
                                FileName = file.Name.Substring(underscoreIndex + 1),
                                Inline = false
                            };
                            httpContext.Response.Headers.Add("Content-Disposition", cd.ToString());

                            using (var img = Image.Load(file.FullName))
                            {
                                // For production application we would recommend you create a FontCollection
                                // singleton and manually install the ttf fonts yourself as using SystemFonts
                                // can be expensive and you risk font existing or not existing on a deployment
                                // by deployment basis.
                                Font font = SystemFonts.CreateFont("Arial", 10); 

                                using (var img2 = img.Clone(ctx => ApplyScalingWaterMarkSimple(ctx, font, "This is from cache", Color.White, 5)))
                                {
                                    using var m = new MemoryStream();
                                    img2.Save(m, new SixLabors.ImageSharp.Formats.Bmp.BmpEncoder());
                                    m.Seek(0, SeekOrigin.Begin);
                                    await m.CopyToAsync(httpContext.Response.Body);
                                }
                            }
                            lastCacheHit = DateTime.UtcNow;
                            return;
                        }
                        else
                        {
                            memStream = new MemoryStream();
                            httpContext.Response.Body = memStream;
                        }
                    }
                }
                

                await Next(httpContext);
                // (other middleware) <= (we're here) <= MVC
                // here we can save the image in the cache
                if (memStream!=null && httpContext.Response.ContentType == "image/bmp")
                {
                    var dirInfo = new DirectoryInfo(ImageCachingOptions.CacheDirectoryPath);
                    if (dirInfo.GetFileSystemInfos("*.bmp").Length >= ImageCachingOptions.CacheCapacity)
                        return;
                    if (httpContext.Response.Headers.TryGetValue("Content-Disposition", out var values))
                    {
                        var cd = new ContentDisposition(values);
                        var path = Path.Combine(ImageCachingOptions.CacheDirectoryPath, imageId+"_"+cd.FileName);
                        using var fileStream = new FileStream(path, FileMode.Create, FileAccess.Write, FileShare.None);
                        memStream.Position = 0;
                        await memStream.CopyToAsync(fileStream);
                        lastCacheHit = DateTime.UtcNow;
                    }
                }
            }
            finally
            {
                if (memStream != null)
                {
                    memStream.Position = 0;
                    await memStream.CopyToAsync(oldStream);
                    httpContext.Response.Body = oldStream;
                }
            }
        }

        private bool ClearCacheIfTimeout()
        {
            if(DateTime.UtcNow - lastCacheHit > ImageCachingOptions.ExpirationInterval)
            {
                var dirInfo = new DirectoryInfo(ImageCachingOptions.CacheDirectoryPath);
                foreach(var file in dirInfo.GetFiles())
                {
                    file.Delete();
                }
                return true;
            }
            return false;
        }

        private static IImageProcessingContext ApplyScalingWaterMarkSimple(IImageProcessingContext processingContext,
            Font font,
            string text,
            Color color,
            float padding)
        {
            Size imgSize = processingContext.GetCurrentSize();

            float targetWidth = imgSize.Width - (padding * 2);
            float targetHeight = imgSize.Height - (padding * 2);

            // measure the text size
            SizeF size = TextMeasurer.Measure(text, new RendererOptions(font));

            //find out how much we need to scale the text to fill the space (up or down)
            float scalingFactor = Math.Min(imgSize.Width / size.Width, imgSize.Height / size.Height);

            //create a new font
            Font scaledFont = new Font(font, scalingFactor * font.Size, FontStyle.Bold);

            var center = new PointF(imgSize.Width / 2, imgSize.Height / 2);
            var textGraphicOptions = new TextGraphicsOptions(true)
            {
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center,
            };

            return processingContext.DrawText(textGraphicOptions, text, scaledFont, color, center);
        }
    }

    public static class ImageCachingExtensions
    {
        public static void UseImageCaching(this IApplicationBuilder applicationBuilder, ImageCachingOptions options)
        {
            applicationBuilder.UseMiddleware<ImageCachingMiddleware>(options);
        }
    }

    public class ImageCachingOptions
    {
        public string ControllerName { get; set; }
        public string ActionName { get; set; }
        public string CacheDirectoryPath { get; set; }
        public int CacheCapacity { get; set; }
        public TimeSpan ExpirationInterval { get; set; }

    }
}

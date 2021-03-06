﻿// Code generated by Microsoft (R) AutoRest Code Generator 0.16.0.0
// Changes may cause incorrect behavior and will be lost if the code is
// regenerated.

namespace Epam.AspNet.Education.ApiTests
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.Rest;
    using Models;

    /// <summary>
    /// Extension methods for MyAPI.
    /// </summary>
    public static partial class MyAPIExtensions
    {
            /// <summary>
            /// Lists all categories (image bytes not included).
            /// </summary>
            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            public static IList<CategoryDto> GetAllCategories(this IMyAPI operations)
            {
                return Task.Factory.StartNew(s => ((IMyAPI)s).GetAllCategoriesAsync(), operations, CancellationToken.None, TaskCreationOptions.None, TaskScheduler.Default).Unwrap().GetAwaiter().GetResult();
            }

            /// <summary>
            /// Lists all categories (image bytes not included).
            /// </summary>
            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            /// <param name='cancellationToken'>
            /// The cancellation token.
            /// </param>
            public static async Task<IList<CategoryDto>> GetAllCategoriesAsync(this IMyAPI operations, CancellationToken cancellationToken = default(CancellationToken))
            {
                using (var _result = await operations.GetAllCategoriesWithHttpMessagesAsync(null, cancellationToken).ConfigureAwait(false))
                {
                    return _result.Body;
                }
            }

            /// <summary>
            /// Returns the image of the specified category.
            /// </summary>
            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            /// <param name='id'>
            /// id of the category
            /// </param>
            public static IDictionary<string, object> GetCategoryImage(this IMyAPI operations, int id)
            {
                return Task.Factory.StartNew(s => ((IMyAPI)s).GetCategoryImageAsync(id), operations, CancellationToken.None, TaskCreationOptions.None, TaskScheduler.Default).Unwrap().GetAwaiter().GetResult();
            }

            /// <summary>
            /// Returns the image of the specified category.
            /// </summary>
            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            /// <param name='id'>
            /// id of the category
            /// </param>
            /// <param name='cancellationToken'>
            /// The cancellation token.
            /// </param>
            public static async Task<IDictionary<string, object>> GetCategoryImageAsync(this IMyAPI operations, int id, CancellationToken cancellationToken = default(CancellationToken))
            {
                using (var _result = await operations.GetCategoryImageWithHttpMessagesAsync(id, null, cancellationToken).ConfigureAwait(false))
                {
                    return _result.Body;
                }
            }

            /// <summary>
            /// Changes image of the specified category.
            /// </summary>
            /// In Postman, select 'form-data' payload, then select 'file' type, and the
            /// file chooser will appear.
            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            /// <param name='id'>
            /// id of the category
            /// </param>
            /// <param name='file'>
            /// image file
            /// </param>
            public static object UpdateCategoryImage(this IMyAPI operations, int id, string file = default(string))
            {
                return Task.Factory.StartNew(s => ((IMyAPI)s).UpdateCategoryImageAsync(id, file), operations, CancellationToken.None, TaskCreationOptions.None, TaskScheduler.Default).Unwrap().GetAwaiter().GetResult();
            }

            /// <summary>
            /// Changes image of the specified category.
            /// </summary>
            /// In Postman, select 'form-data' payload, then select 'file' type, and the
            /// file chooser will appear.
            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            /// <param name='id'>
            /// id of the category
            /// </param>
            /// <param name='file'>
            /// image file
            /// </param>
            /// <param name='cancellationToken'>
            /// The cancellation token.
            /// </param>
            public static async Task<object> UpdateCategoryImageAsync(this IMyAPI operations, int id, string file = default(string), CancellationToken cancellationToken = default(CancellationToken))
            {
                using (var _result = await operations.UpdateCategoryImageWithHttpMessagesAsync(id, file, null, cancellationToken).ConfigureAwait(false))
                {
                    return _result.Body;
                }
            }

            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            /// <param name='productId'>
            /// </param>
            public static void GetProduct(this IMyAPI operations, int productId)
            {
                Task.Factory.StartNew(s => ((IMyAPI)s).GetProductAsync(productId), operations, CancellationToken.None, TaskCreationOptions.None, TaskScheduler.Default).Unwrap().GetAwaiter().GetResult();
            }

            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            /// <param name='productId'>
            /// </param>
            /// <param name='cancellationToken'>
            /// The cancellation token.
            /// </param>
            public static async Task GetProductAsync(this IMyAPI operations, int productId, CancellationToken cancellationToken = default(CancellationToken))
            {
                await operations.GetProductWithHttpMessagesAsync(productId, null, cancellationToken).ConfigureAwait(false);
            }

            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            public static void GetAllProducts(this IMyAPI operations)
            {
                Task.Factory.StartNew(s => ((IMyAPI)s).GetAllProductsAsync(), operations, CancellationToken.None, TaskCreationOptions.None, TaskScheduler.Default).Unwrap().GetAwaiter().GetResult();
            }

            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            /// <param name='cancellationToken'>
            /// The cancellation token.
            /// </param>
            public static async Task GetAllProductsAsync(this IMyAPI operations, CancellationToken cancellationToken = default(CancellationToken))
            {
                await operations.GetAllProductsWithHttpMessagesAsync(null, cancellationToken).ConfigureAwait(false);
            }

            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            /// <param name='body'>
            /// </param>
            public static void CreateProduct(this IMyAPI operations, Product body = default(Product))
            {
                Task.Factory.StartNew(s => ((IMyAPI)s).CreateProductAsync(body), operations, CancellationToken.None, TaskCreationOptions.None, TaskScheduler.Default).Unwrap().GetAwaiter().GetResult();
            }

            /// <param name='operations'>
            /// The operations group for this extension method.
            /// </param>
            /// <param name='body'>
            /// </param>
            /// <param name='cancellationToken'>
            /// The cancellation token.
            /// </param>
            public static async Task CreateProductAsync(this IMyAPI operations, Product body = default(Product), CancellationToken cancellationToken = default(CancellationToken))
            {
                await operations.CreateProductWithHttpMessagesAsync(body, null, cancellationToken).ConfigureAwait(false);
            }

    }
}

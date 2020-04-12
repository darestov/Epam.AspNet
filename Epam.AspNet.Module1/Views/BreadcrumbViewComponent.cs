using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Epam.AspNet.Module1.Views
{
    public class BreadcrumbViewComponent: ViewComponent
    {
        public class BreadcrumbViewModel
        {
            public bool IsFormView { get; set; }
            public IReadOnlyList<BreadcrumbSection> Sections { get; set; }
            public BreadcrumbViewModel()
            {
                Sections = new List<BreadcrumbSection>().AsReadOnly();
            }
        }

        public class BreadcrumbSection
        {
            public string DisplayName { get; set; }
            public string ControllerName { get; set; }
            public string ActionName { get; set; }
            public bool IsActive { get; set; }
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var endpointFeature = HttpContext.Features[typeof(IEndpointFeature)] as IEndpointFeature;
            Endpoint endpoint = endpointFeature?.Endpoint;
            if (endpoint != null)
            {
                var descriptor = endpoint.Metadata.GetMetadata<ControllerActionDescriptor>();

                bool isFormView = descriptor.ActionName.ToLower() == "edit" || descriptor.ActionName.ToLower() == "new";
                var sections = CreateSections(descriptor, isFormView);

                var model = new BreadcrumbViewModel()
                {
                    IsFormView = isFormView,
                    Sections = sections.AsReadOnly()
                };

                return View(model);
            }
            return View(new BreadcrumbViewModel());
        }

        private List<BreadcrumbSection> CreateSections(ControllerActionDescriptor descriptor, bool isFormView)
        {
            var sections = new List<BreadcrumbSection>
            {
                new BreadcrumbSection { DisplayName = "Home", ControllerName="Home", ActionName="Index" },
            };

            if (descriptor.ControllerName == "Home")
            {
                sections.Last().IsActive = true;
                return sections;
            }

            var s = new BreadcrumbSection { DisplayName = descriptor.ControllerName, ControllerName = descriptor.ControllerName, ActionName = "Index" };
            sections.Add(s);
            if(!isFormView)
            {
                sections.Last().IsActive = true;
                return sections;
            }

            s = new BreadcrumbSection { DisplayName = descriptor.ActionName, ControllerName = descriptor.ControllerName, ActionName = descriptor.ActionName, IsActive = true };
            sections.Add(s);
            return sections;
        }
    }
}


using Microsoft.Rest;
using System;
using Xunit;

namespace Epam.AspNet.Education.ApiTests
{
    public class CategoriesSystemTests
    {   
        Uri baseUri = new Uri("https://localhost:44391");

        [Fact]
        public void AllCategories_ShouldReturnList()
        {
            MyAPI api = new MyAPI(baseUri, new BasicAuthenticationCredentials());
            var categories = api.GetAllCategories();
            Assert.NotNull(categories);
            Assert.NotEmpty(categories);
        }
    }
}

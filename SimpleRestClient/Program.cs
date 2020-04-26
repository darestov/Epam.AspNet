using Epam.AspNet.Module1.Models;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace SimpleRestClient
{
    class Program
    {
        const string base_uri = "https://localhost:44391/";
        static async Task Main(string[] args)
        {
            try
            {
                // get categories list
                HttpClient httpClient = new HttpClient();
                httpClient.BaseAddress = new Uri(base_uri);

                var result = await httpClient.GetAsync("api/categories");
                result.EnsureSuccessStatusCode();
                var categories = await result.Content.ReadAsAsync<IEnumerable<Category>>();
                Console.WriteLine("==== Categories ====");
                foreach (var cat in categories)
                {
                    Console.WriteLine("ID: {0}", cat.CategoryID);
                    Console.WriteLine("Name: {0}", cat.CategoryName);
                    Console.WriteLine();
                }
                Console.WriteLine();
                Console.WriteLine();

                // get products list
                result = await httpClient.GetAsync("api/products");
                result.EnsureSuccessStatusCode();
                var products = await result.Content.ReadAsAsync<IEnumerable<Product>>();
                Console.WriteLine("==== Products ====");
                foreach (var prod in products)
                {
                    Console.WriteLine("ID: {0}", prod.ProductID);
                    Console.WriteLine("Name: {0}", prod.ProductName);
                    Console.WriteLine("Category: {0}", prod.Category?.CategoryName);
                    Console.WriteLine("Supplier: {0}", prod.Supplier?.CompanyName);
                    Console.WriteLine();
                }
            }
            catch(HttpRequestException ex)
            {
                Console.WriteLine("Error: "+ex.Message);
            }
        }
    }
}

using Epam.AspNet.Module1.DataAccess;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Epam.AspNet.Education.Tests
{
    static class DatabaseHelper
    {
        public const string DatabaseId = "Epam.AspNet.Education.InMemoryDb";

        static DatabaseHelper()
        {
            Options = new DbContextOptionsBuilder<NorthwindContext>()
                .UseInMemoryDatabase(DatabaseId)
                .Options;
        }

        public static DbContextOptions<NorthwindContext> Options { get; private set; }


    }
}

﻿// Code generated by Microsoft (R) AutoRest Code Generator 0.16.0.0
// Changes may cause incorrect behavior and will be lost if the code is
// regenerated.

namespace Epam.AspNet.Education.ApiTests.Models
{
    using System;
    using System.Linq;
    using System.Collections.Generic;
    using Newtonsoft.Json;
    using Microsoft.Rest;
    using Microsoft.Rest.Serialization;

    public partial class Category
    {
        /// <summary>
        /// Initializes a new instance of the Category class.
        /// </summary>
        public Category() { }

        /// <summary>
        /// Initializes a new instance of the Category class.
        /// </summary>
        public Category(int? categoryID = default(int?), string categoryName = default(string), string description = default(string), byte[] picture = default(byte[]))
        {
            CategoryID = categoryID;
            CategoryName = categoryName;
            Description = description;
            Picture = picture;
        }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "categoryID")]
        public int? CategoryID { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "categoryName")]
        public string CategoryName { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "description")]
        public string Description { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "picture")]
        public byte[] Picture { get; set; }

    }
}

using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Epam.AspNet.Module1.Models.Validation
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = true)]
    public class NobodyLikesBroccoliAttribute: ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if(validationContext.ObjectType.GetProperty(validationContext.MemberName).PropertyType!=typeof(string))
            {
                throw new InvalidOperationException("This attribute is only valid on string properties.");
            }
            string productName = (string)value;
            if (productName=="Broccoli")
            {
                return new ValidationResult("Author of this app does not like broccoli. It is not allowed.");
            }
            return ValidationResult.Success;
        }
    }
}

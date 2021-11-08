using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AzureTablesDemoApplication.Models
{
    public class FilterResultsInputModel : IValidatableObject
    {

        public string PartitionKey { get; set; }

        public string RowKey { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            List<ValidationResult> errors = new List<ValidationResult>();

            return new ValidationResult[] { ValidationResult.Success };
        }
    }
}

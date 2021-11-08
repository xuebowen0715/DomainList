using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AzureTablesDemoApplication.Models
{
    public class DomainInputModel
    {

        public string HostName { get; set; }

        public string DomainName { get; set; }

        public bool IsTest { get; set; }

        public bool IsProduction { get; set; }

    }
}

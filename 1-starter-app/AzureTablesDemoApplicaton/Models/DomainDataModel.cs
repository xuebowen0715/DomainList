using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;

namespace AzureTablesDemoApplication.Models
{
    public class DomainDataModel 
    {
        // Captures all of the domain data properties -- isTest, isProduction, etc
        private Dictionary<string, object> _properties = new Dictionary<string, object>();

        public string HostName { get; set; }

        public string DomainName { get; set; }

        public DateTimeOffset? Timestamp { get; set; }

        public string Etag { get; set; }

        public object this[string key] 
        { 
            get => (_properties.ContainsKey(key)) ? _properties[key] : null; 
            set => _properties[key] = value; 
        }

        public ICollection<string> PropertyNames => _properties.Keys;

        public int PropertyCount => _properties.Count;


        public bool ContainsKey(string key)
        {
            return _properties.ContainsKey(key);
        }

    }
}

using AzureTablesDemoApplication.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AzureTablesDemoApplication.Data
{
    public static class SampleDomainData
    {
        public static IEnumerable<string> GetSampleCities()
        {
            return SampleData.Select(x => x.HostName).Distinct();
        }

        public static IEnumerable<DomainInputModel> GetSampleData()
        {
            var cityData = SampleData.Select(x => new DomainInputModel()
                      {
                          HostName = x.HostName,
                          DomainName = x.DomainName,
                          IsTest = x.IsTest,
                          IsProduction = x.IsProduction,
                      });

            return cityData;

        }

        public static DomainInputModel[] SampleData = new DomainInputModel[]
        {
            new DomainInputModel() { HostName = "Contoso", DomainName = "contoso.com", IsTest = false, IsProduction = true},
            new DomainInputModel() { HostName = "Contoso", DomainName = "contoso-internal.com", IsTest = true, IsProduction = false},
            new DomainInputModel() { HostName = "Contoso", DomainName = "contoso-external.com", IsTest = false, IsProduction = true},
            new DomainInputModel() { HostName = "Contoso", DomainName = "contoso-test.com", IsTest = true, IsProduction = false},
            new DomainInputModel() { HostName = "TestHost", DomainName = "testhost.com", IsTest = false, IsProduction = true},
            new DomainInputModel() { HostName = "TestHost", DomainName = "internal-testhost.com", IsTest = true, IsProduction = false},
            new DomainInputModel() { HostName = "TestHost", DomainName = "external-testhost.com", IsTest = true, IsProduction = false},
        };
    }






}

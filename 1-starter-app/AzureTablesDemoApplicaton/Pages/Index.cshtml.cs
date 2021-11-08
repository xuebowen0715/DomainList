using AzureTablesDemoApplication.Data;
using AzureTablesDemoApplication.Entities;
using AzureTablesDemoApplication.Models;
using AzureTablesDemoApplication.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AzureTablesDemoApplication.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private TablesService _tablesService;


        public string[] EXCLUDE_FORM_KEYS = { "hostName", "domainName", "etag", "__RequestVerificationToken" };


        public IndexModel(ILogger<IndexModel> logger, TablesService tablesService)
        {
            _logger = logger;
            _tablesService = tablesService;
        }


        public IEnumerable<string> FieldNames { get; set; }
        public IEnumerable<DomainDataModel> DomainObservations { get; set; }


        public void OnGet()
        {
            DomainObservations = _tablesService.GetAllRows();

            FieldNames = DomainObservations.SelectMany(e => e.PropertyNames).Distinct();           
        }


        public IActionResult OnPostInsertTableEntity(DomainInputModel model)
        {
            _tablesService.InsertTableEntity(model);

            return RedirectToPage("index", "Get");
        }

        public IActionResult OnPostUpsertTableEntity(DomainInputModel model)
        {
            _tablesService.UpsertTableEntity(model);

            return RedirectToPage("index", "Get");
        }

        public IActionResult OnPostRemoveEntity(string hostName, string domainName)
        {
            _tablesService.RemoveEntity(hostName, domainName);

            return RedirectToPage("index", "Get");
        }


        public IActionResult OnPostInsertSampleData()
        {
            var bulkData = SampleDomainData.GetSampleData();

            foreach (var item in bulkData)
                _tablesService.UpsertTableEntity(item);

            return RedirectToPage("index", "Get");
        }


        public IActionResult OnPostUpdateEntity(string hostName, string domainName, string etag)
        {
            UpdateDomainObject domainObject = new UpdateDomainObject();
            domainObject.HostName = hostName;
            domainObject.DomainName = domainName;
            domainObject.Etag = etag;

            // The rest of the properties and values are in the form.  But we want to exclude the elements we
            // already have from the model and the __RequestVerificationToken when we build our dictionary
            var propertyNames = Request.Form.Keys.Where(key => !EXCLUDE_FORM_KEYS.Contains(key));
            foreach (string name in propertyNames)
            {
                string value = Request.Form[name].First();

                if (Double.TryParse(value, out double number))
                    domainObject[name] = number;
                else
                    domainObject[name] = value;
            }

            _tablesService.UpdateEntity(domainObject);

            return RedirectToPage("index", "Get");
        }


    }
}

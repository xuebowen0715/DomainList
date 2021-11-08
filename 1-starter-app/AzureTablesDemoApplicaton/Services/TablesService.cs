using Azure;
using Azure.Data.Tables;
using AzureTablesDemoApplication.Entities;
using AzureTablesDemoApplication.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AzureTablesDemoApplication.Services
{
    public class TablesService
    {
        private TableClient _tableClient;

        public TablesService(TableClient tableClient)
        {
            _tableClient = tableClient;
        }

        // in our case, PartitionKey = HostName, RowKey = DomainName, rest properties are stored in TableEntity
        public string[] EXCLUDE_TABLE_ENTITY_KEYS = { "PartitionKey", "RowKey", "odata.etag", "Timestamp" };



        public IEnumerable<DomainDataModel> GetAllRows()
        {
            Pageable<TableEntity> entities = _tableClient.Query<TableEntity>();

            return entities.Select(e => MapTableEntityToWeatherDataModel(e));
        }

        public DomainDataModel MapTableEntityToWeatherDataModel(TableEntity entity)
        {
            DomainDataModel observation = new DomainDataModel();
            observation.HostName = entity.PartitionKey;
            observation.DomainName = entity.RowKey;
            observation.Timestamp = entity.Timestamp;
            observation.Etag = entity.ETag.ToString();

            var measurements = entity.Keys.Where(key => !EXCLUDE_TABLE_ENTITY_KEYS.Contains(key));
            foreach (var key in measurements)
            {
                observation[key] = entity[key];
            }
            return observation;
        }

        public IEnumerable<DomainDataModel> GetFilteredRows(FilterResultsInputModel inputModel)
        {
            List<string> filters = new List<string>();

            if (!String.IsNullOrEmpty(inputModel.PartitionKey))
                filters.Add($"PartitionKey eq '{inputModel.PartitionKey}'");
            if (!String.IsNullOrEmpty(inputModel.RowKey))
                filters.Add($"RowKey eq '{inputModel.RowKey}'");

            string filter = String.Join(" and ", filters);
            Pageable<TableEntity> entities = _tableClient.Query<TableEntity>(filter);

            return entities.Select(e => MapTableEntityToWeatherDataModel(e));
        }



        public void InsertTableEntity(DomainInputModel model)
        {
            TableEntity entity = new TableEntity();
            entity.PartitionKey = model.HostName;
            entity.RowKey = model.DomainName;

            // The other values are added like a items to a dictionary
            entity["IsTest"] = model.IsTest;
            entity["IsProduction"] = model.IsProduction;

            _tableClient.AddEntity(entity);
        }


        public void UpsertTableEntity(DomainInputModel model)
        {
            TableEntity entity = new TableEntity();
            entity.PartitionKey = model.HostName;
            entity.RowKey = model.DomainName;

            // The other values are added like a items to a dictionary
            entity["IsTest"] = model.IsTest;
            entity["IsProduction"] = model.IsProduction;

            _tableClient.UpsertEntity(entity);
        }

        public void RemoveEntity(string partitionKey, string rowKey)
        {
            _tableClient.DeleteEntity(partitionKey, rowKey);
        }


        public void UpdateEntity(UpdateDomainObject weatherObject)
        {
            string partitionKey = weatherObject.HostName;
            string rowKey = weatherObject.DomainName;

            // Use the partition key and row key to get the entity
            TableEntity entity = _tableClient.GetEntity<TableEntity>(partitionKey, rowKey).Value;

            foreach (string propertyName in weatherObject.PropertyNames)
            {
                var value = weatherObject[propertyName];
                entity[propertyName] = value;
            }

            _tableClient.UpdateEntity(entity, new ETag(weatherObject.Etag));
        }

    }
}

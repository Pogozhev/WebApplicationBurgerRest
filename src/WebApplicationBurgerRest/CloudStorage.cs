using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;

namespace WebApplicationBurgerRest
{
    public class CloudStorage
    {
        public CloudStorage()
        {
        }
        static readonly string storageConnectionString = String.Format("DefaultEndpointsProtocol={0};AccountName={1};AccountKey={2}", "https", "encomchats", "ZxRejQtLIP10EEggLWQ6sN8Epo13jQc/6SO8PYrKJVD4hAhMAdhZmgpeXVa9fFNv7FDJLc/BU/0ySuqcjs9cIg==");

        CloudStorageAccount storageAccount = CloudStorageAccount.Parse(storageConnectionString);


        public async Task CreateTable(string name)
        {

            var task = Task.Factory.StartNew(() =>
            {
                // Create the table client.
                CloudTableClient tableClient = storageAccount.CreateCloudTableClient();

                // Retrieve a reference to the table.
                CloudTable table = tableClient.GetTableReference(name);

                // Create the table if it doesn't exist.
                table.CreateIfNotExistsAsync();
            }
                );
            await task.ConfigureAwait(false);
        }

        public async Task InsertMessage(string nameTable, MessageItem item)
        {

            // Create the table client.
            CloudTableClient tableClient = storageAccount.CreateCloudTableClient();
            CloudTable table = tableClient.GetTableReference(nameTable);

            //var dateTime = new DateTime();
            //dateTime = DateTime.Now;
            //string text = dateTime.ToString("MM/dd/yyyy HH:mm:ss.fff",
            //CultureInfo.InvariantCulture);
            // Create a new customer entity.

            // Create the TableOperation object that inserts the customer entity.
            TableOperation insertOperation = TableOperation.Insert(item);

            // Execute the insert operation.
            await table.ExecuteAsync(insertOperation);
        }

        public async Task InsertMessages(string nameTable, List<MessageItem> items)
        {

            // Create the table client.
            CloudTableClient tableClient = storageAccount.CreateCloudTableClient();
            CloudTable table = tableClient.GetTableReference(nameTable);
            // Create the batch operation.
            TableBatchOperation batchOperation = new TableBatchOperation();
            foreach (var element in items)
            {
                batchOperation.Insert(element);
            }
            await table.ExecuteBatchAsync(batchOperation);
        }

        public async Task<List<MessageItem>> GetAllItems(string nameTable, string partitionKey)
        {
           var result = new List<MessageItem>();
            var task = Task.Factory.StartNew(() =>
            {
                // Create the table client.
                CloudTableClient tableClient = storageAccount.CreateCloudTableClient();

                // Create the CloudTable object that represents the "people" table.
                CloudTable table = tableClient.GetTableReference(nameTable);

                // Construct the query operation for all customer entities where PartitionKey="Smith".
                TableQuery<MessageItem> query =
                    new TableQuery<MessageItem>().Where(TableQuery.GenerateFilterCondition("PartitionKey",
                        QueryComparisons.Equal, partitionKey));
               result = ExecuteQueryAsync(table, query).Result.ToList();
                //return table.ExecuteQuery(query).ToList();

            }
                );
            await task.ConfigureAwait(false);
            return result;

        }

        public async Task<MessageItem> GetItem(string nameTable, string idChat, string idMessage)
        {
            var result = new MessageItem();
            var task = Task.Factory.StartNew(() =>
            {
                // Create the table client.
                CloudTableClient tableClient = storageAccount.CreateCloudTableClient();

                // Create the CloudTable object that represents the "people" table.
                CloudTable table = tableClient.GetTableReference(nameTable);

                // Create a retrieve operation that takes a customer entity.
                TableOperation retrieveOperation = TableOperation.Retrieve<MessageItem>(idChat, idMessage);

                // Execute the retrieve operation.
                TableResult retrievedResult = table.ExecuteAsync(retrieveOperation).Result;


                // Print the phone number of the result.
                if (retrievedResult.Result != null)
                    result =(MessageItem)retrievedResult.Result;
            }
                );
            await task.ConfigureAwait(false);
            return result;
        }

        public async Task<List<MessageItem>> GetItem(string nameTable, string partitionKey, int count)
        {
            var result = new List<MessageItem>();
            var task = Task.Factory.StartNew(() =>
            {
                // Create the table client.
                CloudTableClient tableClient = storageAccount.CreateCloudTableClient();

                // Create the CloudTable object that represents the "people" table.
                CloudTable table = tableClient.GetTableReference(nameTable);

                // Construct the query operation for all customer entities where PartitionKey="Smith".
                TableQuery<MessageItem> query =
                    new TableQuery<MessageItem>().Where(TableQuery.GenerateFilterCondition("PartitionKey",
                        QueryComparisons.Equal, partitionKey)).Take(count);

                result= ExecuteQueryAsync(table, query).Result.ToList();

            }
               );
            await task.ConfigureAwait(false);
            return result;

        }

        public async Task<bool> ExistChat(string nameTable, string partitionKey)
        {
            var exist = false;
            var task = Task.Factory.StartNew(() =>
            {
                // Create the table client.
                CloudTableClient tableClient = storageAccount.CreateCloudTableClient();

                // Create the CloudTable object that represents the "people" table.
                CloudTable table = tableClient.GetTableReference(nameTable);

                // Construct the query operation for all customer entities where PartitionKey="Smith".
                TableQuery<MessageItem> query =
                    new TableQuery<MessageItem>().Where(TableQuery.GenerateFilterCondition("PartitionKey",
                        QueryComparisons.Equal, partitionKey)).Take(1);

                var result = ExecuteQueryAsync(table, query).Result.ToList();
                if (result.Count > 0)
                    exist= true;
            }
              );
            await task.ConfigureAwait(false);
            return exist;
        }
        //public async Task<Dictionary<string, string>> GetDataMessages(string nameTable, string idChat)
        //{
        //    // Create the table client.
        //    CloudTableClient tableClient = storageAccount.CreateCloudTableClient();

        //    // Create the CloudTable that represents the "people" table.
        //    CloudTable table = tableClient.GetTableReference("nameTable");

        //    // Define the query, and select only the Email property.
        //    TableQuery<DynamicTableEntity> projectionQuery = new TableQuery<DynamicTableEntity>().Select(new string[] { "MessageData" });

        //    // Define an entity resolver to work with the entity after retrieval.
        //    EntityResolver<string> resolver = (pk, rk, ts, props, etag) => props.ContainsKey("MessageData") ? props["MessageData"].StringValue : null;

        //    foreach (string projectedEmail in table.ExecuteQuery(projectionQuery, resolver, null, null))
        //    {
        //        Console.WriteLine(projectedEmail);
        //    }
        //    return null;
        //}

        private static async Task<IList<T>> ExecuteQueryAsync<T>(CloudTable table, TableQuery<T> query, CancellationToken ct = default(CancellationToken), Action<IList<T>> onProgress = null) where T : ITableEntity, new()
        {

            var items = new List<T>();
            TableContinuationToken token = null;

            do
            {

                TableQuerySegment<T> seg = await table.ExecuteQuerySegmentedAsync<T>(query, token);
                token = seg.ContinuationToken;
                items.AddRange(seg);
                if (onProgress != null) onProgress(items);

            } while (token != null && !ct.IsCancellationRequested);

            return items;
        }
    }
}

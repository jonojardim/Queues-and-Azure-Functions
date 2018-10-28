using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Queue;

namespace TechSession
{
  public static class HTTPFunction
  {
    [FunctionName("HTTPFunction")]
    public static async Task<HttpResponseMessage> Run([HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)]HttpRequestMessage req, TraceWriter log)
    {
      //GET "name" from query string in the URL
      string name = req.GetQueryNameValuePairs()
          .FirstOrDefault(q => string.Compare(q.Key, "name", true) == 0)
          .Value;
      
      //Connect to the storage account
      CloudStorageAccount storageAccount = CloudStorageAccount.Parse("[CLOUD_STORAGE_CONNECTION_STRING]");
      // Create the queue client.
      CloudQueueClient queueClient = storageAccount.CreateCloudQueueClient();
      // Retrieve a reference to a queue.
      CloudQueue queue = queueClient.GetQueueReference("QUEUE_NAME");
      // Create the queue if it doesn't already exist.
      queue.CreateIfNotExists();
      // Create a message and add it to the queue.
      CloudQueueMessage message = new CloudQueueMessage(name);
      queue.AddMessage(message);

      return req.CreateResponse(HttpStatusCode.OK, "You sent " + name);
    }
  }
}

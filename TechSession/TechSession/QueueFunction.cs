using System;
using System.Data.SqlClient;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;

namespace TechSession
{
  public static class QueueFunction
  {
    [FunctionName("QueueFunction")]
    public static void Run([QueueTrigger("QUEUE_NAME", Connection = "")]string myQueueItem, TraceWriter log)
    {
      log.Info($"C# Queue trigger function processed: {myQueueItem}");
      //Variables
      string connetionString = null;
      SqlConnection connection;
      SqlCommand command;
      string sql = null;

      //Add your DB connection string
      connetionString = "DB_CONNECTION_STRING";
      //SQL table named "Queue" with three columns, "Key -> UNIQUEIDENTIFIER", "Name -> NVARCHAR(50)", "Created -> DATETIME"
      sql = "INSERT INTO Queue SELECT NEWID(), '" + myQueueItem + "', GETDATE()";
      //Execute SQL statement
      connection = new SqlConnection(connetionString);
      connection.Open();
      command = new SqlCommand(sql, connection);
      command.ExecuteNonQuery();
      command.Dispose();
      connection.Close();

    }
  }
}

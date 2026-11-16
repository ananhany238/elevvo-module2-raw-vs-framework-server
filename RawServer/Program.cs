using System;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace SimpleHttpServer
{
    class Program
    {
        static async Task Main(string[] args)
        {
            HttpListener listener = new HttpListener();
            listener.Prefixes.Add("http://localhost:3000/");
            listener.Start();
            Console.WriteLine("Server is running on http://localhost:3000");

            while (true)
            {
                var context = await listener.GetContextAsync();

                try
                {
                    string method = context.Request.HttpMethod;
                    string path = context.Request.Url?.AbsolutePath ?? "/";

                    Console.WriteLine($"{method} {path}");

                    if (method == "GET" && path == "/")
                    {
                        var welcomeData = new { Message = "Welcome to the Elevvo API" };
                        await SendJson(context.Response, 200, welcomeData);
                    }
                    else if (method == "GET" && path == "/api/users")
                    {
                        var users = new[]
                        {
                            new { Id = 1, Name = "Ali", Role = "Developer" },
                            new { Id = 2, Name = "Sara", Role = "Designer" }
                        };
                        await SendJson(context.Response, 200, users);
                    }
                    else if (method == "GET" && path == "/api/health")
                    {
                        var healthStatus = new
                        {
                            Status = "OK",
                            Timestamp = DateTime.UtcNow.ToString("O")
                        };
                        await SendJson(context.Response, 200, healthStatus);
                    }
                    else
                    {
                        var errorData = new { Error = "Route not found" };
                        await SendJson(context.Response, 404, errorData);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error handling request: {ex.Message}");
                }
                finally
                {
                    context.Response.Close();
                }
            }
        }

        static async Task SendJson(HttpListenerResponse response, int statusCode, object data)
        {
            var options = new JsonSerializerOptions(JsonSerializerDefaults.Web);
            string json = JsonSerializer.Serialize(data, options);
            byte[] buffer = Encoding.UTF8.GetBytes(json);

            response.StatusCode = statusCode;
            response.ContentType = "application/json";
            response.ContentLength64 = buffer.Length;

            await response.OutputStream.WriteAsync(buffer, 0, buffer.Length);
        }
    }
}
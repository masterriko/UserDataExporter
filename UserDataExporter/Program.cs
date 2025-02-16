using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using CsvHelper;
using System.Globalization;
using System.IO;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;

namespace UserDataExporter
{
    class Program
    {
        static async Task Main(string[] args)
        {
            // get CLIENT_SECRET and CLIENT_ID from json file
            var projectDirectory = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName;
            var configuration = new ConfigurationBuilder()
                .SetBasePath(projectDirectory)
                .AddJsonFile("appsettings.json")
                .Build();

            // Retrieve the client ID and client secret from the config file
            var clientId = configuration["ApiSettings:ClientId"];
            var clientSecret = configuration["ApiSettings:ClientSecret"];

            // Get access token from CLIENT_SECRET and CLIENT_ID
            string accessToken = await GetAccessTokenAsync(clientId, clientSecret);

            if (string.IsNullOrEmpty(accessToken))
            {
                Console.WriteLine("Failed to get access token.");
                return;
            }

            // call the API to get user data 
            var users = await GetUsersAsync(accessToken);

            if (users != null)
            {
                // if everything goes well save users to users.csv
                ExportToCsv(users);
                Console.WriteLine("Data exported to users.csv");
            }
            else
            {
                Console.WriteLine("Failed to retrieve users.");
            }
        }

        /// <summary>
        /// Get access token
        /// </summary>
        /// <param name="clientId"></param>
        /// <param name="clientSecret"></param>
        /// <returns>token string</returns>
        public static async Task<string> GetAccessTokenAsync(string clientId, string clientSecret)
        {
            using (HttpClient client = new HttpClient())
            {
                var values = new Dictionary<string, string>
            {
                { "grant_type", "client_credentials" },
                { "client_id", clientId },
                { "client_secret", clientSecret },
                { "scope", "api" }
            };

                var content = new FormUrlEncodedContent(values);

                // get response from the client 
                var response = await client.PostAsync("https://login.allhours.com/connect/token", content);

                if (response.IsSuccessStatusCode)
                {
                    // convert the response to string
                    var responseString = await response.Content.ReadAsStringAsync();

                    // deserialize it to json
                    dynamic json = JsonConvert.DeserializeObject(responseString);

                    // return the access token
                    return json.access_token;
                }
                else
                {
                    Console.WriteLine("Failed to authenticate. Response: " + response.StatusCode);
                    return null;
                }
            }
        }

        /// <summary>
        /// Calls the API and retrieves users
        /// </summary>
        /// <param name="accessToken"></param>
        /// <returns>a list of users</returns>
        public static async Task<List<User>> GetUsersAsync(string accessToken)
        {
            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("authorization", "Bearer " + accessToken);

                var response = await client.GetAsync("https://api4.allhours.com/api/v1/Users");

                if (response.IsSuccessStatusCode)
                {
                    var responseString = await response.Content.ReadAsStringAsync();
                    dynamic json = JsonConvert.DeserializeObject(responseString);
                    List<User> users = new List<User>();

                    foreach (var user in json)
                    {
                        users.Add(new User
                        {
                            CustomID = user.CustomId,
                            FirstName = user.FirstName,
                            LastName = user.LastName,
                            Mobile = user.Mobile
                        });
                    }

                    return users;
                }
                else
                {
                    Console.WriteLine("Failed to retrieve users. Response: " + response.StatusCode);
                    return null;
                }
            }
        }

        /// <summary>
        /// Export to csv
        /// </summary>
        /// <param name="users"></param>
        public static void ExportToCsv(List<User> users)
        {
            using (var writer = new StreamWriter("users.csv"))
            using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
            {
                csv.WriteHeader<User>();
                csv.NextRecord();
                csv.WriteRecords(users);
            }
        }
    }
}

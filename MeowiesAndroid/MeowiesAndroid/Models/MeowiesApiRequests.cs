using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace MeowiesAndroid.Models;

public class MeowiesApiRequests
{
    public static async Task PostUserToDb(User user)
    {
        var userJson = $"{{\"Name\": \"{user.Name}\"," +
                       $"\"Email\": \"{user.Email}\"," +
                       $"\"Password\": \"{user.Password}\"," +
                       $"\"Birthday\": \"{user.Birthday}\"," +
                       $"\"ProfilePicture\": \"{user.ProfilePicture.ToString()}\"}}";

        using var client = new HttpClient();

            var responseOne = await client.PostAsync("http://192.168.0.10:8080/user",
                new StringContent(userJson, Encoding.UTF8, "application/json"));
            if (responseOne.StatusCode != (HttpStatusCode)201)
            {
                throw new ConstraintException($"User with email {user.Email} already exists");
            }
    }
    
    public static async Task<User?>? Authorize(string email, string password)
    {
        var authJson = $"{{\"Email\": \"{email}\"," +
                       $"\"Password\": \"{password}\"}}";

        using var client = new HttpClient();
        try
        {
            var responseOne = await client.PostAsync("http://192.168.0.10:8080/login",
                new StringContent(authJson, Encoding.UTF8, "application/json"));
            var responseString = await responseOne.Content.ReadAsStringAsync();
            //var task = JsonDeserializers.GetUserAsync(responseString);
            var user = JsonConvert.DeserializeObject<User>(responseString);
            return user;
            //var user = await task!;
            Console.WriteLine(responseOne);
            Console.WriteLine(responseString);
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            throw;
        }
    }
}
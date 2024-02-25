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
            var user = JsonConvert.DeserializeObject<User>(responseString);
            return user;
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            throw;
        }
    }

    public static async Task<List<Bookmark>?>? GetBookmarksForUser(int id)
    {
        using var client = new HttpClient();
        try
        {
            var responseOne = await client.GetAsync($"http://192.168.0.10:8080/bookmark/{id}");
            var responseString = await responseOne.Content.ReadAsStringAsync();
            var bookmarks = JsonConvert.DeserializeObject<List<Bookmark>>(responseString);
            return bookmarks;
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            throw;
        }
    }
    
    public static async Task PostBookmarkToDb(Bookmark bookmark)
    {
        var bookmarkJson = $"{{\"MovieId\": \"{bookmark.MovieId}\"," +
                       $"\"UserId\": \"{bookmark.UserId}\"}}";

        using var client = new HttpClient();

        var responseOne = await client.PostAsync("http://192.168.0.10:8080/bookmark",
            new StringContent(bookmarkJson, Encoding.UTF8, "application/json"));
        if (responseOne.StatusCode != (HttpStatusCode)201)
        {
            throw new ConstraintException($"Some mistake occured.");
        }
    }
    
    public static async Task<String> FindBookmark(Bookmark bookmark)
    {
        var bookmarkJson = $"{{\"MovieId\": \"{bookmark.MovieId}\"," +
                           $"\"UserId\": \"{bookmark.UserId}\"}}";

        using var client = new HttpClient();

        var responseOne = await client.PostAsync("http://192.168.0.10:8080/bookmark/find",
            new StringContent(bookmarkJson, Encoding.UTF8, "application/json"));
        var responseString = await responseOne.Content.ReadAsStringAsync();
        return responseString;
    }
    
    public static async Task RemoveFromBookmarks(int id)
    {
        using var client = new HttpClient();
        await client.DeleteAsync($"http://192.168.0.10:8080/bookmark/{id}");
    }

    public static async Task ChangeProfPic(string userEmail, int picNumber)
    {
        var picJson = $"{{\"Email\": \"{userEmail}\"," +
                           $"\"PicNum\": {picNumber}}}";
        
        using var client = new HttpClient();

        var responseOne = await client.PostAsync("http://192.168.0.10:8080/change/pic",
            new StringContent(picJson, Encoding.UTF8, "application/json"));
        if (responseOne.StatusCode != (HttpStatusCode)202)
        {
            throw new ConstraintException($"Some mistake occurred.");
        }
    }
}
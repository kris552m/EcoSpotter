using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Http.Json;
using EcoSpotterPostPrototype.Model;

namespace EcoSpotterPostPrototype.Services
{
    public class ApiService
    {
        private readonly HttpClient _httpClient;
        private const string BaseUrl = "http://localhost:5138";

        public ApiService()
        {
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = new Uri(BaseUrl);
        }
        public async Task<List<Post>> GetPostsAsync()
        {
            try
            {
                var response = await _httpClient.GetAsync("/post");
                if (!response.IsSuccessStatusCode)
                {
                    Console.WriteLine($"Error fetching posts: {response.ReasonPhrase}");
                    return new List<Post>();
                }
                var posts = await response.Content.ReadFromJsonAsync<List<Post>>();
                return posts;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching posts: {ex.Message}");
                return new List<Post>();
            }
        }
        public async Task<Post> GetPostByIdAsync(int id)
        {
            try
            {
                var response = await _httpClient.GetAsync($"/post/{id}");
                response.EnsureSuccessStatusCode();
                var post = await response.Content.ReadFromJsonAsync<Post>();
                return post ?? new Post();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching post by ID: {ex.Message}");
                return new Post();
            }
        }
        public async Task<bool> CreatePostAsync(Post post)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync("/post", post);
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error creating post: {ex.Message}");
                return false;
            }
        }
        public async Task<bool> UpdatePostAsync(Post post)
        {
            try
            {
                var response = await _httpClient.PutAsJsonAsync($"/post/{post.Id}", post);
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error updating post: {ex.Message}");
                return false;
            }
        }
        public async Task<bool> DeletePostAsync(int id)
        {
            try
            {
                var response = await _httpClient.DeleteAsync($"/post/{id}");
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error deleting post: {ex.Message}");
                return false;
            }
        }
        public async Task<List<Profile>> GetProfilesAsync()
        {
            try
            {
                var response = await _httpClient.GetAsync("/profile");
                response.EnsureSuccessStatusCode();
                var profiles = await response.Content.ReadFromJsonAsync<List<Profile>>();
                return profiles ?? new List<Profile>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching profiles: {ex.Message}");
                return new List<Profile>();
            }
        }
        public async Task<Profile> GetProfileByIdAsync(int id)
        {
            try
            {
                var response = await _httpClient.GetAsync($"/profile/{id}");
                response.EnsureSuccessStatusCode();
                var profile = await response.Content.ReadFromJsonAsync<Profile>();
                return profile ?? new Profile();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching profile by ID: {ex.Message}");
                return new Profile();
            }
        }
        public async Task<bool> CreateProfileAsync(Profile profile)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync("/profile", profile);
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error creating profile: {ex.Message}");
                return false;
            }
        }
        public async Task<bool> UpdateProfileAsync(Profile profile)
        {
            try
            {
                var response = await _httpClient.PutAsJsonAsync($"/profile/{profile.Id}", profile);
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error updating profile: {ex.Message}");
                return false;
            }
        }
        public async Task<bool> DeleteProfileAsync(int id)
        {
            try
            {
                var response = await _httpClient.DeleteAsync($"/profile/{id}");
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error deleting profile: {ex.Message}");
                return false;
            }
        }
    }
}

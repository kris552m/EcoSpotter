using System.Collections.ObjectModel;
using System.Threading.Tasks;
using EcoSpotterPostPrototype.Model;
using EcoSpotterPostPrototype.Services;

namespace EcoSpotterPostPrototype;

public partial class FeedView : ContentPage
{
    public ObservableCollection<Post> Posts { get; set; } = new();

    public FeedView()
    {
        InitializeComponent();
        UpdateProfileImage();
        AppSession.Instance.PropertyChanged += (s, e) =>
        {
            if (e.PropertyName == nameof(AppSession.Instance.CurrentUser))
            {
                UpdateProfileImage();
            }
        };

        // Link the item container's source to our data collection
        FeedCollectionView.ItemsSource = Posts;
        
        LoadPostsFromApi();
    }
    private async void OnProfileHeaderClicked(object sender, EventArgs e)
    {
        // Get the names of available profiles for the menu options
        var profileNames = AppSession.Instance.AvailableProfiles
                                     .Select(p => p.Name)
                                     .ToArray();

        // Display a clean native popup listing the profiles to switch to
        string selectedAction = await DisplayActionSheet("Switch Profile", "Cancel", null, profileNames);

        // If the user didn't hit cancel, find and swap to the chosen profile
        if (selectedAction != "Cancel" && !string.IsNullOrEmpty(selectedAction))
        {
            var newProfile = AppSession.Instance.AvailableProfiles
                                       .FirstOrDefault(p => p.Name == selectedAction);

            if (newProfile != null)
            {
                AppSession.Instance.CurrentUser = newProfile;
                await DisplayAlert("Profile Changed", $"Logged in as {newProfile.Name}", "OK");
            }
        }
    }
    private void UpdateProfileImage()
    {
        // Direct assignment on the main thread ensures smooth UI rendering
        MainThread.BeginInvokeOnMainThread(() =>
        {
            ProfileHeaderButton.Source = AppSession.Instance.CurrentUser.ProfilePictureUrl;
        });
    }
    private async void RefreshButton_Clicked(object sender, EventArgs e)
    {
        await LoadPostsFromApi();
    }
    private async Task LoadPostsFromApi()
    {
    
        LoadingOverlay.IsVisible = true;
        LoadingIndicator.IsRunning = true;
      
        try
        {
            var apiService = new ApiService();
            var postsFromApi = await apiService.GetPostsAsync();
            MainThread.BeginInvokeOnMainThread(() =>
            {
                Posts.Clear();
                foreach (var post in postsFromApi)
                {
                    Posts.Add(post);
                }
            });
        }
        catch (Exception ex)
        {
            MainThread.BeginInvokeOnMainThread(async () =>
            {
                await DisplayAlert("Error", $"Failed to load posts: {ex.Message}", "OK");
            });
        }
        finally
        {
            
            LoadingIndicator.IsRunning = false;
            LoadingOverlay.IsVisible = false;
            
        }
    }
}
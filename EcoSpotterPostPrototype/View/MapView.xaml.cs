#if !WINDOWS
using EcoSpotterPostPrototype.Model;
using EcoSpotterPostPrototype.Services;
using Microsoft.Maui.Controls.Maps;
using Microsoft.Maui.Maps;
using System.Collections.ObjectModel;

namespace EcoSpotterPostPrototype.View;

public partial class MapView : ContentPage
{
	private ObservableCollection<Post> Posts = new();
	public MapView()
	{
		InitializeComponent();
	}
	protected override async void OnAppearing()
	{
		base.OnAppearing();
		await LoadPostsFromApi();
		await CenterMapOnUserAsync();
		CreatePinPerPost();
	}

	private void CreatePinPerPost()
	{
		foreach (Post p in Posts)
		{
			Pin postPin = new Pin
			{
				Label = p.AuthorProfile?.Name ?? "user",
				Address = p.Location,
				Type = PinType.Generic,
				Location = new Location(p.Latitude, p.Longitude)
			};
			MyMap.Pins.Add(postPin);
		}
	}
	private async Task LoadPostsFromApi()
	{
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
                await DisplayAlertAsync("Error", $"Failed to load posts: {ex.Message}", "OK");
            });
        }
    }
	private async Task CenterMapOnUserAsync()
	{
		try
		{
			PermissionStatus status = await Permissions.CheckStatusAsync<Permissions.LocationWhenInUse>();
			if (status != PermissionStatus.Granted) return;

			var request = new GeolocationRequest(GeolocationAccuracy.Medium, TimeSpan.FromSeconds(10));
			Location? location = await Geolocation.Default.GetLocationAsync(request);
			location ??= await Geolocation.Default.GetLastKnownLocationAsync();

			if (location != null)
			{
				Location mapCenter = new Location(location.Latitude, location.Longitude);
				MapSpan mapSpan = MapSpan.FromCenterAndRadius(mapCenter, Distance.FromKilometers(10));
				MyMap.MoveToRegion(mapSpan);

				Pin userPin = new Pin
				{
					Label = "You are here",
					Address = "Current position",
					Type = PinType.Place,
					Location = new Location(location.Latitude, location.Longitude)
				};
			}
		}
		catch (Exception ex)
		{
			System.Diagnostics.Debug.WriteLine($"[Map error] {ex.Message}");
		}

    }
}
//Bør aldrig kører da mapview er utilgængeligt hvis man ikke er på windows. Men computeren ville gerne ha det
#else
using EcoSpotterPostPrototype.Model;
using System.Collections.ObjectModel;
using Microsoft.Maui.Controls;

namespace EcoSPotterPostPrototype.View;
public partial class MapView : ContentPage
{
private ObservableCollection<Post> Posts = new();
	public MapView()
	{
		// No XAML or map types on Windows; show a simple placeholder UI.
		Content = new StackLayout
		{
			VerticalOptions = LayoutOptions.FillAndExpand,
			HorizontalOptions = LayoutOptions.FillAndExpand,
			Children =
			{
				new Label
				{
					Text = "Map is not available on Windows.",
					HorizontalOptions = LayoutOptions.Center,
					VerticalOptions = LayoutOptions.Center
				}
			}
		};
	}
}
#endif
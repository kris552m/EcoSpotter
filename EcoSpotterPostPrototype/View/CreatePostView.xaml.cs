using CommunityToolkit.Maui.Core;
using EcoSpotterPostPrototype.Services;
using EcoSpotterPostPrototype.Model;

namespace EcoSpotterPostPrototype;

public partial class CreatePostView : ContentPage
{
    private string _beforeImagePath = string.Empty;
    private string _afterImagePath = string.Empty;
    private double _longitude = default;
    private double _latitude = default;
    private string _location = string.Empty;
    private readonly ApiService apiService;

    public CreatePostView()
    {
        InitializeComponent();
        apiService = new ApiService();
        SetLocationAsync();

    }
    private async void SetLocationAsync()
    {
        PermissionStatus status = await Permissions.CheckStatusAsync<Permissions.LocationWhenInUse>();
        if (status != PermissionStatus.Granted)
        {
            status = await Permissions.RequestAsync<Permissions.LocationWhenInUse>();
            if (status != PermissionStatus.Granted)
            {
                // Handle the case where the user denied the permission
                await DisplayAlert("Permission Denied", "Location permission is required to capture the location.", "OK");
                return;
            }
        }
        try
        {
            var location = await Geolocation.GetLastKnownLocationAsync();
            if (location != null)
            {
                var placemarks = await Geocoding.Default.GetPlacemarksAsync(location);
                _location = placemarks.FirstOrDefault()?.Locality ?? "Unknown Location";
                this._latitude = location.Latitude;
                this._longitude = location.Longitude;
                
            }
            else
            {
                // Handle the case where the location is not available
                await DisplayAlert("Location Error", "Unable to retrieve location.", "OK");
            }
        }
        catch (Exception ex)
        {
            // Handle exceptions, such as permissions issues
            await DisplayAlert("Error", $"An error occurred while retrieving location: {ex.Message}", "OK");
        }
    }

    private async void OnShutterClicked(object sender, EventArgs e)
    {
        try
        {
            await cameraView.CaptureImage(CancellationToken.None);
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", $"An error occurred while capturing the image: {ex.Message}", "OK");
        }
    }
    private async void OnCaptureBeforeClicked(object sender, EventArgs e)
    {

        // BeforeImagePreview.Source = ImageSource.FromFile(_beforeImagePath);

        // Animate/Switch Layout Visibilities to Form State
        CameraStateLayout.IsVisible = false;
        FormStateLayout.IsVisible = true;
    }

    private async void OnCaptureAfterClicked(object sender, EventArgs e)
    {
        CameraTitle.Text = "Capture After Image";
        CameraStateLayout.IsVisible = true;
        FormStateLayout.IsVisible = false;
    }

    private async void cameraView_MediaCaptured(object sender, MediaCapturedEventArgs e)
    {
        if (e.Media == null) return;

        string fileName;
        var targetFile = string.Empty;
        bool isBeforePicture = (_beforeImagePath == string.Empty);

        if (isBeforePicture)
        {
            fileName = $"BeforePicture_{DateTime.Now:yyyyMMdd_HHmmss}.jpg";
            _beforeImagePath = Path.Combine(FileSystem.CacheDirectory, fileName);
            targetFile = _beforeImagePath;
        }
        else
        {
            fileName = $"AfterPicture_{DateTime.Now:yyyyMMdd_HHmmss}.jpg";
            _afterImagePath = Path.Combine(FileSystem.CacheDirectory, fileName);
            targetFile = _afterImagePath;
        }

        // Save the stream data cleanly to your file path
        using (var fileStream = File.Create(targetFile))
        {
            await e.Media.CopyToAsync(fileStream);
        } // The 'using' statement closes the file, completing the write

        // --- UPDATE UI STATES AFTER THE FILE IS FULLY WRITTEN ---
        
        MainThread.BeginInvokeOnMainThread(() =>
        {
            if (isBeforePicture)
            {
                // 1. Assign the Image Source so the preview shows the saved photo
                BeforeImagePreview.Source = ImageSource.FromFile(_beforeImagePath);

                // 2. Hide the camera view and display the form
                CameraStateLayout.IsVisible = false;
                FormStateLayout.IsVisible = true;
            }
            else
            {
                // 1. Assign the After Image preview
                AfterImagePreviewImage.Source = ImageSource.FromFile(_afterImagePath);

                // 2. Hide the "Take After Picture" button and show the new photo preview
                AfterImagePreviewContent.IsVisible = true;
                AfterImageButton.IsVisible = false;
                PostBtnContainer.IsVisible = true;

                // 3. Bring the user BACK to the form view so they can see everything!
                CameraStateLayout.IsVisible = false;
                FormStateLayout.IsVisible = true;
            }
        });
    }

    private async void PostBtn_Clicked(object sender, EventArgs e)
    {
        Post newPost = new Post(
            AppSession.Instance.CurrentUser.Id,
            DescriptionEditor.Text,
            new PostImage( 
                _beforeImagePath, 
                _afterImagePath
            ),
            _longitude,
            _latitude,
            _location
        );
        await apiService.CreatePostAsync(newPost);
        DescriptionEditor.Text = string.Empty;
        _beforeImagePath = string.Empty;
        _afterImagePath = string.Empty;
        _longitude = default;
        _latitude = default;
        _location = string.Empty;
        await Shell.Current.GoToAsync("//FeedView");
    }
}
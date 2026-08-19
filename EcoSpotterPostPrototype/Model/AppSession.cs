using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using EcoSpotterPostPrototype.Services;

namespace EcoSpotterPostPrototype.Model
{
    public class AppSession : INotifyPropertyChanged
    {
        private readonly ApiService apiService = new ApiService();
        private static AppSession? _instance;
        public static AppSession Instance => _instance ??= new AppSession();

        public List<Profile> AvailableProfiles { get; set; } = new List<Profile> { new Profile { Id = 1, Name = "Default User" } };
        
        private Profile _currentUser;
        public Profile CurrentUser
        {
            get => _currentUser;
            set
            {
                if (_currentUser != value)
                {
                    _currentUser = value;
                    OnPropertyChanged();
                }
            }
        }

        private AppSession()
        {
            _currentUser = AvailableProfiles[0];
            RefreshProfilesAsync();
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public async void RefreshProfilesAsync()
        {
            try
            {
                var profiles = await apiService.GetProfilesAsync().ConfigureAwait(false);
                if (profiles != null && profiles.Count > 0)
                {
                    AvailableProfiles = profiles;
                    OnPropertyChanged(nameof(AvailableProfiles));

                    if (!AvailableProfiles.Contains(_currentUser))
                    {
                        _currentUser = AvailableProfiles[0];
                        OnPropertyChanged(nameof(CurrentUser));
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error refreshing profiles: {ex.Message}");
            }
        }
    }
}

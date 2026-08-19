using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;

namespace EcoSpotterBackend.Model;

public class AppSession : INotifyPropertyChanged
{
    private static AppSession? _instance;
    public static AppSession Instance => _instance ??= new AppSession();

    public List<Profile> AvailableProfiles { get; }
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
        // 1. Initialize our three default profiles
        AvailableProfiles = new List<Profile>
        {
            new Profile { Name = "Kristoffer", ProfilePictureUrl = "https://api.dicebear.com/7.x/avataaars/svg?seed=Kristoffer" },
            new Profile { Name = "John", ProfilePictureUrl = "https://api.dicebear.com/7.x/avataaars/svg?seed=John" },
            new Profile { Name = "Phil", ProfilePictureUrl = "https://api.dicebear.com/7.x/avataaars/svg?seed=Phil" }
        };

        // Default to Kristoffer on app startup
        _currentUser = AvailableProfiles[0];
    }

    public event PropertyChangedEventHandler? PropertyChanged;
    protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}

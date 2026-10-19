
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Lab1.ViewModels;

public class SettingsViewModel : INotifyPropertyChanged
{
    private const string ThemeKey = "AppTheme";

    public List<string> Themes { get; } = new()
    {
        "Системна",
        "Світла",
        "Темна"
    };

    private string _selectedTheme;

    public string SelectedTheme
    {
        get => _selectedTheme;
        set
        {
            if (_selectedTheme == value)
                return;

            _selectedTheme = value;
            OnPropertyChanged();

            ApplyTheme(value);

            Preferences.Set(ThemeKey, value);
        }
    }

    public SettingsViewModel()
    {
        _selectedTheme = Preferences.Get(
            ThemeKey,
            "Системна");

        ApplyTheme(_selectedTheme);
    }

    private static void ApplyTheme(string theme)
    {
        if (Application.Current == null)
            return;

        Application.Current.UserAppTheme = theme switch
        {
            "Світла" => AppTheme.Light,
            "Темна" => AppTheme.Dark,
            _ => AppTheme.Unspecified
        };
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    private void OnPropertyChanged(
        [CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(
            this,
            new PropertyChangedEventArgs(propertyName));
    }
}

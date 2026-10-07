using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using Lab1.Services;

namespace Lab1.ViewModels;

public class CategoriesViewModel : INotifyPropertyChanged
{
    private string _newCategory = string.Empty;

    private readonly CategoryService _categoryService;

    public ObservableCollection<string> Categories =>
        _categoryService.Categories;

    public string NewCategory
    {
        get => _newCategory;
        set
        {
            if (_newCategory != value)
            {
                _newCategory = value;
                OnPropertyChanged();
            }
        }
    }

    public ICommand AddCategoryCommand { get; }
    public ICommand DeleteCategoryCommand { get; }

    public CategoriesViewModel(CategoryService categoryService)
    {
        _categoryService = categoryService;

        AddCategoryCommand = new Command(AddCategory);
        DeleteCategoryCommand = new Command<string>(DeleteCategory);
    }

    private void AddCategory()
    {
        if (string.IsNullOrWhiteSpace(NewCategory))
            return;

        _categoryService.AddCategory(NewCategory);

        NewCategory = string.Empty;
    }

    private void DeleteCategory(string category)
    {
        _categoryService.DeleteCategory(category);
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    protected void OnPropertyChanged(
        [CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(
            this,
            new PropertyChangedEventArgs(propertyName));
    }
}
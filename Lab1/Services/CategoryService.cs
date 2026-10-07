using System.Collections.ObjectModel;

namespace Lab1.Services;

public class CategoryService
{
    public ObservableCollection<string> Categories { get; }

    public CategoryService()
    {
        Categories = new ObservableCollection<string>
        {
            "Їжа",
            "Транспорт",
            "Розваги",
            "Покупки",
            "Комунальні послуги"
        };
    }

    public void AddCategory(string category)
    {
        if (string.IsNullOrWhiteSpace(category))
            return;

        category = category.Trim();

        if (Categories.Contains(category))
            return;

        Categories.Add(category);
    }

    public void DeleteCategory(string category)
    {
        if (string.IsNullOrWhiteSpace(category))
            return;

        Categories.Remove(category);
    }
}
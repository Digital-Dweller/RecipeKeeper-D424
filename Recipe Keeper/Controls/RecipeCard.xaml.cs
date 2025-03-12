using System.Windows.Input;

namespace Recipe_Keeper.Controls;

public partial class RecipeCard : ContentView
{
    public static readonly BindableProperty FavoritedProperty = BindableProperty.Create(nameof(FavoriteCommandBinding), typeof(ICommand), typeof(RecipeCard), null);
    public static readonly BindableProperty ImageSourceProperty = BindableProperty.Create(nameof(ImageSourceBinding), typeof(List<string>), typeof(RecipeCard), null);
    public static readonly BindableProperty TitleBindingProperty = BindableProperty.Create(nameof(TitleBinding), typeof(List<string>), typeof(RecipeCard), null);
    public static readonly BindableProperty FavoriteImageProperty = BindableProperty.Create(nameof(FavoriteImageBinding), typeof(List<string>), typeof(RecipeCard), null);
    public static readonly BindableProperty DescriptionProperty = BindableProperty.Create(nameof(DescriptionBinding), typeof(List<string>), typeof(RecipeCard), null);

    public RecipeCard()
	{
		InitializeComponent();
	}
    public ICommand FavoriteCommandBinding
    {
        get => (ICommand)GetValue(FavoritedProperty);
        set => SetValue(FavoritedProperty, value);
    }
    public string ImageSourceBinding
    {
        get => (string)GetValue(ImageSourceProperty);
        set => SetValue(ImageSourceProperty, value);
    }
    public string TitleBinding
    {
        get => (string)GetValue(TitleBindingProperty);
        set => SetValue(TitleBindingProperty, value);
    }
    public string FavoriteImageBinding
    {
        get => (string)GetValue(FavoriteImageProperty);
        set => SetValue(FavoriteImageProperty, value);
    }
    public string DescriptionBinding
    {
        get => (string)GetValue(DescriptionProperty);
        set => SetValue(DescriptionProperty, value);
    }
}
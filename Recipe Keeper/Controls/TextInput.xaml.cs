namespace Recipe_Keeper.Controls;

public partial class TextInput : ContentView
{
    public static readonly BindableProperty TitleProperty = BindableProperty.Create(nameof(Title), typeof(string), typeof(Label), string.Empty);
    public string Title
    {
        get => (string)GetValue(TitleProperty); 
        set => SetValue(TitleProperty, value);
    }
    public static readonly BindableProperty PlaceholderProperty = BindableProperty.Create(nameof(Placeholder), typeof(string), typeof(Entry), string.Empty);
    public string Placeholder
    {
        get => (string)GetValue(PlaceholderProperty); set => SetValue(PlaceholderProperty, value);
    }
    public Entry InnerEntry => innerEntry;
    public TextInput()
	{
		InitializeComponent();
        BindingContext = this;
    }
}
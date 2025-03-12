using System.Windows.Input;

namespace Recipe_Keeper.Controls;

public partial class AddDirection : ContentView
{
    public static readonly BindableProperty ClickedProperty = BindableProperty.Create(nameof(deleteBinding), typeof(ICommand), typeof(AddDirection), null);
    public static readonly BindableProperty DescriptionTextProperty = BindableProperty.Create(nameof(directionDescription), typeof(string), typeof(AddDirection), null);
    public static readonly BindableProperty TitleTextProperty = BindableProperty.Create(nameof(directionTitle), typeof(string), typeof(AddDirection), null);
    
    public ICommand deleteBinding
    {
        get => (ICommand)GetValue(ClickedProperty);
        set => SetValue(ClickedProperty, value);
    }

    public string directionTitle
    {
        get => (string)GetValue(TitleTextProperty);
        set => SetValue(TitleTextProperty, value);
    }

    public string directionDescription
    {
        get => (string)GetValue(DescriptionTextProperty);
        set => SetValue(DescriptionTextProperty, value);
    }
    public AddDirection()
    {
        InitializeComponent();
        BindingContext = this;
    }
}
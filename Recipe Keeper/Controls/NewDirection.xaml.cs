using System.Windows.Input;

namespace Recipe_Keeper.Controls;

public partial class NewDirection : ContentView
{
    public static readonly BindableProperty ClickedProperty = BindableProperty.Create(nameof(saveBinding), typeof(ICommand), typeof(NewDirection), null);

    public NewDirection()
	{
		InitializeComponent();
        BindingContext = this;
    }

    public ICommand saveBinding
    {
        get => (ICommand)GetValue(ClickedProperty);
        set => SetValue(ClickedProperty, value);
    }
    public string DirectionTitle => newDirectionTitle.Text ?? string.Empty;
    public string DirectionDescription => newDirectionDescription.Text ?? string.Empty;

}
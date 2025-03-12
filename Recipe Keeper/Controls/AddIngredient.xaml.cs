using System.Windows.Input;
using Recipe_Keeper.Classes.Utilities;

namespace Recipe_Keeper.Controls;

public partial class AddIngredient : ContentView
{
    public static readonly BindableProperty ClickedProperty = BindableProperty.Create(nameof(deleteBinding), typeof(ICommand), typeof(AddIngredient), null);
    public static readonly BindableProperty qtyTextProperty = BindableProperty.Create(nameof(qtyText), typeof(string), typeof(AddIngredient), null);
    public static readonly BindableProperty unitTextProperty = BindableProperty.Create(nameof(unitText), typeof(string), typeof(AddIngredient), null);
    public static readonly BindableProperty ingredientTextProperty = BindableProperty.Create(nameof(ingredientText), typeof(string), typeof(AddIngredient), null);

    public ICommand deleteBinding
    {
        get => (ICommand)GetValue(ClickedProperty);
        set => SetValue(ClickedProperty, value);
    }

    public string qtyText
    {
        get => (string)GetValue(qtyTextProperty);
        set => SetValue(qtyTextProperty, value);
    }

    public string unitText
    {
        get => (string)GetValue(unitTextProperty);
        set => SetValue(unitTextProperty, value);
    }

    public string ingredientText
    {
        get => (string)GetValue(ingredientTextProperty);
        set => SetValue(ingredientTextProperty, value);
    }
    public AddIngredient()
	{
		InitializeComponent();
        BindingContext = this;

    }
}
using System.Windows.Input;
using Recipe_Keeper.Classes.Utilities;

namespace Recipe_Keeper.Controls;

public partial class NewIngredient : ContentView
{
    public static readonly BindableProperty ClickedProperty = BindableProperty.Create(nameof(saveBinding), typeof(ICommand), typeof(NewIngredient), null);
    public static readonly BindableProperty unitListProperty = BindableProperty.Create(nameof(unitsListBinding), typeof(List<string>), typeof(NewIngredient), null);


    public List<string> unitsListBinding
    {
        get => (List<string>)GetValue(unitListProperty);
        set => SetValue(unitListProperty, value);
    }
    public ICommand saveBinding
    {
        get => (ICommand)GetValue(ClickedProperty);
        set => SetValue(ClickedProperty, value);
    }
    public NewIngredient()
	{
		InitializeComponent();
        BindingContext = this;
        unitsListBinding = new List<string>();
        unitsListBinding = Units.units;
    }

    public string ingredientQuantity => qty.Text ?? string.Empty;
    
    public string ingredientUnit => unit.SelectedItem?.ToString() ?? string.Empty;
    public string ingredientTitle => ingredient.Text ?? string.Empty;
}
using System.Windows.Input;

namespace Recipe_Keeper.Controls;

public partial class UiNavButton : ContentView
{
    public static readonly BindableProperty TitleProperty = BindableProperty.Create(nameof(Title), typeof(string), typeof(UiNavButton), string.Empty);
    public static readonly BindableProperty SourceProperty = BindableProperty.Create(nameof(Source), typeof(string), typeof(UiNavButton), string.Empty);
    public string Title
    {
        get => (string)GetValue(TitleProperty);
        set => SetValue(TitleProperty, value);
    }
    public string Source
    {
        get => (string)GetValue(SourceProperty);
        set => SetValue(SourceProperty, value);
    }

    public event EventHandler Tapped;

    public UiNavButton()
	{
		InitializeComponent();
        BindingContext = this;

        //Add tap gesture to the control.
        var tapGesture = new TapGestureRecognizer();
        tapGesture.Tapped += (s, e) => Tapped?.Invoke(this, EventArgs.Empty);
        GestureRecognizers.Add(tapGesture);
    }
}
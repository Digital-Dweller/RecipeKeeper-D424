using System.Windows.Input;
using Microsoft.Maui.Controls.Shapes;

namespace Recipe_Keeper.Controls;

public partial class NavIcon : ContentView
{
    public static readonly BindableProperty titleProperty = BindableProperty.Create(nameof(Title), typeof(string), typeof(NavIcon), string.Empty);
    public static readonly BindableProperty bgColorProperty = BindableProperty.Create(nameof(bgColor), typeof(string), typeof(NavIcon), string.Empty);
    public static readonly BindableProperty imgSourceProperty = BindableProperty.Create(nameof(imgSource), typeof(string), typeof(NavIcon), string.Empty);
    public static readonly BindableProperty EdgeCoverWidthProperty = BindableProperty.Create(nameof(edgeCoverWidth), typeof(string), typeof(NavIcon), string.Empty);
    public string Title
    {
        get => (string)GetValue(titleProperty);
        set => SetValue(titleProperty, value);
    }

    public string bgColor
    {
        get => (string)GetValue(bgColorProperty);
        set => SetValue(bgColorProperty, value);
    }
    public string imgSource
    {
        get => (string)GetValue(imgSourceProperty);
        set => SetValue(imgSourceProperty, value);
    }
    public string edgeCoverWidth
    {
        get => (string)GetValue(EdgeCoverWidthProperty);
        set => SetValue(EdgeCoverWidthProperty, value);
    }

    public event EventHandler Tapped;
    public NavIcon()
	{
		InitializeComponent();
        BindingContext = this;

        //Add tap gesture to the control.
        var tapGesture = new TapGestureRecognizer();
        tapGesture.Tapped += (s, e) => Tapped?.Invoke(this, EventArgs.Empty);
        GestureRecognizers.Add(tapGesture);
    }
}
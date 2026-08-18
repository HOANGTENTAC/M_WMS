using System.Windows.Input;

namespace M_WMS.Controls.CheckBoxs;

public partial class WmsCheckBox : ContentView
{
	public WmsCheckBox()
	{
		InitializeComponent();
    }
    #region Bindable Properties

    public static readonly BindableProperty IsCheckedProperty =
        BindableProperty.Create(nameof(IsChecked), typeof(bool), typeof(WmsCheckBox), false, BindingMode.TwoWay, propertyChanged: OnIsCheckedChanged);

    public bool IsChecked
    {
        get => (bool)GetValue(IsCheckedProperty);
        set => SetValue(IsCheckedProperty, value);
    }

    public event EventHandler<CheckedChangedEventArgs>? IsCheckedChanged;

    private static void OnIsCheckedChanged(BindableObject bindable, object oldValue, object newValue)
    {
        var control = (WmsCheckBox)bindable;
        bool isChecked = (bool)newValue;
        control.AnimateState(isChecked);
        control.IsCheckedChanged?.Invoke(control, new CheckedChangedEventArgs(isChecked));

        if (control.CheckedChangedCommand?.CanExecute(isChecked) == true)
        {
            control.CheckedChangedCommand.Execute(isChecked);
        }
    }

    public static readonly BindableProperty CheckedChangedCommandProperty =
        BindableProperty.Create(nameof(CheckedChangedCommand), typeof(ICommand), typeof(WmsCheckBox));

    public ICommand CheckedChangedCommand
    {
        get => (ICommand)GetValue(CheckedChangedCommandProperty);
        set => SetValue(CheckedChangedCommandProperty, value);
    }

    public static readonly BindableProperty TextProperty =
        BindableProperty.Create(nameof(Text), typeof(string), typeof(WmsCheckBox), string.Empty);

    public string Text
    {
        get => (string)GetValue(TextProperty);
        set => SetValue(TextProperty, value);
    }

    public static readonly BindableProperty BoxSizeProperty =
        BindableProperty.Create(nameof(BoxSize), typeof(double), typeof(WmsCheckBox), 20.0);

    public double BoxSize
    {
        get => (double)GetValue(BoxSizeProperty);
        set => SetValue(BoxSizeProperty, value);
    }

    public static readonly BindableProperty CheckedColorProperty =
        BindableProperty.Create(nameof(CheckedColor), typeof(Color), typeof(WmsCheckBox), Colors.Navy);

    public Color CheckedColor
    {
        get => (Color)GetValue(CheckedColorProperty);
        set => SetValue(CheckedColorProperty, value);
    }

    public static readonly BindableProperty UncheckedColorProperty =
        BindableProperty.Create(nameof(UncheckedColor), typeof(Color), typeof(WmsCheckBox), Colors.Gray);

    public Color UncheckedColor
    {
        get => (Color)GetValue(UncheckedColorProperty);
        set => SetValue(UncheckedColorProperty, value);
    }

    public static readonly BindableProperty CheckMarkColorProperty =
        BindableProperty.Create(nameof(CheckMarkColor), typeof(Color), typeof(WmsCheckBox), Colors.White);

    public Color CheckMarkColor
    {
        get => (Color)GetValue(CheckMarkColorProperty);
        set => SetValue(CheckMarkColorProperty, value);
    }

    public static readonly BindableProperty FontSizeProperty =
        BindableProperty.Create(nameof(FontSize), typeof(double), typeof(WmsCheckBox), 14.0);

    public double FontSize
    {
        get => (double)GetValue(FontSizeProperty);
        set => SetValue(FontSizeProperty, value);
    }

    public static readonly BindableProperty TextColorProperty =
        BindableProperty.Create(nameof(TextColor), typeof(Color), typeof(WmsCheckBox), Colors.Black);

    public Color TextColor
    {
        get => (Color)GetValue(TextColorProperty);
        set => SetValue(TextColorProperty, value);
    }

    // 9. Khoảng cách giữa Ô Checkbox và Label
    public static readonly BindableProperty SpacingProperty =
        BindableProperty.Create(nameof(Spacing), typeof(double), typeof(WmsCheckBox), 8.0);

    public double Spacing
    {
        get => (double)GetValue(SpacingProperty);
        set => SetValue(SpacingProperty, value);
    }

    public static readonly BindableProperty CornerRadiusProperty =
    BindableProperty.Create(nameof(CornerRadius), typeof(CornerRadius), typeof(WmsCheckBox), new CornerRadius(4.0));

    public CornerRadius CornerRadius
    {
        get => (CornerRadius)GetValue(CornerRadiusProperty);
        set => SetValue(CornerRadiusProperty, value);
    }
    #endregion

    private void OnTapped(object sender, TappedEventArgs e)
    {
        IsChecked = !IsChecked;
    }
    private async void AnimateState(bool isChecked)
    {
        if (isChecked)
        {
            // Hiệu ứng phồng nhẹ (Scale) ô vuông và hiện dấu tích (FadeTo)
            var scaleTask = BoxBorder.ScaleTo(1.15, 100, Easing.CubicOut);
            var fadeTask = CheckPath.FadeTo(1, 150, Easing.CubicIn);

            await Task.WhenAll(scaleTask, fadeTask);
            await BoxBorder.ScaleTo(1.0, 100, Easing.CubicIn);
        }
        else
        {
            // Hiệu ứng ẩn dấu tích và thu nhỏ về bình thường
            var scaleTask = BoxBorder.ScaleTo(0.9, 80, Easing.CubicOut);
            var fadeTask = CheckPath.FadeTo(0, 100, Easing.CubicOut);

            await Task.WhenAll(scaleTask, fadeTask);
            await BoxBorder.ScaleTo(1.0, 80, Easing.CubicIn);
        }
    }
}
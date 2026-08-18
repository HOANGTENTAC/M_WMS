namespace M_WMS.Controls.RadioButtons;

public partial class WmsRadioButton : ContentView
{
    // Sử dụng List thường chứa WeakReference
    private static readonly List<WeakReference<WmsRadioButton>> RegisteredButtons = new();

    public static readonly BindableProperty GroupNameProperty =
        BindableProperty.Create(nameof(GroupName), typeof(string), typeof(WmsRadioButton), string.Empty, propertyChanged: OnGroupNameChanged);

    public static readonly BindableProperty IsCheckedProperty =
        BindableProperty.Create(nameof(IsChecked), typeof(bool), typeof(WmsRadioButton), false, BindingMode.TwoWay, propertyChanged: OnIsCheckedChanged);

    public static readonly BindableProperty TextProperty =
        BindableProperty.Create(nameof(Text), typeof(string), typeof(WmsRadioButton), string.Empty);

    public static readonly BindableProperty ControlMarginProperty =
        BindableProperty.Create(nameof(ControlMargin), typeof(Thickness), typeof(WmsRadioButton), new Thickness(0));

    public static readonly BindableProperty ControlPaddingProperty =
        BindableProperty.Create(nameof(ControlPadding), typeof(Thickness), typeof(WmsRadioButton), new Thickness(0));

    public static readonly BindableProperty SpacingProperty =
        BindableProperty.Create(nameof(Spacing), typeof(double), typeof(WmsRadioButton), 6.0);

    public static readonly BindableProperty CircleSizeProperty =
        BindableProperty.Create(nameof(CircleSize), typeof(double), typeof(WmsRadioButton), 18.0, propertyChanged: OnPropertyChangedInvalidate);

    public static readonly BindableProperty ActiveColorProperty =
        BindableProperty.Create(nameof(ActiveColor), typeof(Color), typeof(WmsRadioButton), Colors.DeepSkyBlue, propertyChanged: OnPropertyChangedInvalidate);

    public static readonly BindableProperty CircleColorProperty =
        BindableProperty.Create(nameof(CircleColor), typeof(Color), typeof(WmsRadioButton), Colors.Gray, propertyChanged: OnPropertyChangedInvalidate);
    
    public static readonly BindableProperty InnerCircleColorProperty =
        BindableProperty.Create(nameof(InnerCircleColor), typeof(Color), typeof(WmsRadioButton), Colors.DeepSkyBlue, propertyChanged: OnPropertyChangedInvalidate);
    
    public static readonly BindableProperty TextColorProperty =
        BindableProperty.Create(nameof(TextColor), typeof(Color), typeof(WmsRadioButton), Colors.Black);

    public static readonly BindableProperty FontSizeProperty =
        BindableProperty.Create(nameof(FontSize), typeof(double), typeof(WmsRadioButton), 14.0);

    public string GroupName { get => (string)GetValue(GroupNameProperty); set => SetValue(GroupNameProperty, value); }
    public bool IsChecked { get => (bool)GetValue(IsCheckedProperty); set => SetValue(IsCheckedProperty, value); }
    public string Text { get => (string)GetValue(TextProperty); set => SetValue(TextProperty, value); }
    public Thickness ControlMargin { get => (Thickness)GetValue(ControlMarginProperty); set => SetValue(ControlMarginProperty, value); }
    public Thickness ControlPadding { get => (Thickness)GetValue(ControlPaddingProperty); set => SetValue(ControlPaddingProperty, value); }
    public double Spacing { get => (double)GetValue(SpacingProperty); set => SetValue(SpacingProperty, value); }
    public double CircleSize { get => (double)GetValue(CircleSizeProperty); set => SetValue(CircleSizeProperty, value); }
    public Color ActiveColor { get => (Color)GetValue(ActiveColorProperty); set => SetValue(ActiveColorProperty, value); }
    public Color CircleColor { get => (Color)GetValue(CircleColorProperty); set => SetValue(CircleColorProperty, value); }
    public Color InnerCircleColor { get => (Color)GetValue(InnerCircleColorProperty); set => SetValue(InnerCircleColorProperty, value); }
    public Color TextColor { get => (Color)GetValue(TextColorProperty); set => SetValue(TextColorProperty, value); }
    public double FontSize { get => (double)GetValue(FontSizeProperty); set => SetValue(FontSizeProperty, value); }

    public WmsRadioButton()
    {
        InitializeComponent();
        RegisterButton(this);
        UpdateState();
    }

    private static void RegisterButton(WmsRadioButton button)
    {
        lock (RegisteredButtons)
        {
            // Dọn dẹp các nút đã bị huỷ bộ nhớ
            RegisteredButtons.RemoveAll(reference => !reference.TryGetTarget(out _));
            RegisteredButtons.Add(new WeakReference<WmsRadioButton>(button));
        }
    }

    private static void OnIsCheckedChanged(BindableObject bindable, object oldValue, object newValue)
    {
        if (bindable is WmsRadioButton button)
        {
            bool isChecked = (bool)newValue;
            if (isChecked)
            {
                UncheckOtherButtonsInGroup(button);
            }
            button.UpdateState();
        }
    }

    private static void OnGroupNameChanged(BindableObject bindable, object oldValue, object newValue)
    {
        if (bindable is WmsRadioButton button && button.IsChecked)
        {
            UncheckOtherButtonsInGroup(button);
        }
    }

    private static void OnPropertyChangedInvalidate(BindableObject bindable, object oldValue, object newValue)
    {
        if (bindable is WmsRadioButton button)
        {
            button.UpdateState();
        }
    }

    // SỬA LỖI 1: Bỏ chọn chính xác tuyệt đối các nút khác trong cùng Group
    private static void UncheckOtherButtonsInGroup(WmsRadioButton currentButton)
    {
        if (string.IsNullOrWhiteSpace(currentButton.GroupName)) return;

        lock (RegisteredButtons)
        {
            foreach (var reference in RegisteredButtons.ToList())
            {
                if (reference.TryGetTarget(out var button))
                {
                    // So sánh GroupName không phân biệt hoa thường và không so sánh với chính mình
                    if (button != currentButton &&
                        string.Equals(button.GroupName, currentButton.GroupName, StringComparison.Ordinal))
                    {
                        if (button.IsChecked)
                        {
                            button.IsChecked = false;
                        }
                    }
                }
            }
        }
    }

    private void UpdateState()
    {
        if (OuterBorder == null || InnerCircle == null) return;

        double innerSize = Math.Max(1, CircleSize * 0.55);
        InnerCircle.WidthRequest = innerSize;
        InnerCircle.HeightRequest = innerSize;

        if (IsChecked)
        {
            OuterBorder.Stroke = ActiveColor;
            InnerCircle.IsVisible = true;
        }
        else
        {
            OuterBorder.Stroke = CircleColor;
            InnerCircle.IsVisible = false;
        }
    }

    // SỬA LỖI 2: Animation an toàn, không dùng duration = 0ms và kiểm tra Hủy animation cũ trước khi chạy
    private async void OnTapped(object sender, EventArgs e)
    {
        if (!IsChecked)
        {
            IsChecked = true;
        }

        try
        {
            // Hủy các animation đang chạy dở trên control để tránh xung đột
            CircleContainer.CancelAnimations();
            ClickEffectCircle.CancelAnimations();

            // Reset trạng thái ban đầu một cách an toàn
            ClickEffectCircle.Scale = 0;
            ClickEffectCircle.Opacity = 0.3;

            // Chạy Animation phóng to (Thời lượng tối thiểu 16ms ~ 1 frame)
            await Task.WhenAll(
                CircleContainer.ScaleTo(1.2, 100, Easing.CubicOut),
                ClickEffectCircle.ScaleTo(1.8, 120, Easing.Linear),
                ClickEffectCircle.FadeTo(0, 120, Easing.Linear)
            );

            // Thu nhỏ về lại kích thước chuẩn
            await CircleContainer.ScaleTo(1.0, 100, Easing.CubicIn);

            // Reset giá trị sau khi thu nhỏ xong
            ClickEffectCircle.Scale = 0;
            ClickEffectCircle.Opacity = 0.3;
        }
        catch
        {
            // Bảo vệ ứng dụng không bị crash nếu có sự cố giao diện ngắt đột ngột
            CircleContainer.Scale = 1.0;
            ClickEffectCircle.Scale = 0;
        }
    }
}
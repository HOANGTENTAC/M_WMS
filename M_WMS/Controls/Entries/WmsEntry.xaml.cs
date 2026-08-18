using System.Windows.Input;

namespace M_WMS.Controls.Entries;

public partial class WmsEntry : ContentView
{
    private bool _isHovered = false;
    public event EventHandler<TextChangedEventArgs> TextChanged;
    public event EventHandler RightIconClicked;
    public WmsEntry()
	{
		InitializeComponent();
        IsClearButtonVisible = false;
    }
    // 1. Text Property
    public static readonly BindableProperty TextProperty =
        BindableProperty.Create(nameof(Text), typeof(string), typeof(WmsEntry), string.Empty, BindingMode.TwoWay);

    public string Text
    {
        get => (string)GetValue(TextProperty);
        set => SetValue(TextProperty, value);
    }

    // 2. Placeholder Property
    public static readonly BindableProperty PlaceholderProperty =
        BindableProperty.Create(nameof(Placeholder), typeof(string), typeof(WmsEntry), string.Empty);

    public string Placeholder
    {
        get => (string)GetValue(PlaceholderProperty);
        set => SetValue(PlaceholderProperty, value);
    }

    // 3. InnerPadding Property (Điều chỉnh khoảng cách giữa Text và Viền)
    public static readonly BindableProperty InnerPaddingProperty =
        BindableProperty.Create(nameof(InnerPadding), typeof(Thickness), typeof(WmsEntry), new Thickness(8, 4));

    public Thickness InnerPadding
    {
        get => (Thickness)GetValue(InnerPaddingProperty);
        set => SetValue(InnerPaddingProperty, value);
    }

    // 4. Border Color
    public static readonly BindableProperty BorderColorProperty =
        BindableProperty.Create(nameof(BorderColor), typeof(Color), typeof(WmsEntry), Color.FromArgb("#E1E1E1"));

    public Color BorderColor
    {
        get => (Color)GetValue(BorderColorProperty);
        set => SetValue(BorderColorProperty, value);
    }

    // 5. Border Thickness
    public static readonly BindableProperty BorderThicknessProperty =
        BindableProperty.Create(
            nameof(BorderThickness), 
            typeof(double), 
            typeof(WmsEntry),
            1.0);

    public double BorderThickness
    {
        get => (double)GetValue(BorderThicknessProperty);
        set => SetValue(BorderThicknessProperty, value);
    }

    // 6. Corner Radius
    public static readonly BindableProperty CornerRadiusProperty =
        BindableProperty.Create(nameof(CornerRadius), typeof(CornerRadius), typeof(WmsEntry), new CornerRadius(6));

    public CornerRadius CornerRadius
    {
        get => (CornerRadius)GetValue(CornerRadiusProperty);
        set => SetValue(CornerRadiusProperty, value);
    }

    // 7. Background Color
    public static readonly BindableProperty ContainerBackgroundColorProperty =
        BindableProperty.Create(nameof(ContainerBackgroundColor), typeof(Color), typeof(WmsEntry), Colors.Transparent);

    public Color ContainerBackgroundColor
    {
        get => (Color)GetValue(ContainerBackgroundColorProperty);
        set => SetValue(ContainerBackgroundColorProperty, value);
    }

    // 8. Text Color
    public static readonly BindableProperty TextColorProperty =
        BindableProperty.Create(nameof(TextColor), typeof(Color), typeof(WmsEntry), Colors.Black);

    public Color TextColor
    {
        get => (Color)GetValue(TextColorProperty);
        set => SetValue(TextColorProperty, value);
    }

    // 9. Placeholder Color
    public static readonly BindableProperty PlaceholderColorProperty =
        BindableProperty.Create(nameof(PlaceholderColor), typeof(Color), typeof(WmsEntry), Colors.Gray);

    public Color PlaceholderColor
    {
        get => (Color)GetValue(PlaceholderColorProperty);
        set => SetValue(PlaceholderColorProperty, value);
    }

    // 10. IsPassword
    public static readonly BindableProperty IsPasswordProperty =
        BindableProperty.Create(nameof(IsPassword), typeof(bool), typeof(WmsEntry), false);

    public bool IsPassword
    {
        get => (bool)GetValue(IsPasswordProperty);
        set => SetValue(IsPasswordProperty, value);
    }
    // 11. VerticalTextAlignment Property (Mặc định căn giữa theo chiều dọc)
    public static readonly BindableProperty VerticalTextAlignmentProperty =
        BindableProperty.Create(
            nameof(VerticalTextAlignment),
            typeof(TextAlignment),
            typeof(WmsEntry),
            TextAlignment.Center);

    public TextAlignment VerticalTextAlignment
    {
        get => (TextAlignment)GetValue(VerticalTextAlignmentProperty);
        set => SetValue(VerticalTextAlignmentProperty, value);
    }

    // 12. HorizontalTextAlignment Property (Mặc định căn giữa theo chiều ngang)
    public static readonly BindableProperty HorizontalTextAlignmentProperty =
        BindableProperty.Create(
            nameof(HorizontalTextAlignment),
            typeof(TextAlignment),
            typeof(WmsEntry),
            TextAlignment.Start);

    public TextAlignment HorizontalTextAlignment
    {
        get => (TextAlignment)GetValue(HorizontalTextAlignmentProperty);
        set => SetValue(HorizontalTextAlignmentProperty, value);
    }

    // 13. Left Icon Source & Visibility
    public static readonly BindableProperty LeftIconSourceProperty =
        BindableProperty.Create(nameof(LeftIconSource), typeof(ImageSource), typeof(WmsEntry), null);

    public ImageSource LeftIconSource
    {
        get => (ImageSource)GetValue(LeftIconSourceProperty);
        set => SetValue(LeftIconSourceProperty, value);
    }

    public static readonly BindableProperty IsLeftIconVisibleProperty =
        BindableProperty.Create(nameof(IsLeftIconVisible), typeof(bool), typeof(WmsEntry), false);

    public bool IsLeftIconVisible
    {
        get => (bool)GetValue(IsLeftIconVisibleProperty);
        set => SetValue(IsLeftIconVisibleProperty, value);
    }

    // 14. Right Icon Source & Visibility
    public static readonly BindableProperty RightIconSourceProperty =
        BindableProperty.Create(nameof(RightIconSource), typeof(ImageSource), typeof(WmsEntry), null);

    public ImageSource RightIconSource
    {
        get => (ImageSource)GetValue(RightIconSourceProperty);
        set => SetValue(RightIconSourceProperty, value);
    }

    public static readonly BindableProperty IsRightIconVisibleProperty =
        BindableProperty.Create(nameof(IsRightIconVisible), typeof(bool), typeof(WmsEntry), false);

    public bool IsRightIconVisible
    {
        get => (bool)GetValue(IsRightIconVisibleProperty);
        set => SetValue(IsRightIconVisibleProperty, value);
    }

    // BindableProperty quản lý việc ẩn/hiện nút Clear Custom
    public static readonly BindableProperty IsClearButtonVisibleProperty =
        BindableProperty.Create(
            nameof(IsClearButtonVisible), 
            typeof(bool), 
            typeof(WmsEntry),
            defaultValue: false);

    public bool IsClearButtonVisible
    {
        get => (bool)GetValue(IsClearButtonVisibleProperty);
        set => SetValue(IsClearButtonVisibleProperty, value);
    }

    // 2. BindableProperty Keyboard (Kiểu bàn phím: Numeric, Email, Telephone...)
    public static readonly BindableProperty KeyboardProperty =
        BindableProperty.Create(
            nameof(Keyboard),
            typeof(Keyboard),
            typeof(WmsEntry),
            Keyboard.Default);

    public Keyboard Keyboard
    {
        get => (Keyboard)GetValue(KeyboardProperty);
        set => SetValue(KeyboardProperty, value);
    }

    // 3. BindableProperty TextChangedCommand (Tùy chọn: hỗ trợ MVVM Command khi text thay đổi)
    public static readonly BindableProperty TextChangedCommandProperty =
        BindableProperty.Create(
            nameof(TextChangedCommand),
            typeof(ICommand),
            typeof(WmsEntry),
            null);

    public ICommand TextChangedCommand
    {
        get => (ICommand)GetValue(TextChangedCommandProperty);
        set => SetValue(TextChangedCommandProperty, value);
    }

    public static readonly BindableProperty IsUnderlineProperty =
        BindableProperty.Create(
            nameof(IsUnderline),
            typeof(bool),
            typeof(WmsEntry),
            false);

    public bool IsUnderline
    {
        get => (bool)GetValue(IsUnderlineProperty);
        set => SetValue(IsUnderlineProperty, value);
    }

    // 1. FontAwesome Glyph Trái / Phải
    public static readonly BindableProperty LeftIconGlyphProperty =
        BindableProperty.Create(nameof(LeftIconGlyph), typeof(string), typeof(WmsEntry), null,
            propertyChanged: (b, o, n) => ((WmsEntry)b).UpdateIconVisibilities());

    public string LeftIconGlyph
    {
        get => (string)GetValue(LeftIconGlyphProperty);
        set => SetValue(LeftIconGlyphProperty, value);
    }

    public static readonly BindableProperty RightIconGlyphProperty =
        BindableProperty.Create(nameof(RightIconGlyph), typeof(string), typeof(WmsEntry), null,
            propertyChanged: (b, o, n) => ((WmsEntry)b).UpdateIconVisibilities());

    public string RightIconGlyph
    {
        get => (string)GetValue(RightIconGlyphProperty);
        set => SetValue(RightIconGlyphProperty, value);
    }

    // 2. Màu sắc & Size của FontAwesome Icon
    public static readonly BindableProperty IconColorProperty =
        BindableProperty.Create(nameof(IconColor), typeof(Color), typeof(WmsEntry), Colors.Gray);

    public Color IconColor
    {
        get => (Color)GetValue(IconColorProperty);
        set => SetValue(IconColorProperty, value);
    }

    public static readonly BindableProperty IconSizeProperty =
        BindableProperty.Create(nameof(IconSize), typeof(double), typeof(WmsEntry), 16.0);

    public double IconSize
    {
        get => (double)GetValue(IconSizeProperty);
        set => SetValue(IconSizeProperty, value);
    }

    // 3. Biến quản lý trạng thái hiển thị nội bộ
    public bool IsLeftIconImageVisible => LeftIconSource != null && string.IsNullOrEmpty(LeftIconGlyph);
    public bool IsLeftIconGlyphVisible => !string.IsNullOrEmpty(LeftIconGlyph);

    public bool IsRightIconImageVisible => RightIconSource != null && string.IsNullOrEmpty(RightIconGlyph);
    public bool IsRightIconGlyphVisible => !string.IsNullOrEmpty(RightIconGlyph);

    private void UpdateIconVisibilities()
    {
        OnPropertyChanged(nameof(IsLeftIconImageVisible));
        OnPropertyChanged(nameof(IsLeftIconGlyphVisible));
        OnPropertyChanged(nameof(IsRightIconImageVisible));
        OnPropertyChanged(nameof(IsRightIconGlyphVisible));
    }
    // 4. Handler chuyển tiếp sự kiện TextChanged từ Entry nội bộ ra ngoài
    private void OnInternalEntryTextChanged(object sender, TextChangedEventArgs e)
    {
        // Bắn event ra ngoài cho trang gọi CustomEntry
        TextChanged?.Invoke(this, e);

        // Thực thi Command nếu có gán trong ViewModel
        if (TextChangedCommand != null && TextChangedCommand.CanExecute(e.NewTextValue))
        {
            TextChangedCommand.Execute(e.NewTextValue);
        }
    }
    private void OnPointerEntered(object sender, PointerEventArgs e)
    {
        _isHovered = true;
        UpdateBorderVisualState();
    }
    private void OnPointerExited(object sender, PointerEventArgs e)
    {
        _isHovered = false;
        UpdateBorderVisualState();
    }
    private void UpdateClearButtonVisibility()
    {
        // An toàn: Đảm bảo internalEntry đã được khởi tạo xong
        if (internalEntry == null)
        {
            IsClearButtonVisible = false;
            return;
        }

        // BẮT BỘC: Entry đang được Focus VÀ Text không rỗng thì MỚI HIỆN
        IsClearButtonVisible = IsEnabled
                                   && internalEntry.IsFocused
                                   && !string.IsNullOrEmpty(Text);
    }
    // Tự động kiểm tra độ dài Text để Bật/Tắt nút Clear
    protected override void OnPropertyChanged(string propertyName = null)
    {
        base.OnPropertyChanged(propertyName);

        // 1. Khi Text thay đổi
        if (propertyName == nameof(Text))
        {
            UpdateClearButtonVisibility();
        }
        // 2. Bổ sung: Khi IsEnabled của CustomEntry bị thay đổi (VD: khi IsLoading thay đổi)
        else if (propertyName == IsEnabledProperty.PropertyName)
        {
            // Nếu bị Disable (IsEnabled = false) thì ép nhả Focus của Entry nội bộ
            if (!IsEnabled && internalEntry != null)
            {
                internalEntry.Unfocus(); // Ép Entry nhả Focus
            }

            // Cập nhật lại việc Ẩn/Hiện nút Clear
            UpdateClearButtonVisibility();
            UpdateBorderVisualState();
        }
    }

    // Sự kiện khi bấm vào nút Clear nhỏ
    private void OnCustomClearTapped(object sender, TappedEventArgs e)
    {
        Text = string.Empty;
        internalEntry.Focus(); // Giữ con trỏ nhập liệu ở lại Entry
    }

    // Handlers xử lý Focus & Event Click Icon Phải
    private void OnEntryFocused(object sender, FocusEventArgs e)
    {
        //VisualStateManager.GoToState(borderContainer, "Focused");
        UpdateBorderVisualState();
        UpdateClearButtonVisibility();
    }
    private void OnEntryUnfocused(object sender, FocusEventArgs e)
    {
        //VisualStateManager.GoToState(borderContainer, "Normal");
        UpdateBorderVisualState();
        UpdateClearButtonVisibility(); // Sẽ ẩn nút Clear vì IsFocused lúc này = false
    }
    private void UpdateBorderVisualState()
    {
        if (internalEntry == null) return;

        // 1. Định nghĩa các màu dành cho trạng thái Disabled (Khi IsEnabled = false)
        Color disabledBorderColor = Color.FromArgb("#E0E0E0"); // Màu viền xám nhạt
        Color disabledTextColor = Color.FromArgb("#A0A0A0");   // Màu chữ xám vừa
        Color disabledBgColor = Color.FromArgb("#F5F5F5");     // Màu nền xám nhạt (nếu dùng dạng Bo góc)

        // Kiểm tra nếu Entry đang bị Disable
        if (!IsEnabled)
        {
            // Đổi màu chữ Entry sang xám
            internalEntry.TextColor = disabledTextColor;

            if (IsUnderline)
            {
                // Dạng Underline: Đổi thanh gạch chân sang màu xám
                if (underlineView != null)
                {
                    underlineView.Color = disabledBorderColor;
                    underlineView.HeightRequest = BorderThickness;
                }
            }
            else
            {
                // Dạng Bo góc: Đổi viền và nền sang màu xám
                if (borderContainer != null)
                {
                    borderContainer.Stroke = disabledBorderColor;
                    borderContainer.StrokeThickness = BorderThickness;
                    borderContainer.BackgroundColor = disabledBgColor;
                }
            }
            return; // Dừng hàm, không xử lý Focus/Hover nữa
        }

        internalEntry.TextColor = TextColor;

        bool isFocused = internalEntry.IsFocused;
        // Xác định màu sắc & độ dày theo trạng thái
        Color activeColor = BorderColor; // Mặc định
        double activeThickness = BorderThickness;

        if (isFocused)
        {
            activeColor = Color.FromArgb("#2196F3"); // Màu viền khi Focus (Xanh dương)
            //activeThickness = BorderThickness + 1;  // Tăng độ dày nhẹ khi Focus
        }
        else if (_isHovered)
        {
            activeColor = Color.FromArgb("#2196F3"); // Màu viền khi Hover (Di chuột)
        }

        // Áp dụng hiệu ứng đúng cho từng dạng
        if (IsUnderline)
        {
            // Nếu là dạng Underline -> Đổi màu & Độ dày cho BoxView
            if (underlineView != null)
            {
                underlineView.Color = activeColor;
                underlineView.HeightRequest = activeThickness;
            }
        }
        else
        {
            // Nếu là dạng Bo góc 4 cạnh -> Đổi màu & Độ dày cho Border
            if (borderContainer != null)
            {
                borderContainer.Stroke = activeColor;
                borderContainer.StrokeThickness = activeThickness;
                borderContainer.BackgroundColor = ContainerBackgroundColor;
            }
        }
    }
    private void OnRightIconTapped(object sender, TappedEventArgs e) => RightIconClicked?.Invoke(this, EventArgs.Empty);
}
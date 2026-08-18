using System.Windows.Input;

namespace M_WMS.Controls.Buttons;

public partial class IconButton : ContentView
{
    public enum IconPosition { Left, Right, Top, Bottom }
    public event EventHandler Clicked;

    public static readonly BindableProperty TextProperty =
        BindableProperty.Create(nameof(Text), typeof(string), typeof(IconButton), string.Empty);

    public static readonly BindableProperty TextColorProperty =
        BindableProperty.Create(nameof(TextColor), typeof(Color), typeof(IconButton), Colors.White);

    public static readonly BindableProperty IconSourceProperty =
        BindableProperty.Create(nameof(IconSource), typeof(ImageSource), typeof(IconButton), null,
            propertyChanged: (bindable, oldValue, newValue) => ((IconButton)bindable).UpdateLayout());

    public static readonly BindableProperty PositionProperty =
        BindableProperty.Create(nameof(Position), typeof(IconPosition), typeof(IconButton), IconPosition.Left,
            propertyChanged: (bindable, oldValue, newValue) => ((IconButton)bindable).UpdateLayout());

    public static readonly BindableProperty CommandProperty =
        BindableProperty.Create(nameof(Command), typeof(ICommand), typeof(IconButton), null);

    public static readonly BindableProperty CommandParameterProperty =
        BindableProperty.Create(nameof(CommandParameter), typeof(object), typeof(IconButton), null);

    public static readonly BindableProperty CornerRadiusProperty =
        BindableProperty.Create(
            nameof(CornerRadius),
            typeof(CornerRadius),
            typeof(IconButton),
            new CornerRadius(8)); // Mặc định bo góc 8px
    public static readonly BindableProperty IconFontAwesomeProperty =
        BindableProperty.Create(
            nameof(IconFontAwesome),
            typeof(string),
            typeof(IconButton),
            string.Empty,
            propertyChanged: OnFontAwesomePropertyChanged);

    // 2. Property cho Màu Icon
    public static readonly BindableProperty FontAwesomeIconColorProperty =
        BindableProperty.Create(
            nameof(FontAwesomeIconColor),
            typeof(Color),
            typeof(IconButton),
            Colors.White,
            propertyChanged: OnFontAwesomePropertyChanged);

    // 3. Property cho Kích thước Icon
    public static readonly BindableProperty FontAwesomeSizeProperty =
        BindableProperty.Create(
            nameof(FontAwesomeSize),
            typeof(double),
            typeof(IconButton),
            20.0,
            propertyChanged: OnFontAwesomePropertyChanged);
    public CornerRadius CornerRadius
    {
        get => (CornerRadius)GetValue(CornerRadiusProperty);
        set => SetValue(CornerRadiusProperty, value);
    }
    public static readonly BindableProperty IconHeightProperty =
        BindableProperty.Create(
            nameof(IconHeight),
            typeof(double),
            typeof(IconButton),
            20.0);

    // Khai báo BindableProperty cho IconWidth (Mặc định: 20)
    public static readonly BindableProperty IconWidthProperty =
        BindableProperty.Create(
            nameof(IconWidth),
            typeof(double),
            typeof(IconButton),
            20.0);

    public static readonly BindableProperty ButtonBackgroundColorProperty =
        BindableProperty.Create(
            nameof(ButtonBackgroundColor),
            typeof(Color),
            typeof(IconButton),
            Color.FromArgb("#007AFF"));

    public Color ButtonBackgroundColor
    {
        get => (Color)GetValue(ButtonBackgroundColorProperty);
        set => SetValue(ButtonBackgroundColorProperty, value);
    }
    public string IconFontAwesome
    {
        get => (string)GetValue(IconFontAwesomeProperty);
        set => SetValue(IconFontAwesomeProperty, value);
    }

    public Color FontAwesomeIconColor
    {
        get => (Color)GetValue(FontAwesomeIconColorProperty);
        set => SetValue(FontAwesomeIconColorProperty, value);
    }

    public double FontAwesomeSize
    {
        get => (double)GetValue(FontAwesomeSizeProperty);
        set => SetValue(FontAwesomeSizeProperty, value);
    }
    public string Text { get => (string)GetValue(TextProperty); set => SetValue(TextProperty, value); }
    public Color TextColor { get => (Color)GetValue(TextColorProperty); set => SetValue(TextColorProperty, value); }
    public ImageSource IconSource { get => (ImageSource)GetValue(IconSourceProperty); set => SetValue(IconSourceProperty, value); }
    public IconPosition Position { get => (IconPosition)GetValue(PositionProperty); set => SetValue(PositionProperty, value); }
    public ICommand Command { get => (ICommand)GetValue(CommandProperty); set => SetValue(CommandProperty, value); }
    public object CommandParameter { get => GetValue(CommandParameterProperty); set => SetValue(CommandParameterProperty, value); }
    public double IconHeight
    {
        get => (double)GetValue(IconHeightProperty);
        set => SetValue(IconHeightProperty, value);
    }

    public double IconWidth
    {
        get => (double)GetValue(IconWidthProperty);
        set => SetValue(IconWidthProperty, value);
    }
    public static readonly BindableProperty FontSizeProperty =
        BindableProperty.Create(
            nameof(FontSize),
            typeof(double),
            typeof(IconButton),
            14.0);

    public double FontSize
    {
        get => (double)GetValue(FontSizeProperty);
        set => SetValue(FontSizeProperty, value);
    }
    public IconButton()
	{
		InitializeComponent();

        base.Background = Colors.Transparent;

        this.PropertyChanged += OnIconButtonPropertyChanged;

        UpdateLayout();
    }
    private void OnIconButtonPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
    {
        // Khi thuộc tính IsEnabled thay đổi -> Cập nhật độ mờ (Opacity)
        if (e.PropertyName == nameof(IsEnabled))
        {
            UpdateEnabledState();
        }
    }
    private void UpdateEnabledState()
    {
        // 1. Cập nhật độ mờ (Opacity)
        this.Opacity = IsEnabled ? 1.0 : 0.5;

        // 2. Vô hiệu hóa Input / Tap Gesture trên Border bên trong nếu có
        if (MainBorder != null)
        {
            MainBorder.IsEnabled = IsEnabled;
        }
    }
    private static void OnPositionChanged(BindableObject bindable, object oldValue, object newValue)
    {
        ((IconButton)bindable).UpdateLayout();
    }
    private static void OnCommandChanged(BindableObject bindable, object oldValue, object newValue)
    {
        var control = (IconButton)bindable;

        if (oldValue is ICommand oldCmd)
        {
            oldCmd.CanExecuteChanged -= control.OnCanExecuteChanged;
        }

        if (newValue is ICommand newCmd)
        {
            newCmd.CanExecuteChanged += control.OnCanExecuteChanged;
            // Kiểm tra ban đầu khi Command được gán
            control.UpdateCanExecute();
        }
    }
    private void OnCanExecuteChanged(object sender, EventArgs e)
    {
        UpdateCanExecute();
    }
    private void UpdateCanExecute()
    {
        if (Command != null)
        {
            bool canExecute = Command.CanExecute(CommandParameter);

            // CHỈ VÔ HIỆU HÓA khi CanExecute = false.
            // Tuyệt đối KHÔNG gán IsEnabled = true khi CanExecute = true, 
            // vì nó sẽ làm mất (overwrite) Binding IsEnabled="IsLoading" từ XAML!
            if (!canExecute)
            {
                this.IsEnabled = false;
            }
        }
    }
    private void UpdateLayout()
    {
        if (ContainerGrid == null) return;

        ContainerGrid.RowDefinitions.Clear();
        ContainerGrid.ColumnDefinitions.Clear();

        // Kiểm tra xem nút có chứa Icon hay không
        bool hasIcon = IconSource != null;

        // Nếu không có Icon -> Tắt hoàn toàn khoảng cách (Spacing = 0)
        // Nếu có Icon -> Bật khoảng cách 8px
        ContainerGrid.ColumnSpacing = hasIcon ? 8 : 0;
        ContainerGrid.RowSpacing = hasIcon ? 8 : 0;

        // Nếu KHÔNG có Icon -> Cho Text làm chủ toàn bộ Grid (chỉ 1 cột, 1 hàng)
        if (!hasIcon)
        {
            ContainerGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto });
            ContainerGrid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });

            Grid.SetColumn(TextLabel, 0);
            Grid.SetRow(TextLabel, 0);
            return;
        }

        // Nếu CÓ Icon -> Chia cột/hàng theo vị trí Position bình thường
        switch (Position)
        {
            case IconPosition.Left:
                ContainerGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto });
                ContainerGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto });
                ContainerGrid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });

                Grid.SetColumn(IconImage, 0); Grid.SetRow(IconImage, 0);
                Grid.SetColumn(TextLabel, 1); Grid.SetRow(TextLabel, 0);
                break;

            case IconPosition.Right:
                ContainerGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto });
                ContainerGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto });
                ContainerGrid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });

                Grid.SetColumn(TextLabel, 0); Grid.SetRow(TextLabel, 0);
                Grid.SetColumn(IconImage, 1); Grid.SetRow(IconImage, 0);
                break;

            case IconPosition.Top:
                ContainerGrid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
                ContainerGrid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
                ContainerGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto });

                Grid.SetRow(IconImage, 0); Grid.SetColumn(IconImage, 0);
                Grid.SetRow(TextLabel, 1); Grid.SetColumn(TextLabel, 0);
                break;

            case IconPosition.Bottom:
                ContainerGrid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
                ContainerGrid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
                ContainerGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto });

                Grid.SetRow(TextLabel, 0); Grid.SetColumn(TextLabel, 0);
                Grid.SetRow(IconImage, 1); Grid.SetColumn(IconImage, 0);
                break;
        }
    }
    private static void OnFontAwesomePropertyChanged(BindableObject bindable, object oldValue, object newValue)
    {
        var control = (IconButton)bindable;

        if (!string.IsNullOrEmpty(control.IconFontAwesome))
        {
            control.IconSource = new FontImageSource
            {
                FontFamily = "FA-Solid", // Nhớ đổi đúng tên Alias FontAwesome bạn đã đăng ký ở MauiProgram.cs
                Glyph = control.IconFontAwesome,
                Color = control.FontAwesomeIconColor,
                Size = control.FontAwesomeSize
            };

            // Cập nhật lại kích thước khung chứa Image tương ứng
            control.IconWidth = control.FontAwesomeSize;
            control.IconHeight = control.FontAwesomeSize;
        }
    }
    private async void OnTapped(object sender, EventArgs e)
    {
        if (!IsEnabled) return;

        // Tạo hiệu ứng thu nhỏ và mờ đi nhanh
        await Task.WhenAll(
            this.ScaleTo(0.95, 80, Easing.CubicOut),
            this.FadeTo(0.7, 80, Easing.CubicOut)
        );

        // Bật ngược trở lại trạng thái ban đầu
        await Task.WhenAll(
            this.ScaleTo(1.0, 80, Easing.CubicIn),
            this.FadeTo(1.0, 80, Easing.CubicIn)
        );

        // Bắn sự kiện Clicked sau khi hoàn thành animation
        Clicked?.Invoke(this, e);
        if (Command != null && Command.CanExecute(CommandParameter))
        {
            Command.Execute(CommandParameter);
        }
    }
}
using System.Windows.Input;

namespace M_WMS.Controls.Card;

public partial class CardView : ContentView
{
	public CardView()
	{
		InitializeComponent();
	}
    public static readonly BindableProperty CardTitleProperty =
        BindableProperty.Create(nameof(CardTitle), typeof(string), typeof(CardView), string.Empty);

    public string CardTitle
    {
        get => (string)GetValue(CardTitleProperty);
        set => SetValue(CardTitleProperty, value);
    }

    public static readonly BindableProperty CardDescriptionProperty =
        BindableProperty.Create(nameof(CardDescription), typeof(string), typeof(CardView), string.Empty);

    public string CardDescription
    {
        get => (string)GetValue(CardDescriptionProperty);
        set => SetValue(CardDescriptionProperty, value);
    }

    public static readonly BindableProperty CardPriceProperty =
        BindableProperty.Create(nameof(CardPrice), typeof(string), typeof(CardView), string.Empty);

    public string CardPrice
    {
        get => (string)GetValue(CardPriceProperty);
        set => SetValue(CardPriceProperty, value);
    }

    public static readonly BindableProperty CardImageProperty =
        BindableProperty.Create(nameof(CardImage), typeof(ImageSource), typeof(CardView), null);

    public ImageSource CardImage
    {
        get => (ImageSource)GetValue(CardImageProperty);
        set => SetValue(CardImageProperty, value);
    }

    public static readonly BindableProperty ImageHeightProperty =
        BindableProperty.Create(
            nameof(ImageHeight),
            typeof(double),
            typeof(CardView),
            150.0); // Giá trị mặc định là 150

    public double ImageHeight
    {
        get => (double)GetValue(ImageHeightProperty);
        set => SetValue(ImageHeightProperty, value);
    }

    // 2. Thêm BindableProperty cho Chiều rộng ảnh
    public static readonly BindableProperty ImageWidthProperty =
        BindableProperty.Create(
            nameof(ImageWidth),
            typeof(double),
            typeof(CardView),
            -1.0); // -1.0 trong MAUI tương đương với Auto (tự co giãn theo khung)

    public double ImageWidth
    {
        get => (double)GetValue(ImageWidthProperty);
        set => SetValue(ImageWidthProperty, value);
    }

    public static readonly BindableProperty TextAlignmentProperty =
        BindableProperty.Create(
        nameof(TextAlignment),
        typeof(TextAlignment),
        typeof(CardView),
        TextAlignment.Start); // Mặc định căn trái

    public TextAlignment TextAlignment
    {
        get => (TextAlignment)GetValue(TextAlignmentProperty);
        set => SetValue(TextAlignmentProperty, value);
    }

    public static readonly BindableProperty TextAlignmentDescriptionProperty =
    BindableProperty.Create(
    nameof(TextAlignmentDescription),
    typeof(TextAlignment),
    typeof(CardView),
    TextAlignment.Start); // Mặc định căn trái

    public TextAlignment TextAlignmentDescription
    {
        get => (TextAlignment)GetValue(TextAlignmentDescriptionProperty);
        set => SetValue(TextAlignmentDescriptionProperty, value);
    }

    public static readonly BindableProperty TextAlignmentPriceProperty =
        BindableProperty.Create(
        nameof(TextAlignmentPrice),
        typeof(TextAlignment),
        typeof(CardView),
        TextAlignment.Start);

    public TextAlignment TextAlignmentPrice
    {
        get => (TextAlignment)GetValue(TextAlignmentPriceProperty);
        set => SetValue(TextAlignmentPriceProperty, value);
    }

    // 1. Property nhận lệnh thực thi khi click (Command)
    public static readonly BindableProperty CommandProperty =
        BindableProperty.Create(nameof(Command), typeof(ICommand), typeof(CardView), null);

    public static readonly BindableProperty FontSizeProperty =
    BindableProperty.Create(nameof(FontSize), typeof(double), typeof(CardView), 14.0);

    public double FontSize
    {
        get => (double)GetValue(FontSizeProperty);
        set => SetValue(FontSizeProperty, value);
    }

    public ICommand Command
    {
        get => (ICommand)GetValue(CommandProperty);
        set => SetValue(CommandProperty, value);
    }

    // 2. Property chứa dữ liệu gửi kèm khi click (ví dụ: đối tượng Item hoặc ID)
    public static readonly BindableProperty CommandParameterProperty =
        BindableProperty.Create(nameof(CommandParameter), typeof(object), typeof(CardView), null);

    public object CommandParameter
    {
        get => GetValue(CommandParameterProperty);
        set => SetValue(CommandParameterProperty, value);
    }
}
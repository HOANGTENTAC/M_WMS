using M_WMS.Controls.DatePickers;
using M_WMS.Controls.Enums;
using Microsoft.Maui.Controls.Shapes;
namespace M_WMS.Controls.Popups;

public partial class WmsCalendarPopup : Border
{
    private WmsCalendarViewMode _viewMode = WmsCalendarViewMode.Day;
    private DateTime? _selectedDate;
    private DateTime _currentMonth;
    private readonly WmsDatePicker _owner;
    public WmsCalendarPopup(WmsDatePicker owner)
	{
		InitializeComponent();
        _owner = owner;

        _selectedDate = _owner.Date ?? DateTime.Now;

        _currentMonth = owner.Date ?? DateTime.Today;

        BuildCalendar();
        TimePanel.IsVisible = owner.HasTime;
        OkButton.IsVisible = _owner.HasTime;
        CancelButton.IsVisible = _owner.HasTime;
        SecondPicker.IsVisible = owner.HasSecond;
        SecondColon.IsVisible = owner.HasSecond;
        btnClear.IsVisible = _owner.AllowClear;
        InitializeTimePicker();
    }
    private void InitializeTimePicker()
    {
        var dt = _owner.Date ?? DateTime.Now;

        HourPicker.Items.Clear();
        MinutePicker.Items.Clear();
        SecondPicker.Items.Clear();

        for (int i = 0; i < 24; i++)
        {
            HourPicker.Items.Add(i.ToString("00"));
        }

        for (int i = 0; i < 60; i++)
        {
            MinutePicker.Items.Add(i.ToString("00"));
            SecondPicker.Items.Add(i.ToString("00"));
        }

        HourPicker.SelectedIndex = dt.Hour;
        MinutePicker.SelectedIndex = dt.Minute;
        SecondPicker.SelectedIndex = dt.Second;
    }
    private bool NeedTime(string format)
    {
        if (string.IsNullOrWhiteSpace(format))
            return false;

        return format.Contains("HH");
    }
    private void BuildCalendar()
    {
        CalendarGrid.Children.Clear();

        UpdateHeader();

        var firstDay = new DateTime(
            _currentMonth.Year,
            _currentMonth.Month,
            1);

        int startColumn = (int)firstDay.DayOfWeek;

        var firstDisplayDate = firstDay.AddDays(-startColumn);

        for (int index = 0; index < 42; index++)
        {
            var date = firstDisplayDate.AddDays(index);

            var dayView = CreateDayView(date);

            Grid.SetRow(dayView, index / 7);
            Grid.SetColumn(dayView, index % 7);

            CalendarGrid.Children.Add(dayView);
        }
    }
    private Border CreateDayView(DateTime date)
    {
        var label = new Label
        {
            Text = date.Day.ToString(),
            HorizontalOptions = LayoutOptions.Center,
            VerticalOptions = LayoutOptions.Center,
            HorizontalTextAlignment = TextAlignment.Center,
            VerticalTextAlignment = TextAlignment.Center,
            FontSize = 16,
            TextColor = Colors.Black
        };
        switch (date.DayOfWeek)
        {
            case DayOfWeek.Sunday:
                label.TextColor = Colors.Red;
                break;

            case DayOfWeek.Saturday:
                label.TextColor = Colors.DodgerBlue;
                break;

            default:
                label.TextColor = Colors.Black;
                break;
        }
        if (date.Month != _currentMonth.Month)
        {
            label.TextColor = Colors.LightGray;
            label.Opacity = 0.8;
        }
        var border = new Border
        {
            WidthRequest = 38,
            HeightRequest = 38,
            Margin = new Thickness(2),
            StrokeShape = new RoundRectangle
            {
                CornerRadius = 19
            },
            StrokeThickness = 1,
            Stroke = Colors.Transparent,
            BackgroundColor = Colors.Transparent,
            HorizontalOptions = LayoutOptions.Center,
            VerticalOptions = LayoutOptions.Center,
            Content = label
        };

        if (_selectedDate.HasValue &&
            date.Date == _selectedDate.Value.Date)
        {
            border.BackgroundColor = Colors.DodgerBlue;
            border.Stroke = Colors.DodgerBlue;
            label.TextColor = Colors.White;
        }
        else if (date.Date == DateTime.Today)
        {
            border.Stroke = Colors.DodgerBlue;
            border.StrokeThickness = 2;
            label.TextColor = Colors.DodgerBlue;
            label.FontAttributes = FontAttributes.Bold;
        }

        var pointer = new PointerGestureRecognizer();

        pointer.PointerEntered += (_, _) =>
        {
            if (_selectedDate?.Date == date.Date)
                return;

            border.Background = Color.FromArgb("#EEF5FF");
        };

        pointer.PointerExited += (_, _) =>
        {
            if (_selectedDate?.Date == date.Date)
                return;

            border.Background = Colors.Transparent;
        };

        border.GestureRecognizers.Add(pointer);

        var tap = new TapGestureRecognizer();
        tap.Tapped += (s, e) => SelectDate(date);

        border.GestureRecognizers.Add(tap);

        return border;
    }
    private Label CreateDayLabel(int day)
    {
        var date = new DateTime(
            _currentMonth.Year,
            _currentMonth.Month,
            day);


        var label = new Label
        {
            Text = day.ToString(),

            WidthRequest = 40,
            HeightRequest = 40,

            HorizontalTextAlignment = TextAlignment.Center,
            VerticalTextAlignment = TextAlignment.Center,

            TextColor = Colors.Black,
            FontSize = 16
        };

        if (_selectedDate.HasValue &&
            date.Date == _selectedDate.Value.Date)
        {
            label.BackgroundColor = Colors.DodgerBlue;
            label.TextColor = Colors.White;
        }

        else if (date.Date == DateTime.Today)
        {
            label.TextColor = Colors.DodgerBlue;
            label.FontAttributes = FontAttributes.Bold;
        }


        var tap = new TapGestureRecognizer();

        tap.Tapped += (s, e) =>
        {
            SelectDate(date);
        };


        label.GestureRecognizers.Add(tap);


        return label;
    }
    private async void SelectDate(DateTime date)
    {
        _selectedDate = date;

        _currentMonth = new DateTime(
            date.Year,
            date.Month,
            1);

        if (!_owner.HasTime)
        {
            _owner.Date = date;
            //_owner.RaiseUnfocused();
            await WmsPopupService.CloseAsync(this);
            return;
        }

        BuildCalendar();
    }
    private void PrevMonth_Clicked(object sender, EventArgs e)
    {
        switch (_viewMode)
        {
            case WmsCalendarViewMode.Day:
                _currentMonth = _currentMonth.AddMonths(-1);
                BuildCalendar();
                break;

            case WmsCalendarViewMode.Month:
                _currentMonth = _currentMonth.AddYears(-1);
                BuildMonth();
                UpdateHeader();
                break;

            case WmsCalendarViewMode.Year:
                _currentMonth = _currentMonth.AddYears(-12);
                BuildYear();
                UpdateHeader();
                break;
        }
    }

    private void NextMonth_Clicked(object sender, EventArgs e)
    {
        switch (_viewMode)
        {
            case WmsCalendarViewMode.Day:
                _currentMonth = _currentMonth.AddMonths(1);
                BuildCalendar();
                break;

            case WmsCalendarViewMode.Month:
                _currentMonth = _currentMonth.AddYears(1);
                BuildMonth();
                UpdateHeader();
                break;

            case WmsCalendarViewMode.Year:
                _currentMonth = _currentMonth.AddYears(12);
                BuildYear();
                UpdateHeader();
                break;
        }
    }
    private async void Ok_Clicked(object sender, EventArgs e)
    {
        if (_selectedDate == null)
            return;

        int hour = HourPicker.SelectedIndex;
        int minute = MinutePicker.SelectedIndex;

        int second = _owner.HasSecond
            ? SecondPicker.SelectedIndex
            : 0;

        //_owner.Date = new DateTime(
        //    _selectedDate.Value.Year,
        //    _selectedDate.Value.Month,
        //    _selectedDate.Value.Day,
        //    hour,
        //    minute,
        //    second);
        _owner.Date = _selectedDate;
        //_owner.RaiseUnfocused();
        await WmsPopupService.CloseAsync(this);
    }
    private async void Cancel_Clicked(object sender, EventArgs e)
    {
        //_owner.RaiseUnfocused();
        await WmsPopupService.CloseAsync(this);
    }
    private async void Today_Clicked(object sender, EventArgs e)
    {
        var now = DateTime.Now;

        _currentMonth = new DateTime(
            now.Year,
            now.Month,
            1);

        _selectedDate = now;

        BuildCalendar();

        if (!_owner.HasTime)
        {
            _owner.Date = now.Date;
           // _owner.RaiseUnfocused();
            await WmsPopupService.CloseAsync(this);
            return;
        }

        HourPicker.SelectedIndex = now.Hour;
        MinutePicker.SelectedIndex = now.Minute;

        if (_owner.HasSecond)
        {
            SecondPicker.SelectedIndex = now.Second;
        }
    }
    private async void Clear_Clicked(object sender, EventArgs e)
    {
        _owner.ClearDate();
        //_owner.Date = null;

        //_owner.RaiseUnfocused();

        await WmsPopupService.CloseAsync(this);
    }
    private void UpdateView()
    {
        PART_DayView.IsVisible = _viewMode == WmsCalendarViewMode.Day;

        PART_MonthView.IsVisible = _viewMode == WmsCalendarViewMode.Month;

        PART_YearView.IsVisible = _viewMode == WmsCalendarViewMode.Year;

        switch (_viewMode)
        {
            case WmsCalendarViewMode.Day:
                BuildCalendar();
                break;
            case WmsCalendarViewMode.Month:
                BuildMonth();
                break;
            case WmsCalendarViewMode.Year:
                BuildYear();
                break;
        }
    }
    private void Header_Tapped(object sender, TappedEventArgs e)
    {
        switch (_viewMode)
        {
            case WmsCalendarViewMode.Day:
                _viewMode = WmsCalendarViewMode.Month;
                break;

            case WmsCalendarViewMode.Month:
                _viewMode = WmsCalendarViewMode.Year;
                break;

            case WmsCalendarViewMode.Year:
                _viewMode = WmsCalendarViewMode.Day;
                break;
        }
        UpdateHeader();
        UpdateView();

        if (_viewMode == WmsCalendarViewMode.Day)
        {
            BuildCalendar();
        }
    }
    private void UpdateHeader()
    {
        switch (_viewMode)
        {
            case WmsCalendarViewMode.Day:
                PART_Header.Text = _currentMonth.ToString("yyyy/MM");
                break;

            case WmsCalendarViewMode.Month:
                PART_Header.Text = _currentMonth.Year.ToString();
                break;

            case WmsCalendarViewMode.Year:
                int start = GetStartYear();
                PART_Header.Text = $"{start} - {start + 11}";
                break;
        }
    }
    private void BuildMonth()
    {
        //PART_MonthGrid.Children.Clear();
        PART_MonthGrid.Clear();

        string[] months =
        {
            "01","02","03",
            "04","05","06",
            "07","08","09",
            "10","11","12"
        };

        for (int i = 0; i < 12; i++)
        {
            int month = i + 1;

            var border = CreateMonthItem(months[i], month);

            Grid.SetRow(border, i / 3);
            Grid.SetColumn(border, i % 3);

            PART_MonthGrid.Children.Add(border);
        }
    }
    private void BuildYear()
    {
        PART_YearGrid.Children.Clear();

        int startYear = (_currentMonth.Year / 12) * 12;

        for (int i = 0; i < 12; i++)
        {
            int year = startYear + i;

            var item = CreateYearItem(year);

            Grid.SetRow(item, i / 3);
            Grid.SetColumn(item, i % 3);

            PART_YearGrid.Children.Add(item);
        }
    }
    private Border CreateMonthItem(string text, int month)
    {
        var label = new Label
        {
            Text = text,
            HorizontalOptions = LayoutOptions.Center,
            VerticalOptions = LayoutOptions.Center,
            HorizontalTextAlignment = TextAlignment.Center,
            VerticalTextAlignment = TextAlignment.Center,
            FontSize = 16,
            TextColor = Colors.Black
        };

        var border = new Border
        {
            WidthRequest = 48,
            HeightRequest = 48,
            Margin = 6,
            StrokeShape = new RoundRectangle
            {
                CornerRadius = 26
            },
            Content = label,
            Background = Colors.Transparent,
            Stroke = Colors.Transparent,
        };

        if (month == _currentMonth.Month)
        {
            border.Background = Colors.DodgerBlue;
            border.Stroke = Colors.DodgerBlue;
            label.TextColor = Colors.White;
        }


        var pointer = new PointerGestureRecognizer();

        pointer.PointerEntered += (_, _) =>
        {
            if (month != _currentMonth.Month)
                border.Background = Color.FromArgb("#EEF5FF");
        };

        pointer.PointerExited += (_, _) =>
        {
            if (month != _currentMonth.Month)
                border.Background = Colors.Transparent;
        };

        border.GestureRecognizers.Add(pointer);

        var tap = new TapGestureRecognizer();

        tap.Tapped += (_, _) =>
        {
            SelectMonth(month);
        };

        border.GestureRecognizers.Add(tap);

        return border;
    }
    private Border CreateYearItem(int year)
    {
        var label = new Label
        {
            Text = year.ToString(),
            HorizontalTextAlignment = TextAlignment.Center,
            VerticalTextAlignment = TextAlignment.Center,
            HorizontalOptions = LayoutOptions.Center,
            VerticalOptions = LayoutOptions.Center,
            TextColor = Colors.Black,
        };

        var border = new Border
        {
            WidthRequest = 48,
            HeightRequest = 48,
            Margin = 6,
            StrokeShape = new RoundRectangle
            {
                CornerRadius = 26
            },
            Content = label,
            Background = Colors.Transparent,
            Stroke = Colors.Transparent
        };

        if (year == _currentMonth.Year)
        {
            border.Background = Colors.DodgerBlue;
            border.Stroke = Colors.DodgerBlue;
            label.TextColor = Colors.White;
        }

        var pointer = new PointerGestureRecognizer();

        pointer.PointerEntered += (_, _) =>
        {
            if (year != _currentMonth.Year)
                border.Background = Color.FromArgb("#EEF5FF");
        };

        pointer.PointerExited += (_, _) =>
        {
            if (year != _currentMonth.Year)
                border.Background = Colors.Transparent;
        };

        border.GestureRecognizers.Add(pointer);

        border.GestureRecognizers.Add(
            new TapGestureRecognizer
            {
                Command = new Command(() =>
                {
                    SelectYear(year);
                })
            });

        return border;
    }
    private void SelectMonth(int month)
    {
        _currentMonth = new DateTime(
            _currentMonth.Year,
            month,
            1);

        _viewMode = WmsCalendarViewMode.Day;

        UpdateHeader();

        UpdateView();

        BuildCalendar();
    }
    private void SelectYear(int year)
    {
        _currentMonth = new DateTime(
            year,
            _currentMonth.Month,
            1);

        _viewMode = WmsCalendarViewMode.Month;

        UpdateHeader();
        UpdateView();
    }
    private int GetStartYear()
    {
        return _currentMonth.Year - (_currentMonth.Year % 12);
    }
    private void Back_Clicked(object sender, EventArgs e)
    {
        switch (_viewMode)
        {
            case WmsCalendarViewMode.Year:
                _viewMode = WmsCalendarViewMode.Month;
                break;

            case WmsCalendarViewMode.Month:
                _viewMode = WmsCalendarViewMode.Day;
                break;
        }

        UpdateHeader();
        UpdateView();
    }
    public async Task AnimateInAsync()
    {
        await ThisBorder.ScaleTo(
            1.0,
            180,
            Easing.CubicOut);
    }

    public async Task AnimateOutAsync()
    {
        await ThisBorder.ScaleTo(
            0.85,
            120,
            Easing.CubicIn);
    }
}
using M_WMS.Controls.Popups;

namespace M_WMS.Controls.Selects;

public partial class WmsSelectPopup : ContentView
{
    //public double ItemHeight { get; set; } = 40;
    //public IList? ItemsSource
    //{
    //    get => PART_CollectionView.ItemsSource as IList;
    //    set => PART_CollectionView.ItemsSource = value;
    //}
    public WmsSelectPopup()
	{
		InitializeComponent();
        //Color = Colors.Transparent;
        InitializePopup();

        //Closed += WmsSelectPopup_Closed;
        ////CalculatePopupHeight();
        ////CalculateHeight();

        PART_ClearButton.IsVisible = false;

        PART_SearchEntry.TextChanged += (_, e) =>
        {
            PART_ClearButton.IsVisible =
                !string.IsNullOrWhiteSpace(e.NewTextValue);
        };
        PART_SearchBorder.IsVisible = IsSearchVisible;
    }
    //private void WmsSelectPopup_Closed(object? sender, PopupClosedEventArgs e)
    //{
    //    PART_CollectionView.SelectionChanged -= OnSelectionChanged;

    //    Closed -= WmsSelectPopup_Closed;
    //}
    partial void InitializePopup();
    //public void UpdatePopupSize()
    //{
    //    const double headerHeight = 50;
    //    const double searchHeight = 60;
    //    //const double itemHeight = 40;

    //    double screenHeight =
    //        DeviceDisplay.Current.MainDisplayInfo.Height /
    //        DeviceDisplay.Current.MainDisplayInfo.Density;

    //    double maxPopupHeight = screenHeight * 0.8;

    //    int count = ItemsSource?.Count ?? 0;

    //    double listHeight = count * ItemHeight;

    //    double popupHeight =
    //        Math.Min(headerHeight + searchHeight + listHeight + 20,
    //                 maxPopupHeight);

    //    Size = new Size(320, popupHeight);

    //    PART_CollectionView.HeightRequest =
    //        popupHeight - headerHeight - searchHeight - 20;
    //}
    //private void CalculateHeight()
    //{
    //    double screenHeight =
    //        DeviceDisplay.Current.MainDisplayInfo.Height /
    //        DeviceDisplay.Current.MainDisplayInfo.Density;

    //    PART_CollectionView.MaximumHeightRequest = screenHeight * 0.6;
    //}
    //private void OnCloseClicked(object sender, EventArgs e)
    //{
    //    Close();
    //}

    //private void CalculatePopupHeight()
    //{
    //    double screenHeight =
    //        DeviceDisplay.Current.MainDisplayInfo.Height /
    //        DeviceDisplay.Current.MainDisplayInfo.Density;

    //    PART_Border.MaximumHeightRequest = screenHeight * 0.8;
    //}

    private async void OnCloseTapped(object sender, TappedEventArgs e)
    {
        //await WmsPopupService.CloseAsync();
        await WmsPopupService.CloseAsync(this);
    }
    private async void OnOutsideTapped(object sender, TappedEventArgs e)
    {
        await WmsPopupService.CloseAsync(this);
    }
    private void OnFilterTapped(object sender, TappedEventArgs e)
    {
        IsSearchVisible = !IsSearchVisible;

        //if (PART_SearchBorder.IsVisible)
        //    await HideSearchAsync();
        //else
        //    await ShowSearchAsync();

        //UpdatePopupSize();
    }
  
    public async Task AnimateInAsync()
    {
        await PART_Border.ScaleTo(
            1.0,
            180,
            Easing.CubicOut);
    }

    public async Task AnimateOutAsync()
    {
        await PART_Border.ScaleTo(
            0.85,
            120,
            Easing.CubicIn);
    }
}
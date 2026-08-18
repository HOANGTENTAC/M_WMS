using CommunityToolkit.Maui.Extensions;
using M_WMS.Controls.Helpers;

namespace M_WMS.Controls.Selects;

public partial class WmsSelect : ContentView
{
    private bool IsPopupOpen;
    public WmsSelect()
    {
        InitializeComponent();
        InitializeContent();
        InitializeStyle();
        InitializeEvents();
        UpdateArrow();
    }
    private async Task OnOpenAsync()
    {
        if (Application.Current?.Windows.FirstOrDefault()?.Page is not Page page)
            return;

        //var popup = new WmsSelectPopup
        //{
        //    Title = Title,
        //    ItemsSource = ItemsSource,
        //    SelectedItem = SelectedItem,
        //    DisplayMemberPath = DisplayMemberPath,
        //    ItemTemplate = ItemTemplate
        //};
        var popup = new WmsSelectPopup();
        popup.EmptyText = EmptyText;
        popup.Title = Title;
        popup.SearchPlaceholder = SearchPlaceholder;
        popup.DisplayMemberPath = DisplayMemberPath;
        popup.SelectedValuePath = SelectedValuePath;

        popup.ItemTemplate = ItemTemplate;

        // Quan trọng: gán SelectedItem trước
        popup.SelectedItem = SelectedItem;

        // Cuối cùng mới gán ItemsSource
        popup.ItemsSource = ItemsSource;
        popup.SelectedValue = SelectedValue;
        popup.ItemSelected += Popup_ItemSelected;

        var border = FindParentBorder();

        border?.SetFocused();
        IsPopupOpen = true;
        ApplyArrow();
        await page.ShowPopupAsync(popup);
        IsPopupOpen = false;
        ApplyArrow();
        border?.SetNormal();
        popup.ItemSelected -= Popup_ItemSelected;
    }
    private void UpdateSelectedItemByValue()
    {
        if (ItemsSource == null)
            return;

        foreach (var item in ItemsSource)
        {
            if (DisplayHelper.PropertyEquals(
                    item,
                    SelectedValuePath,
                    SelectedValue))
            {
                SelectedItem = item;

                UpdateSelectedIndex();

                UpdateDisplay();

                break;
            }
        }
    }
}
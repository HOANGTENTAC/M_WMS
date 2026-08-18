using M_WMS.Controls.Models;

namespace M_WMS.Controls.Selects
{
    public partial class WmsSelectPopup
    {
        private void FilterItems(string? keyword)
        {
            keyword = keyword?.Trim() ?? string.Empty;

            _displayItems.Clear();

            IEnumerable<WmsSelectItem> items;

            if (string.IsNullOrWhiteSpace(keyword))
            {
                items = _allItems;
            }
            else
            {
                items = _allItems.Where(x =>
                    x.Text.Contains(keyword,
                        StringComparison.OrdinalIgnoreCase));
            }

            foreach (var item in items)
                _displayItems.Add(item);

            UpdateSelection(SelectedItem);
            UpdatePopupSize();

            PART_ClearButton.IsVisible =
                !string.IsNullOrWhiteSpace(keyword);
        }
        private void OnSearchTextChanged(
        object? sender,
        TextChangedEventArgs e)
        {
            FilterItems(e.NewTextValue);
        }
        private void OnClearTapped(object? sender, TappedEventArgs e)
        {
            PART_SearchEntry.Text = string.Empty;

            PART_SearchEntry.Focus();
        }
    }
}

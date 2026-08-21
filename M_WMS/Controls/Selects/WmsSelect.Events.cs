using M_WMS.Controls.Borders;
using M_WMS.Controls.Popups;
using System.Collections.Specialized;

namespace M_WMS.Controls.Selects
{
    public partial class WmsSelect
    {
        private void InitializeEvents()
        {
            var tap = new TapGestureRecognizer();

            tap.Tapped += OnTapped;

            //PART_Border.GestureRecognizers.Add(tap);
            PART_Grid.GestureRecognizers.Add(tap);
        }
        private async void OnTapped(object? sender, TappedEventArgs e)
        {
            if (!IsEnabled)
                return;

            if (IsReadOnly)
                return;

            SetState(WmsSelectState.Pressed);

            await Task.Delay(80);

            SetState(WmsSelectState.Normal);

            await OnOpenAsync();
        }
        private async void Popup_ItemSelected(object? sender, WmsSelectItemSelectedEventArgs e)
        {
            //SelectedItem = item;
            SelectedItem = e.SelectedItem;
            SelectedValue = e.SelectedValue;

            UpdateSelectedIndex();
            UpdateDisplay();

            //await WmsPopupService.CloseAsync();
            if (sender is WmsSelectPopup popup)
            {
                IsPopupOpen = false;
                ApplyArrow();
                popup.ItemSelected -= Popup_ItemSelected;

                await WmsPopupService.CloseAsync(popup);
            }
        }
        private void OnItemsChanged(object? sender, NotifyCollectionChangedEventArgs e)
        {
            ItemsSource = _items;
        }
        private WmsBorder? FindParentBorder()
        {
            Element? parent = Parent;

            while (parent != null)
            {
                if (parent is WmsBorder border)
                    return border;

                parent = parent.Parent;
            }

            return null;
        }
    }
}

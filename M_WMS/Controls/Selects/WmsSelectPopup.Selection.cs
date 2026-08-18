using M_WMS.Controls.Helpers;
using M_WMS.Controls.Models;

namespace M_WMS.Controls.Selects
{
    public partial class WmsSelectPopup
    {
        //public event EventHandler<object?>? ItemSelected;
        public event EventHandler<WmsSelectItemSelectedEventArgs>? ItemSelected;
        private async void OnSelectionChanged(
            object? sender,
            SelectionChangedEventArgs e)
        {
            //if (_ignoreNextSelectionChanged)
            //{
            //    _ignoreNextSelectionChanged = false;
            //    return;
            //}

            if (e.CurrentSelection.Count == 0)
                return;

            if (e.CurrentSelection[0] is not WmsSelectItem item)
                return;

            if (Equals(item.Value, SelectedItem))
            {
                PART_CollectionView.SelectedItem = null;
                return;
            }

            SelectedItem = item.Value;
            SelectedValue = DisplayHelper.GetPropertyValue(item.Value,SelectedValuePath);

            UpdateSelection(SelectedItem);

            //await Dispatcher.DispatchAsync(async () =>
            //{
            //    await Task.Yield();
            //});

            //ItemSelected?.Invoke(this, SelectedItem);
            ItemSelected?.Invoke(
            this,
            new WmsSelectItemSelectedEventArgs
            {
                SelectedItem = SelectedItem,
                SelectedValue = SelectedValue
            });

            await Task.Delay(180);
            PART_CollectionView.SelectedItem = null;
            //Close(SelectedItem);
            await CloseAsync();
            //await CloseAsync();
        }
        private void UpdateSelection(object? selectedItem)
        {
            foreach (var item in _allItems)
            {
                item.IsSelected = Equals(item.Value, selectedItem);

                ApplySelectionStyle(item);
            }
        }
        private void ApplySelectionStyle(WmsSelectItem item)
        {
            if (item.IsSelected)
            {
                item.BackgroundBrush = new SolidColorBrush(Color.FromArgb("#E3F2FD"));
                item.TextColor = Colors.DodgerBlue;
                item.FontAttributes = FontAttributes.Bold;
            }
            else
            {
                item.BackgroundBrush = new SolidColorBrush(Colors.Transparent);
                item.TextColor = Colors.Black;
                item.FontAttributes = FontAttributes.None;
            }
        }
    }
}

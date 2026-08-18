using M_WMS.Controls.Helpers;
using M_WMS.Controls.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace M_WMS.Controls.Selects
{
    public partial class WmsSelectPopup
    {
        public string? SelectedValuePath { get; set; }

        public object? SelectedValue { get; set; }

        private const double HeaderHeight = 50;

        private const double SearchHeight = 50;

        private const double FooterPadding = 20;

        private const double PopupWidth = 320;

        private const double BottomPadding = 10;
        private void RefreshItems()
        {
            //_isInitializing = true;
            _allItems.Clear();
            _displayItems.Clear();

            if (ItemsSource != null)
            {
                foreach (var item in ItemsSource)
                {
                    var selectItem = new WmsSelectItem
                    {
                        Value = item,
                        Text = DisplayHelper.GetDisplayText(item, DisplayMemberPath),
                        IsSelected = Equals(item, SelectedItem),
                        Template = ItemTemplate
                    };

                    _allItems.Add(selectItem);
                    _displayItems.Add(selectItem);
                }
            }

            PART_CollectionView.ItemsSource = _displayItems;
            if (ItemTemplate != null)
            {
                PART_CollectionView.ItemTemplate = ItemTemplate;
            }
            UpdateSelection(SelectedItem);
            UpdatePopupSize();
            ScrollToSelectedItem();
            //_isInitializing = false;
        }

        //        public void UpdatePopupSize()
        //        {
        //            double searchHeight = PART_SearchBorder.IsVisible ? SearchHeight : 0;
        //            if (PART_CollectionView.ItemsSource is not IList items)
        //                return;

        //            double screenHeight =
        //                DeviceDisplay.Current.MainDisplayInfo.Height /
        //                DeviceDisplay.Current.MainDisplayInfo.Density;

        //            double maxPopupHeight = screenHeight * MaxPopupHeightRatio;

        //            double listHeight = Math.Min(
        //                items.Count * ItemHeight,
        //                maxPopupHeight - HeaderHeight - searchHeight);

        //#if ANDROID
        //            PART_CollectionView.HeightRequest = listHeight;
        //            PART_CollectionView.MaximumHeightRequest = listHeight;

        //            Size = new Size(
        //                PopupWidth,
        //                HeaderHeight +
        //                searchHeight +
        //                listHeight +
        //                BottomPadding);
        //#endif

        //        }
        public void UpdatePopupSize()
        {
            if (PART_CollectionView.ItemsSource is not IList items)
                return;

            double screenHeight =
                DeviceDisplay.Current.MainDisplayInfo.Height /
                DeviceDisplay.Current.MainDisplayInfo.Density;

            double maxPopupHeight = screenHeight * MaxPopupHeightRatio;

            double searchHeight = IsSearchVisible ? SearchHeight : 0;

            double maxListHeight =
                maxPopupHeight -
                HeaderHeight -
                searchHeight -
                BottomPadding;

            double minListHeight = ItemHeight * 5;

            double listHeight =
                Math.Clamp(
                    items.Count * ItemHeight,
                    minListHeight,
                    maxListHeight);

            PART_CollectionView.HeightRequest = listHeight;

            Size = new Size(
                PopupWidth,
                HeaderHeight +
                searchHeight +
                listHeight +
                BottomPadding);
        }
        //private async void ScrollToSelectedItem()
        //{
        //    if (SelectedItem == null)
        //        return;

        //    var item = _displayItems.FirstOrDefault(x =>
        //        Equals(x.Value, SelectedItem));

        //    if (item == null)
        //        return;

        //    //await Task.Delay(50);

        //    //_ignoreNextSelectionChanged = true;

        //    //PART_CollectionView.SelectedItem = item;

        //    PART_CollectionView.ScrollTo(
        //        item,
        //        position: ScrollToPosition.Center,
        //        animate: false);
        //}
        private async void ScrollToSelectedItem()
        {
            if (SelectedItem == null)
                return;

            var item = _displayItems.FirstOrDefault(x =>
                Equals(x.Value, SelectedItem));

            if (item == null)
                return;

            // Đợi CollectionView render xong
            await MainThread.InvokeOnMainThreadAsync(async () =>
            {
                await Task.Yield();

                PART_CollectionView.ScrollTo(
                    item,
                    position: ScrollToPosition.Center,
                    animate: false);
            });
        }
        private async Task ShowSearchAsync()
        {
            PART_SearchBorder.IsVisible = true;
            PART_SearchBorder.HeightRequest = 0;
            PART_SearchBorder.Opacity = 0;

            PART_SearchEntry.Text = string.Empty;
            FilterItems(string.Empty);
            UpdatePopupSize();

            await Task.WhenAll(
                PART_SearchBorder.FadeTo(1, 150),
                PART_SearchBorder.AnimateAsync(0, SearchHeight, h =>
                {
                    PART_SearchBorder.HeightRequest = h;
                })
            );

            PART_SearchEntry.Focus();
        }
        private async Task HideSearchAsync()
        {
            await Task.WhenAll(
                PART_SearchBorder.FadeTo(0, 120, Easing.CubicIn),
                PART_SearchBorder.AnimateAsync(SearchHeight, 0, h =>
                {
                    PART_SearchBorder.HeightRequest = h;
                })
            );

            PART_SearchBorder.IsVisible = false;
            UpdatePopupSize();
        }

        //private async Task ShowSearchAsync()
        //{
        //    PART_SearchBorder.IsVisible = true;

        //    PART_SearchBorder.Opacity = 0;
        //    PART_SearchBorder.ScaleY = 0.8;

        //    PART_SearchEntry.Text = string.Empty;

        //    FilterItems(string.Empty);

        //    UpdatePopupSize();

        //    await Task.WhenAll(
        //        PART_SearchBorder.FadeTo(1, 150, Easing.CubicOut),
        //        PART_SearchBorder.ScaleYTo(1, 150, Easing.CubicOut)
        //    );

        //    PART_SearchEntry.Focus();
        //}
        //private async Task HideSearchAsync()
        //{
        //    await Task.WhenAll(
        //        PART_SearchBorder.FadeTo(0, 120, Easing.CubicIn),
        //        PART_SearchBorder.ScaleYTo(0.8, 120, Easing.CubicIn)
        //    );

        //    PART_SearchBorder.IsVisible = false;

        //    UpdatePopupSize();
        //}
        private void UpdateLoadingState()
        {
            PART_LoadingGrid.IsVisible = IsLoading;

            PART_CollectionView.IsVisible = !IsLoading;
        }
        public void UpdateTitle()
        {
            PART_Title.Text = Title;
        }
        public void UpdateSearchPlaceholder()
        {
           PART_SearchEntry.Placeholder = SearchPlaceholder;
        }
    }
}

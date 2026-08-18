using M_WMS.Controls.Models;
using System.Collections;
using System.Collections.ObjectModel;

namespace M_WMS.Controls.Selects
{
    public partial class WmsSelectPopup
    {
        private IList _itemsSource = new List<object>();

        public IList ItemsSource
        {
            get => _itemsSource;
            set
            {
                _itemsSource = value ?? new List<object>();

                RefreshItems();
            }
        }

        public object? SelectedItem { get; set; }

        //public string Title { get; set; } = "Select";

        //public string SearchPlaceholder { get; set; } = "Search...";

        public double ItemHeight { get; set; } = 40;

        public double MaxPopupHeightRatio { get; set; } = 0.8;

        public string DisplayMemberPath { get; set; } = string.Empty;

        private readonly ObservableCollection<WmsSelectItem> _displayItems = [];
        private readonly List<WmsSelectItem> _allItems = [];

        public DataTemplate? ItemTemplate { get; set; }
        //private bool _ignoreNextSelectionChanged;

        public static readonly BindableProperty IsSearchVisibleProperty =
            BindableProperty.Create(
                nameof(IsSearchVisible),
                typeof(bool),
                typeof(WmsSelectPopup),
                false,
                propertyChanged: OnIsSearchVisibleChanged);

        public bool IsSearchVisible
        {
            get => (bool)GetValue(IsSearchVisibleProperty);
            set => SetValue(IsSearchVisibleProperty, value);
        }
        private static async void OnIsSearchVisibleChanged(
            BindableObject bindable,
            object oldValue,
            object newValue)
        {
            var popup = (WmsSelectPopup)bindable;

            if ((bool)newValue)
                await popup.ShowSearchAsync();
            else
                await popup.HideSearchAsync();
        }
        public static readonly BindableProperty EmptyTextProperty =
            BindableProperty.Create(
                nameof(EmptyText),
                typeof(string),
                typeof(WmsSelectPopup),
                "No data found");

        public string EmptyText
        {
            get => (string)GetValue(EmptyTextProperty);
            set => SetValue(EmptyTextProperty, value);
        }
        public static readonly BindableProperty IsLoadingProperty =
        BindableProperty.Create(
        nameof(IsLoading),
        typeof(bool),
        typeof(WmsSelectPopup),
        false,
        propertyChanged: OnLoadingChanged);

        public bool IsLoading
        {
            get => (bool)GetValue(IsLoadingProperty);
            set => SetValue(IsLoadingProperty, value);
        }
        private static void OnLoadingChanged(
        BindableObject bindable,
        object oldValue,
        object newValue)
        {
            ((WmsSelectPopup)bindable).UpdateLoadingState();
        }
        public static readonly BindableProperty TitleProperty =
        BindableProperty.Create(
        nameof(Title),
        typeof(string),
        typeof(WmsSelectPopup),
        "Select",
        propertyChanged: OnTitleChanged);

        public string Title
        {
            get => (string)GetValue(TitleProperty);
            set => SetValue(TitleProperty, value);
        }
        private static void OnTitleChanged(
            BindableObject bindable,
            object oldValue,
            object newValue)
        {
            if (bindable is WmsSelectPopup popup)
            {
                popup.UpdateTitle();
            }
        }

        public static readonly BindableProperty SearchPlaceholderProperty =
        BindableProperty.Create(
        nameof(SearchPlaceholder),
        typeof(string),
        typeof(WmsSelectPopup),
        "Search...",
        propertyChanged: OnSearchPlaceholderChanged);

        public string SearchPlaceholder
        {
            get => (string)GetValue(SearchPlaceholderProperty);
            set => SetValue(SearchPlaceholderProperty, value);
        }
        private static void OnSearchPlaceholderChanged(
            BindableObject bindable,
            object oldValue,
            object newValue)
        {
            if (bindable is WmsSelectPopup popup)
            {
                popup.UpdateSearchPlaceholder();
            }
        }
    }
}

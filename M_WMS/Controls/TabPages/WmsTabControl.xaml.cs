using M_WMS.Controls.Enums;
using M_WMS.Controls.Models;
using System.Collections.ObjectModel;

namespace M_WMS.Controls.TabPages;

public partial class WmsTabControl : ContentView
{
    public WmsTabControl()
	{
		InitializeComponent();
	}
    public ObservableCollection<WmsTabItem> ItemsSource
    {
        get => (ObservableCollection<WmsTabItem>)GetValue(ItemsSourceProperty);
        set => SetValue(ItemsSourceProperty, value);
    }

    public static readonly BindableProperty ItemsSourceProperty =
        BindableProperty.Create(
            nameof(ItemsSource),
            typeof(ObservableCollection<WmsTabItem>),
            typeof(WmsTabControl),
            new ObservableCollection<WmsTabItem>(),
            propertyChanged: OnItemsSourceChanged);

    public int SelectedIndex
    {
        get => (int)GetValue(SelectedIndexProperty);
        set => SetValue(SelectedIndexProperty, value);
    }

    public static readonly BindableProperty SelectedIndexProperty =
        BindableProperty.Create(
            nameof(SelectedIndex),
            typeof(int),
            typeof(WmsTabControl),
            0);

    public WmsTabCacheMode CacheMode
    {
        get => (WmsTabCacheMode)GetValue(CacheModeProperty);
        set => SetValue(CacheModeProperty, value);
    }

    public static readonly BindableProperty CacheModeProperty =
        BindableProperty.Create(
            nameof(CacheMode),
            typeof(WmsTabCacheMode),
            typeof(WmsTabControl),
            WmsTabCacheMode.Cache);

    private static void OnItemsSourceChanged(
        BindableObject bindable,
        object oldValue,
        object newValue)
    {
        var control = (WmsTabControl)bindable;

        control.PART_TabCollection.ItemsSource =
            control.ItemsSource;

        if (control.ItemsSource.Count > 0)
        {
            control.PART_TabCollection.SelectedItem =
                control.ItemsSource[0];
        }
    }
    private void OnSelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (e.CurrentSelection.FirstOrDefault() is not WmsTabItem tab)
            return;

        if (CacheMode == WmsTabCacheMode.Cache)
        {
            if (tab.CachedView == null)
            {
                tab.CachedView = tab.ViewFactory?.Invoke();
            }

            ContentHost.Content = tab.CachedView;
        }
        else
        {
            ContentHost.Content = tab.ViewFactory?.Invoke();
        }

        SelectedIndex = ItemsSource.IndexOf(tab);
    }
}
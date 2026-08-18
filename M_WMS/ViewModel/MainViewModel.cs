using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using M_WMS.Helpers;
using M_WMS.Pages;
using M_WMS.Resources.Languages;
using System.Collections.ObjectModel;

namespace M_WMS.ViewModel
{
    public partial class TabItemModel : ObservableObject
    {
        [ObservableProperty]
        private string _title = string.Empty;
        public string Icon { get; set; }
        public Type TargetView { get; set; }

        [ObservableProperty]
        private bool _isSelected;
    }
    public partial class MainViewModel : ObservableObject
    {
        private readonly IServiceProvider _serviceProvider;
        public ObservableCollection<TabItemModel> Tabs { get; set; } = new();

        [ObservableProperty]
        private View _currentView;

        public MainViewModel(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            //// Khởi tạo danh sách Tab động
            //Tabs.Add(new TabItemModel { Title = LocalizationResourceManager.Instance["Home"], Icon = "home.png", TargetView = typeof(HomeView), IsSelected = true });
            //Tabs.Add(new TabItemModel { Title = LocalizationResourceManager.Instance["Setting"], Icon = "setting.png", TargetView = typeof(SettingView) });

            //CurrentView = Tabs[0].TargetView;
            InitTabs();
            LocalizationResourceManager.Instance.PropertyChanged += (s, e) =>
            {
                UpdateTabTitles();
            };
        }
        private void UpdateTabTitles()
        {
            if (Tabs.Count >= 2)
            {
                // AppResources.Home và AppResources.Setting lúc này đã tự trả về tiếng mới
                Tabs[0].Title = AppResources.Home;
                Tabs[1].Title = AppResources.Setting;
            }
        }
        private void InitTabs()
        {
            Tabs.Add(new TabItemModel { Title = AppResources.Home, Icon = "home.png", TargetView = typeof(HomeView), IsSelected = true });
            Tabs.Add(new TabItemModel { Title = AppResources.Setting, Icon = "setting.png", TargetView = typeof(SettingView) });

            CurrentView = (View)_serviceProvider.GetRequiredService(Tabs[0].TargetView);
        }
        [RelayCommand]
        private void SelectTab(TabItemModel selectedTab)
        {
            if (selectedTab == null || selectedTab.IsSelected) return;

            foreach (var tab in Tabs)
                tab.IsSelected = (tab == selectedTab);

            CurrentView = (View)_serviceProvider.GetRequiredService(selectedTab.TargetView);
        }
    }
}

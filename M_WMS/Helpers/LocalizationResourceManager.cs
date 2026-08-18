using M_WMS.Resources.Languages;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace M_WMS.Helpers
{
    public class LocalizationResourceManager : INotifyPropertyChanged
    {
        private const string LanguageKey = "selected_language";
        private static readonly Lazy<LocalizationResourceManager> _instance =
            new Lazy<LocalizationResourceManager>(() => new LocalizationResourceManager());
        //public string CurrentLanguageCode => (AppResources.Culture ?? CultureInfo.CurrentUICulture).TwoLetterISOLanguageName;
        public static LocalizationResourceManager Instance => _instance.Value;

        private LocalizationResourceManager()
        {
            InitCulture();
        }
        public void InitCulture()
        {
            // Đọc ngôn ngữ từ Preferences, nếu chưa có thì lấy theo hệ thống máy
            string savedLanguage = Preferences.Get(LanguageKey, CultureInfo.CurrentCulture.TwoLetterISOLanguageName);

            // Thiết lập Culture cho ứng dụng mà không cần lưu lại Preferences
            SetCulture(new CultureInfo(savedLanguage), savePreference: false);
        }
        public string this[string text]
        {
            get => AppResources.ResourceManager.GetString(text, AppResources.Culture) ?? string.Empty;
        }

        public void SetCulture(CultureInfo culture, bool savePreference = true)
        {
            AppResources.Culture = culture;

            // Cài đặt Culture cho toàn bộ luồng (Thread) chạy app
            CultureInfo.DefaultThreadCurrentCulture = culture;
            CultureInfo.DefaultThreadCurrentUICulture = culture;
            Thread.CurrentThread.CurrentCulture = culture;
            Thread.CurrentThread.CurrentUICulture = culture;

            if (savePreference)
            {
                Preferences.Set(LanguageKey, culture.TwoLetterISOLanguageName);
            }

            // Báo cho UI Bindings cập nhật lại chuỗi chữ
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(null));
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        // Thêm Property này để truy xuất nhanh
        public CultureInfo CurrentCulture => AppResources.Culture ?? CultureInfo.CurrentUICulture;

        // Hoặc lấy thẳng mã ISO 2 ký tự
        public string CurrentLanguageCode => CurrentCulture.TwoLetterISOLanguageName;
    }
}

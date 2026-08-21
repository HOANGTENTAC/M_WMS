using M_WMS.Controls.Borders;
using M_WMS.Controls.Selects;
using Microsoft.Maui.Layouts;

namespace M_WMS.Controls.Popups
{
    public static class WmsPopupService
    {
        private static WmsBorder? _border;
        private static readonly Dictionary<ContentPage, AbsoluteLayout> _roots = new();

        private static readonly Dictionary<View, WmsPopupHost> _hosts = new();

        public static async Task ShowAsync(View popup, WmsBorder? border = null)
        {
            _border = border;
            if (Shell.Current?.CurrentPage is not ContentPage page)
                return;

            var root = GetOrCreateRoot(page);

            var host = new WmsPopupHost(popup);

            AbsoluteLayout.SetLayoutBounds(
                host,
                new Rect(0, 0, 1, 1));

            AbsoluteLayout.SetLayoutFlags(
                host,
                AbsoluteLayoutFlags.All);

            root.Children.Add(host);

            _hosts[popup] = host;

            if (popup is WmsSelectPopup selectPopup)
            {
                await selectPopup.AnimateInAsync();
            }
            else if(popup is WmsCalendarPopup calendarPopup)
            {
                await calendarPopup.AnimateInAsync();
            }

            if (border != null)
            {
                border?.SetFocused();
            }
        }

        public static async Task CloseAsync(View popup)
        {
            if (!_hosts.TryGetValue(popup, out var host))
                return;

            if (popup is WmsSelectPopup selectPopup)
            {
                await selectPopup.AnimateOutAsync();
            }

            if (host.Parent is AbsoluteLayout root)
            {
                root.Children.Remove(host);
            }

            if(_border != null)
            {
                _border?.SetNormal();
            }

            _hosts.Remove(popup);
        }

        private static AbsoluteLayout GetOrCreateRoot(ContentPage page)
        {
            if (_roots.TryGetValue(page, out var root))
                return root;

            var originalContent = page.Content;

            root = new AbsoluteLayout
            {
                HorizontalOptions = LayoutOptions.Fill,
                VerticalOptions = LayoutOptions.Fill
            };

            page.Content = root;

            if (originalContent != null)
            {
                AbsoluteLayout.SetLayoutBounds(
                    originalContent,
                    new Rect(0, 0, 1, 1));

                AbsoluteLayout.SetLayoutFlags(
                    originalContent,
                    AbsoluteLayoutFlags.All);

                root.Children.Add(originalContent);
            }

            _roots[page] = root;

            return root;
        }
    }
}

namespace M_WMS.Services
{
    public static class WmsPopupService
    {
        private static Grid? _host;
        private static Grid? _container;
        private static bool _isAnimating;

        public static bool IsInitialized =>
            _host != null &&
            _container != null;


        public static void Initialize(
            Grid host,
            Grid container)
        {
            _host = host;
            _container = container;
        }

        public static async Task ShowAsync(View popup)
        {
            if (_isAnimating)
                return;

            _isAnimating = true;
            try
            {
                if (_host == null || _container == null)
                    throw new InvalidOperationException(
                        "WmsPopupService chưa được Initialize");


                _container.Children.Clear();

                popup.Opacity = 0;
                popup.Scale = 0.95;

                popup.HorizontalOptions = LayoutOptions.Center;
                popup.VerticalOptions = LayoutOptions.Center;

                _container.Children.Add(popup);
                _host.Opacity = 0;
                _host.IsVisible = true;
                await _host.FadeTo(1, 140);
                await Task.WhenAll(
                    popup.FadeTo(1, 140, Easing.CubicOut),
                    popup.ScaleTo(1, 140, Easing.CubicOut)
                );
            }
            finally
            {
                _isAnimating = false;
            }
        }


        public static async Task CloseAsync()
        {
            if (_isAnimating)
                return;

            _isAnimating = true;
            try
            {
                if (_host == null || _container == null)
                    return;

                if (_container.Children.FirstOrDefault() is View popup)
                {
                    await Task.WhenAll(
                        popup.FadeTo(0, 120, Easing.CubicIn),
                        popup.ScaleTo(0.95, 120, Easing.CubicIn)
                    );
                }

                _container.Children.Clear();
                await _host.FadeTo(0, 120);
                _host.IsVisible = false;
                _host.Opacity = 1;
            }
            finally
            {
                _isAnimating = false;
            }
        }
    }
}

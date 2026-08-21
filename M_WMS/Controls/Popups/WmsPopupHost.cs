namespace M_WMS.Controls.Popups
{
    public class WmsPopupHost : Grid
    {
        public WmsPopupHost(View popup)
        {
            HorizontalOptions = LayoutOptions.Fill;
            VerticalOptions = LayoutOptions.Fill;

            ZIndex = 9999;

            // =========================
            // FULL SCREEN OVERLAY
            // =========================

            var overlay = new Grid
            {
                BackgroundColor = Color.FromArgb("#66000000"),
                HorizontalOptions = LayoutOptions.Fill,
                VerticalOptions = LayoutOptions.Fill,

                // Phải nhận touch
                InputTransparent = false,

                ZIndex = 0
            };

            overlay.GestureRecognizers.Add(
                new TapGestureRecognizer
                {
                    Command = new Command(async () =>
                    {
                        await WmsPopupService.CloseAsync(popup);
                    })
                });

            Children.Add(overlay);


            // =========================
            // POPUP
            // =========================

            popup.HorizontalOptions = LayoutOptions.Center;
            popup.VerticalOptions = LayoutOptions.Center;

            popup.InputTransparent = false;

            popup.ZIndex = 1;

            Children.Add(popup);
        }
    }
}

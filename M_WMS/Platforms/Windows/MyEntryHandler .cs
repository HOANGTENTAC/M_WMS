using Microsoft.Maui.Handlers;
//using Microsoft.UI.Xaml.Controls;
//using Microsoft.UI.Xaml.Media;

namespace M_WMS.Platforms.Windows
{
    public class MyEntryHandler : EntryHandler
    {
        //protected override void ConnectHandler(TextBox platformView)
        //{
        //    base.ConnectHandler(platformView);

        //    //// Nền trắng trong mọi trạng thái
        //    //var whiteBrush = new Microsoft.UI.Xaml.Media.SolidColorBrush(Microsoft.UI.Colors.White);
        //    //platformView.Resources["TextControlBackground"] = whiteBrush;
        //    //platformView.Resources["TextControlBackgroundFocused"] = whiteBrush;
        //    //platformView.Resources["TextControlBackgroundPointerOver"] = whiteBrush;

        //    // Bỏ underline
        //    platformView.BorderThickness = new Microsoft.UI.Xaml.Thickness(0);
        //    var transparentBrush = new Microsoft.UI.Xaml.Media.SolidColorBrush(Microsoft.UI.Colors.Transparent);
        //    platformView.Resources["TextControlBorderBrush"] = transparentBrush;
        //    platformView.Resources["TextControlBorderBrushFocused"] = transparentBrush;
        //    platformView.Resources["TextControlBorderBrushPointerOver"] = transparentBrush;

        //    //// Placeholder giữ màu xám trong mọi trạng thái
        //    //var grayBrush = new Microsoft.UI.Xaml.Media.SolidColorBrush(Microsoft.UI.Colors.Gray);
        //    //platformView.Resources["TextControlPlaceholderForeground"] = grayBrush;
        //    //platformView.Resources["TextControlPlaceholderForegroundFocused"] = grayBrush;
        //    //platformView.Resources["TextControlPlaceholderForegroundPointerOver"] = grayBrush;
        //}

        //protected override void DisconnectHandler(TextBox platformView)
        //{
        //    platformView.GotFocus -= OnGotFocus;
        //    platformView.LostFocus -= OnLostFocus;

        //    base.DisconnectHandler(platformView);
        //}

        //private void OnGotFocus(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
        //{
        //    if (sender is TextBox tb)
        //    {
        //        tb.Background = new Microsoft.UI.Xaml.Media.SolidColorBrush(Microsoft.UI.Colors.Gray);
        //    }
        //}

        //private void OnLostFocus(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
        //{
        //    if (sender is TextBox tb)
        //    {
        //        tb.Background = new Microsoft.UI.Xaml.Media.SolidColorBrush(Microsoft.UI.Colors.White);
        //    }
        //}
    }
}

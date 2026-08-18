using CommunityToolkit.Maui;
using M_WMS.Services;
using M_WMS.ViewModel;
using Microsoft.Extensions.Logging;
using Microsoft.Maui.Handlers;
using M_WMS.Controls.Entries;
using M_WMS.ViewModel.ArrivalProcessViewModels;
using M_WMS.ViewModel.ShipmentProcessViewModels;
using ZXing.Net.Maui.Controls;
using M_WMS.Controls.Popups;
using M_WMS.Controls.Models;
using M_WMS.Services.Popups;
using M_WMS.Pages;



#if ANDROID
using Android.Graphics.Drawables;
using AndroidX.AppCompat.Widget;
#endif
namespace M_WMS
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .UseBarcodeReader()
                .UseMauiCommunityToolkit()   // thêm dòng này
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                    //fonts.AddFont("Fa-Solid-900.ttf", "FASolid");
                    fonts.AddFont("Font Awesome 7 Free-Solid-900.otf", "FA-Solid");
                })
                .ConfigureMauiHandlers(handlers =>
                {
#if WINDOWS
                    //handlers.AddHandler(typeof(Entry), typeof(M_WMS.Platforms.Windows.MyEntryHandler));
#endif

                    EditorHandler.Mapper.AppendToMapping("CustomEditor", (handler, view) =>
                    {
#if ANDROID
                        handler.PlatformView.SetPadding(6, 2, 6, 2);
                        //handler.PlatformView.Background = null;
                        handler.PlatformView.Background = new ColorDrawable(Android.Graphics.Color.Transparent);
#endif
//#if WINDOWS
//                        handler.PlatformView.Padding = new Microsoft.UI.Xaml.Thickness(6, 2, 6, 2);
//                        handler.PlatformView.BorderThickness = new Microsoft.UI.Xaml.Thickness(0);
//                        handler.PlatformView.BorderBrush = new Microsoft.UI.Xaml.Media.SolidColorBrush(Microsoft.UI.Colors.Transparent);
//                        handler.PlatformView.Background = null;
//#endif
                    });
                });

            // Bỏ background và padding mặc định của Native Entry
            Microsoft.Maui.Handlers.EntryHandler.Mapper.AppendToMapping("NoBorder", (handler, view) =>
            {
#if ANDROID
                handler.PlatformView.Gravity = Android.Views.GravityFlags.CenterVertical;
                handler.PlatformView.SetBackgroundResource(0); // Bỏ background mặc định
                handler.PlatformView.SetPadding(0, 0, 0, 0);   // Xóa padding mặc định
#elif IOS || MACCATALYST
                handler.PlatformView.BorderStyle = UIKit.UITextBorderStyle.None;
                handler.PlatformView.VerticalAlignment = UIKit.UIControlContentVerticalAlignment.Center;
//#elif WINDOWS
//                // Xóa viền trên Windows
//                handler.PlatformView.BorderThickness = new Microsoft.UI.Xaml.Thickness(0);
    
//                // Thu nhỏ Padding chữ bên trong (Trái, Trên, Phải, Dưới)
//                handler.PlatformView.Padding = new Microsoft.UI.Xaml.Thickness(0);
    
//                // Tùy chọn: Xóa màu nền xám mặc định khi focus/hover
//                //handler.PlatformView.Background = new Microsoft.UI.Xaml.Media.SolidColorBrush(Microsoft.UI.Colors.Transparent);
#endif
            });

            //EntryHandler.Mapper.AppendToMapping(nameof(Entry), (handler, view) =>
            //    {
            //        if (view is Entry)
            //        {
            //            handler.PlatformView.Background = null;
            //        }
            //    });
            //    PickerHandler.Mapper.AppendToMapping(nameof(Picker), (handler, view) =>
            //    {
            //        if (view is Picker)
            //        {
            //            handler.PlatformView.Background = null;
            //        }
            //    });
            //#if WINDOWS
            //            EntryHandler.Mapper.AppendToMapping("WmsEntry", (handler, view) =>
            //            {
            //                if (view is WmsEntry &&
            //                    handler.PlatformView is TextBox textBox)
            //                {
            //                    var transparentBrush =
            //                        new Microsoft.UI.Xaml.Media.SolidColorBrush(
            //                            Microsoft.UI.Colors.Transparent);

            //                    textBox.Resources["TextControlBackgroundFocused"] = transparentBrush;
            //                    textBox.Resources["TextControlBackground"] = transparentBrush;

            //                    // Placeholder color
            //                    textBox.Resources["TextControlPlaceholderForeground"] =
            //                        new Microsoft.UI.Xaml.Media.SolidColorBrush(
            //                            Microsoft.UI.Colors.Gray);
            //                }
            //            });
            //#endif
            //builder.Services.AddHttpClient("ApiCpos", client =>
            //{
            //    // Nếu chạy trên Android emulator thì dùng 10.0.2.2 thay cho localhost
            //    client.BaseAddress = new Uri("https://p01-wap01.azurewebsites.net/");
            //});
            //builder.Services.AddHttpClient("ApiErp", client =>
            //{
            //    // Nếu chạy trên Android emulator thì dùng 10.0.2.2 thay cho localhost
            //    client.BaseAddress = new Uri("http://10.0.2.2:44357/");
            //});
#if DEBUG
            builder.Logging.AddDebug();
#endif            
            builder.Services.AddSingleton<IPopupDialogService, PopupDialogService>();
            //builder.Services.AddTransientPopup<CustomPopup, CustomPopupViewModel>();
            builder.Services.AddTransient<ApiService>();
            // view model
            builder.Services.AddTransient<LoginViewModel>();
            //builder.Services.AddSingleton<HomeViewModel>();
            builder.Services.AddTransient<HomeViewModel>();
            builder.Services.AddTransient<SettingViewModel>();
            builder.Services.AddTransient<MainViewModel>();
            builder.Services.AddTransient<ArrivalProcessViewModel>();
            builder.Services.AddTransient<ArrivalProcessListViewModel>();
            builder.Services.AddTransient<ShipmentProcessListViewModel>();
            builder.Services.AddTransient<ShipmentProcessViewModel>();
            builder.Services.AddTransient<ReadFileExcelViewModel>();

            // page 
            builder.Services.AddTransient<HomeView>();
            builder.Services.AddTransient<SettingView>();
            return builder.Build();
        }
    }
}

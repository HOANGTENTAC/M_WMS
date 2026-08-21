using M_WMS.Controls.Popups;

namespace M_WMS
{
    public partial class App : Application
    {
        public static CustomPopupView GlobalPopup { get; private set; } = null!;
        public App()
        {
            InitializeComponent();
            GlobalPopup = new CustomPopupView();
        }

        protected override Window CreateWindow(IActivationState? activationState)
        {
            return new Window(new AppShell());
        }
    }
}
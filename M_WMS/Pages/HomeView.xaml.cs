using M_WMS.ViewModel;

namespace M_WMS.Pages;

public partial class HomeView : ContentView
{
    private readonly HomeViewModel _viewModel;
    private bool _isAnimating = false;
    public HomeView(HomeViewModel vm)
	{
		InitializeComponent();
        BindingContext = vm;
        _viewModel = vm;
    }
    protected override async void OnParentSet()
    {
        base.OnParentSet();
        await _viewModel.InitializeAsync();
        //if (Parent != null && BindingContext is HomeViewModel vm)
        //{
            //// Bắt đầu chạy Animation Skeleton
            //StartSkeletonAnimation();

            //// Delay 150ms để UI chuyển trang mượt rồi mới load dữ liệu
            //Dispatcher.DispatchDelayed(TimeSpan.FromMilliseconds(150), async () =>
            //{
            //    await vm.InitializeAsync();
            //    StopSkeletonAnimation();
            //});
        //}
    }
    //private void StartSkeletonAnimation()
    //{
    //    _isAnimating = true;

    //    // Tạo hiệu ứng thở (Pulse/Shimmer animation)
    //    Animation parentAnimation = new Animation();
    //    var fadeOut = new Animation(v => Sk1.Opacity = Sk2.Opacity = Sk3.Opacity = Sk4.Opacity = v, 1, 0.3, Easing.CubicInOut);
    //    var fadeIn = new Animation(v => Sk1.Opacity = Sk2.Opacity = Sk3.Opacity = Sk4.Opacity = v, 0.3, 1, Easing.CubicInOut);

    //    parentAnimation.Add(0, 0.5, fadeOut);
    //    parentAnimation.Add(0.5, 1, fadeIn);

    //    parentAnimation.Commit(this, "SkeletonPulse", 16, 1000, repeat: () => _isAnimating);
    //}

    //private void StopSkeletonAnimation()
    //{
    //    _isAnimating = false;
    //    this.AbortAnimation("SkeletonPulse");
    //}
}
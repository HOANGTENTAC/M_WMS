namespace M_WMS.Controls.Helpers
{
    public static class AnimationExtensions
    {
        public static Task AnimateAsync(
        this VisualElement view,
        double start,
        double end,
        Action<double> callback,
        uint length = 150)
        {
            var tcs = new TaskCompletionSource();

            var animation = new Animation(v => callback(v), start, end);

            animation.Commit(
                view,
                "HeightAnimation",
                16,
                length,
                Easing.CubicOut,
                (v, c) => tcs.SetResult());

            return tcs.Task;
        }
    }
}

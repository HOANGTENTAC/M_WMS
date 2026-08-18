namespace M_WMS.Controls.Extensions
{
    internal static class VisualElementExtensions
    {
        public static Point GetAbsolutePosition(this VisualElement view)
        {
            double x = view.X;
            double y = view.Y;

            Element? parent = view.Parent;

            while (parent != null)
            {
                if (parent is VisualElement visual)
                {
                    x += visual.X;
                    y += visual.Y;
                }

                parent = parent.Parent;
            }

            return new Point(x, y);
        }
        public static Rect GetAbsoluteBounds(this VisualElement view)
        {
            var p = view.GetAbsolutePosition();

            return new Rect(
                p.X,
                p.Y,
                view.Width,
                view.Height);
        }
        public static T? FindParent<T>(this Element element)
        where T : Element
        {
            Element? parent = element.Parent;

            while (parent != null)
            {
                if (parent is T result)
                    return result;

                parent = parent.Parent;
            }

            return null;
        }
        public static ContentPage? GetParentPage(
        this Element element)
        {
            return element.FindParent<ContentPage>();
        }
    }
}

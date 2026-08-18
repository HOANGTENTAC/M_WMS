namespace M_WMS.Controls
{
    public interface IWmsFocusable
    {
        bool IsControlFocused { get; }

        event EventHandler? Focused;

        event EventHandler? Unfocused;

        void RaiseFocused();

        void RaiseUnfocused();
    }
}

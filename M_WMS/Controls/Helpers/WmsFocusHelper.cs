namespace M_WMS.Controls.Helpers
{
    public static class WmsFocusHelper
    {
        public static bool ChangeFocus(
        bool currentState,
        bool newState,
        Action onFocused,
        Action onUnfocused)
        {
            if (currentState == newState)
                return currentState;

            if (newState)
                onFocused();
            else
                onUnfocused();

            return newState;
        }
    }
}

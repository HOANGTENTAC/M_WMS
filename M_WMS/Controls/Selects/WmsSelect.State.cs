namespace M_WMS.Controls.Selects
{
    public partial class WmsSelect
    {
        public enum WmsSelectState
        {
            Normal,
            Pressed,
            Disabled,
            ReadOnly
        }
        private void SetState(WmsSelectState state)
        {
            if (_currentState == state)
                return;

            _currentState = state;

            ApplyState();
        }

        private void ApplyState()
        {
            switch (_currentState)
            {
                case WmsSelectState.Normal:

                    PART_Grid.Background = BackgroundBrush;

                    Opacity = 1;

                    break;

                case WmsSelectState.Pressed:

                    PART_Grid.Background = PressedBackgroundBrush;

                    Opacity = 1;

                    break;

                case WmsSelectState.Disabled:

                    PART_Grid.Background = BackgroundBrush;

                    Opacity = 0.5;

                    break;

                case WmsSelectState.ReadOnly:

                    PART_Grid.Background = BackgroundBrush;

                    Opacity = 0.8;

                    break;
            }
        }
    }
}

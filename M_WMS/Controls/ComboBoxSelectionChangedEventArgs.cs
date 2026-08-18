namespace M_WMS.Controls
{
    public sealed class ComboBoxSelectionChangedEventArgs : EventArgs
    {
        public ComboBoxSelectionChangedEventArgs(
        object? previousItem,
        object? currentItem,
        int previousIndex,
        int currentIndex)
        {
            PreviousItem = previousItem;
            CurrentItem = currentItem;
            PreviousIndex = previousIndex;
            CurrentIndex = currentIndex;
        }

        /// <summary>
        /// Item trước khi thay đổi.
        /// </summary>
        public object? PreviousItem { get; }

        /// <summary>
        /// Item hiện tại.
        /// </summary>
        public object? CurrentItem { get; }

        /// <summary>
        /// Index trước khi thay đổi.
        /// </summary>
        public int PreviousIndex { get; }

        /// <summary>
        /// Index hiện tại.
        /// </summary>
        public int CurrentIndex { get; }
    }
}

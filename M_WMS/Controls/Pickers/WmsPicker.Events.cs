namespace M_WMS.Controls.Pickers
{
    public partial class WmsPicker
    {
        private void InitializeEvents()
        {
            PART_Picker.SelectedIndexChanged += OnPickerSelectedIndexChanged;
        }
        private void OnSelectedIndexChanged(object? sender, EventArgs e)
        {
            if (!Equals(SelectedItem, PART_Picker.SelectedItem))
            {
                SelectedItem = PART_Picker.SelectedItem;
            }

            if (SelectedIndex != PART_Picker.SelectedIndex)
            {
                SelectedIndex = PART_Picker.SelectedIndex;
            }
        }
        private void OnPickerSelectedIndexChanged(object? sender, EventArgs e)
        {
            if (!Equals(SelectedItem, PART_Picker.SelectedItem))
                SelectedItem = PART_Picker.SelectedItem;

            if (SelectedIndex != PART_Picker.SelectedIndex)
                SelectedIndex = PART_Picker.SelectedIndex;

            //UpdateDisplayText();
        }
    }
}

using System.Collections;
namespace M_WMS.Controls.Pickers
{
    public partial class WmsPicker
    {
        private void ApplyItemsSource()
        {
            PART_Picker.ItemsSource = ItemsSource;
        }
        private void ApplySelectedIndex()
        {
            if (PART_Picker.SelectedIndex != SelectedIndex)
                PART_Picker.SelectedIndex = SelectedIndex;
        }
        //private void ApplyPlaceholder()
        //{
        //    //PART_DisplayLabel.Margin = new Thickness(12, 0, 30, 0);
        //    //PART_DisplayLabel.Text = Placeholder;
        //    //PART_DisplayLabel.ZIndex = 1;
        //    //PART_DisplayLabel.InputTransparent = true;
        //    //UpdateDisplayText();
        //}
        private void ApplySelectedItem()
        {
            if (!Equals(PART_Picker.SelectedItem, SelectedItem))
            {
                PART_Picker.SelectedItem = SelectedItem;
            }

            //UpdateDisplayText();
        }
        private void ApplyItemDisplayBinding()
        {
            PART_Picker.ItemDisplayBinding = ItemDisplayBinding;
        }
        private void ApplyTextColor()
        {
            PART_Picker.TextColor = TextColor;
        }
        //private void UpdatePlaceholder()
        //{
        //    bool show = PART_Picker.SelectedItem == null;

        //    PART_DisplayLabel.Opacity = show ? 1 : 0;
        //}
        //private void UpdateDisplayText()
        //{
        //    if (SelectedItem == null)
        //    {
        //        PART_DisplayLabel.Text = Placeholder;
        //        PART_DisplayLabel.TextColor = Colors.Gray;
        //        return;
        //    }

        //    PART_DisplayLabel.Text = SelectedItem.ToString();
        //    PART_DisplayLabel.TextColor = TextColor;
        //}
    }
}

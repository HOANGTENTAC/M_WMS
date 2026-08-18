namespace M_WMS.Controls.Pickers
{
    public partial class WmsPicker
    {
        private void InitializeContent()
        {
            CreateControls();

            BuildLayout();

            Content = PART_Border;

            ApplyItemsSource();
            //ApplyPlaceholder();
            ApplyItemDisplayBinding();
            ApplySelectedIndex();
            ApplySelectedItem();
        }

        private void CreateControls()
        {
            PART_Border = new Border();

            PART_Grid = new Grid();

            PART_Picker = new Picker();

            PART_DropDownIcon = new Image();

            //PART_DisplayLabel = new Label
            //{
            //    VerticalOptions = LayoutOptions.Center,
            //    HorizontalOptions = LayoutOptions.Fill,
            //    Margin = new Thickness(12, 0, 30, 0),
            //    VerticalTextAlignment = TextAlignment.Center,
            //    LineBreakMode = LineBreakMode.TailTruncation,
            //    InputTransparent = true
            //};
        }

        private void BuildLayout()
        {
            //PART_Grid.Children.Add(PART_Picker);

            //PART_Grid.Children.Add(PART_DropDownIcon);


            //PART_Grid.Add(PART_PlaceholderLabel);

            PART_Grid.Add(PART_Picker);
            //PART_Grid.Add(PART_DisplayLabel);
            PART_Grid.Add(PART_DropDownIcon);

            PART_Border.Content = PART_Grid;
        }
    }
}

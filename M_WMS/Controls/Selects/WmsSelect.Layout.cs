namespace M_WMS.Controls.Selects
{
    public partial class WmsSelect
    {
        private void InitializeContent()
        {
            //CreateControls();

            //BuildLayout();

            //Content = PART_Border;
            //Content = PART_Grid;
        }

        private void CreateControls()
        {
            //PART_Border = new Border();

            PART_Grid = new Grid();

            PART_Display = new Label { 
                LineBreakMode = LineBreakMode.TailTruncation,
                VerticalOptions = LayoutOptions.Center,
                MaxLines = 2,
            };

            PART_Arrow = new Image();

            PART_LeadingIcon = new Image
            {
                Aspect = Aspect.AspectFill,
                IsVisible = false
            };

            PART_LeadingFontAwesomeIcon = new Label
            {
                FontFamily = "FA-Solid",
                HorizontalTextAlignment = TextAlignment.Center,
                VerticalTextAlignment = TextAlignment.Center,
                IsVisible = false
            };
        }

        private void BuildLayout()
        {
            PART_Grid.ColumnDefinitions.Add(new ColumnDefinition(GridLength.Auto));
            PART_Grid.ColumnDefinitions.Add(new ColumnDefinition(GridLength.Star));
            PART_Grid.ColumnDefinitions.Add(new ColumnDefinition(GridLength.Auto));

            PART_Grid.Add(PART_LeadingIcon, 0);
            PART_Grid.Add(PART_LeadingFontAwesomeIcon, 0);
            PART_Grid.Add(PART_Display, 1);
            PART_Grid.Add(PART_Arrow, 2);

            //PART_Border.Content = PART_Grid;
        }
        private void ApplySelectedItem()
        {
            UpdateSelectedIndex();
            UpdateDisplay();
        }

        private void ApplySelectedIndex()
        {
            if (ItemsSource == null)
                return;

            if (SelectedIndex < 0)
            {
                SelectedItem = null;

                UpdateDisplay();

                return;
            }

            int index = 0;

            foreach (var item in ItemsSource)
            {
                if (index == SelectedIndex)
                {
                    SelectedItem = item;

                    break;
                }

                index++;
            }
            UpdateDisplay();
        }
        private void ApplyDisplayMemberPath()
        {
        }
    }
}

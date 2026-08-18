namespace M_WMS.Controls.Pickers
{
    public partial class WmsPicker : ContentView
    {
        #region Controls

        private Border PART_Border = null!;
        private Grid PART_Grid = null!;
        private Picker PART_Picker = null!;
        private Image PART_DropDownIcon = null!;
        #endregion

        public WmsPicker()
        {
            InitializeControl();
        }

        private void InitializeControl()
        {
            InitializeContent();
            InitializeStyle();
            InitializeEvents();
        }
    }
}

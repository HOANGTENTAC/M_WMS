using CommunityToolkit.Maui.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace M_WMS.Controls.Selects
{
    public partial class WmsSelectPopup
    {
        partial void InitializePopup()
        {
            InitializeLayout();

            InitializeEvents();
        }

        private void InitializeLayout()
        {
            PART_Title.Text = Title;

            PART_SearchEntry.Placeholder = SearchPlaceholder;

            PART_ClearButton.IsVisible = false;
        }
        private void InitializeEvents()
        {
            PART_CollectionView.SelectionChanged += OnSelectionChanged;
            PART_SearchEntry.TextChanged += OnSearchTextChanged;
        }
    }
}

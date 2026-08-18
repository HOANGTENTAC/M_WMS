using M_WMS.Controls.Models;

namespace M_WMS.Controls.Selects
{
    internal class WmsSelectTemplateSelector : DataTemplateSelector
    {
        public DataTemplate? UserTemplate { get; set; }

        public DataTemplate DefaultTemplate { get; }

        public WmsSelectTemplateSelector()
        {
            DefaultTemplate = new DataTemplate(() =>
            {
                var label = new Label();

                label.SetBinding(Label.TextProperty, nameof(WmsSelectItem.Text));

                label.SetBinding(Label.TextColorProperty, nameof(WmsSelectItem.TextColor));

                label.SetBinding(Label.FontAttributesProperty, nameof(WmsSelectItem.FontAttributes));

                return label;
            });
        }

        protected override DataTemplate OnSelectTemplate(
            object item,
            BindableObject container)
        {
            return UserTemplate ?? DefaultTemplate;
        }
    }
}

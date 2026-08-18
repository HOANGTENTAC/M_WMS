using M_WMS.Controls.Models;

namespace M_WMS.Controls.Selects;

public partial class WmsSelectItemContainer : ContentView
{

    public WmsSelectItemContainer()
    {
        InitializeComponent();
    }

    protected override void OnBindingContextChanged()
    {
        base.OnBindingContextChanged();

        BuildContent();
    }

    private void BuildContent()
    {
        if (BindingContext is not WmsSelectItem item)
            return;

        if (item.Template != null)
        {
            var view = (View)item.Template.CreateContent();

            view.BindingContext = item.Value;

            PART_Content.Content = view;

            return;
        }

        PART_Content.Content = new Label
        {
            VerticalOptions = LayoutOptions.Center
        };

        ((Label)PART_Content.Content).SetBinding(
            Label.TextProperty,
            nameof(WmsSelectItem.Text));

        ((Label)PART_Content.Content).SetBinding(
            Label.TextColorProperty,
            nameof(WmsSelectItem.TextColor));

        ((Label)PART_Content.Content).SetBinding(
            Label.FontAttributesProperty,
            nameof(WmsSelectItem.FontAttributes));
    }
}
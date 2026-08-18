namespace M_WMS.Controls.Borders;

public partial class WmsBorder : ContentView
{
    private View? _pendingContent;
    public WmsBorder()
	{
		InitializeComponent();
        base.Content = PART_Root;
        UpdateBorder();
        SetNormal();
        //PART_ContentHost.BindingContext = this;
        if (_pendingContent != null)
        {
            PART_ContentHost.Content = _pendingContent;
            _pendingContent = null;
        }
    }
}
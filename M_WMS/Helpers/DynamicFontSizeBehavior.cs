using System.ComponentModel;

namespace M_WMS.Helpers
{
    public class DynamicFontSizeBehavior : Behavior<Label>
    {
        public double MaxWidth { get; set; } = 200; // Chiều rộng tối đa của vùng chứa
        public double DefaultFontSize { get; set; } = 24; // Kích thước chữ gốc mong muốn

        protected override void OnAttachedTo(Label bindable)
        {
            base.OnAttachedTo(bindable);
            bindable.SizeChanged += OnLabelSizeChanged;
            bindable.PropertyChanged += OnLabelPropertyChanged; // Dùng PropertyChanged thay cho TextChanged
        }

        protected override void OnDetachingFrom(Label bindable)
        {
            base.OnDetachingFrom(bindable);
            bindable.SizeChanged -= OnLabelSizeChanged;
            bindable.PropertyChanged -= OnLabelPropertyChanged;
        }

        private void OnLabelPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            // Chỉ tính toán lại khi thuộc tính "Text" của Label thực sự thay đổi
            if (e.PropertyName == Label.TextProperty.PropertyName)
            {
                AdjustFontSize(sender as Label);
            }
        }

        private void OnLabelSizeChanged(object sender, EventArgs e)
        {
            AdjustFontSize(sender as Label);
        }

        private void AdjustFontSize(Label label)
        {
            if (label == null || string.IsNullOrEmpty(label.Text)) return;

            // Tạm thời ngắt sự kiện để tránh vòng lặp vô hạn khi thay đổi FontSize
            label.PropertyChanged -= OnLabelPropertyChanged;

            // Đặt lại font size về mặc định trước khi tính toán co giãn
            label.FontSize = DefaultFontSize;

            // Ước tính chiều rộng của chữ (hệ số 0.55 dựa trên font mặc định hệ điều hành)
            double estimatedWidth = label.Text.Length * (label.FontSize * 0.55);

            // Vòng lặp giảm kích thước chữ nếu vượt quá chiều rộng cho phép
            while (estimatedWidth > MaxWidth && label.FontSize > 9)
            {
                label.FontSize -= 1;
                estimatedWidth = label.Text.Length * (label.FontSize * 0.55);
            }

            // Kích hoạt lại sự kiện sau khi tính toán xong
            label.PropertyChanged += OnLabelPropertyChanged;
        }
    }
}

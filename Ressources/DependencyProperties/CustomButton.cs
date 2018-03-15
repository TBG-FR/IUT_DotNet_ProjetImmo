using System.Windows;
using System.Windows.Media;

namespace DependencyProperties
{
    public class CustomButton : System.Windows.Controls.Button
    {
        //public Brush InBorderBrush { get; set; }
        //public Thickness InBorderThickness { get; set; }

        public static readonly DependencyProperty InBorderBrushProperty =
            DependencyProperty.Register("InBorderBrush",
                                        typeof(Brush),
                                        typeof(CustomButton),
                                        new PropertyMetadata(new SolidColorBrush(Colors.White)));
        public Brush InBorderBrush
        {
            get
            {
                return (Brush)GetValue(InBorderBrushProperty);
            }
            set
            {
                SetValue(InBorderBrushProperty, value);
            }
        }

        public static readonly DependencyProperty InBorderThicknessProperty =
            DependencyProperty.Register("InBorderThickness",
                                        typeof(Thickness),
                                        typeof(CustomButton));
        public Thickness InBorderThickness
        {
            get
            {
                return (Thickness)GetValue(InBorderThicknessProperty);
            }
            set
            {
                SetValue(InBorderThicknessProperty, value);
            }
        }


    }
}

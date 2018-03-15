using System.Windows;
using System.Windows.Media;

namespace DependencyProperties
{
    public class Button
    {

        public static readonly DependencyProperty InBorderBrushProperty =
            DependencyProperty.RegisterAttached("InBorderBrush",
                                                typeof(Brush),
                                                typeof(Button));
        public static Brush GetInBorderBrush(DependencyObject target)
        {
            return (Brush)target.GetValue(InBorderBrushProperty);
        }
        public static void SetInBorderBrush(DependencyObject target, Brush value)
        {
            target.SetValue(InBorderBrushProperty, value);
        }

        public static readonly DependencyProperty InBorderThicknessProperty =
            DependencyProperty.RegisterAttached("InBorderThickness",
                                                typeof(Thickness),
                                                typeof(Button));
        public static Thickness GetInBorderThickness(DependencyObject target)
        {
            return (Thickness)target.GetValue(InBorderThicknessProperty);
        }
        public static void SetInBorderThickness(DependencyObject target, Thickness value)
        {
            target.SetValue(InBorderThicknessProperty, value);
        }

    }
}

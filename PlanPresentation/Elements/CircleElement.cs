using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace PlanPresentation.Elements
{
    public abstract class CircleElement
    {
        protected CircleElement(string content, Rect cellSize, Brush colorBrush)
        {
            var ellipseSize = new[] {cellSize.Height / 2.0, cellSize.Width / 2.0}.Min();

            var grid = new Grid();
            grid.Children.Add(new Border
            {
                BorderThickness = new Thickness(0.5),
                BorderBrush = new SolidColorBrush(Colors.LightSeaGreen)
            });
            grid.Children.Add(new Ellipse
            {
                Width = ellipseSize,
                Height = ellipseSize,
                Fill = colorBrush
            });
            grid.Children.Add(new TextBlock
            {
                Text = content,
                FontSize = ellipseSize / 2,
                VerticalAlignment = VerticalAlignment.Center,
                HorizontalAlignment = HorizontalAlignment.Center
            });

            Element = grid;
        }

        public Grid Element { get; }
    }
}
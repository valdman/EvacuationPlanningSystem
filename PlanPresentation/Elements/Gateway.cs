using System.Windows;
using System.Windows.Media;

namespace PlanPresentation.Elements
{
    public class Gateway : CircleElement
    {
        public Gateway(int gatewayCapasity, Rect cellSize) : base(gatewayCapasity.ToString(), cellSize,
            Brushes.LightGreen)
        {
        }
    }
}
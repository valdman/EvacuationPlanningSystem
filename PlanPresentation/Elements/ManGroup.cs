using System.Windows;
using System.Windows.Media;

namespace PlanPresentation.Elements
{
    public class ManGroup : CircleElement
    {
        public ManGroup(int capasity, Rect cellSize) : base(capasity.ToString(), cellSize, Brushes.Beige)
        {
        }
    }
}
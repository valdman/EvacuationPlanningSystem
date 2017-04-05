using System;
using System.Drawing;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using PlanService;
using PlanService.Entities;
using Color = System.Windows.Media.Color;
using Point = System.Windows.Point;
using Rectangle = System.Windows.Shapes.Rectangle;

namespace PlanPresentation
{
    public class PlanPresentor
    {
        private Canvas _viewWindow;
        public Plan _currentPlan { get; private set; }
        private readonly PlanGenerator _planGenerator;

        private Rect CellSize => new Rect{
            Height = _viewWindow.ActualHeight / _currentPlan.Height,
            Width = _viewWindow.ActualWidth / _currentPlan.Width
        };

        public PlanPresentor(ref Canvas viewWindow)
        {
            _viewWindow = viewWindow;
            _planGenerator = new PlanGenerator();
        }

        public void DrawPlan()
        {
            for (var x = 0; x < _currentPlan.Width; x++)
            {
                for (var y = 0; y < _currentPlan.Height; y++)
                {
                    var centerPoint = new Point(x * CellSize.Width + CellSize.Width / 2.0,
                        y * CellSize.Height + CellSize.Height / 2.0);
                    DrawCell(_currentPlan[x, y], centerPoint);
                }
            }
        }

        public void RegeneratePlan(int width, int heigth)
        {
            _currentPlan = _planGenerator.CreateRandomPlan(width, heigth);
            _viewWindow.Children.Clear();
        }

        public void DrawCell(Cell cell, Point centerPoint)
        {
            var myRect = new Border
            {
                Height = CellSize.Height,
                Width = CellSize.Width,
                BorderThickness = new Thickness(cell.CellState.HasFlag(CellState.Left) ? 1 : 0,
                                                cell.CellState.HasFlag(CellState.Top) ? 1 : 0,
                                                cell.CellState.HasFlag(CellState.Right) ? 1 : 0,
                                                cell.CellState.HasFlag(CellState.Bottom) ? 1 : 0),
                BorderBrush = new SolidColorBrush(Color.FromRgb(0, 0, 0))
            };

            Canvas.SetLeft(myRect, centerPoint.X - myRect.Width / 2);
            Canvas.SetTop(myRect, centerPoint.Y - myRect.Height / 2);


            _viewWindow.Children.Add(myRect);
        }
    }
}
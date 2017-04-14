using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Petzold.Media2D;
using PlanPresentation.Elements;
using PlanService.Entities;

namespace PlanPresentation
{
    public class Drawer
    {
        private readonly PlanPresentor _planPresentor;
        public Rect CellSize => new Rect
        {
            Height = _planPresentor.ViewWindow.ActualHeight / _planPresentor.CurrentPlan.Height,
            Width = _planPresentor.ViewWindow.ActualWidth / _planPresentor.CurrentPlan.Width
        };

        public Point CenterOfCell(int x, int y)
        {
            return new Point(x * CellSize.Width + CellSize.Width / 2.0,
                y * CellSize.Height + CellSize.Height / 2.0);
        }

        public Drawer(PlanPresentor planPresentor)
        {
            _planPresentor = planPresentor;
        }

        public void DrawPlan()
        {
            _planPresentor.ViewWindow.Children.Clear();
            for (var x = 0; x < (_planPresentor.CurrentPlan.Width); x++)
            for (var y = 0; y < (_planPresentor.CurrentPlan.Height); y++)
            {
                var centerPoint = CenterOfCell(x, y);
                DrawCell(_planPresentor.CurrentPlan[x, y], centerPoint);
            }
        }

        public void DrawGatesAndPeople()
        {
            DrawPlan();

            for (var x = 0; x < (_planPresentor.CurrentPlan.Width); x++)
            for (var y = 0; y < (_planPresentor.CurrentPlan.Height); y++)
            {
                var manNumberHere = _planPresentor.CurrentPlan[x, y].NumberOfManHere;
                var gateCapasityHere = _planPresentor.CurrentPlan[x, y].GateCapasity;

                if (gateCapasityHere != 0)
                {
                    var gatewayGrid = new Gateway(gateCapasityHere, CellSize).Element;
                    DrawCircleWithContent(gatewayGrid, x, y);
                }
                else if (manNumberHere != 0)
                {
                    var manGroup = new ManGroup(manNumberHere, CellSize).Element;
                    DrawCircleWithContent(manGroup, x, y);
                }
            }
        }
        
        public void DrawSolution()
        {
            var r = new Random();
            foreach (var way in _planPresentor?.CurrentSolution ?? Enumerable.Empty<Way>())
            {
                int i;
                var points = way.WayOut.Select(point => CenterOfCell(point.X, point.Y)).ToList();
                var count = points.Count;
                var wayColor = new SolidColorBrush(Color.FromRgb((byte)r.Next(1, 255),
                    (byte)r.Next(1, 255), (byte)r.Next(1, 233)));
                var arrowLength = new[] {CellSize.X, CellSize.Y}.Min();
                for (i = 0; i < count - 1; i++)
                {
                    var wayLine = new ArrowLine
                    {
                        Stroke = wayColor,
                        StrokeThickness = 2,
                        X1 = points[i].X,
                        Y1 = points[i].Y,
                        X2 = points[i + 1].X,
                        Y2 = points[i + 1].Y,
                        ArrowLength = 1,
                        Opacity = 0.5
                    };
                    _planPresentor.ViewWindow.Children.Add(wayLine);
                }
            }
        }

        private void DrawCircleWithContent(UIElement element, int x, int y)
        {
            var center = CenterOfCell(x, y);
            Canvas.SetLeft(element, center.X - CellSize.Width / 4);
            Canvas.SetTop(element, center.Y - CellSize.Height / 4);

            _planPresentor.ViewWindow.Children.Add(element);
        }

        private void DrawCell(Cell cell, Point centerPoint)
        {
            var cellToDraw = new Border
            {
                Height = CellSize.Height,
                Width = CellSize.Width,
                BorderThickness = new Thickness(cell.CellState.HasFlag(CellState.Left) ? 1 : 0,
                    cell.CellState.HasFlag(CellState.Top) ? 1 : 0,
                    cell.CellState.HasFlag(CellState.Right) ? 1 : 0,
                    cell.CellState.HasFlag(CellState.Bottom) ? 1 : 0),
                BorderBrush = new SolidColorBrush(Color.FromRgb(0, 0, 0))
            };

            Canvas.SetLeft(cellToDraw, centerPoint.X - cellToDraw.Width / 2);
            Canvas.SetTop(cellToDraw, centerPoint.Y - cellToDraw.Height / 2);


            _planPresentor.ViewWindow.Children.Add(cellToDraw);
        }
    }
}
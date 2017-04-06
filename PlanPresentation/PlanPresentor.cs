using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using PlanPresentation.Elements;
using PlanService;
using PlanService.Entities;
using Color = System.Windows.Media.Color;
using Point = System.Windows.Point;

namespace PlanPresentation
{
    public class PlanPresentor
    {
        private readonly Canvas _viewWindow;
        private Plan _currentPlan;
        private IList<Way> _currentSolution;
        private readonly PlanGenerator _planGenerator;
        private PlanResolver _planResolver;

        private Rect CellSize => new Rect{
            Height = _viewWindow.ActualHeight / _currentPlan.Height,
            Width = _viewWindow.ActualWidth / _currentPlan.Width
        };

        private Point CenterOfCell(int x, int y) => new Point(x * CellSize.Width + CellSize.Width / 2.0,
            y * CellSize.Height + CellSize.Height / 2.0);

        public void DrawPlan()
        {
            _viewWindow.Children.Clear();
            for (var x = 0; x < (_currentPlan?.Width ?? 0); x++)
            {
                for (var y = 0; y < (_currentPlan?.Height ?? 0); y++)
                {
                    var centerPoint = CenterOfCell(x, y);
                    DrawCell(_currentPlan[x, y], centerPoint);
                }
            }
        }

        public void DrawGatesAndPeople()
        {
            DrawPlan();

            for (var x = 0; x < (_currentPlan?.Width ?? 0); x++)
            {
                for (var y = 0; y < (_currentPlan?.Height ?? 0); y++)
                {
                    var manNumberHere = _currentPlan[x, y].NumberOfManHere;
                    var gateCapasityHere = _currentPlan[x, y].GateCapasity;

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


        }

        public void RegeneratePlan(int width, int heigth)
        {
            _currentPlan = _planGenerator.CreateRandomPlan(width, heigth);
            _planResolver = new PlanResolver(_currentPlan);
        }

        public void RunRandomSimulation(IEnumerable<int> gatesCapasities, int peopleNumber)
        {
            _planGenerator
                .LocateGatesAndPeopleRandom(_currentPlan, gatesCapasities, peopleNumber);
            DrawGatesAndPeople();
            ResolveNewPlan();
        }
        
        public PlanPresentor(ref Canvas viewWindow)
        {
            _viewWindow = viewWindow;
            _planGenerator = new PlanGenerator();
        }

        private void DrawCircleWithContent(UIElement element, int x, int y)
        {
            var center = CenterOfCell(x, y);
            Canvas.SetLeft(element, center.X - CellSize.Width / 4);
            Canvas.SetTop(element, center.Y - CellSize.Height / 4);

            _viewWindow.Children.Add(element);
        }

        private void DrawCell(PlanService.Entities.Cell cell, Point centerPoint)
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


            _viewWindow.Children.Add(cellToDraw);
        }

        public void DrawSolution()
        {
            throw new NotImplementedException();
        }

        private void ResolveNewPlan()
        {
            _currentSolution = new List<Way>();
            for (var x = 0; x < (_currentPlan?.Width ?? 0); x++)
            {
                for (var y = 0; y < (_currentPlan?.Height ?? 0); y++)
                {
                    var cell = _currentPlan[x, y];
                    if (cell.NumberOfManHere <= 0) continue;

                    var beginPoint = new System.Drawing.Point(x, y);
                    foreach (var solution in _planResolver.FindGateway(beginPoint))
                    {
                        _currentSolution.Add(solution);
                    }
                }
            }

            _currentSolution.ToList()
                .ForEach(solution => Debug.WriteLine(
                    $"{solution.PeopleOnWay}: {string.Join(", ", solution.WayOut.ToList().Select(point => $"[x={point.X}; y={point.Y}"))}]"));
        }
    }
}
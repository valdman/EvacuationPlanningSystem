using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using PlanService.Entities;

namespace PlanService
{
    public class PlanGenerator
    {
        public Plan CreateRandomPlan(int width, int height)
        {
            var plan = new Plan(width, height);
            
            VisitCell(plan, _randomGenerator.Next(width), _randomGenerator.Next(height));
            plan.Display();
            return plan;
        }


        private IEnumerable<RemoveWallAction> GetNeighboursOfCell(Plan plan, Point cellCoords)
        {
            if (cellCoords.X > 0) yield return new RemoveWallAction { Neighbour = new Point(cellCoords.X - 1, cellCoords.Y), Wall = CellState.Left };
            if (cellCoords.Y > 0) yield return new RemoveWallAction { Neighbour = new Point(cellCoords.X, cellCoords.Y - 1), Wall = CellState.Top };
            if (cellCoords.X < plan.Width - 1) yield return new RemoveWallAction { Neighbour = new Point(cellCoords.X + 1, cellCoords.Y), Wall = CellState.Right };
            if (cellCoords.Y < plan.Height - 1) yield return new RemoveWallAction { Neighbour = new Point(cellCoords.X, cellCoords.Y + 1), Wall = CellState.Bottom };
        }

        private void VisitCell(Plan plan, int x, int y)
        {
            plan[x, y].CellState |= CellState.Visited;
            foreach (var p in GetNeighboursOfCell(plan, new Point(x, y))
                    .Shuffle(_randomGenerator)
                    .Where(z => !plan[z.Neighbour.X, z.Neighbour.Y]
                    .CellState.HasFlag(CellState.Visited)))
            {
                plan[x, y].CellState -= p.Wall;
                plan[p.Neighbour.X, p.Neighbour.Y].CellState -= p.Wall.OppositeWall();
                VisitCell(plan, p.Neighbour.X, p.Neighbour.Y);
            }
        }

        private readonly Random _randomGenerator = new Random();

        private struct RemoveWallAction
        {
            public Point Neighbour;
            public CellState Wall;
        }
    }
}
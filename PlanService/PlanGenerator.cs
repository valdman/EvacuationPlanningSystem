using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using PlanService.Entities;

namespace PlanService
{
    public class PlanGenerator
    {
        private readonly Random _randomGenerator = new Random();

        public Plan CreateRandomPlan(int width, int height)
        {
            var plan = new Plan(width, height);

            VisitCell(plan, _randomGenerator.Next(width), _randomGenerator.Next(height));
            return plan;
        }

        public Plan LocateGatesAndPeopleRandom(Plan plan, IEnumerable<int> gatesCapasities, int peopleNumber)
        {
            foreach (var cell in plan)
            {
                cell.GateCapasity = 0;
                cell.NumberOfManHere = 0;
            }

            var randomIndexGenerator = new Random();
            for (var i = 0; i < peopleNumber; i++)
            {
                var x = randomIndexGenerator.Next(plan.Width);
                var y = randomIndexGenerator.Next(plan.Height);
                plan[x, y].NumberOfManHere++;
            }

            foreach (var gatesCapasity in gatesCapasities)
            {
                var x = randomIndexGenerator.Next(plan.Width);
                var y = randomIndexGenerator.Next(plan.Height);
                plan[x, y].GateCapasity += gatesCapasity;
            }

            return plan;
        }

        private IEnumerable<RemoveWallAction> GetNeighboursOfCell(Plan plan, Point cellCoords)
        {
            if (cellCoords.X > 0)
                yield return new RemoveWallAction
                {
                    Neighbour = new Point(cellCoords.X - 1, cellCoords.Y),
                    Wall = CellState.Left
                };
            if (cellCoords.Y > 0)
                yield return new RemoveWallAction
                {
                    Neighbour = new Point(cellCoords.X, cellCoords.Y - 1),
                    Wall = CellState.Top
                };
            if (cellCoords.X < plan.Width - 1)
                yield return new RemoveWallAction
                {
                    Neighbour = new Point(cellCoords.X + 1, cellCoords.Y),
                    Wall = CellState.Right
                };
            if (cellCoords.Y < plan.Height - 1)
                yield return new RemoveWallAction
                {
                    Neighbour = new Point(cellCoords.X, cellCoords.Y + 1),
                    Wall = CellState.Bottom
                };
        }

        private void VisitCell(Plan plan, Point beginPoint)
        {
            plan[beginPoint].CellState |= CellState.Visited;
            foreach (var p in GetNeighboursOfCell(plan, beginPoint)
                .Shuffle(_randomGenerator)
                .Where(z => !plan[z.Neighbour.X, z.Neighbour.Y]
                    .CellState.HasFlag(CellState.Visited)))
            {
                plan[beginPoint].CellState -= p.Wall;
                plan[p.Neighbour.X, p.Neighbour.Y].CellState -= p.Wall.OppositeWall();
                VisitCell(plan, p.Neighbour);
            }
        }

        private struct RemoveWallAction
        {
            public Point Neighbour;
            public CellState Wall;
        }
    }
}
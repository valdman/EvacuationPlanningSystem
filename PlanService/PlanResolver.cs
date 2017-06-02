using System;
using System.Collections.Generic;
using System.Drawing;
using PlanService.Entities;

namespace PlanService
{
    public class PlanResolver
    {
        public IEnumerable<Way> CalculateSinkWeigth(Plan plan, Point beginPoint)
        {
            var commutationTable = new List<>>();
            for (var x = 0; x < plan.Width; x++)
            {
                for (var y = 0; y < plan.Height; y++)
                {
                    ifplan[x, y]
                }
            }
            var ways = GetWaysToAllGatesFromPoint(plan, beginPoint);

            return ways;
        }

        private List<Way> GetWaysToAllGatesFromPoint(Plan plan, Point beginPoint)
        {

            foreach (var cell in plan)
                cell.CellState &= ~CellState.Visited;

            var ways = new List<Way>();
            var backtrack = new Dictionary<Point, Point>();
            var queue = new Queue<Point>();

            queue.Enqueue(beginPoint);

            while (queue.Count != 0)
            {
                var here = queue.Dequeue();
                plan[here].CellState |= CellState.Visited;

                var gateCapasityHere = plan[here].GateCapasity;

                if (gateCapasityHere > 0)
                {
                    var wayToAnotherGate = GetWayByBacktrack(backtrack, here, beginPoint);
                    wayToAnotherGate.PeopleOnWay = gateCapasityHere;
                    ways.Add(wayToAnotherGate);
                }

                var neighbours = GetNeighbours(plan, here);
                foreach (var neighbour in neighbours)
                {
                    backtrack.Add(neighbour, here);
                    queue.Enqueue(neighbour);
                }
            }


            foreach (var cell in plan)
                cell.CellState &= ~CellState.Visited;

            return ways;
        }

        private Way GetWayByBacktrack(Dictionary<Point, Point> backtrack, Point endPoint, Point beginPoint)
        {
            var way = new Way();
            var here = endPoint;
            while (here != beginPoint)
            {
                way.WayOut.Add(here);
                here = backtrack[here];
            }
            way.WayOut.Add(beginPoint);
            way.WayOut.Reverse();
            return way;
        }

        public IEnumerable<Point> GetNeighbours(Plan plan, Point cellToGetNeighbours)
        {
            var cellState = plan[cellToGetNeighbours].CellState;
            if (!cellState.HasFlag(CellState.Left))
            {
                var leftCell = new Point(cellToGetNeighbours.X - 1, cellToGetNeighbours.Y);
                if (!plan[leftCell].CellState.HasFlag(CellState.Visited))
                    yield return leftCell;
            }

            if (!cellState.HasFlag(CellState.Top))
            {
                var topCell = new Point(cellToGetNeighbours.X, cellToGetNeighbours.Y - 1);
                if (!plan[topCell].CellState.HasFlag(CellState.Visited))
                    yield return topCell;
            }
            if (!cellState.HasFlag(CellState.Right))
            {
                var rightCell = new Point(cellToGetNeighbours.X + 1, cellToGetNeighbours.Y);
                if (!plan[rightCell].CellState.HasFlag(CellState.Visited))
                    yield return rightCell;
            }
            if (!cellState.HasFlag(CellState.Bottom))
            {
                var bottomCell = new Point(cellToGetNeighbours.X, cellToGetNeighbours.Y + 1);
                if (!plan[bottomCell].CellState.HasFlag(CellState.Visited))
                    yield return bottomCell;
            }
        }
    }
}
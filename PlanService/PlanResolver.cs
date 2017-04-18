using System.Collections.Generic;
using System.Drawing;
using PlanService.Entities;

namespace PlanService
{
    public class PlanResolver
    {
        public IEnumerable<Way> CalculateSinkWeigth(Plan plan, Point beginPoint)
        {
            var planCopy = plan.DeepCopy();

            foreach (var cell in planCopy)
                cell.CellState &= ~CellState.Visited;

            var ways = new List<Way>();
            var backtrack = new Dictionary<Point, Point>();
            var queue = new Queue<Point>();

            queue.Enqueue(beginPoint);

            while (queue.Count != 0)
            {
                var here = queue.Dequeue();
                planCopy[here].CellState |= CellState.Visited;

                var gateCapasityHere = planCopy[here].GateCapasity;
                var manHere = planCopy[beginPoint].NumberOfManHere;

                if (gateCapasityHere > 0)
                {
                    if (manHere <= gateCapasityHere)
                    {
                        var endedWay = GetWayByBacktrack(backtrack, here, beginPoint, manHere);
                        endedWay.PeopleOnWay = planCopy[beginPoint].NumberOfManHere;
                        planCopy[beginPoint].NumberOfManHere -= manHere;
                        planCopy[here].GateCapasity -= manHere;
                        ways.Add(endedWay);
                        break;
                    }
                    var wayToAnotherGate = GetWayByBacktrack(backtrack, here, beginPoint, manHere);
                    wayToAnotherGate.PeopleOnWay = gateCapasityHere;
                    planCopy[beginPoint].NumberOfManHere -= gateCapasityHere;
                    ways.Add(wayToAnotherGate);
                }

                var neighbours = GetNeighbours(planCopy, here);
                foreach (var neighbour in neighbours)
                {
                    backtrack.Add(neighbour, here);
                    queue.Enqueue(neighbour);
                }
            }

            return ways;
        }

        private Way GetWayByBacktrack(Dictionary<Point, Point> backtrack, Point endPoint, Point beginPoint, int manHere)
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
            way.PeopleOnWay = manHere;
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
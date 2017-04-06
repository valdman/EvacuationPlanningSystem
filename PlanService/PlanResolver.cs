using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using PlanService.Entities;
using Point = System.Drawing.Point;

namespace PlanService
{
    public class PlanResolver
    {
        public PlanResolver(Plan plan)
        {
            _plan = plan;
        }

        public IEnumerable<Way> FindGateway(Point beginPoint)
        {
            foreach (var cell in _plan)
            {
                cell.CellState &= ~CellState.Visited;
            }

            var ways = new List<Way>();
            var backtrack = new Dictionary<Point, Point>();
            var queue = new Queue<Point>();

            queue.Enqueue(beginPoint);

            while (queue.Count != 0)
            {
                var here = queue.Dequeue();
                _plan[here].CellState |= CellState.Visited;

                var gateCapasityHere = _plan[here].GateCapasity;
                var manHere = _plan[beginPoint].NumberOfManHere;

                if (gateCapasityHere > 0)
                {
                    if (manHere <= gateCapasityHere)
                    {
                        var endedWay = GetWayByBacktrack(backtrack, here, beginPoint, manHere);
                        endedWay.PeopleOnWay = _plan[beginPoint].NumberOfManHere;
                        _plan[beginPoint].NumberOfManHere -= manHere;
                        _plan[here].GateCapasity -= manHere;
                        ways.Add(endedWay);
                        break;
                    }
                    var wayToAnotherGate = GetWayByBacktrack(backtrack, here, beginPoint, manHere);
                    wayToAnotherGate.PeopleOnWay = gateCapasityHere;
                    _plan[beginPoint].NumberOfManHere -= gateCapasityHere;
                    ways.Add(wayToAnotherGate);
                }

                var neighbours = GetNeighbours(here);
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


        public IEnumerable<Point> GetNeighbours(Point cellToGetNeighbours)
        {
            var cellState = _plan[cellToGetNeighbours].CellState;
            if (!cellState.HasFlag(CellState.Left))
            {
                var leftCell = new Point(cellToGetNeighbours.X - 1, cellToGetNeighbours.Y);
                if(!_plan[leftCell].CellState.HasFlag(CellState.Visited))
                    yield return leftCell;
            }

            if (!cellState.HasFlag(CellState.Top))
            {
                var topCell = new Point(cellToGetNeighbours.X, cellToGetNeighbours.Y - 1);
                if (!_plan[topCell].CellState.HasFlag(CellState.Visited))
                    yield return topCell;
            }
            if (!cellState.HasFlag(CellState.Right))
            {
                var rightCell = new Point(cellToGetNeighbours.X + 1, cellToGetNeighbours.Y);
                if (!_plan[rightCell].CellState.HasFlag(CellState.Visited))
                    yield return rightCell;
            }
            if (!cellState.HasFlag(CellState.Bottom))
            {
                var bottomCell = new Point(cellToGetNeighbours.X , cellToGetNeighbours.Y + 1);
                if (!_plan[bottomCell].CellState.HasFlag(CellState.Visited))
                    yield return bottomCell;
            }
        }

        private Plan _plan;
    }
}
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using TransportNetService.Entities;

namespace TransportNetService
{
    internal class PotencialMethodOptimizer : IOptimizer
    {
        [Flags]
        public enum Direction
        {
            Bottom = 0,
            Right = 2,
            Left = 3,
            Top = 4
        }

        public TransportTable optimize(TransportTable table)
        {
            var workTable = new optimizingTable(table);


            while (!workTable.isOptimal())
                workTable = rebuildTable(workTable);
            workTable.updatePlan();
            return workTable;
        }


        private optimizingTable rebuildTable(optimizingTable table)
        {
            var cycle = moveToCycle(table.problemCell, table);
            var currentFlag = 1;


            var minMarkedValue = Math.Abs(cycle.Min(p => table.OptimizingPlan[p.X, p.Y].mark));

            foreach (var point in cycle)
            {
                table.OptimizingPlan[point.X, point.Y].Delivery +=
                    currentFlag * minMarkedValue;
                currentFlag *= -1;
            }

            return new optimizingTable(table);
        }

        private List<Point> moveToCycle(Point pointer, optimizingTable table)
        {
            var iterationsLimit = table.Sources.Length * table.Sinks.Length;
            var cycle = new List<Point>();

            if (horizontalCycleFound(table.problemCell, iterationsLimit, table, ref cycle))
                if (cycle.Count < 4)
                    throw new Exception("cycle cells count less than 4");
                else
                    return cycle;
            throw new Exception("cannot build cycle");
        }


        private bool horizontalCycleFound(Point pointer, int itherationsLimit, optimizingTable table,
            ref List<Point> cycle)
        {
            --itherationsLimit;
            if (itherationsLimit == 0)
                throw new StackOverflowException();
            for (var j = 0; j < table.Sinks.Length; ++j)
            {
                if (j == pointer.Y) continue;
                if (!table.basicCells.Exists(p => p.X == pointer.X && p.Y == j)) continue;
                if (verticalCycleFound(new Point(pointer.X, j), table, ref cycle, itherationsLimit))
                {
                    cycle.Add(new Point(pointer.X, j));
                    return true;
                }
            }
            return false;
        }

        private bool verticalCycleFound(Point pointer, optimizingTable table, ref List<Point> cycle,
            int itherationLimit)
        {
            for (var i = 0; i < table.Sources.Length; i++)
            {
                if (table.problemCell.X == pointer.X && table.problemCell.Y == pointer.Y)
                {
                    cycle.Add(new Point(i, pointer.Y));
                    return true;
                }
                if (i == pointer.X) continue;
                if (!table.basicCells.Exists(p => p.Y == pointer.Y && p.X == i)) continue;
                if (horizontalCycleFound(new Point(i, pointer.Y), itherationLimit, table, ref cycle))
                {
                    cycle.Add(new Point(i, pointer.Y));
                    return true;
                }
            }

            return false;
        }


        private class optimizingCell : PlanElement
        {
            public bool visited;
            public int mark { get; set; }

            public int sign { get; set; }
        }

        private class optimizingTable : TransportTable
        {
            public readonly List<Point> basicCells;

            public readonly optimizingCell[,] OptimizingPlan;
            public Point problemCell;

            public optimizingTable(TransportTable toCopy) : base(toCopy)
            {
                OptimizingPlan = new optimizingCell[toCopy.Sources.Length, toCopy.Sinks.Length];
                for (var i = 0; i < toCopy.Sources.Length; i++)
                for (var j = 0; j < toCopy.Sinks.Length; j++)
                {
                    var currentCell = toCopy.Plan[i, j];
                    var newCell = new optimizingCell {Cost = currentCell.Cost, Delivery = currentCell.Delivery};
                    OptimizingPlan[i, j] = newCell;
                }
                basicCells = findBasicCells();
                while (isDegeneracy())
                    addRandomBasicCell();
                calculatePotencials();
            }

            private bool isDegeneracy()
            {
                return basicCells.Count() != Sources.Length + Sinks.Length - 1;
            }

            private void addRandomBasicCell()
            {
                var i = 0;
                var j = 0;
                var randomGenerator = new Random();
                while (basicCells.Exists(cell => cell.X == i && cell.Y == j))
                {
                    i = randomGenerator.Next(0, Sources.Length);
                    j = randomGenerator.Next(0, Sinks.Length);
                }
                basicCells.Add(new Point(i, j));
            }

            private List<Point> findBasicCells()
            {
                var _basicCells = new List<Point>();

                for (var i = 0; i < Sources.Length; i++)
                for (var j = 0; j < Sinks.Length; j++)
                    if (Plan[i, j].Delivery != 0)
                    {
                        _basicCells.Add(new Point(i, j));
                        OptimizingPlan[i, j].mark = 0;
                    }

                return _basicCells;
            }

            private void calculatePotencials()
            {
                Sources[0].Potencial = 0;

                var basicArr = basicCells.ToArray();
                for (var i = 0; i < basicArr.Length; i++)
                {
                    Sinks[basicArr[i].Y].Potencial = Plan[basicArr[i].X, basicArr[i].Y].Cost -
                                                     Sources[basicArr[i].X].Potencial;

                    Sources[basicArr[i].X].Potencial = Plan[basicArr[i].X, basicArr[i].Y].Cost -
                                                       Sinks[basicArr[i].Y].Potencial;
                }
            }

            private void setMarks()
            {
                var minMarkCell = new Point();
                var minMark = int.MaxValue;
                for (var i = 0; i < Sources.Length; i++)
                for (var j = 0; j < Sinks.Length; j++)
                    if (basicCells.Exists(cell => cell.X == i && cell.Y == j))
                    {
                        OptimizingPlan[i, j].mark = OptimizingPlan[i, j].Cost - Sources[i].Potencial -
                                                    Sinks[j].Potencial;
                        if (OptimizingPlan[i, j].mark < minMark)
                        {
                            minMarkCell.X = i;
                            minMarkCell.Y = j;
                        }
                    }
                if (minMark != int.MaxValue)
                    problemCell = minMarkCell;
            }

            public bool isOptimal()
            {
                setMarks();
                return problemCell.IsEmpty;
            }

            public void updatePlan()
            {
                Plan = OptimizingPlan;
            }
        }
    }
}
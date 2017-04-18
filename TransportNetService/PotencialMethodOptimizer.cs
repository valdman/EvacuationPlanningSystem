using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TransportNetService
{
    class PotencialMethodOptimizer : IOptimizer
    {
        public TransportTable optimize(TransportTable table)
        {
            List<Point> basicCells = _findBasicCells(table);
            while (_isDegeneracy(table, basicCells))
            {
                _addRandomBasicCell(ref basicCells, table);
            }

            return _calculatePotencials(table, basicCells);
        }

        private List<Point> _findBasicCells(TransportTable table)
        {
            List<Point> basicCells = new List<Point>();

            for (int i = 0; i < table.Sources.Length; i++)
            {
                for (int j = 0; j < table.Sinks.Length; j++)
                {
                    if (table.Plan[i, j].Delivery != 0)
                    {
                        basicCells.Add(new Point(i, j));
                    }
                }
            }

            return basicCells;
        }

        private bool _isDegeneracy(TransportTable table, IEnumerable<Point> basicCells)
        {
            return basicCells.Count() != (table.Sources.Length + table.Sinks.Length - 1);
        }

        private void _addRandomBasicCell(ref List<Point> basicCells, TransportTable table)
        {
            var i = 0;
            var j = 0;
            var randomGenerator = new Random();
            while (basicCells.Exists(cell => cell.X == i && cell.Y == j))
            {
                i = randomGenerator.Next(0, table.Sources.Length);
                j = randomGenerator.Next(0, table.Sinks.Length);
            }
            basicCells.Add(new Point(i, j));
        }

        private TransportTable _calculatePotencials(TransportTable table, IEnumerable<Point> basicCells)
        {
            table.Sources[0].Potencial = 0;

            var basicArr = basicCells.ToArray();
            for (int i = 0; i < basicArr.Length; i++)
            {
                table.Sinks[basicArr[i].Y].Potencial = table.Plan[basicArr[i].X, basicArr[i].Y].Cost -
                                                       table.Sources[basicArr[i].X].Potencial;
                table.Sources[basicArr[i].Y].Potencial = table.Plan[basicArr[i].X, basicArr[i].Y].Cost -
                                                       table.Sinks[basicArr[i].X].Potencial;
            }
            return table;

        }

       
    }
}

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TransportNetService
{
    class PotencialMethodOptimizer
    {

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

        private void _addRandomBasicCell(ref IEnumerable<Point> IbasicCells, TransportTable table)
        {
            var i = 0;
            var j = 0;
            var randomGenerator = new Random();
            var basicCells = IbasicCells.ToList();
            while (basicCells.Exists(cell => cell.X == i && cell.Y == j))
            {
                i = randomGenerator.Next(0, table.Sources.Length);
                j = randomGenerator.Next(0, table.Sinks.Length);
            }
            basicCells.Add(new Point(i, j));
        }

        private void _calculatePotencials(TransportTable table, IEnumerable<Point> basicCells)
        {

        }

        private TransportTable table;
    }
}

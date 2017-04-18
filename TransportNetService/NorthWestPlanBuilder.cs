using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TransportNetService
{
    class NorthWestPlanBuilder : IPlanBuilder
    {   

        public TransportTable Build(TransportTable table)
        {
            int col = 0;
            int row = 0;
            int sourcesLength = table.Sources.Length;
            int sinksLength = table.Sinks.Length;

            while (col < sourcesLength && row < sinksLength)
            {
                try
                {
                    if (table.Sources[col].Value == 0)
                    {
                        col++;
                    }
                    if (table.Sinks[row].Value == 0)
                    {
                        row++;
                    }
                    table.Plan[col, row].Delivery = Math.Min(table.Sources[col].Value, table.Sinks[row].Value);
                    table.Sources[col].Value -= table.Plan[col, row].Delivery;
                    table.Sinks[row].Value -= table.Plan[col, row].Delivery;
                }
                catch (IndexOutOfRangeException)
                {
                    break;
                }

            }

            return table;
        }
    }
}

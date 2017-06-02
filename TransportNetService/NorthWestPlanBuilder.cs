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
                    if (table.Sources[col].Value == 0 && (col + 1) < sourcesLength)
                    {
                        col++;
                    }
                    if (table.Sinks[row].Value == 0 && (row + 1) < sinksLength)
                    {
                        row++;
                    }
                    if (table.Sinks[row].Value == 0 && table.Sources[col].Value == 0)
                    {
                        col++;
                        row++;
                    }

                    var thisDelivery = Math.Min(table.Sources[col].Value, table.Sinks[row].Value);
                    table.Plan[col, row].Delivery = thisDelivery;
                    table.Sources[col].Value -= thisDelivery;
                    table.Sinks[row].Value -= thisDelivery;

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

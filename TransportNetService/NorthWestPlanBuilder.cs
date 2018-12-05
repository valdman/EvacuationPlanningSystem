using System;

namespace TransportNetService
{
    internal class NorthWestPlanBuilder : IPlanBuilder
    {
        public TransportTable Build(TransportTable table)
        {
            var col = 0;
            var row = 0;
            var sourcesLength = table.Sources.Length;
            var sinksLength = table.Sinks.Length;

            while (col < sourcesLength && row < sinksLength)
                try
                {
                    if (table.Sources[col].Value == 0 && col + 1 < sourcesLength)
                        col++;
                    if (table.Sinks[row].Value == 0 && row + 1 < sinksLength)
                        row++;
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

            return table;
        }
    }
}
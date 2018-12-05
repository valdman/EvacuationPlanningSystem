using System;
using System.Linq;
using TransportNetService.Application;
using TransportNetService.Entities;

namespace TransportNetService
{
    public class TransportNetResolver : ITransportNetResolver
    {
        private readonly IOptimizer optimizer;
        private readonly IPlanBuilder planBuilder;


        private readonly TransportTable resultTable;


        public TransportNetResolver(TransportTable table)
        {
            optimizer = new PotencialMethodOptimizer();
            planBuilder = new NorthWestPlanBuilder();

            var rawTable = _isSolvable(table) ? table : _toClosed(table);

            resultTable = optimizer.optimize(planBuilder.Build(rawTable));
        }


        public TransportTable GetResultTable()
        {
            return resultTable;
        }

        private bool _isSolvable(TransportTable table)
        {
            var SourcesValue = table.Sources.Sum(node => node.Value);
            var SinksValue = table.Sinks.Sum(node => node.Value);

            return SourcesValue == SinksValue;
        }

        private TransportTable _toClosed(TransportTable table)
        {
            var sources = table.Sources.ToList();
            var sinks = table.Sinks.ToList();
            int[,] costs;


            var difference = sources.Sum(node => node.Value) - sinks.Sum(node => node.Value);
            var fictionNode = new Node(-1, Math.Abs(difference));
            if (difference < 0)
                sources.Add(fictionNode);
            else
                sinks.Add(fictionNode);

            costs = new int[sources.Count, sinks.Count];

            for (var i = 0; i < sources.Count; i++)
            for (var j = 0; j < sinks.Count; j++)
                costs[i, j] = i < table.Sources.Length && j < table.Sinks.Length ? table.Plan[i, j].Cost : 0;

            return new TransportTable(sources, sinks, costs);
        }
    }
}
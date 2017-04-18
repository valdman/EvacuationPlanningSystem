using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TransportNetService.Application;
using TransportNetService.Entities;

namespace TransportNetService
{
    public class TransportNetResolver : ITransportNetResolver
    {
       

        public TransportNetResolver(TransportTable table)
        {
            optimizer = new PotencialMethodOptimizer();
            planBuilder = new NorthWestPlanBuilder();

            TransportTable rawTable = _isSolvable(table) ? table : _toClosed(table);

            resultTable = optimizer.optimize(planBuilder.Build(rawTable));
        }

        public TransportNetResolver()
        {
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

                Node[] sources = table.Sources;
                Node[] sinks = table.Sinks;
                int[,] costs;

            
                var difference = sources.Sum(node => node.Value) - sinks.Sum(node => node.Value);
                Node fictionNode = new Node(-1, Math.Abs(difference));
                if (difference < 0)
                {
                    sinks.Concat(new Node[] { fictionNode });
                    
                }
                else
                {
                    sources.Concat(new Node[] { fictionNode });
                    
                }
                
                costs = new int[sources.Length, sinks.Length];

                for (int i = 0; i < sources.Length; i++)
                {
                    for (int j = 0; j < sinks.Length; j++)
                    {
                        costs[i, j] = (i < table.Sources.Length && j < table.Sinks.Length) ? table.Plan[i, j].Cost : 0;
                    }
                }

                return new TransportTable(sources, sinks, costs);
                
        }



        private TransportTable resultTable;
        private IOptimizer optimizer;
        private IPlanBuilder planBuilder;
        
    }

}

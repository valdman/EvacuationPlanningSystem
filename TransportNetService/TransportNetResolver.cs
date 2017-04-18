using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TransportNetService.Entities;

namespace TransportNetService
{
    class TransportNetResolver : ITransportNetResolver
    {
        public TransportTable GetTransportTable()
        {
            return resultTable;
        }

        public TransportNetResolver(TransportTable table)
        {
            if (_isSolvable(table))
            {
                this.rawTable = table;
            }
            else
            {
                this.rawTable = _toClosed(table);
            }
        }

               
        private bool _isSolvable(TransportTable table)
        {
            var SourcesValue = table.Sources.Sum(node => node.Value);
            var SinksValue = table.Sinks.Sum(node => node.Value);

            return SourcesValue == SinksValue;
        }

        private TransportTable _toClosed(TransportTable table)
        {

            this.sources = sources;
            this.sinks = sinks;

            if (!_isSolvable(sources, sinks))
            {
                var difference = sources.Sum(node => node.Value) - sinks.Sum(node => node.Value);
                Node fictionNode = new Node(-1, Math.Abs(difference));
                if (difference < 0)
                {
                    this.sinks.Concat(new Node[] { fictionNode });
                    plan = new PlanElement[this.sources.Length, this.sinks.Length];
                    for (int i = 0; i < this.sources.Length; i++)
                    {
                        for (int j = 0; j < this.sinks.Length - 1; j++)
                        {
                            plan[i, j] = new PlanElement()
                            {
                                Cost = costs[i, j]
                            };
                        }

                        plan[i, this.sinks.Length] = new PlanElement()
                        {
                            Cost = 0
                        };

                    }
                }
                else
                {
                    this.sources.Concat(new Node[] { fictionNode });
                    plan = new PlanElement[this.sources.Length, this.sinks.Length];
                    for (int i = 0; i < this.sources.Length - 1; i++)
                    {
                        for (int j = 0; j < this.sinks.Length; j++)
                        {
                            plan[i, j] = new PlanElement()
                            {
                                Cost = costs[i, j]
                            };
                            plan[this.sources.Length, j] = new PlanElement()
                            {
                                Cost = 0
                            };

                        }

                    }
                }
            }
            else
            {
                plan = new PlanElement[this.sources.Length, this.sinks.Length];
                for (int i = 0; i < this.sources.Length; i++)
                {
                    for (int j = 0; j < this.sinks.Length; j++)
                    {
                        plan[i, j] = new PlanElement()
                        {
                            Cost = costs[i, j]
                        };
                    }
                }
            }

        }



        private TransportTable rawTable;
        private TransportTable resultTable;
        private IOptimizer optimizer;
        private IPlanBuilder planBuilder;
    }

}

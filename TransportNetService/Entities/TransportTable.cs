using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TransportNetService.Entities;

namespace TransportNetService
{
    public class TransportTable
    {

        public TransportTable(IEnumerable<Node> sources, IEnumerable<Node> sinks, int[,] costs)
        {
            
            _createClosedTable(sources.ToArray(), sinks.ToArray(), costs);
           _createTransportPlan();

        }

        private bool _isSolvable(Node[] sources, Node[] sinks)
        {
            var SourcesValue = sources.Sum(node => node.Value);
            var SinksValue = sinks.Sum(node => node.Value);

            return SourcesValue == SinksValue;
        }


        private void _createClosedTable(Node[] sources, Node[] sinks, int[,] costs)
        {
            
            this.sources = sources;
            this.sinks = sinks;

            if (!_isSolvable(sources, sinks))
            {
                var difference = sources.Sum(node => node.Value) - sinks.Sum(node => node.Value);
                Node fictionNode = new Node(-1, Math.Abs(difference));
                if (difference < 0)
                {
                    this.sinks.Concat(new Node[] {fictionNode});
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
                    this.sources.Concat(new Node[] {fictionNode});
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

        private void _createTransportPlan()
        {
            int col = 0;
            int row = 0;
            int sourcesLength = sources.Length;
            int sinksLength = sinks.Length;


            while (col < sourcesLength && row < sinksLength)
            {

                   
                    plan[col, row].SetDelivery(sources[col].Value, sinks[row].Value);
                    sources[col].Value -= plan[col, row].Delivery;
                    sinks[row].Value -= plan[col, row].Delivery;
                    if (sources[col].Value == 0) { col++; }
                    if (sinks[row].Value == 0) { row++; }

            }

        }



        public Node[] Sources;
        public Node[] Sinks;
        public PlanElement[,] Plan;
    }
}

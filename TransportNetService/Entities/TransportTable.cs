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
            Sources = sources.ToArray();
            Sinks = sinks.ToArray();

            Plan = new PlanElement[Sources.Length, Sinks.Length];

            for (int i = 0; i < Sources.Length; i++)
            {
                for (int j = 0; j < Sinks.Length; j++)
                {
                    var planElement = new PlanElement();
                    planElement.Cost = costs[i, j];
                    Plan[i, j] = planElement;
                }
            }

        }

       
        public Node[] Sources;
        public Node[] Sinks;
        public PlanElement[,] Plan;
    }
}

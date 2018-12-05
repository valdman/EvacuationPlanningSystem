using System.Collections.Generic;
using System.Linq;
using TransportNetService.Entities;

namespace TransportNetService
{
    public class TransportTable
    {
        public PlanElement[,] Plan;
        public Node[] Sinks;

        public Node[] Sources;

        public TransportTable(IEnumerable<Node> sources, IEnumerable<Node> sinks, int[,] costs)
        {
            Sources = sources.ToArray();
            Sinks = sinks.ToArray();

            Plan = new PlanElement[Sources.Length, Sinks.Length];

            for (var i = 0; i < Sources.Length; i++)
            for (var j = 0; j < Sinks.Length; j++)
            {
                var planElement = new PlanElement();
                planElement.Cost = costs[i, j];
                planElement.Delivery = 0;
                Plan[i, j] = planElement;
            }
        }

        protected TransportTable(TransportTable toCopy)
        {
            Sources = toCopy.Sources;
            Sinks = toCopy.Sinks;
            Plan = toCopy.Plan;
        }

        public int ZFuncMinCost()
        {
            var cost = 0;
            for (var i = 0; i < Sources.Length; i++)
            for (var j = 0; j < Sinks.Length; j++)
            {
                var currentNode = Plan[i, j];
                cost += currentNode.Delivery * currentNode.Cost;
            }
            return cost;
        }
    }
}
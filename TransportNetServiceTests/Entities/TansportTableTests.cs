using Microsoft.VisualStudio.TestTools.UnitTesting;
using TransportNetService;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TransportNetService;
using TransportNetService.Application;
using TransportNetService.Entities;

namespace TransportNetService.Tests
{
    [TestClass()]
    public class TansportTableTests
    {
        private TransportTable _table;

        
        [TestMethod()]
        public void TansportTableTest()
        {
            //arrange
            Node[] sources = new Node[4]
            {
                new Node(1, 2), 
                new Node(2, 1), 
                new Node(3, 1), 
                new Node(4, 1)
            };
            Node[] sinks = new Node[4]
            {
                new Node(1, 1),
                new Node(2, 1),
                new Node(3, 2),
                new Node(4, 1)
            };
            int[,] costs = new int[sources.Length, sinks.Length];
            Random rand = new Random();
            for (int i = 0; i < sources.Length; i++)
            {
                for (int j = 0; j < sinks.Length; j++)
                {
                    costs[i, j] = rand.Next(1, 6);
                }
            }

            _table = new TransportTable(sources, sinks, costs);

            //act

            ITransportNetResolver reslolver = new TransportNetResolver(_table);

            _table = reslolver.GetResultTable();

            for (int i = 0; i < sources.Length; i++)
            {
                for (int j = 0; j < sinks.Length; j++)
                {
                    if (_table.Plan[i, j].Delivery != 0)
                    {

                        Debug.WriteLine($"humans{i} ({_table.Sources[i].Potencial}) -> exit{j} ({_table.Sinks[j].Potencial}) (({_table.Plan[i, j].Delivery}))");
                    }
                }
            }

            //assert

        }
    }
}
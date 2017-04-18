using Microsoft.VisualStudio.TestTools.UnitTesting;
using TransportNetService;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TransportNetService;
using TransportNetService.Entities;

namespace TransportNetService.Tests
{
    [TestClass()]
    public class TansportTableTests
    {
        private TansportTable _table;

        

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

            //act
            
            _table = new TansportTable(sources, sinks, costs);
            for (int i = 0; i < sources.Length; i++)
            {
                for (int j = 0; j < sinks.Length; j++)
                {
                    if(_table.Plan[i, j].Delivery != 0)
                    Debug.WriteLine($"humans{i} -> exit{j} ({_table.Plan[i, j].Delivery})");
                }
            }

            //assert

        }
    }
}
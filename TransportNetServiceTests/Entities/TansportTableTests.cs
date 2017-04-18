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
            Random rand = new Random();
            Node[] sources = new Node[rand.Next(3, 8)];
            Node[] sinks = new Node[rand.Next(3, 8)];

            for (int i = 0; i < sources.Length; i++)
            {
                sources[i] = new Node(i, rand.Next(1, 4));
            }

            for (int i = 0; i < sinks.Length; i++)
            {
                sinks[i] = new Node(i, rand.Next(1, 4));
            }

            int[,] costs = new int[sources.Length, sinks.Length];
           
            for (int i = 0; i < sources.Length; i++)
            {
                Debug.Write($"human{sources[i].Id} ({sources[i].Value}) ");
                Debug.WriteLine("");
                int j;

                for (j = 0; j < sinks.Length; j++)
                {
                    costs[i, j] = rand.Next(1, 6);
                    Debug.Write($"{costs[i, j]} for exit{sinks[j].Id} ({sinks[j].Value})");
                }
                Debug.WriteLine("");
            }

            _table = new TransportTable(sources, sinks, costs);

            //act

            ITransportNetResolver reslolver = new TransportNetResolver(_table);

            _table = reslolver.GetResultTable();

            Debug.WriteLine("");
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
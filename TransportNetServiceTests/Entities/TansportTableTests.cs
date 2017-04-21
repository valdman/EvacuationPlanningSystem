using Microsoft.VisualStudio.TestTools.UnitTesting;
using TransportNetService;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
                    Debug.Write($"{costs[i, j]} for exit{sinks[j].Id} ({sinks[j].Value} )");
                }
                Debug.WriteLine("");
            }

            _table = new TransportTable(sources, sinks, costs);

            //act

            ITransportNetResolver reslolver = new TransportNetResolver(_table);

            _table = reslolver.GetResultTable();

            Debug.WriteLine("");

            for (int i = 0; i < _table.Sources.Length; i++)
            {
                for (int j = 0; j < _table.Sinks.Length; j++)
                {
                    if (_table.Plan[i, j].Delivery != 0 && _table.Sinks[j].Id != -1 && _table.Sources[i].Id != -1)
                    {
                        Debug.WriteLine($"humans{i}  -> exit{j} (({_table.Plan[i, j].Delivery}))");

                    } else if (_table.Sinks[j].Id < 0 && _table.Plan[i, j].Delivery != 0)
                    {
                        Debug.WriteLine($"humans{i} проебались (({_table.Plan[i, j].Delivery}))");
                    }
                }
            }

            //assert

        }
    }
}
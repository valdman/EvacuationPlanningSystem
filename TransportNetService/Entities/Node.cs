using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TransportNetService.Entities
{
    public class Node
    {
        public Node(int id, int value)
        {
            Id = id;
            Value = value;
        }
        public int Id { get; private set; }
        public int Value { get; set; }
        public int Potencial;
    }
}

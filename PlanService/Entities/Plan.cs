using System;
using System.Diagnostics;
using System.Text;

namespace PlanService.Entities
{
    public class Plan
    {
        public Plan(int width, int height)
        {
            Width = width;
            Height = height;
            Cells = new Cell[width, height];
            for (var x = 0; x < width; x++)
            { 
                for (var y = 0; y < height; y++)
                {
                    Cells[x, y] = new Cell(CellState.Initial, 0);
                }
            }

        RandomGenerator = new Random();
        }

        public Cell this[int x, int y]
        {
            get { return Cells[x, y]; }
            set { Cells[x, y] = value; }
        }

        public string Display()
        {
            var result = new StringBuilder(string.Empty);
            var firstLine = string.Empty;
            for (var y = 0; y < Height; y++)
            {
                var sbTop = new StringBuilder();
                var sbMid = new StringBuilder();
                for (var x = 0; x < Width; x++)
                {
                    sbTop.Append(this[x, y].CellState.HasFlag(CellState.Top) ? "+--" : "+  ");
                    sbMid.Append(this[x, y].CellState.HasFlag(CellState.Left) ? "|  " : "   ");
                }
                if (firstLine == string.Empty)
                    firstLine = sbTop.ToString();
                result.AppendLine(sbTop + "+");
                result.AppendLine(sbMid + "|");
                result.AppendLine(sbMid + "|");
            }
            result.AppendLine(firstLine);
            Debug.WriteLine(result.ToString());
            return result.ToString();
        }

        public Cell[,] Cells { get; }
        public int Width { get; private set; }
        public int Height { get; private set; }
        public Random RandomGenerator { get; private set; }
    }
}

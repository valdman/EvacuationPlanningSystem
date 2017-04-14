using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;

namespace PlanService.Entities
{
    public struct Plan : IEnumerable<Cell>
    {
        public Plan(int width, int height)
        {
            Width = width;
            Height = height;
            Cells = new Cell[width, height];
            for (var x = 0; x < width; x++)
            for (var y = 0; y < height; y++)
                Cells[x, y] = new Cell(CellState.Initial);

            RandomGenerator = new Random();
        }

        public Cell this[int x, int y]
        {
            get => Cells[x, y];
            set => Cells[x, y] = value;
        }

        public Cell this[Point point]
        {
            get => Cells[point.X, point.Y];
            set => Cells[point.X, point.Y] = value;
        }

        public Cell[,] Cells { get; set; }
        public int Width { get; }
        public int Height { get; }
        public Random RandomGenerator { get; set; }

        public IEnumerator<Cell> GetEnumerator()
        {
            for (var x = 0; x < Width; x++)
            for (var y = 0; y < Height; y++)
                yield return this[x, y];
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public Plan ShallowCopy()
        {
            return (Plan)this.MemberwiseClone();
        }

        public Plan DeepCopy()
        {
            var other = (Plan)this.MemberwiseClone();
            other.Cells = new Cell[Width,Height];
            for (var x = 0; x < Width; x++)
            {
                for (var y = 0; y < Height; y++)
                {
                    other.Cells[x, y] = new Cell
                    {
                        CellState = Cells[x, y].CellState,
                        NumberOfManHere = Cells[x, y].NumberOfManHere,
                        GateCapasity = Cells[x, y].GateCapasity
                    };
                }
            }
            other.RandomGenerator = new Random();
            return other;
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using PlanService.Entities;

namespace PlanService
{
    public static class Extensions
    {
        public static IEnumerable<T> Shuffle<T>(this IEnumerable<T> source, Random randomGenerator)
        {
            var e = source.ToArray();
            for (var i = e.Length - 1; i >= 0; i--)
            {
                var swapIndex = randomGenerator.Next(i + 1);
                yield return e[swapIndex];
                e[swapIndex] = e[i];
            }
        }

        public static CellState OppositeWall(this CellState cellState)
        {
            return (CellState) (((int) cellState >> 2) | ((int) cellState << 2)) & CellState.Initial;
        }

        public static bool HasFlag(this CellState cellState, CellState flag)
        {
            return ((int) cellState & (int) flag) != 0;
        }
    }
}
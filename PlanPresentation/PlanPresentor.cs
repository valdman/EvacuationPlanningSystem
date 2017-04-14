using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using PlanService;
using PlanService.Entities;

namespace PlanPresentation
{
    public class PlanPresentor
    {
        public Canvas ViewWindow { get; private set; }
        public Plan CurrentPlan { get; private set; }
        public IList<Way> CurrentSolution { get; private set; }
        public Drawer Drawer { get; private set; }
        private readonly PlanGenerator _planGenerator;
        private PlanResolver _planResolver;

        public PlanPresentor(ref Canvas viewWindow)
        {
            ViewWindow = viewWindow;
            _planGenerator = new PlanGenerator();
            Drawer = new Drawer(this);
        }
        
        public void RegeneratePlan(int width, int heigth)
        {
            CurrentPlan = _planGenerator.CreateRandomPlan(width, heigth);
            _planResolver = new PlanResolver();
        }

        public void RunRandomSimulation(IEnumerable<int> gatesCapasities, int peopleNumber)
        {
            CurrentPlan = _planGenerator
                .LocateGatesAndPeopleRandom(CurrentPlan, gatesCapasities, peopleNumber);
            ResolvePlan();
        }

        private void ResolvePlan()
        {
            CurrentSolution = new List<Way>();
            for (var x = 0; x < CurrentPlan.Width; x++)
            for (var y = 0; y < CurrentPlan.Height; y++)
            {
                var cell = CurrentPlan[x, y];
                if (cell.NumberOfManHere <= 0) continue;

                var beginPoint = new System.Drawing.Point(x, y);
                foreach (var solution in _planResolver.FindGateway(CurrentPlan, beginPoint))
                    CurrentSolution.Add(solution);
            }

            CurrentSolution.ToList()
                .ForEach(solution => Debug.WriteLine(
                    $"{solution.PeopleOnWay}: {string.Join("->", solution.WayOut.ToList().Select(point => $"[x={point.X}; y={point.Y}]"))}"));
        }
    }
}
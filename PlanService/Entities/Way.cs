using System.Collections.Generic;
using System.Drawing;

namespace PlanService.Entities
{
    public class Way
    {
        public Way()
        {
            WayOut = new List<Point>();
            PeopleOnWay = 0;
        }

        public List<Point> WayOut { get; }
        public int PeopleOnWay { get; set; }
    }
}
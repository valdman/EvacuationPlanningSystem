namespace PlanService.Entities
{
    public class Cell
    {
        public Cell(CellState cellState)
        {
            CellState = cellState;
        }

        public Cell()
        {
            
        }

        public CellState CellState { get; set; }
        public int NumberOfManHere { get; set; }
        public int GateCapasity { get; set; }
    }
}
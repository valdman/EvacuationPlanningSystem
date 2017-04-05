namespace PlanService.Entities
{
    public class Cell
    {
        public Cell(CellState cellState, int numberOfManHere)
        {
            CellState = cellState;
            NumberOfManHere = numberOfManHere;
        }

        public CellState CellState { get; set; }
        public int NumberOfManHere { get; set; }
    }
}
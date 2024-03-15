namespace HexStrategyInRazor.Map.Data
{
    public class WMCellData
    {
        public int unitsCount { get; set; }
        public int buildingsCount { get; set; }
        public int defenceCount { get; set; }
        public string positionId { get; set; }
        public string controllerName { get; set; }
        public bool inBattle { get; set; }
    }
}
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
        public string cellColorHTML { get; set; }

        public int positionX { get; set; }
        public int positionY { get; set; }
        public List<int> sendArmyToPositionX { get; set; }
        public List<int> sendArmyToPositionY { get; set; }
	}
}
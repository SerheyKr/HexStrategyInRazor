namespace HexStrategyInRazor.Map.Data
{
	public class WorldMapData
	{
		public List<WMRowData> Rows { get; set; }
		public int TurnsCount { get; set; }
		public bool IsEnded { get; set; }
		public string EndText { get; set; }
		public int TotalArmy { get; set; }
	}
}
using HexStrategyInRazor.Map.Data;

namespace HexStrategyInRazor.Map
{
    public class WMRow
	{
		public List<WMCell> Cells = new List<WMCell>();

		public WMRowData ToData()
		{
			var cellsData = new List<WMCellData>();

			cellsData.AddRange(Cells.Select(x => x.ToData()));

			return new WMRowData() 
			{
				Cells = cellsData,
			};
		}
	}
}
using HexStrategyInRazor.DB.Models;
using HexStrategyInRazor.Map.Data;

namespace HexStrategyInRazor.Map
{
	public class WMRow
	{
		public List<WMCell> Cells = new List<WMCell>();
		public WorldMap MapRef;
		public int DbId;
		public int PositionX;

		public WMRow(WorldMap mapRef)
		{
			MapRef = mapRef;
		}

		public WMRowData ToData()
		{
			var cellsData = new List<WMCellData>();

			cellsData.AddRange(Cells.Select(x => x.ToData()));

			return new WMRowData()
			{
				Cells = cellsData,
			};
		}

		public static WMRow Load(RowModel data, WorldMap map)
		{
			var row = new WMRow(map);

			row.DbId = data.Id;
			row.PositionX = data.PositionX;

			row.Cells = data.Cells.Select(x => WMCell.Load(x, map, row)).ToList();

			return row;
		}

		public RowModel ToDBData()
		{
			return new RowModel()
			{
				Cells = Cells.Select(x => x.ToDBData()).ToList(),
				MapId = MapRef.DbId,
				Id = DbId,
				PositionX = PositionX,
			};
		}
	}
}
using HexStrategyInRazor.Map.DB.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace HexStrategyInRazor.DB.Models
{
	public class CellModel: AbstractModel
	{
		public int UnitsCount { get; set; }
		public string ControllerId { get; set; }
		public int CellPositionX { get; set; }
		public int CellPositionY { get; set; }

		public bool IsControlledByBot { get; set; }
		public int RowId { get; set; }
		public virtual RowModel Row { get; set; }
		public virtual List<PathModel> Paths { get; set; }
		public int CellIndex { get; set; }

		public CellModel() { }

		public CellModel(int id, int rowId, int unitsCount, string controllerId, int cellPositionX, int cellPositionY) : base(id)
		{
			RowId = rowId;
			UnitsCount = unitsCount;
			ControllerId = controllerId;
			CellPositionX = cellPositionX;
			CellPositionY = cellPositionY;
		}
	}
}

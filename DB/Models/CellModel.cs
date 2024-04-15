using HexStrategyInRazor.Map.DB.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace HexStrategyInRazor.DB.Models
{
	public class CellModel: AbstractModel
	{
		public int UnitsCount { get; set; }
		public int ControllerId { get; set; }
		public int CellPositionX { get; set; }
		public int CellPositionY { get; set; }

		public int RawId { get; set; }
		public virtual RawModel Raw { get; set; }
		public virtual List<PathModel> Paths { get; set; }

		public CellModel() { }

		public CellModel(int id, int rawId, int unitsCount, int controllerId, int cellPositionX, int cellPositionY) : base(id)
		{
			RawId = rawId;
			UnitsCount = unitsCount;
			ControllerId = controllerId;
			CellPositionX = cellPositionX;
			CellPositionY = cellPositionY;
		}
	}
}

using HexStrategyInRazor.Map.DB.Models;

namespace HexStrategyInRazor.DB.Models
{
	public class RowModel: AbstractModel
	{
		public int MapId { get; set; }

		public virtual MapModel Map { get; set; }
		public virtual ICollection<CellModel> Cells { get; set; } = new List<CellModel>();
		public int PositionX { get; set; }

		public RowModel() { }

		public RowModel(int id, int mapId) : base(id)
		{
			MapId = mapId;
		}
	}
}

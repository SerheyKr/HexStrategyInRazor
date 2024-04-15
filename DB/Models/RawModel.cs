using HexStrategyInRazor.Map.DB.Models;

namespace HexStrategyInRazor.DB.Models
{
	public class RawModel: AbstractModel
	{
		public int MapId { get; set; }

		public virtual MapModel Map { get; set; }
		public virtual ICollection<CellModel> Cells { get; set; } = new List<CellModel>();

		public RawModel() { }

		public RawModel(int id, int mapId) : base(id)
		{
			MapId = mapId;
		}
	}
}

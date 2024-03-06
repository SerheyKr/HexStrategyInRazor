using HexStrategyInRazor.Map.DB.Models;

namespace HexStrategyInRazor.DB.Models
{
	public class CellModel: BaseModel
	{
		public int MapId { get; set; }
		public int UnitsCount { get; set; }
		public int DefenceCount { get; set; }
		public int BuildingsCount { get; set; }
	}
}

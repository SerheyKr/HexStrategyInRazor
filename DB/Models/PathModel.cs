using HexStrategyInRazor.Map.DB.Models;

namespace HexStrategyInRazor.DB.Models
{
	public class PathModel : AbstractModel
	{
		public int CellFromdID { get; set; }
		public int CellToID { get; set; }

		public virtual CellModel CellFrom { get; set; }
		public virtual CellModel CellTo { get; set; }
	}
}

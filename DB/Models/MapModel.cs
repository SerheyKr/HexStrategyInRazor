using HexStrategyInRazor.Map.DB.Models;

namespace HexStrategyInRazor.DB.Models
{
	public class MapModel : AbstractModel
	{
		public string PlayerCookieId { get; set; }
		public string BotId { get; set; }
		public int TotalTurnsCount { get; set; }

		public virtual ICollection<RowModel> Rows { get; set; } = new List<RowModel>();
		public virtual UserModel User { get; set; }

		public MapModel()
		{

		}

		public MapModel(int id, string playerCookieId) : base(id)
		{
			PlayerCookieId = playerCookieId;
		}
	}
}

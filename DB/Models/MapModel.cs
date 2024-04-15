using HexStrategyInRazor.Map.DB.Models;

namespace HexStrategyInRazor.DB.Models
{
	public class MapModel: AbstractModel
	{
		public string PlayerCookieId { get; set; }

		public virtual ICollection<RawModel> Raws { get; set; } = new List<RawModel>();
		public virtual UserModel User { get; set; }

		public MapModel() 
		{
		
		}

		public MapModel(int id, string playerCookieId): base(id) 
		{
			PlayerCookieId = playerCookieId;
		}
	}
}

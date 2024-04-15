using HexStrategyInRazor.DB.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HexStrategyInRazor.Map.DB.Models
{
	public class UserModel: AbstractModel
	{
		public string CookieID { get; set; }
		public int MapId { get; set; }

		public virtual MapModel Map { get; set; } = null!;

		public UserModel() 
		{
			
		}

		public UserModel(int id, int mapId, string cookieID): base(id)
		{
			CookieID = cookieID;
			MapId = mapId;
		}
	}
}

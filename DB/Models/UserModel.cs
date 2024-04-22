using HexStrategyInRazor.DB.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HexStrategyInRazor.Map.DB.Models
{
	public class UserModel: IAbstractModel
	{
		[Key]
		public string CookieID { get; set; }
		public int? MapId { get; set; }

		public virtual MapModel Map { get; set; } = null!;

		public UserModel() 
		{
			
		}

		public UserModel(int id, int mapId, string cookieID)
		{
			CookieID = cookieID;
			MapId = mapId;
		}
	}
}

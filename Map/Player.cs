using System.Drawing;
using HexStrategyInRazor.DB.Models;
using HexStrategyInRazor.Map.DB.Models;
using Microsoft.AspNetCore.Mvc;

namespace HexStrategyInRazor.Map
{
	public class Player
	{
		public Color PlayerColor;
		public string PlayerName;
		public WorldMap CurrentMap;
		public bool IsMainPlayer = false;
		public string PlayerId;
		public int DbId;
		public int MapId;

		public Player(Color PlayerColor, string playerName) // TODO for bots
		{
			this.PlayerColor = PlayerColor;
			this.PlayerName = playerName;
		}

		public Player()
		{
			
		}

		public virtual void OnTurnEnd()
		{

		}

		public static Player? Load(UserModel userModel)
		{
			if (userModel == null)
				return null;
			return new Player
			{
				IsMainPlayer = true,
				PlayerId = userModel.CookieID,
				PlayerColor = Color.Blue,
				PlayerName = "Y",
				MapId = userModel.MapId ?? 0,
			};
		}

		public UserModel ToDBData()
		{
			if (CurrentMap == null)
			{
				return new UserModel()
				{
					CookieID = PlayerId,
					MapId = null,
				};
			}
			
			return new UserModel
			{
				CookieID = this.PlayerId,
				MapId = CurrentMap.DbId
			};
		}
	}
}
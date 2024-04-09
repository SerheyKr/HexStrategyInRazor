using System.Drawing;
using HexStrategyInRazor.DB.Models;
using Microsoft.AspNetCore.Mvc;

namespace HexStrategyInRazor.Map
{
	public class Player
	{
		public Color PlayerColor;
		public string PlayerName;
		public BrowserUser User;
		public WorldMap CurrentMap;
		public bool IsMainPlayer = false;
		public string PlayerId;
		public int TotalArmy;

		public Player(Color PlayerColor, string playerName) // TODO for bots
		{
			this.PlayerColor = PlayerColor;
			this.PlayerName = playerName;
			PlayerId = Guid.NewGuid().ToString();
			TotalArmy = CurrentMap?.AllCells.FindAll(x => x.Controller == this).Sum(x => x.UnitsCount) ?? 0;
		}

		public Player(Color PlayerColor, string playerName, BrowserUser controller, bool isMainPlayer = false)
		{
			this.PlayerColor = PlayerColor;
			this.PlayerName = playerName;
			User = controller;
			IsMainPlayer = isMainPlayer;
			PlayerId = Guid.NewGuid().ToString();
		}

		public virtual void OnTurnEnd()
		{
			TotalArmy = CurrentMap?.AllCells.FindAll(x => x.Controller == this).Sum(x => x.UnitsCount) ?? 0;
		}

		public PlayerData ToData()
		{
			return new PlayerData
			{ 
				TotalArmy = CurrentMap?.AllCells.FindAll(x => x.Controller == this).Sum(x => x.UnitsCount) ?? 0,
			};
		}
	}

	public class PlayerData
	{
		public int TotalArmy { get; set; }
		public int TotalBuildings { get; set; }
	}
}
using System.Drawing;
using HexStrategyInRazor.DB.Models;
using HexStrategyInRazor.Map;
using Microsoft.AspNetCore.Mvc;

namespace HexStrategyInRazor.Generator
{
	public class Player
	{
		public Color PlayerColor;
		public string PlayerName;
		public int Monies;
		public BrowserUser User;
		public WorldMap CurrentMap;
		public bool IsMainPlayer = false;
		public string PlayerId;

		public Player(Color PlayerColor, string playerName) // TODO for bots
		{
			this.PlayerColor = PlayerColor;
			this.PlayerName = playerName;
			Monies = 5;
			PlayerId = Guid.NewGuid().ToString();
		}

		public Player(Color PlayerColor, string playerName, BrowserUser controller, bool isMainPlayer = false)
		{
			this.PlayerColor = PlayerColor;
			this.PlayerName = playerName;
			Monies = 5;
			User = controller;
			IsMainPlayer = isMainPlayer;
			PlayerId = Guid.NewGuid().ToString();
		}

		public PlayerData ToData()
		{
			return new PlayerData
			{ 
				Monies = Monies,
				TotalArmy = CurrentMap?.AllUnits.FindAll(x => x.Controller == this).Sum(x => x.UnitsCount) ?? 0,
			};
		}
	}

	public class PlayerData
	{
		public int Monies { get; set; }
		public int TotalArmy { get; set; }
		public int TotalBuildings { get; set; }
	}
}
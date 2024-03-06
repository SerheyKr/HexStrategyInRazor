using System.Drawing;
using HexStrategyInRazor.DB.Models;
using HexStrategyInRazor.Map;

namespace HexStrategyInRazor.Generator
{
	public class Player
	{
		public Color PlayerColor;
		public string PlayerName;
		public int Monies;
		public BrowserUser User;
		public bool IsMainPlayer = false;

		public Player(Color PlayerColor, string playerName) // TODO for bots
		{
			
		}

		public Player(Color PlayerColor, string playerName, BrowserUser controller, bool isMainPlayer = false)
		{
			this.PlayerColor = PlayerColor;
			this.PlayerName = playerName;
			Monies = 5;
			User = controller;
			IsMainPlayer = isMainPlayer;
		}
	}
}
using System.Drawing;

namespace WebApplication1.Generator
{
	public class Player
	{
		public Color playerColor;
		public string playerName;
		public int monies = 5;

		public Player()
		{
			
		}

		public Player(Color PlayerColor, string playerName)
		{
			playerColor = PlayerColor;
			this.playerName = playerName;
		}
	}
}
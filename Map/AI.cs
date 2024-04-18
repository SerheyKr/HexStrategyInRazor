using HexStrategyInRazor.Generator;
using System.Drawing;
using System.Numerics;

namespace HexStrategyInRazor.Map
{
	public class AI: Player
	{
		private readonly int choisenStrategy = 0;

		public AI(): base(Color.Red, "E")
		{

		}

		public AI(Color PlayerColor, string playerName) : base(PlayerColor, playerName)
		{
			PlayerId = Guid.NewGuid().ToString();
		}

		public override void OnTurnEnd()
		{
			base.OnTurnEnd();

			switch (choisenStrategy) 
			{
				case 0:
					{
						RushStrategy();
						break;
					}
			}
		}

		//We just picking random player cell and moving to it
		public void RushStrategy()
		{
			List<WMCell> listBotCells = CurrentMap.AllCells.Where(x => x.Controller == this).ToList();
			List<WMCell> listPlayerCells = CurrentMap.AllCells.Where(x => x.Controller == CurrentMap.MainPlayer).ToList();

			if (listPlayerCells.Count == 0) // We won
				return;

			listBotCells.ForEach(cell =>
			{
				cell.ClearAllWays();
				var path = CurrentMap.FindPath(cell, listPlayerCells.PickRandom());

				if (path != null && path.Count >= 2)
				{
					CurrentMap.CreateMovement(cell, path[1]);
				}
			});
		}
	}
}

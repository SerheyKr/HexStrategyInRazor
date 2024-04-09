using HexStrategyInRazor.Generator;
using System.Drawing;
using System.Numerics;

namespace HexStrategyInRazor.Map
{
	public class AI : Player
	{
		private int choisenStrategy = 0;

		public AI(Color PlayerColor, string playerName) : base(PlayerColor, playerName)
		{
			choisenStrategy = 0; // TODO create various strategies
		}

		public override void OnTurnEnd()
		{
			base.OnTurnEnd();

			switch(choisenStrategy) 
			{
				case 0:
					{
						RushStrategy();
						break;
					}
			}
		}

		//We just rush to player
		private void RushStrategy()
		{
			List<WMCell> listBotCells = CurrentMap.AllCells.Where(x => x.Controller == this).ToList();
			List<WMCell> listPlayerCells = CurrentMap.AllCells.Where(x => x.Controller == CurrentMap.MainPlayer).ToList();
			listBotCells.ForEach(x => x.ClearAllWays());

			listBotCells.ForEach(cell =>
			{
				var path = CurrentMap.FindPath(cell, listPlayerCells.PickRandom());

				if (path == null || path.Count < 2)
				{
					return;
				}

				cell.AddNeighborsSendArmy(path[1]);
			});
		}
	}
}

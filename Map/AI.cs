using HexStrategyInRazor.Generator;
using System.Drawing;
using System.Numerics;

namespace HexStrategyInRazor.Map
{
	public class AI(Color PlayerColor, string playerName) : Player(PlayerColor, playerName)
	{
		private readonly int choisenStrategy = 0;

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

		//We just rush to player
		public void RushStrategy()
		{
			List<WMCell> listBotCells = CurrentMap.AllCells.Where(x => x.Controller == this).ToList();
			List<WMCell> listPlayerCells = CurrentMap.AllCells.Where(x => x.Controller == CurrentMap.MainPlayer).ToList();

			listBotCells.ForEach(cell =>
			{
				cell.ClearAllWays();
				var path = CurrentMap.FindPath(cell, listPlayerCells.PickRandom());

				if (path != null && path.Count >= 2)
				{
					cell.AddNeighborsSendArmy(path[1]);
				}
			});
		}
	}
}

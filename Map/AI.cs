using HexStrategyInRazor.Generator;
using System.Drawing;
using System.Numerics;

namespace HexStrategyInRazor.Map
{
	public class AI: Player
	{
		public int stategy = 1;

		public AI(): base(Color.Red, "E")
		{

		}

		public override void OnTurnEnd()
		{
			base.OnTurnEnd();

			switch(stategy) 
			{
				case 0:
					SmartBot();
					break;
				case 1:
					RushStrategy();
					break;
			}
		}

		public void RushStrategy()
		{
			List<WMCell> listBotCells = CurrentMap.AllCells.Where(x => x.Controller == this).ToList();
			List<WMCell> listPlayerCells = CurrentMap.AllCells.Where(x => x.Controller == CurrentMap.MainPlayer).ToList();

			listBotCells.ForEach(cell =>
			{
				cell.ClearAllWays();
				var path = CurrentMap.FindPath(cell, listPlayerCells.PickRandom());

				CreateMovement(cell, path);
			});
		}

		private void SmartBot()
		{
			List<WMCell> listBotCells = CurrentMap.AllCells.Where(x => x.Controller == this).ToList();
			List<WMCell> listPlayerCells = CurrentMap.AllCells.Where(x => x.Controller == CurrentMap.MainPlayer).ToList();

			if (listPlayerCells.Count == 0) // We won
				return;

			listBotCells.ForEach(cell =>
				cell.ClearAllWays());

			listBotCells.ForEach(cell =>
			{
				if (cell.Neighbors.Exists(x => x.Controller != this))
				{
					cell.Neighbors.Where(x => x.Controller != this).ToList().ForEach(x => CurrentMap.CreateMovement(cell, x));
				}
				else
				{
					var path = CurrentMap.FindPath(cell, listPlayerCells.PickRandom());
					CreateMovement(cell, path);

					if (cell.UnitsCount > 1)
					{
						path = CurrentMap.FindPath(cell, CurrentMap.AllCells.PickRandom());
						CreateMovement(cell, path);
					}

				}
			});
		}

		private void CreateMovement(WMCell cell, List<WMCell>? path)
		{
			if (path != null && path.Count >= 2)
			{
				CurrentMap.CreateMovement(cell, path[1]);
			}
		}
	}
}

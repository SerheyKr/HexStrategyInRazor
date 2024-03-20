using HexStrategyInRazor.Generator;

namespace HexStrategyInRazor.Map
{
	public class Army: ICloneable
	{
		public int UnitsCount;
		public Player Controller;

		public bool moves = false;
		public WMCell currentCell;
		public WMCell nextCell;

		public WMCell From;
		public List<WMCell> Path;
		public WMCell To;

		public void DoMove()
		{ 
			if (currentCell != To)
			{
				moves = true;
			} else
			{
				moves = false;
			}
		}

		object ICloneable.Clone()
		{
			return new Army()
			{
				UnitsCount = UnitsCount,
				Controller = Controller,
				currentCell = currentCell,
			};
		}
	}
}
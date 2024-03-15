using HexStrategyInRazor.Generator;

namespace HexStrategyInRazor.Map
{
    public class Army
	{
		public int UnitsCount;
		public Player Controller;

		public bool moves = false;
        public WMCell currentCell;

        public WMCell From;
        public List<WMCell> Path;
        public WMCell To;
    };
}
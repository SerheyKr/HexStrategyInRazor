using System.ComponentModel.DataAnnotations;

namespace HexStrategyInRazor.Map.DB.Models
{
	public abstract class AbstractModel: IAbstractModel
	{
		[Key]
		public int Id { get; set; }
		public AbstractModel() { }

		public AbstractModel(int id) 
		{
			Id = id;
		}
	}

	public interface IAbstractModel
	{
		
	}
}

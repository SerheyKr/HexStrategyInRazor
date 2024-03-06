using System.ComponentModel.DataAnnotations;

namespace HexStrategyInRazor.Map.DB.Models
{
    public abstract class BaseModel
    {
        [Key]
        public int Id { get; set; }
    }
}

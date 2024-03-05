using System.ComponentModel.DataAnnotations;

namespace WebApplication1.DB.Models
{
    public abstract class BaseModel
    {
        [Key]
        public int Id { get; set; }
    }
}

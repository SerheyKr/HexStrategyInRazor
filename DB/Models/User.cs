using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HexStrategyInRazor.Map.DB.Models
{
    public class User: BaseModel
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public string CookieID { get; set; } = "";
        
        public bool IsDecripted = false;

        public User() 
        {
            
        }

        public override string ToString()
        {
            return $"{UserName} {Password} {Email} {CookieID}";
        }
    }
}

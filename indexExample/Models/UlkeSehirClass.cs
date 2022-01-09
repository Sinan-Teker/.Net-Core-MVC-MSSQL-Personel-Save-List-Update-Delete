using System.ComponentModel.DataAnnotations;

namespace indexExample.Models
{
    public class UlkeSehirClass
    {
        public int Id { get; set; }
        public string UlkeSehir { get; set; }    
        public int UlkeId { get; set; } 
    }
}

using System.ComponentModel.DataAnnotations;

namespace indexExample.Models
{
    public class PersonelMedyaClass
    {
        [Key]
        public int Id { get; set; } 
        public int personelId { get; set; }
        public int MedyaId { get; set; }
    }
}

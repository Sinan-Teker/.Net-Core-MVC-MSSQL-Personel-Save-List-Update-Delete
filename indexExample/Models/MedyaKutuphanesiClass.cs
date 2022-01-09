using System.ComponentModel.DataAnnotations;

namespace indexExample.Models
{
    public class MedyaKutuphanesiClass
    {
        [Key]
        public int IdMedya { get; set; }
        public string MedyaAdı { get; set; }  
        public string MedyaURL { get; set; }
    }
}

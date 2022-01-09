using System.ComponentModel.DataAnnotations;

namespace indexExample.Models
{
    public class PersonelEgitimClass
    {
        [Key]
        public int Id { get; set; }
        public int personelId{ get; set; }
        public int okulTipiID { get; set; }
        public string okulAdı { get; set; }
        public string mezunTarihi { get; set; }
    }
}

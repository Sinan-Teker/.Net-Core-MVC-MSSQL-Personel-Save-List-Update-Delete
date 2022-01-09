using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace indexExample.Models
{
    public class PersonellerClass
    {
        [Key]
        public int Id { get; set; }
        public string Ad { get; set; }
        public string SoyAd { get; set; }
        public long TelNo { get; set; }
        public string dogumTarihi { get; set; }
        public string dogduguUlke { get; set; }
        public string dogduguSehir { get; set; }
        public string Aciklama { get; set; }
        public int MedyaId { get; set; }
        [NotMapped]
        public IFormFile ImageFile { get; set; }
        [NotMapped]
        public string MedyaAdı { get; set; }
        [NotMapped]
        public string MedyaURL { get; set; }
        [NotMapped]
        public string mezunTarihi { get; set; }
        [NotMapped]
        public List<PersonelEgitimClass> personelEgitim { get; set; }
    }
}

using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace indexExample.Models
{
    public class ConnectionStringClass:DbContext
    {
        public ConnectionStringClass(DbContextOptions<ConnectionStringClass> options):base(options)
        {

        }
        public DbSet<PersonellerClass> personelKayit { get; set; }
        public DbSet<UlkeSehirClass> UlkeSehir { get; set; }
        public DbSet<MedyaKutuphanesiClass> medyaKutuphanesi { get; set; }
        public DbSet<OkulTipleriClass> okulTipleri { get; set; }
        public DbSet<PersonelEgitimClass> personelEgitim { get; set; }
        public DbSet<PersonelMedyaClass> personelMedya { get; set; }
    }
}

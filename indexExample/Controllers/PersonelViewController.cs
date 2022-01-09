using indexExample.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace indexExample.Controllers
{
    public class PersonelViewController : Controller
    {
        private readonly ConnectionStringClass _csc;

        public PersonelViewController(ConnectionStringClass csc)
        {
            _csc = csc;
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Show()
        {
            var data = _csc.personelKayit.FromSqlRaw(@"select p.Id,p.Ad,p.SoyAd,p.TelNo,p.dogumTarihi,u1.UlkeSehir as dogduguUlke,u2.UlkeSehir as dogduguSehir,p.Aciklama,u3.IdMedya as MedyaId,u3.MedyaURL as MedyaURL,u4.mezunTarihi as mezunTarihi from personelKayit p
                left join UlkeSehir u1 ON u1.Id= p.dogduguUlke 
                left join UlkeSehir u2 ON u2.Id= p.dogduguSehir
                left join medyaKutuphanesi u3 ON u3.IdMedya= p.MedyaId
                left join personelEgitim u4 on u4.personelId = p.Id
				where mezunTarihi = (SELECT MAX(personelEgitim.mezunTarihi) from personelEgitim where personelId = p.Id)").ToList();

            var personelEgitim = _csc.personelEgitim.FirstOrDefault();

            for (var i = 0; i < data.Count; i++)
            {
                var medya = _csc.medyaKutuphanesi.FirstOrDefault(x => x.IdMedya == data[i].MedyaId);
                var mezunTarihi = _csc.personelEgitim.FromSqlRaw(@"select * from personelEgitim
                                                                            where mezunTarihi = (SELECT MAX( personelEgitim.mezunTarihi ) from personelEgitim where personelId = " + data[i].Id + ")").ToList();
                data[i].MedyaURL = medya.MedyaURL;
                data[i].mezunTarihi = mezunTarihi[0].mezunTarihi;
            }
            return View(data);
        }

        public IActionResult Update()
        {
            return View();
        }

        public async Task<IActionResult> Delete(int Id)
        {
            var data = await _csc.personelKayit.FindAsync(Id);
            _csc.personelKayit.Remove(data);
            await _csc.SaveChangesAsync();
            return RedirectToAction(nameof(Show));
        }

        [HttpPost]
        [Route("/PersonelView/deleteOkul")]
        public void deleteOkul(PersonellerClass okulSil)
        {
            List<PersonelEgitimClass> personelVeri = new List<PersonelEgitimClass>();

            for(int items = 0; items < okulSil.personelEgitim.Count; items++){ 
            
                if (okulSil.personelEgitim[items].okulTipiID != 0)
                {
                    personelVeri.Add(okulSil.personelEgitim[items]);

                    var personEgitim = _csc.personelEgitim.Where(x => x.okulTipiID == okulSil.personelEgitim[items].okulTipiID && x.okulAdı == okulSil.personelEgitim[items].okulAdı && x.mezunTarihi == okulSil.personelEgitim[items].mezunTarihi ).FirstOrDefault();
                    _csc.personelEgitim.Remove(personEgitim);
                    _csc.SaveChanges();
                }

            }

        }


        [HttpGet]
        [Route("/PersonelView/VerileriGelsin/{id}")]
        public PersonellerClass VerileriGelsin(int id)
        {

            var data = _csc.personelKayit.FromSqlRaw(@"select p.Id,p.Ad,p.SoyAd,p.TelNo,p.dogumTarihi,u1.UlkeSehir as dogduguUlke,u2.UlkeSehir as dogduguSehir,p.Aciklama,u3.IdMedya as MedyaId,u3.MedyaURL as MedyaURL,u4.mezunTarihi as mezunTarihi from personelKayit p
                left join UlkeSehir u1 ON u1.Id= p.dogduguUlke 
                left join UlkeSehir u2 ON u2.Id= p.dogduguSehir
                left join medyaKutuphanesi u3 ON u3.IdMedya= p.MedyaId
                left join personelEgitim u4 on u4.personelId = p.Id
				where mezunTarihi = (SELECT MAX(personelEgitim.mezunTarihi) from personelEgitim where personelId = " + id + ")").FirstOrDefault();

            var medya = _csc.medyaKutuphanesi.FirstOrDefault(x => x.IdMedya == data.MedyaId);
            data.MedyaURL = medya.MedyaURL;
            data.MedyaAdı = medya.MedyaAdı;
            var personelEgitim = _csc.personelEgitim.FromSqlRaw(@"select p.Id,p.mezunTarihi,p.okulAdı,p.personelId, u1.Id as okulTipiID from personelEgitim p
							left join okulTipleri u1 ON u1.Id = p.okulTipiID
							where personelId =" + data.Id + "").ToList();
            data.personelEgitim = personelEgitim;

            return data;

        }

        [Route("/PersonelView/Update/{id}")]
        public IActionResult Update(int id)
        {
            return View();
        }

        [HttpPost]
        [Route("/PersonelView/UpdateKayit")]
        public void UpdateKayit(PersonellerClass guncelle)
        {
            var model = new PersonellerClass()
            {
                Ad = guncelle.Ad,
                SoyAd = guncelle.SoyAd,
                TelNo = guncelle.TelNo,
                dogumTarihi = guncelle.dogumTarihi,
                dogduguUlke = guncelle.dogduguUlke,
                dogduguSehir = guncelle.dogduguSehir,
                Aciklama = guncelle.Aciklama,
                Id = guncelle.Id,
                MedyaId = guncelle.MedyaId,

            };

            _csc.personelKayit.Update(model);
            _csc.SaveChanges();

            List<PersonelEgitimClass> personelEgit = _csc.personelEgitim.FromSqlRaw(@"select * from personelEgitim").ToList();
            //List<PersonelEgitimClass> personelEgit2 = new List<PersonelEgitimClass>();

            for (int items = 0; items < guncelle.personelEgitim.Count; items++)
            {

                {
                    if (guncelle.personelEgitim[items].okulTipiID != 0)
                    {
                        try
                        {
                            int newId = personelEgit[items].Id;

                            PersonelEgitimClass personelEski = _csc.personelEgitim.FirstOrDefault(x => x.Id == newId);

                            var modelEmpOkul = new PersonelEgitimClass()
                            {
                                Id = newId,
                                personelId = model.Id,
                                mezunTarihi = guncelle.personelEgitim[items].mezunTarihi,
                                okulTipiID = guncelle.personelEgitim[items].okulTipiID,
                                okulAdı = guncelle.personelEgitim[items].okulAdı
                            };

                            _csc.Entry(personelEski).CurrentValues.SetValues(modelEmpOkul);
                            _csc.SaveChanges();
                        }
                        catch (ArgumentOutOfRangeException e)
                        {
                            var modelEmpOkul = new PersonelEgitimClass()
                            {
                                personelId = model.Id,
                                mezunTarihi = guncelle.personelEgitim[items].mezunTarihi,
                                okulTipiID = guncelle.personelEgitim[items].okulTipiID,
                                okulAdı = guncelle.personelEgitim[items].okulAdı
                            };

                            _csc.personelEgitim.Add(modelEmpOkul);
                            _csc.SaveChanges();
                        }

                    }
                }
            }
        }
    }
}
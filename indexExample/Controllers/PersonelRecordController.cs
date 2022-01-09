using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using indexExample.Models;
using Microsoft.AspNetCore.Hosting;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.IO;
using System;
using Microsoft.AspNetCore.Http;

namespace indexExample.Controllers
{
    public class PersonelRecordController : Controller
    {

        private readonly ConnectionStringClass _csc;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public PersonelRecordController(ConnectionStringClass csc, IWebHostEnvironment webHostEnvironment)
        {
            _csc = csc;
            _webHostEnvironment = webHostEnvironment;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Create()
        {
            return View();
        }

        public List<OkulTipleriClass> OkulCekme()
        {
            var data = _csc.okulTipleri.ToList();

            return data;
        }
        public List<UlkeSehirClass> UlkeCekme()
        {
            var data = _csc.UlkeSehir.Where(x => x.UlkeId == 0).ToList();

            return data;

        }

        [HttpGet]
        [Route("/PersonelRecord/SehirCekme/{value}")]
        public List<UlkeSehirClass> SehirCekme(string value)
        {
            var data = _csc.UlkeSehir.FromSqlRaw(@"Select * FROM [personelKayit].[dbo].[UlkeSehir] where UlkeId=" + value).ToList();
            return data;
        }


        [HttpPost]
        [Route("/PersonelRecord/CreateKayit")]
        public void Kayit(PersonellerClass p)
        {

            var model1 = new PersonellerClass()
            {
                Ad = p.Ad,
                SoyAd = p.SoyAd,
                TelNo = p.TelNo,
                dogumTarihi = p.dogumTarihi,
                dogduguUlke = p.dogduguUlke,
                dogduguSehir = p.dogduguSehir,
                Aciklama = p.Aciklama,
                personelEgitim = p.personelEgitim,
                MedyaId = p.MedyaId
            };

                _csc.personelKayit.Add(model1);
                _csc.SaveChanges();

                int idEmp = model1.Id;
                int idMed = model1.MedyaId;
                var model3 = new PersonelMedyaClass() { personelId = idEmp, MedyaId = idMed };
                _csc.personelMedya.Add(model3);
                _csc.SaveChanges();

                for (int items = 0; items < p.personelEgitim.Count; items++)
                {
                    if (p.personelEgitim[items].okulTipiID != 0)
                    {
                        var modelEmpOkul = new PersonelEgitimClass()
                        {
                            personelId = model1.Id,
                            mezunTarihi = p.personelEgitim[items].mezunTarihi,
                            okulTipiID = p.personelEgitim[items].okulTipiID,
                            okulAdı = p.personelEgitim[items].okulAdı
                        };

                        _csc.personelEgitim.Add(modelEmpOkul);
                        _csc.SaveChanges();

                    }
                }
        }


        [HttpPost]
        [Route("PersonelRecord/CreateMedia/")]
        public int CreateMedia(IFormFile iFormFile)
        {
            int id = 0;

            string fileName = null;
            string wwwRootPath = null;
            string path = null;
            string url = null;
            if (iFormFile != null)
            {
                wwwRootPath = _webHostEnvironment.WebRootPath;
                fileName = Path.GetFileNameWithoutExtension(iFormFile.FileName);
                string extension = Path.GetExtension(iFormFile.FileName);
                fileName = fileName + DateTime.Now.ToString("yymmssfff") + extension;
                path = Path.Combine(wwwRootPath + "/MedyaKutuphanesi/", fileName);
                url = Path.Combine("../../MedyaKutuphanesi/", fileName);
                using (var fileStream = new FileStream(path, FileMode.Create))
                {
                    iFormFile.CopyTo(fileStream);
                }
                var model2 = new MedyaKutuphanesiClass()
                {
                    MedyaAdı = fileName,
                    MedyaURL = url,
                };
                _csc.medyaKutuphanesi.Add(model2);
                _csc.SaveChanges();
                id = model2.IdMedya;
            }
            return id;
        }
    }
}

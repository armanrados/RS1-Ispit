using FIT_Api_Examples.Data;
using FIT_Api_Examples.Helper;
using FIT_Api_Examples.Helper.AutentifikacijaAutorizacija;
using FIT_Api_Examples.Modul3_MaticnaKnjiga.Models;
using FIT_Api_Examples.Modul3_MaticnaKnjiga.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Linq;

namespace FIT_Api_Examples.Modul4_MaticnaKnjiga.Kontroler
{
    //[Authorize]
    [ApiController]
    [Route("[controller]/[action]")]
    public class MaticnaKnjigaController : ControllerBase
    {
        private readonly ApplicationDbContext _dbContext;

        public MaticnaKnjigaController(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

     

        [HttpGet]
        [Route("/GetMaticna")]
        public ActionResult<MaticnaKnjigaVM> GetMaticna(int id)
        {
            if (!HttpContext.GetLoginInfo().isLogiran)
                return BadRequest("Niste logirani.");

            var student = _dbContext.Student.Find(id);

            if (student == null)
                return BadRequest("Student ne postoji!");

            var maticna = new MaticnaKnjigaVM()
            {
                id = student.id,
                ime = student.ime,
                prezime = student.prezime,
                akGodina = _dbContext.AkademskaGodina.Select(x => new CmbStavke() { id = x.id, opis = x.opis }).ToList(),
                godinaStudija = _dbContext.UpisAkGodine.Include(x => x.student)
                                                        .Include(x => x.akademskaGodina)
                                                        .Include(x => x.evidentiraoKorisnik)
                                                        .ToList()
            };

            return Ok(maticna);
        }

    

        [HttpGet]
        [Route ("/GetGodine")]
        public ActionResult GetGodine()
        {
            var godine = _dbContext.AkademskaGodina.Select(x => new CmbStavke() { id = x.id, opis = x.opis }).ToList();

            return Ok(godine);
        }

        [HttpPost("{id}")]
        public ActionResult UpisiZimski(int id, [FromBody] ZimskiSemestar semestar)
        {
            if (!HttpContext.GetLoginInfo().isLogiran)
                return BadRequest("Niste logirani.");

            var student = _dbContext.Student.Find(id);

            if (student == null)
                return BadRequest("Student ne postoji");
            if(semestar.obnova || !_dbContext.UpisAkGodine.ToList()
                .Exists(ug => ug.godinaStudija == semestar.godinaStudija && ug.studentId == student.id))
            {
                var novi = new UpisAkGodinu();
                _dbContext.Add(novi);

                novi.studentId = student.id;
                novi.evidentiraoKorisnik = HttpContext.GetLoginInfo()?.korisnickiNalog;
                novi.evidentiraoKorisnikId = 1;
                novi.datum_ZimskiUpis = semestar.datum;
                novi.obnovaGodine = semestar.obnova;
                novi.cijenaSkolarine = semestar.cijenaSkolarine;
                novi.akademskaGodinaId = semestar.akGodina;
                novi.godinaStudija = semestar.godinaStudija;

                _dbContext.SaveChanges();

                return Ok(novi);
            }
            return BadRequest();
          
        }

        [HttpPost ("{id}")]
        public ActionResult OvjeriZimski(int id)
        {
            //if (!HttpContext.GetLoginInfo().isLogiran)
            //    return BadRequest("Niste logirani.");

            var godina = _dbContext.UpisAkGodine.Find(id);

            if (godina == null)
                return BadRequest("Ne postoji zimski semestar za ovjeru");

            godina.datum_ZimskiOvjera = DateTime.Now;

            _dbContext.SaveChanges();

            return Ok(godina);
        }
    }
}

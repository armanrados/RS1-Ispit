using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using FIT_Api_Examples.Data;
using FIT_Api_Examples.Helper;
using FIT_Api_Examples.Helper.AutentifikacijaAutorizacija;
using FIT_Api_Examples.Modul0_Autentifikacija.Models;
using FIT_Api_Examples.Modul2.Models;
using FIT_Api_Examples.Modul2.ViewModels;
using FIT_Api_Examples.Modul3_MaticnaKnjiga.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FIT_Api_Examples.Modul2.Controllers
{
    //[Authorize]
    [ApiController]
    [Route("[controller]/[action]")]
    public class StudentController : ControllerBase
    {
        private readonly ApplicationDbContext _dbContext;

        public StudentController(ApplicationDbContext dbContext)
        {
            this._dbContext = dbContext;
        }

      

        [HttpGet]
        public ActionResult<List<Student>> GetAll(string ime_prezime)
        {
            var data = _dbContext.Student
                .Include(s => s.opstina_rodjenja.drzava)
                .Where(x => ime_prezime == null || (x.ime + " " + x.prezime).StartsWith(ime_prezime) || (x.prezime + " " + x.ime).StartsWith(ime_prezime))
                .OrderByDescending(s => s.id)
                .AsQueryable();
            return data.Take(100).ToList();
        }

        [HttpPost]
        [Route("/DodajStudenta")]
        public ActionResult DodajStudenta([FromBody] NoviStudent noviStudent)
        {
            if (!HttpContext.GetLoginInfo().isLogiran)
                return BadRequest("nije logiran");
            
            var opstina = _dbContext.Opstina.Where(o => o.id == noviStudent.OpstinaId).FirstOrDefault();
            if (opstina == null) return BadRequest("Opstina prazna");
            var newStudent = new Student()
            {
                ime = noviStudent.Ime,
                prezime = noviStudent.Prezime,
                opstina_rodjenja = opstina,
                opstina_rodjenja_id = opstina.id

            };
            _dbContext.Student.Add(newStudent);
            _dbContext.SaveChanges();
            return Ok();
        }

        [HttpPut]
        [Route ("/UrediStudenta")]
        public ActionResult UrediStudenta([FromBody] UrediStudent urediStudent)
        {
            if (!HttpContext.GetLoginInfo().isLogiran)
                return BadRequest("nije logiran");
            
            var opstina = _dbContext.Opstina.Where(o =>o.id == urediStudent.OpcinaId).FirstOrDefault();
            var student = _dbContext.Student.Where(s =>s.id == urediStudent.StudentId).FirstOrDefault();
            if (student == null || opstina == null) return BadRequest();
            student.ime = urediStudent.Ime;
            student.prezime = urediStudent.Prezime;
            student.opstina_rodjenja = opstina;
            student.opstina_rodjenja_id= opstina.id;
            _dbContext.SaveChanges();
            return Ok();
        }

        [HttpGet]
        [Route("/GetStudentById")]
        public ActionResult<Student> GetStudentById([FromQuery] int studentId)
        {

            if (!HttpContext.GetLoginInfo().isLogiran)
                return BadRequest("nije logiran");
            var student = _dbContext.Student.Where(s => s.id == studentId).FirstOrDefault();
            if (student == null) return BadRequest();
            return Ok(student);

        }

        [HttpDelete]
        [Route("/ObrisiStudenta")]
        public ActionResult ObrisiStudenta([FromQuery] int studentId)
        {
            if (!HttpContext.GetLoginInfo().isLogiran)
                return BadRequest("nije logiran");

            var student = _dbContext.Student.Where(s => s.id == studentId).FirstOrDefault();
            if (student == null) return BadRequest();
            if (_dbContext.UpisAkGodine.Where(ug => ug.studentId == student.id).ToList().Count >= 1)
            {
                foreach (var godina in _dbContext.UpisAkGodine.Where(ug => ug.studentId == student.id).ToList())
                {
                    _dbContext.Remove(godina);
                    _dbContext.SaveChanges();
                }
            }
            _dbContext.Remove(student);
            _dbContext.SaveChanges();
            return Ok();
        }
    }
}

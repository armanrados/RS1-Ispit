using FIT_Api_Examples.Modul0_Autentifikacija.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System;
using FIT_Api_Examples.Modul2.Models;

namespace FIT_Api_Examples.Modul3_MaticnaKnjiga.Models
{
    public class UpisAkGodinu
    {
        [Key]
        public int id { get; set; }
        public DateTime? datum_ZimskiOvjera { get; set; }
        public DateTime? datum_ZimskiUpis { get; set; }
        public int godinaStudija { get; set; }

        [ForeignKey(nameof(student))]
        public int studentId { get; set; }
        public Student student { get; set; }

        [ForeignKey(nameof(akademskaGodina))]
        public int akademskaGodinaId { get; set; }
        public AkademskaGodina akademskaGodina { get; set; }

        public float? cijenaSkolarine { get; set; }

        [ForeignKey(nameof(evidentiraoKorisnik))]
        public int evidentiraoKorisnikId { get; set; }
        public KorisnickiNalog evidentiraoKorisnik { get; set; }

        public bool obnovaGodine { get; set; }
    }
}

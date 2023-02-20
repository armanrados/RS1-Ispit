using System;

namespace FIT_Api_Examples.Modul3_MaticnaKnjiga.Models
{
    public class ZimskiSemestar
    {
        public DateTime datum { get; set; }
        public int godinaStudija { get; set; }
        public int akGodina { get; set; }
        public float cijenaSkolarine { get; set; }
        public bool obnovaGodine { get; set; }
    }
}

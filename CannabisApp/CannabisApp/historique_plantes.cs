using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CannabisApp
{
    [Table("historique_plantes")]
    public class Historique_Plantes
    {
        public int IdHistorique { get; set; }
        public int IdPlante { get; set; }
        public string initial { get; set; }
        public string final { get; set; }

        public string Action { get; set; }
        public DateTime Timestamp { get; set; }
        public int IdUtilisateur { get; set; }
    }
}

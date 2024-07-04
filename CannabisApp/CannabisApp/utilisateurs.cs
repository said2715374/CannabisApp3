using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CannabisApp
{
    public class utilisateur
    {
        public int id_utilisateur { get; set; }
        public string nom_utilisateur { get; set; }
        public string mot_de_passe { get; set; }
        public int id_role { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EvaluateurReliques.Classes
{
    public class Relique
    {
        private string _nom;
        private int _cout;
        private int _quantite;

        public int Quantite
        {
            get { return _quantite; }
            set { _quantite = value; }
        }

        public int Cout
        {
            get { return _cout; }
            set { _cout = value; }
        }

        public string Nom
        {
            get { return _nom; }
            set { _nom = value; }
        }
    }
}

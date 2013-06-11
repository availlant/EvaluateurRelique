using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace EvaluateurReliques.Classes
{
    public class Joueur : INotifyPropertyChanged
    {
        private int _position;


        private int _pointsAbyssauxTotaux;
        private int _pointsAbyssauxGagnes;
        
        private List<Relique> _sesReliques;
        private List<Relique> _lesReliquesGagnees;

        public Joueur(int position, List<Relique> lesReliques)
        {
            _position = position;
            _sesReliques = new List<Relique>();
            _lesReliquesGagnees = new List<Relique>();
            foreach (var relique in lesReliques)
            {
                _sesReliques.Add(new Relique() { Nom = relique.Nom, Quantite = 0, Cout = relique.Cout });
                _lesReliquesGagnees.Add(new Relique() { Nom = relique.Nom, Quantite = 0, Cout = relique.Cout });
            }
        }

        public int PointsAbyssauxTotaux
        {
            get { return _pointsAbyssauxTotaux; }
            set { 
                _pointsAbyssauxTotaux = value; 
                RaisePropertyChanged("PointsAbyssauxTotaux"); 
            }
        }

        public int PointsAbyssauxGagnes
        {
            get { return _pointsAbyssauxGagnes; }
            set { _pointsAbyssauxGagnes = value; }
        }

        public List<Relique> LesReliquesGagnees
        {
            get { return _lesReliquesGagnees; }
            set { _lesReliquesGagnees = value; }
        }

        public List<Relique> SesReliques
        {
            get { return _sesReliques; }
            set { _sesReliques = value; }
        }

        public int Position
        {
            get { return _position; }
            set { _position = value; }
        }

        public void Reset()
        {
            _pointsAbyssauxGagnes = 0;
            foreach (var relique in _lesReliquesGagnees)
            {
                relique.Quantite = 0;
            }
        }

        public void ResetCompletement()
        {
            PointsAbyssauxGagnes = 0;
            PointsAbyssauxTotaux = 0;
            foreach (var relique in LesReliquesGagnees)
            {
                relique.Quantite = 0;
            }
            foreach (var relique in SesReliques)
            {
                relique.Quantite = 0;
            }
        }

        public void GagneUneRelique(Relique relique)
        {
            PointsAbyssauxGagnes += relique.Cout;
            PointsAbyssauxTotaux += relique.Cout;
            LesReliquesGagnees.Where(o => o.Nom == relique.Nom).First().Quantite++;
            SesReliques.Where(o => o.Nom == relique.Nom).First().Quantite++;
        }

        private void RaisePropertyChanged(string propertyName)
        {
            if (PropertyChanged == null) return;
            PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}

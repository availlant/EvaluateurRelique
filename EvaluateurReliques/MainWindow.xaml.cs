using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using EvaluateurReliques.Classes;
using EvaluateurReliques.Controls;
using System.Collections.ObjectModel;
using EvaluateurReliques.Properties;

namespace EvaluateurReliques
{
    /// <summary>
    /// Logique d'interaction pour MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        List<Joueur> lesJoueurs = new List<Joueur>();
        List<Relique> lesReliques = new List<Relique>();

        const int MaxPlayers = 24;

        public MainWindow()
        {
            InitializeComponent();

            Initialisation();
        }

        private void Initialisation()
        {
            Relique1.SetRelique("Icône antique inférieure", 300, Properties.Resources.icone_inf);
            Relique2.SetRelique("Icône antique", 600, Properties.Resources.icone);
            Relique3.SetRelique("Icône antique supérieure", 900, Properties.Resources.icone_sup);
            Relique4.SetRelique("Icône antique majeure", 1200, Properties.Resources.icone_maj);
            Relique5.SetRelique("Sceau antique inférieure", 600, Properties.Resources.sceau_inf);
            Relique6.SetRelique("Sceau antique", 1200, Properties.Resources.sceau);
            Relique7.SetRelique("Sceau antique supérieure", 1800, Properties.Resources.sceau_sup);
            Relique8.SetRelique("Sceau antique majeure", 2400, Properties.Resources.sceau_maj);
            Relique9.SetRelique("Timbale antique inférieure", 1200, Properties.Resources.timbale_inf);
            Relique10.SetRelique("Timbale antique", 2400, Properties.Resources.timbale);
            Relique11.SetRelique("Timbale antique supérieure", 3600, Properties.Resources.timbale_sup);
            Relique12.SetRelique("Timbale antique majeure", 4800, Properties.Resources.timbale_maj);
            Relique13.SetRelique("Couronne antique inférieure", 2400, Properties.Resources.couronne_inf);
            Relique14.SetRelique("Couronne antique", 4800, Properties.Resources.couronne);
            Relique15.SetRelique("Couronne antique supérieure", 7200, Properties.Resources.couronne_sup);
            Relique16.SetRelique("Couronne antique majeure", 9600, Properties.Resources.couronne_maj);

            foreach (ReliqueControl reliqueControl in this.rootGrid.Children.OfType<ReliqueControl>())
            {
                lesReliques.Add(reliqueControl.GetRelique());
            }

            for (int i = 1; i <= MaxPlayers; i++)
            {
                lesJoueurs.Add(new Joueur(i, lesReliques));
            }

            numPlayerNumber.MaxValue = MaxPlayers;

            InitialiseGrid();
        }

        private void InitialiseGrid()
        {
            datagridJoueurs.AutoGenerateColumns = false;

            datagridJoueurs.Columns.Add(new DataGridTextColumn()
            {
                Header = "Position",
                Binding = new Binding("Position"),
                IsReadOnly = true
            });
            datagridJoueurs.Columns.Add(new DataGridTextColumn()
            {
                Header = "Points abyssaux",
                Binding = new Binding("PointsAbyssauxTotaux")
            });            
        }

        private void LoadDataSource(int nbJoueurs)
        {
            datagridJoueurs.ItemsSource = lesJoueurs.Where(o => o.Position <= nbJoueurs).ToList();
        }

        private void numPlayerNumber_PreviewExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            if (e.Command == ApplicationCommands.Copy ||
                    e.Command == ApplicationCommands.Cut ||
                    e.Command == ApplicationCommands.Paste)
            {
                e.Handled = true;
            }
        }

        private void buttonRepartir_Click(object sender, RoutedEventArgs e)
        {
            int nbJoueurs;
            int.TryParse(numPlayerNumber.Text, out nbJoueurs);            

            foreach (var relique in lesReliques.OrderByDescending(o => o.Cout).Where(o => o.Quantite != 0))
            {
                while (relique.Quantite != 0)
                {
                    var joueur = lesJoueurs.Where(o => o.Position <= nbJoueurs).OrderBy(o => o.PointsAbyssauxTotaux).Count() != 0 ? lesJoueurs.Where(o => o.Position <= nbJoueurs).OrderBy(o => o.PointsAbyssauxTotaux).First() : null;
                    if (joueur != null)
                    {
                        joueur.GagneUneRelique(relique);
                    }
                    relique.Quantite--;
                }
            }

            foreach (var reliqueControl in this.rootGrid.Children.OfType<ReliqueControl>())
            {
                reliqueControl.Update();
            }

            AfficherResultatEtReinitialiserPointsGagnesCeTour();
        }

        private void AfficherResultatEtReinitialiserPointsGagnesCeTour()
        {
            labelResultat.Content = "";

            int nbJoueurs;
            int.TryParse(numPlayerNumber.Text, out nbJoueurs);            

            foreach (var joueur in lesJoueurs.Where(o => o.Position <= nbJoueurs))
            {
                if (joueur.PointsAbyssauxGagnes == 0)
                {
                    labelResultat.Content += "Joueur " + joueur.Position + " ne gagne rien. Il a toujours " + joueur.PointsAbyssauxTotaux + " points abyssaux.\n";
                }
                else
                {
                    labelResultat.Content += "Joueur " + joueur.Position + " gagne : ";
                    int nb = 0;
                    foreach (var relique in joueur.LesReliquesGagnees.Where(o => o.Quantite != 0))
                    {
                        if (nb == 5)
                        {
                            labelResultat.Content += "\n";
                            nb = 0;
                        }
                        nb++;
                        labelResultat.Content += relique.Quantite + " " + relique.Nom + ", ";
                    }
                    labelResultat.Content += "pour un total de " + joueur.PointsAbyssauxGagnes + " points abyssaux. Il a désormais " + joueur.PointsAbyssauxTotaux + " points abyssaux.\n";
                    joueur.Reset();
                }
            }
        }

        private void buttonRazReliques_Click(object sender, RoutedEventArgs e)
        {
            foreach (ReliqueControl reliqueControl in this.rootGrid.Children.OfType<ReliqueControl>())
            {
                reliqueControl.Reset();
            }
        }

        private void buttonRazJoueurs_Click(object sender, RoutedEventArgs e)
        {
            foreach (var joueur in lesJoueurs)
            {
                joueur.ResetCompletement();
            }
        }

        private void numPlayerNumber_TextChanged(object sender, TextChangedEventArgs e)
        {
            int nbJoueurs;
            int.TryParse(numPlayerNumber.Text, out nbJoueurs);

            LoadDataSource(nbJoueurs);
        }
    }
}

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

namespace EvaluateurReliques.Controls
{
    /// <summary>
    /// Suivez les étapes 1a ou 1b puis 2 pour utiliser ce contrôle personnalisé dans un fichier XAML.
    ///
    /// Étape 1a) Utilisation de ce contrôle personnalisé dans un fichier XAML qui existe dans le projet actif.
    /// Ajoutez cet attribut XmlNamespace à l'élément racine du fichier de balisage où il doit 
    /// être utilisé :
    ///
    ///     xmlns:MyNamespace="clr-namespace:EvaluateurReliques.Controls"
    ///
    ///
    /// Étape 1b) Utilisation de ce contrôle personnalisé dans un fichier XAML qui existe dans un autre projet.
    /// Ajoutez cet attribut XmlNamespace à l'élément racine du fichier de balisage où il doit 
    /// être utilisé :
    ///
    ///     xmlns:MyNamespace="clr-namespace:EvaluateurReliques.Controls;assembly=EvaluateurReliques.Controls"
    ///
    /// Vous devrez également ajouter une référence du projet contenant le fichier XAML
    /// à ce projet et régénérer pour éviter des erreurs de compilation :
    ///
    ///     Cliquez avec le bouton droit sur le projet cible dans l'Explorateur de solutions, puis sur
    ///     "Ajouter une référence"->"Projets"->[Recherchez et sélectionnez ce projet]
    ///
    ///
    /// Étape 2)
    /// Utilisez à présent votre contrôle dans le fichier XAML.
    ///
    ///     <MyNamespace:NumericEditor/>
    ///
    /// </summary>
    public class NumericEditor : TextBox
    {
        static NumericEditor()
        {
            //Va charger le style dans Generic.xaml, sauf qu'on veut le style par défaut, donc on n'appelle pas cette méthode.
            //DefaultStyleKeyProperty.OverrideMetadata(typeof(NumericEditor), new FrameworkPropertyMetadata(typeof(NumericEditor)));
        }

        public NumericEditor()
        {
            AddHandler(PreviewTextInputEvent, new TextCompositionEventHandler(NumericEditor_PreviewTextInput));
            AddHandler(PreviewKeyDownEvent, new KeyEventHandler(NumericEditor_PreviewKeyDown));
        }

        private void NumericEditor_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            char caractereEcrit = e.Text[0];
            if (!Char.IsDigit(caractereEcrit))
            {
                e.Handled = true;
            }
            else
            {
                string selection = ((TextBox)e.Source).SelectedText;
                if (selection.Length == 0)
                {
                    var nombre = int.Parse(Text + caractereEcrit);
                    if (nombre > MaxValue)
                        e.Handled = true;
                }
                else
                {
                    string chaine = Text.Substring(0, ((TextBox)e.Source).SelectionStart) + caractereEcrit +
                        Text.Substring(((TextBox)e.Source).SelectionStart + ((TextBox)e.Source).SelectionLength);
                    var nombre = int.Parse(chaine);
                    if (nombre > MaxValue)
                        e.Handled = true;
                }
            }
        }

        private void NumericEditor_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Space)
                e.Handled = true;
        }

        private void NumericEditor_PreviewExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            if (e.Command == ApplicationCommands.Copy ||
                    e.Command == ApplicationCommands.Cut ||
                    e.Command == ApplicationCommands.Paste)
            {
                e.Handled = true;
            }
        }

        public int MaxValue = Int32.MaxValue;
    }
}

using System.Windows;
using System.Windows.Controls;

using EvaluateurReliques.Classes;
using System.Drawing;
using System.Windows.Media.Imaging;
using System.IO;
using System.Drawing.Imaging;

namespace EvaluateurReliques.Controls
{
    /// <summary>
    /// Logique d'interaction pour UserControl1.xaml
    /// </summary>
    public partial class ReliqueControl : UserControl
    {
        private Relique _relique = new Relique();

        public ReliqueControl()
        {
            InitializeComponent();
        }

        public void SetRelique(string name, int cost, Bitmap iconeRelique)
        {
            _relique.Nom = name;
            _relique.Cout = cost;
            _relique.Quantite = 0;

            labelReliqueName.Content = name;
            using (MemoryStream memory = new MemoryStream())
            {
                iconeRelique.Save(memory, ImageFormat.Png);
                memory.Position = 0;
                BitmapImage bitmapImage = new BitmapImage();
                bitmapImage.BeginInit();
                bitmapImage.StreamSource = memory;
                bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                bitmapImage.EndInit();

                this.iconeRelique.Source = bitmapImage;
            }           
        }

        public Relique GetRelique()
        {
            return _relique;
        }

        public void Reset()
        {
            _relique.Quantite = 0;
            labelNumberOfItem.Content = 0;
        }

        private void buttonRemove_Click(object sender, RoutedEventArgs e)
        {
            if (_relique.Quantite != 0) _relique.Quantite--;
            labelNumberOfItem.Content = _relique.Quantite;
        }

        private void buttonAdd_Click(object sender, RoutedEventArgs e)
        {
            labelNumberOfItem.Content = ++_relique.Quantite;
        }

        public void Update()
        {
            labelNumberOfItem.Content = _relique.Quantite;
        }
    }
}

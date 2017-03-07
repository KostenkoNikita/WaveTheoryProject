using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using OxyPlot;

namespace WaveTheoryProject
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private PlotWindowModel viewModel;
        WaveController c;

        public MainWindow()
        {
            viewModel = new PlotWindowModel();
            DataContext = viewModel;
            InitializeComponent();
            c = new PlaneWaveController();
            datagrid.ItemsSource = c.WavePointsFixedX;
            datagrid.ColumnWidth = new DataGridLength(20, DataGridLengthUnitType.Star);

            x0Box.Text = Settings.Init.x0.ToString();
            z0Box.Text = Settings.Init.z0.ToString();
            aBox.Text = Settings.a.ToString();
            sigmaBox.Text = Settings.sigma.ToString();
            kBox.Text = c.k.ToString(Settings.Format);

            x0Box.TextChanged += paramBox_TextChanged;
            z0Box.TextChanged += paramBox_TextChanged;
            sigmaBox.TextChanged += paramBox_TextChanged;
            aBox.TextChanged += paramBox_TextChanged;
        }

        private void paramBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox tmp = (sender as TextBox);
            try
            {
                switch (tmp.Name)
                {
                    case "x0Box":
                        c.x0fixed = Convert.ToDouble(tmp.Text.Replace('.', ','));           
                        break;
                    case "z0Box":
                        c.z0fixed = Convert.ToDouble(tmp.Text.Replace('.', ','));
                        break;
                    case "sigmaBox":
                        c.sigma = Convert.ToDouble(tmp.Text.Replace('.', ','));
                        kBox.Text = c.k.ToString(Settings.Format);
                        break;
                    case "aBox":
                        c.a = Convert.ToDouble(tmp.Text.Replace('.', ','));
                        break;
                }
                datagrid.Items.Refresh();
            }
            catch
            {
                return;
            }
        }
    }
}

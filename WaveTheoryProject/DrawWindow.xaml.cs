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
using System.Reflection;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace WaveTheoryProject
{
    /// <summary>
    /// Логика взаимодействия для DrawWindow.xaml
    /// </summary>
    public partial class DrawWindow : Window
    {
        double t, z0, sigma, a;
        PlotWindow w;
        WaveController c;

        internal DrawWindow(PlotWindow w, WaveController c)
        {
            InitializeComponent();
            Deactivated += (sender, e) => { Close(); };
            tBox.Text = "0";
            z0Box.Text = Settings.Init.z0.ToString(Settings.Format);
            sigmaBox.Text = Settings.sigma.ToString(Settings.Format);
            aBox.Text = Settings.a.ToString(Settings.Format);
            this.w = w;
            this.c = c;
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox tmp = sender as TextBox;
            try
            {
                switch (tmp.Name)
                {
                    case "tBox":
                        t = Convert.ToDouble(tmp.Text.Replace('.', ','));
                        return;
                    case "z0Box":
                        z0 = Convert.ToDouble(tmp.Text.Replace('.', ','));
                        return;
                    case "sigmaBox":
                        sigma = Convert.ToDouble(tmp.Text.Replace('.', ','));
                        return;
                    case "aBox":
                        a = Convert.ToDouble(tmp.Text.Replace('.', ','));
                        return;
                }
            }
            catch
            {
                return;
            }
        }

        private void ico_MouseEnter(object sender, MouseEventArgs e)
        {
            okContainer.Margin = new Thickness(3, 3, 3, 3); 
        }

        private void ico_MouseLeave(object sender, MouseEventArgs e)
        {
            okContainer.Margin = new Thickness(7,7,7,7);
        }

        private void ico_MouseDown(object sender, MouseButtonEventArgs e)
        {
            WavePointsListAtTime l = c.DrawSingleWavePointsList(t, z0, sigma, a);
            w.viewModel.DrawCurve(l);
            w.PlotRefresh();
            Close();
        }

    }
}

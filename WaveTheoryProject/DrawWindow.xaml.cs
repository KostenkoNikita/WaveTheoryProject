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
        double t, z0, k, a;
        PlotWindow w;
        WaveController c;
        bool IsCanal;
        double tmp_delta, tmp_h;

        internal DrawWindow(PlotWindow w, WaveController c)
        {
            InitializeComponent();
            IsCanal = c is CanalWaveController;
            Deactivated += (sender, e) => { Close(); };
            this.Closing += (sender, e) => { if (IsCanal) { Settings.Canal.delta = Settings.InitX0To = tmp_delta; Settings.Canal.h = tmp_h; } };
            tBox.Text = "0";
            z0Box.Text = Settings.Init.z0.ToString(Settings.Format);
            if (IsCanal)
            {
                kBox.Text = Settings.Canal.h.ToString(Settings.Format);
                aBox.Text = Settings.Canal.delta.ToString(Settings.Format);
                tmp_delta = Settings.Canal.delta;
                tmp_h = Settings.Canal.h;
                kBlock.Text = "h =";
                aBlock.Text = "δ =";
                Settings.InitX0From = 0;
                Settings.InitX0To = a;
            }
            else
            {
                kBox.Text = Settings.k.ToString(Settings.Format);
                aBox.Text = Settings.a.ToString(Settings.Format);
            }
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
                        double tmp_t = Convert.ToDouble(tmp.Text.Replace('.', ','));
                        if (tmp_t >= 0)
                        {
                            t = Convert.ToDouble(tmp.Text.Replace('.', ','));
                        }
                        else
                        {
                            tBox.Text = "0";
                        }
                        return;
                    case "z0Box":
                        z0 = Convert.ToDouble(tmp.Text.Replace('.', ','));
                        if (z0 > 0 || (IsCanal && z0<-k)) { z0Box.Text = "0"; }
                        return;
                    case "kBox":
                        k = Convert.ToDouble(tmp.Text.Replace('.', ','));
                        if (IsCanal) { if (k <= 0) { kBox.Text = "1";  } else { Settings.Canal.h = k; } }
                        return;
                    case "aBox":
                        a = Convert.ToDouble(tmp.Text.Replace('.', ','));
                        if (IsCanal) { if (a <= 0) { kBox.Text = "3"; } else { Settings.Canal.delta= Settings.InitX0To = a; } }
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
            WavePointsListAtTime l = c.DrawSingleWavePointsList(t, z0, k, a);
            w.viewModel.DrawCurve(l);
            if (IsCanal)
            {
                w.viewModel.DeleteCanale();
                w.viewModel.DrawCanal();
            }
            w.PlotRefresh();
            Close();
        }

    }
}

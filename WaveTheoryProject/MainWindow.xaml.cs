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
using System.Diagnostics;
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
            WindowReferences.main = this;
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
            tminBox.Text = Settings.InitTimeFrom.ToString(Settings.Format);
            tmaxBox.Text = Settings.InitTimeTo.ToString(Settings.Format);
            hBox.Text = Settings.Time_h.ToString(Settings.Format);

            x0Box.TextChanged += paramBox_TextChanged;
            z0Box.TextChanged += paramBox_TextChanged;
            sigmaBox.TextChanged += paramBox_TextChanged;
            aBox.TextChanged += paramBox_TextChanged;
            tminBox.TextChanged += paramBox_TextChanged;
            tmaxBox.TextChanged += paramBox_TextChanged;
            hBox.TextChanged += paramBox_TextChanged;
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
                    case "tminBox":
                        double tmp_tmin = Convert.ToDouble(tmp.Text.Replace('.', ','));
                        if (tmp_tmin >= 0 && tmp_tmin < Settings.InitTimeTo)
                        {
                            Settings.InitTimeFrom = tmp_tmin;
                            c.Refresh();
                        }
                        else
                        {
                            throw new ArgumentException();
                        }
                        break;
                    case "tmaxBox":
                        double tmp_tmax = Convert.ToDouble(tmp.Text.Replace('.', ','));
                        if (tmp_tmax >= 0 && tmp_tmax > Settings.InitTimeFrom)
                        {
                            Settings.InitTimeTo = tmp_tmax;
                            c.Refresh();
                        }
                        else
                        {
                            throw new ArgumentException();
                        }
                        break;
                    case "hBox":
                        double tmp_h = Convert.ToDouble(tmp.Text.Replace('.', ','));
                        if (tmp_h > 0 && tmp_h < Settings.InitTimeTo-Settings.InitTimeFrom)
                        {
                            Settings.Time_h = tmp_h;
                            c.Refresh();
                        }
                        else
                        {
                            throw new ArgumentException();
                        }
                        break;
                }
                datagrid.Items.Refresh();
            }
            catch
            {
                return;
            }
        }

        private void ico_MouseEnter(object sender, MouseEventArgs e)
        {
            switch ((sender as Image).Name)
            {
                case "plotImage": plotContainer.Margin = new Thickness(1,1,1,1); return;
                case "exitImage": exitImage.Source = Settings.exitIcoSelectedSource; return;
            }
        }

        private void ico_MouseLeave(object sender, MouseEventArgs e)
        {
            switch ((sender as Image).Name)
            {
                case "plotImage": plotContainer.Margin = new Thickness(5,5,5,5); return;
                case "exitImage": exitImage.Source = Settings.exitIcoSource; return;
            }
        }

        private void ico_MouseDown(object sender, MouseButtonEventArgs e)
        {
            switch ((sender as Image).Name)
            {
                case "plotImage":
                    WindowReferences.plot = new PlotWindow(c);
                    WindowReferences.plot.Show();
                    Hide();
                    break;
                case "exitImage": Process.GetCurrentProcess().Kill(); return;
            }
        }
    }
}

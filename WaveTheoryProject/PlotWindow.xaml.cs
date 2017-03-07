using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

using OxyPlot;

namespace WaveTheoryProject
{
    /// <summary>
    /// Логика взаимодействия для PlotWindow.xaml
    /// </summary>
    public partial class PlotWindow : Window
    {
        private PlotWindowModel viewModel;
        WaveController c;
        double currentTime = 0;
        double abs_increment = Settings.Time_h;

        public PlotWindow()
        {
            c = WindowReferences.contoller;//НЕ ЗАБУДЬ УДАЛИТЬ
            viewModel = new PlotWindowModel();
            DataContext = viewModel;
            InitializeComponent();
            plot.Controller = new PlotController();
            plot.Controller.UnbindMouseDown(OxyMouseButton.Left);
            while (!c.IsDrawingAvaliable) { }
            c.DrawWaveAtTime(0, viewModel);
            PlotRefresh();
        }

        private void PlotRefresh()
        {
            plot.InvalidatePlot(true);
        }

        private void PlotClear()
        {
            viewModel.Clear();
        }

        private void tBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            PlotClear();
            c.DrawWaveAtTime(currentTime, viewModel);
            PlotRefresh();
        }

        private void upButton_Click(object sender, RoutedEventArgs e)
        {
            if ((currentTime += abs_increment) <= Settings.InitTimeTo)
            {
                tBox.Text = currentTime.ToString(Settings.Format);
            }
            else
            {
                currentTime -= abs_increment;
            }
        }

        private void downButton_Click(object sender, RoutedEventArgs e)
        {
            if ((currentTime -= abs_increment) >= Settings.InitTimeFrom)
            {
                tBox.Text = currentTime.ToString(Settings.Format);
            }
            else
            {
                currentTime += abs_increment;
            }
        }

        private void playButton_Click(object sender, RoutedEventArgs e)
        {
            Thread t = new Thread(ThreadStart);
            t.Start(new Action(()=> {if ((currentTime += abs_increment) <= Settings.InitTimeTo){tBox.Text = currentTime.ToString(Settings.Format);}else{currentTime -= abs_increment;}}));
        }

        void ThreadStart(object obj)
        {
            Action act = obj as Action;
            int ms = (int)(abs_increment * 1000);
            while (currentTime <= Settings.InitTimeTo - abs_increment)
            {
                Thread.Sleep(ms);
                Dispatcher.Invoke(act);
            }
        }
    }
}

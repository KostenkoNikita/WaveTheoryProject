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
        internal PlotWindowModel viewModel;
        WaveController c;
        double currentTime;
        double abs_increment = Settings.Time_h;
        bool isCanal;
        Thread t;

        internal PlotWindow(WaveController c)
        {
            this.c = c;
            isCanal = c is CanalWaveController;
            viewModel = new PlotWindowModel();
            if (c is CanalWaveController) { viewModel.DrawCanal(); }
            t = new Thread(ThreadStart);
            DataContext = viewModel;
            InitializeComponent();
            drawImage.InvalidateVisual();
            plot.Controller = new PlotController();
            plot.Controller.UnbindMouseDown(OxyMouseButton.Left);
            while (!c.IsDrawingAvaliable) { }
            currentTime = 0;
            tBox.Text = "0";
        }

        internal void PlotRefresh()
        {
            plot.InvalidatePlot(true);
        }

        private void tBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            c.DrawWaveAtTime(currentTime, viewModel);
            if (isCanal)
            {
                viewModel.DeleteCanale();
                viewModel.DrawCanal();
            }
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

        void ThreadStart(object obj)
        {
            Action act = obj as Action;
            Action closeAct = () => { tBox.Text = "0"; };
            while (currentTime <= Settings.InitTimeTo - abs_increment)
            {
                Thread.Sleep(20);
                while (!c.WaveInTimeWasBuilt)
                {
                }
                Dispatcher.Invoke(act);
            }
            currentTime = 0;
            Dispatcher.Invoke(closeAct);
        }

        private void ico_MouseEnter(object sender, MouseEventArgs e)
        {
            switch ((sender as Image).Name)
            {
                case "menuImage": menuContainer.Margin = new Thickness(3,3,3,3); return;
                case "playImage": playContainer.Margin = new Thickness(14,14,14,14); return;
                case "pauseImage": pauseContainer.Margin = new Thickness(3,3,3,3); return;
                case "drawImage": drawContainer.Margin = new Thickness(3, 3, 3, 3); return;
            }
        }

        private void ico_MouseLeave(object sender, MouseEventArgs e)
        {
            switch ((sender as Image).Name)
            {
                case "menuImage": menuContainer.Margin = new Thickness(7,7,7,7); return;
                case "playImage": playContainer.Margin = new Thickness(18,18,18,18); return;
                case "pauseImage": pauseContainer.Margin = new Thickness(7,7,7,7); return;
                case "drawImage": drawContainer.Margin = new Thickness(7,7,7,7); return;
            }
        }

        private void ico_MouseDown(object sender, MouseButtonEventArgs e)
        {
            switch ((sender as Image).Name)
            {
                case "menuImage":
                    WindowReferences.main.Show();
                    viewModel = null;
                    if (t.IsAlive) { t.Abort(); }
                    t = null;
                    Close();
                    return;
                case "playImage":
                    if (t.IsAlive)
                    {
                        t.Abort();
                        t = new Thread(ThreadStart);
                        currentTime = Settings.InitTimeFrom;
                    }
                    else
                    {
                        t = new Thread(ThreadStart);
                    }
                    t.Start(new Action(() => { if ((currentTime += abs_increment) <= Settings.InitTimeTo) { tBox.Text = currentTime.ToString(Settings.Format); } else { currentTime -= abs_increment; } }));
                    return;
                case "pauseImage":
                    t.Abort();
                    return;
                case "drawImage":
                    DrawWindow dw = new DrawWindow(this,c);
                    dw.Show();
                    return;
            }
        }
    }
}

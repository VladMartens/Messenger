using System;
using System.Windows;
using System.Threading;

namespace WpfApplication1
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class Window1 : Window
    {
        Thread t = null;
        /// <summary>
        /// The default constructor that launches the "Program Strat" ​​function in the new thread
        /// </summary>
        public Window1()
        {
            InitializeComponent();
            try
            {
                t = new Thread(StartProgram);
                t.Start();
            }
            catch(Exception ex)
            { MessageBox.Show(ex.Message); }
        }

        /// <summary>
        /// The function responsible for the animation before running the program
        /// </summary>
        private void StartProgram()
        {
            try
            {
                double i = 0;
                while (i < 1)
                {
                    Action act = new Action(() =>
                    {
                        this.Visual_Studio_2013_Logo_svg_png.Opacity = i;
                        this.textBox.Opacity = i;
                    });
                    Thread.Sleep(15);
                    i += 0.005;
                    Dispatcher.Invoke(act);
                }
                Thread.Sleep(500);
                while (i > 0)
                {
                    Action act = new Action(() =>
                    {
                        this.Visual_Studio_2013_Logo_svg_png.Opacity = i;
                        this.textBox.Opacity = i;
                    });
                    Thread.Sleep(15);
                    i -= 0.005;
                    Dispatcher.Invoke(act);
                }
                Thread.Sleep(500);
                while (i < 1)
                {
                    Action act = new Action(() => this.sql_logo_png.Opacity = i);
                    Thread.Sleep(15);
                    i += 0.005;
                    Dispatcher.Invoke(act);
                }
                Thread.Sleep(500);
                while (i > 0)
                {
                    Action act = new Action(() => this.sql_logo_png.Opacity = i);
                    Thread.Sleep(15);
                    i -= 0.005;
                    Dispatcher.Invoke(act);
                }
                Thread.Sleep(500);
                while (i < 1)
                {
                    Action act = new Action(() =>
                    {
                        this.Image1_png.Opacity = i;
                        this.textBox_Copy.Opacity = i;
                    });
                    Thread.Sleep(15);
                    i += 0.005;
                    Dispatcher.Invoke(act);
                }
                Action close = new Action(() => Close());
                Dispatcher.Invoke(close);
            }
            catch
            {  }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            t.Abort();
        }
    }
}


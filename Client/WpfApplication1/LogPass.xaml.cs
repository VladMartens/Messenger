using System;
using System.Text;
using System.Windows;
using System.Net;
using System.Net.Sockets;

namespace WpfApplication1
{
    /// <summary>
    /// Interaction logic for LogPass.xaml
    /// </summary>
    public partial class LogPass : Window
    {
        /// <summary>
        /// Default constructor
        /// </summary>
        public LogPass()
        {
            InitializeComponent();

        }

        /// <summary>
        /// initialize a new window and open it
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button_Click_Registr(object sender, RoutedEventArgs e)
        {
            try
            {
                Registr reg = new Registr();
                reg.ShowDialog();
            }
            catch (Exception ex)
            { MessageBox.Show(ex.Message); }
        }

        /// <summary>
        /// checking for user input
        /// connection to the server and sending the command code and command text
        /// receive from the server a string that stores information about the user
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button_Click_Enter(object sender, RoutedEventArgs e)
        {
            try
            {
                if (textBox.Text.Length == 0  || passwordBox.Password.Length == 0)
                    throw new Exception("Введите логин и пароль");
              

                TcpClient client = new TcpClient();
                client.Connect(IPAddress.Parse(MainWindow.ip_adres_serser), 10000);
                var buf = Encoding.Unicode.GetBytes(string.Format("001{0}\r\n{1}",textBox.Text, passwordBox.Password));

                NetworkStream s = client.GetStream();
                s.Write(buf, 0, buf.Length);

                buf = new byte[4096];
                var iReadServ = s.Read(buf, 0, buf.Length);
                string str = Encoding.Unicode.GetString(buf, 0, iReadServ);

                string[] stringSeparators = new string[] { "\r\n" };
                string[] id_name_surname = str.Split(stringSeparators, StringSplitOptions.None);
                MainWindow.id_user = int.Parse(id_name_surname[0]);
                MainWindow.name_user = id_name_surname[1].Trim(' ');
                MainWindow.surname_user = id_name_surname[2].Trim(' ');

                if (MainWindow.id_user == 0 || MainWindow.name_user == null || MainWindow.surname_user == null)
                    throw new Exception("Неверный логин и пароль, введите свои данные повторно и повторите попытку");
                

                s.Close();
                s = null;

                MainWindow.close = true;
                this.Close();
            }
            catch(Exception ex)
            { MessageBox.Show(ex.Message); }
        }

    }
}


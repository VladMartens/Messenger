using System;
using System.Text;
using System.Windows;
using System.Net;
using System.Net.Sockets;

namespace WpfApplication1
{
    /// <summary>
    /// Interaction logic for Registr.xaml
    /// </summary>
    public partial class Registr : Window
    {
        /// <summary>
        /// Default constructor
        /// </summary>
        public Registr()
        {
            InitializeComponent();
        }

        /// <summary>
        /// The button which sends the entered login and password to the server and checks the matches
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (textBox.Text.Length == 0 || 
                    textBox_Copy.Text.Length == 0 || 
                    textBox_Copy1.Text.Length == 0 || 
                    textBox_Copy2.Text.Length == 0)
                    throw new Exception("Заполните все поля и повторте попытку");
                if (textBox_Copy1.Text.Length > 15)
                    throw new Exception("Длинна логина не должна привышать 15 символов");
                if (textBox_Copy2.Text.Length > 20)
                    throw new Exception("Длинна пароля не должна привышать 20 символов");


                TcpClient client = new TcpClient();
                client.Connect(IPAddress.Parse(MainWindow.ip_adres_serser), 10000);
                var buf = Encoding.Unicode.GetBytes(string.Format("002{0}\r\n{1}\r\n{2}\r\n{3}",
                    textBox.Text, 
                    textBox_Copy.Text, 
                    textBox_Copy1.Text, 
                    textBox_Copy2.Text));

                NetworkStream s = client.GetStream();
                s.Write(buf, 0, buf.Length);

                buf = new byte[4096];
                var iReadServ = s.Read(buf, 0, buf.Length);
                string str = Encoding.Unicode.GetString(buf, 0, iReadServ);

                MessageBox.Show(str);

                s.Close();
                s = null;
            }
            catch (Exception ex)
            { MessageBox.Show(ex.Message); }
        }
    }
}

using System;
using System.Text;
using System.Windows;
using System.Windows.Input;
using System.Net;
using System.Net.Sockets;

namespace WpfApplication1
{
    /// <summary>
    /// Логика взаимодействия для Message.xaml
    /// </summary>
    public partial class Message : Window
    {
        /// <summary>
        /// private variables used in this class
        /// </summary>
        private string NameChat =null;
        private string myid = null;

        /// <summary>
        /// default constructor
        /// Calls the function "show messages"
        /// initialization of the timer, setting the tripping period, and starting
        /// </summary>
        /// <param name="nameChat"></param>
        /// <param name="FriendName"></param>
        /// <param name="FriendSurname"></param>
        /// <param name="MyId"></param>
        public Message(string nameChat,string FriendName,string FriendSurname,string MyId)
        {
            InitializeComponent();
            try
            {
                NameChat = nameChat;
                myid = MyId;
                this.Title = FriendName + " " + FriendSurname;
                ViewMessage(nameChat, MyId);

                System.Windows.Threading.DispatcherTimer dispatcherTimer = new System.Windows.Threading.DispatcherTimer();
                dispatcherTimer.Tick += dispatcherTimer_Tick;
                dispatcherTimer.Interval = new TimeSpan(0, 0, 1);
                dispatcherTimer.Start();
            }
            catch(Exception ex)
            { MessageBox.Show(ex.Message); }
        }

        /// <summary>
        /// update messages by timer tick
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dispatcherTimer_Tick(object sender, EventArgs e)
        {
            try
            { ViewMessage(NameChat, myid); }
            catch(Exception ex)
            { MessageBox.Show(ex.Message); }
            
        }

        /// <summary>
        /// The function of updating messages
        /// connection to the server and sending the command code and name chat
        /// receiving a list of massage from the server as a single line
        /// line split and output in listview
        /// </summary>
        /// <param name="nameChat"></param>
        /// <param name="MyId"></param>
        public void ViewMessage(string nameChat,string MyId)
        {
            try
            {
                Messagelist.Items.Clear();
                TcpClient client = new TcpClient();
                client.Connect(IPAddress.Parse(MainWindow.ip_adres_serser), 10000);
                var buf = Encoding.Unicode.GetBytes(string.Format("008{0}", nameChat));

                NetworkStream s = client.GetStream();
                s.Write(buf, 0, buf.Length);

                buf = new byte[4096];
                var iReadServ = s.Read(buf, 0, buf.Length);
                string str = Encoding.Unicode.GetString(buf, 0, iReadServ);

                string[] stringSeparators = new string[] { "\\" };
                string[] SplitParam = new string[] { "_" };
                string[] Message = str.Split(stringSeparators, StringSplitOptions.None);

                string[] text_idsernder = null;
                int id_sender = 0;
                string message = null;

                Messagelist.Items.Clear();

                for (int i = 0; i < Message.Length - 1; i++)
                {
                    text_idsernder = Message[i].Split(SplitParam, StringSplitOptions.None);
                    message = text_idsernder[0];
                    id_sender = int.Parse(text_idsernder[1]);

                    message = message.Replace("&&", "\r\n");
                    if (id_sender == int.Parse(MyId))
                        Messagelist.Items.Add(new Text { Friend = null, Name = null, My = message });

                    else
                        Messagelist.Items.Add(new Text { Friend = message, Name = null, My = null });
                }
                s.Close();
                s = null;
            }
            catch(Exception ex)
            { MessageBox.Show(ex.Message); }
        }

        /// <summary>
        /// sending a message by pressing the Enter key
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Messagebox_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {

                if (e.Key == Key.Enter)
                    Button_Click(sender, null);
            }
            catch(Exception ex)
            { MessageBox.Show(ex.Message); }
        }

        /// <summary>
        /// sending a message on the button click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                TcpClient client = new TcpClient();
                client.Connect(IPAddress.Parse(MainWindow.ip_adres_serser), 10000);

                string textmessage = this.Messagebox.Text;
                int count_newLine = textmessage.Length / 30;
                string newtext = null;
                int count = 30;
                string[] mas = textmessage.Split(' ');
                for (int i = 0; i < mas.Length; i++)
                {
                    if ((newtext + mas[i]).Length > count)
                    {
                        newtext += "&&" + mas[i] + " ";
                        count += 30;
                    }
                    else
                        newtext += (mas[i] + " ");
                }
                var buf = Encoding.Unicode.GetBytes(string.Format("009{0}\\{1}\\{2}", NameChat, myid, newtext));

                NetworkStream s = client.GetStream();
                s.Write(buf, 0, buf.Length);

                buf = new byte[4096];
                var iReadServ = s.Read(buf, 0, buf.Length);
                string str = Encoding.Unicode.GetString(buf, 0, iReadServ);
                if (int.Parse(str) != 1)
                {
                    MessageBox.Show("Сообщение не отправилось, повторите попытку");
                }
                s.Close();
                s = null;

                ViewMessage(NameChat, myid);
                Messagebox.Clear();
            }
            catch(Exception ex)
            { MessageBox.Show(ex.Message); }
        }
    }
}

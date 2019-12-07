using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Net;
using System.Net.Sockets;
using System.IO;

namespace WpfApplication1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        /// <summary>
        /// static variables
        /// </summary>
        static public bool close;
        static public int id_user = 0;
        static public string name_user = null;
        static public string surname_user = null;
        static public string ip_adres_serser = null;
        

        /// <summary>
        /// The default constructor that calls the registration window
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();
            try
            {
                StreamReader sr = new StreamReader("setting_server.txt");
                ip_adres_serser = sr.ReadLine().Remove(0, 11);
                Window1 w1 = new Window1();
                w1.ShowDialog();
                LogPass lp = new LogPass();
                lp.ShowDialog();
                if (close == false)
                    this.Close();
                else
                {
                    lable1.Content = name_user + " " + surname_user;
                    MessageBox.Show(string.Format("Добро пожаловать {0} {1} ", name_user, surname_user));
                    ShowMessage();
                    TabController.SelectedIndex = 0;
                    TabController.SelectedItem = Message;
                    this.MyId.Content = "Мой Id: " + MainWindow.id_user;
                }
               
            }
            catch(FileNotFoundException)
            {
                FileStream fs = new FileStream("seting_server.txt", FileMode.OpenOrCreate);
                byte[] buf_seting_file = new byte[4096];
                buf_seting_file = Encoding.Unicode.GetBytes("ip server: 192.168.2.103");
                fs.Write(buf_seting_file, 0, buf_seting_file.Length);
                ip_adres_serser = "192.168.2.103";
                Window1 w1 = new Window1();
                w1.ShowDialog();
                LogPass lp = new LogPass();
                lp.ShowDialog();
                if (close == false)
                    this.Close();
                else
                {
                    lable1.Content = name_user + " " + surname_user;
                    MessageBox.Show(string.Format("Добро пожаловать {0} {1} ", name_user, surname_user));
                    ShowMessage();
                    TabController.SelectedIndex = 0;
                    TabController.SelectedItem = Message;
                    this.MyId.Content = "Мой Id: " + MainWindow.id_user;
                }
            }
            catch(DirectoryNotFoundException)
            {
                FileStream fs = new FileStream("seting_server.txt", FileMode.OpenOrCreate);
                byte[] buf_seting_file = new byte[4096];
                buf_seting_file = Encoding.Unicode.GetBytes("ip server: 192.168.2.103");
                fs.Write(buf_seting_file, 0, buf_seting_file.Length);
                ip_adres_serser = "192.168.2.103";
                Window1 w1 = new Window1();
                w1.ShowDialog();
                LogPass lp = new LogPass();
                lp.ShowDialog();
                if (close == false)
                    this.Close();
                else
                {
                    lable1.Content = name_user + " " + surname_user;
                    MessageBox.Show(string.Format("Добро пожаловать {0} {1} ", name_user, surname_user));
                    ShowMessage();
                    TabController.SelectedIndex = 0;
                    TabController.SelectedItem = Message;
                    this.MyId.Content = "Мой Id: " + MainWindow.id_user;
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
                this.Close();
            }

        }

        /// <summary>
        /// a "message" button that calls the message list update function
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button1_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                this.button2.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF4276A4"));
                this.button3.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF4276A4"));
                this.button1.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF2460FF"));
                this.ComboSerch.Visibility = Visibility.Hidden;
                this.SettingFind.Visibility = Visibility.Hidden;
                this.TextSerch.Visibility = Visibility.Hidden;
                this.TextFind.Visibility = Visibility.Hidden;
                this.MyId.Visibility = Visibility.Hidden;
                this.buttonsearch.Visibility = Visibility.Hidden;
                ShowMessage();
                TabController.SelectedItem = TabController.Items[0];
            }
            catch (Exception ex)
            { MessageBox.Show(ex.Message); }
        }

        /// <summary>
        /// "find friend" button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button3_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                this.button2.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF4276A4"));
                this.button1.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF4276A4"));
                this.button3.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF2460FF"));
                this.ComboSerch.Visibility = Visibility.Visible;
                this.SettingFind.Visibility = Visibility.Visible;
                this.TextSerch.Visibility = Visibility.Visible;
                this.TextFind.Visibility = Visibility.Visible;
                this.MyId.Visibility = Visibility.Visible;
                this.buttonsearch.Visibility = Visibility.Visible;
                lvFindFriend.Items.Clear();
                TabController.SelectedItem = TabController.Items[2];
            }
            catch (Exception ex)
            { MessageBox.Show(ex.Message); }
        }

        /// <summary>
        /// a "friends" button that calls the friend list update function
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button2_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                this.button3.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF4276A4"));
                this.button1.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF4276A4"));
                this.button2.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF2460FF"));
                this.ComboSerch.Visibility = Visibility.Hidden;
                this.SettingFind.Visibility = Visibility.Hidden;
                this.TextSerch.Visibility = Visibility.Hidden;
                this.TextFind.Visibility = Visibility.Hidden;
                this.MyId.Visibility = Visibility.Hidden;
                this.buttonsearch.Visibility = Visibility.Hidden;
                ShowFrieds();
                TabController.SelectedItem = TabController.Items[1];
            }
            catch (Exception ex)
            { MessageBox.Show(ex.Message); }

        }

        /// <summary>
        /// button "about" which opens a window about
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                AboutBox1 aboutBox = new AboutBox1();
                aboutBox.ShowDialog();
            }
            catch (Exception ex)
            { MessageBox.Show(ex.Message); }

        }

        /// <summary>
        /// The button that adds to the selected user's friends
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button_Click_add(object sender, RoutedEventArgs e)
        {
            try
            {
                User u = (User)this.lvFindFriend.SelectedItem;
                this.lvFindFriend.SelectedItem = u;
                this.lvFindFriend.Items[this.lvFindFriend.SelectedIndex] = new User { Name = ((User)this.lvFindFriend.SelectedItem).Name };

                if (int.Parse(u.Id) == id_user)
                    throw new Exception("Вы не можете добавить сами себя в друзья");

                TcpClient client = new TcpClient();
                client.Connect(IPAddress.Parse(MainWindow.ip_adres_serser), 10000);

                var buf = Encoding.Unicode.GetBytes(string.Format("005{0}\r\n{1}", u.Id, id_user));

                NetworkStream s = client.GetStream();
                s.Write(buf, 0, buf.Length);

                buf = new byte[4096];
                var iReadServ = s.Read(buf, 0, buf.Length);
                string str = Encoding.Unicode.GetString(buf, 0, iReadServ);


                button2_Click(sender, null);
                MessageBox.Show(str);

                s.Close();
                s = null;
            }
            catch (Exception ex)
            { MessageBox.Show(ex.Message); }

        }

        /// <summary>
        /// create / open a message button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// 
        private void message_add(object sender, RoutedEventArgs e)
        {
            try
            {
                User u = (User)this.lvOrders.SelectedItem;
                this.lvOrders.SelectedItem = u;
                this.lvOrders.Items[this.lvOrders.SelectedIndex] = new User { Name = ((User)this.lvOrders.SelectedItem).Name };

                TcpClient client = new TcpClient();
                client.Connect(IPAddress.Parse(MainWindow.ip_adres_serser), 10000);
                var buf = Encoding.Unicode.GetBytes(string.Format("006{0}\r\n{1}", id_user, u.Id));

                NetworkStream s = client.GetStream();
                s.Write(buf, 0, buf.Length);

                buf = new byte[4096];
                var iReadServ = s.Read(buf, 0, buf.Length);
                string str = Encoding.Unicode.GetString(buf, 0, iReadServ);

                Message m = new Message(str, u.Name, u.Surname, id_user.ToString());
                m.Show();
                button2_Click(sender, null);
            }
            catch (Exception ex)
            { MessageBox.Show(ex.Message); }


        }

        /// <summary>
        /// update friends list function
        /// connection to the server and sending the command code and user id
        /// receiving a list of friends from the server as a single line
        /// line split and output in listview
        /// </summary>
        private void ShowFrieds()
        {
            try
            {
                TcpClient client = new TcpClient();
                client.Connect(IPAddress.Parse(MainWindow.ip_adres_serser), 10000);
                var buf = Encoding.Unicode.GetBytes(string.Format("003{0}", id_user));

                NetworkStream s = client.GetStream();
                s.Write(buf, 0, buf.Length);

                buf = new byte[4096];
                var iReadServ = s.Read(buf, 0, buf.Length);
                string str = Encoding.Unicode.GetString(buf, 0, iReadServ);


                string[] stringSeparators = new string[] { "\r\n" };
                string[] SplitParam = new string[] { " " };
                string[] id_name_surname = str.Split(stringSeparators, StringSplitOptions.None);
                if (id_name_surname.Length <= 1)
                    throw new Exception("Ваш список друзей на данный момент пуст.\r\nВоспользуйтесь поиском друзей что бы добавить людей в список друзей");
                string[] id_name_surname2 = null;
                List<int> id = new List<int>();
                List<string> name = new List<string>();
                List<string> surname = new List<string>();

                lvOrders.Items.Clear();
                for (int i = 0; i < id_name_surname.Length - 1; i++)
                {
                    id_name_surname2 = id_name_surname[i].Split(SplitParam, StringSplitOptions.None);
                    id.Add(int.Parse(id_name_surname2[0]));
                    name.Add(id_name_surname2[1]);
                    surname.Add(id_name_surname2[2]);

                    lvOrders.Items.Add(new User { Id = string.Format("{0}", id[i]), Name = string.Format("{0} {1}", name[i], surname[i]) });
                }

                s.Close();
                s = null;
            }
            catch (Exception ex)
            { MessageBox.Show(ex.Message); }

        }

        /// <summary>
        /// call the "sort friends" function, if the text box is active and the Enter key is pressed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TextSerch_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.Key == Key.Enter)
                    SerchFriends();
            }
            catch (Exception ex)
            { MessageBox.Show(ex.Message); }

        }

        /// <summary>
        /// search function for new friends by specified parameters
        /// </summary>
        private void SerchFriends()
        {
            try
            {
                TcpClient client = new TcpClient();
                client.Connect(IPAddress.Parse(MainWindow.ip_adres_serser), 10000);
                int SerchParam = this.ComboSerch.SelectedIndex;

                var buf = Encoding.Unicode.GetBytes(string.Format("004{0}\r\n{1}", SerchParam, this.TextSerch.Text));

                NetworkStream s = client.GetStream();
                s.Write(buf, 0, buf.Length);

                buf = new byte[4096];
                var iReadServ = s.Read(buf, 0, buf.Length);
                string str = Encoding.Unicode.GetString(buf, 0, iReadServ);


                string[] stringSeparators = new string[] { "\r\n" };
                string[] SplitParam = new string[] { " " };
                string[] id_name_surname = str.Split(stringSeparators, StringSplitOptions.None);
                if (id_name_surname.Length <= 1)
                    throw new Exception("Пользователей с такими параметрами нету");
                string[] id_name_surname2 = null;
                List<int> id = new List<int>();
                List<string> name = new List<string>();
                List<string> surname = new List<string>();

                lvFindFriend.Items.Clear();
                for (int i = 0; i < id_name_surname.Length - 1; i++)
                {
                    id_name_surname2 = id_name_surname[i].Split(SplitParam, StringSplitOptions.None);
                    id.Add(int.Parse(id_name_surname2[0]));
                    name.Add(id_name_surname2[1]);
                    surname.Add(id_name_surname2[2]);

                    lvFindFriend.Items.Add(new User { Id = string.Format("{0}", id[i]), Name = string.Format("{0} {1}", name[i], surname[i]) });
                }

                s.Close();
                s = null;
            }
            catch (Exception ex)
            { MessageBox.Show(ex.Message); }
        }

        /// <summary>
        /// the function of finding new friends by clicking on the button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonsearch_Click(object sender, RoutedEventArgs e)
        {
            try
            { SerchFriends(); }
            catch (Exception ex)
            { MessageBox.Show(ex.Message); }

        }

        /// <summary>
        /// function that displays a list of messages
        /// connection to the server and sending the command code and user id
        /// receiving a list of massage from the server as a single line
        /// line split and output in listview
        /// </summary>
        private void ShowMessage()
        {
            try
            {
                TcpClient client = new TcpClient();
                client.Connect(IPAddress.Parse(MainWindow.ip_adres_serser), 10000);
                var buf = Encoding.Unicode.GetBytes(string.Format("007{0}", id_user));

                NetworkStream s = client.GetStream();
                s.Write(buf, 0, buf.Length);

                buf = new byte[4096];
                var iReadServ = s.Read(buf, 0, buf.Length);
                string str = Encoding.Unicode.GetString(buf, 0, iReadServ);


                string[] stringSeparators = new string[] { "\r\n" };
                string[] SplitParam = new string[] { " " };
                string[] id_name_surname = str.Split(stringSeparators, StringSplitOptions.None);
                if (id_name_surname.Length <= 1)
                    throw new Exception("Ваш список сообщений на данный момент пуст.\r\nВоспользуйтесь списком друзей что бы начать диалог");
                string[] id_name_surname2 = null;
                List<int> id = new List<int>();
                List<string> name = new List<string>();
                List<string> surname = new List<string>();

                Messagelist.Items.Clear();
                for (int i = 0; i < id_name_surname.Length - 1; i++)
                {
                    id_name_surname2 = id_name_surname[i].Split(SplitParam, StringSplitOptions.None);
                    id.Add(int.Parse(id_name_surname2[0]));
                    name.Add(id_name_surname2[1]);
                    surname.Add(id_name_surname2[2]);

                    Messagelist.Items.Add(new User { Id = string.Format("{0}", id[i]), Name = string.Format("{0} {1}", name[i], surname[i]) });
                }

                s.Close();
                s = null;
            }
            catch (Exception ex)
            { MessageBox.Show(ex.Message); }

        }

        /// <summary>
        /// Open the "message" window and send it the following parameters:
        /// name "room", friend's name and surname, user id
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MessageOpen(object sender, RoutedEventArgs e)
        {
            try
            {
                User u = (User)this.Messagelist.SelectedItem;
                this.Messagelist.SelectedItem = u;
                this.Messagelist.Items[this.Messagelist.SelectedIndex] = new User { Name = ((User)this.Messagelist.SelectedItem).Name };
                Message m = null;
                if (id_user < int.Parse(u.Id))
                { m = new Message(string.Format("Message_{0}_{1}", id_user, u.Id), u.Name, u.Surname, id_user.ToString()); }
                else
                { m = new Message(string.Format("Message_{0}_{1}", u.Id, id_user), u.Name, u.Surname, id_user.ToString()); }
                m.Show();
                button1_Click(sender, null);
            }
            catch (Exception ex)
            { MessageBox.Show(ex.Message); }

        }
    }
}

using System;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.Data.SqlClient;
using System.Data.Common;
using System.IO;

namespace Server
{
    /// <summary>
    /// class Server
    /// in it there is a connection of the client and work with his requests
    /// </summary>
    class ServConnect
    {
        /// <summary>
        /// constant string of the database connection
        /// </summary>
        public const string strConnectionString = "Data Source=tcp:server2; Initial Catalog=VladMartens; Integrated Security=false; UID=sa; PWD=ghbvf";

        /// <summary>
        /// Asynchronous function that accepts the client and processes its requests
        /// </summary>
        public async void Connect()
        {
            try
            {
                DbProviderFactory factory = DbProviderFactories.GetFactory("System.Data.SqlClient");
                factory.CreateConnection();
                using (SqlConnection conn = new SqlConnection(ServConnect.strConnectionString))
                {
                    TcpListener serv = new TcpListener(IPAddress.Any, 10000);
                    serv.Start();
                    while (true)
                    {
                       
                        conn.ConnectionString = ServConnect.strConnectionString;
                        conn.Open();
                        DbCommand comm = factory.CreateCommand();
                        comm.Connection = conn;

                        TcpClient client = await Task.Run(() => serv.AcceptTcpClient());

                        byte[] buf = new byte[4096];
                        var iRead = client.GetStream().Read(buf, 0, buf.Length);

                        string[] stringSeparators = new string[] { "\r\n" };
                        string str = Encoding.Unicode.GetString(buf, 0, iRead);

                        int codding = int.Parse(str.Remove(3));
                        str = str.Remove(0, 3);
                        Console.WriteLine("\r\n" + codding + " " + str);

                        /// login to the program with a username and password
                        if (codding == 001)
                        {
                            try
                            {
                                string[] log_pass = str.Split(stringSeparators, StringSplitOptions.None);
                                string log = log_pass[0];
                                string pass = log_pass[1];
                                Console.WriteLine(log + " " + pass); 

                                comm.CommandText = string.Format("Select id,name,surname from Users  where Users.login='{0}' AND Users.pass = '{1}'",
                                    log,pass);
                                DbDataReader reader = comm.ExecuteReader();
                                int id_user = 0;
                                string Name_user = null;
                                string Surname_user = null;
                                
                                while (reader.Read())
                                {
                                    id_user = int.Parse(reader.GetValue(0).ToString());
                                    Name_user = reader.GetValue(1).ToString();
                                    Surname_user = reader.GetValue(2).ToString();
                                }

                                buf = Encoding.Unicode.GetBytes(string.Format("{0}\r\n{1}\r\n{2}", id_user, Name_user, Surname_user));
                                client.GetStream().Write(buf, 0, buf.Length);
                                conn.Close();
                            }
                            catch (Exception ex)
                            {
                                buf = Encoding.Unicode.GetBytes(string.Format("Произошла ошибка: {0}", ex.Message));
                                client.GetStream().Write(buf, 0, buf.Length);
                                conn.Close();
                            }
                        }

                        /// New User Registration
                        if (codding == 002)
                        {
                            try
                            {
                                string[] name_surname_log_pass = str.Split(stringSeparators, StringSplitOptions.None);
                                string name = name_surname_log_pass[0];
                                string surname = name_surname_log_pass[1];
                                string log = name_surname_log_pass[2];
                                string pass = name_surname_log_pass[3];

                                comm.CommandText = string.Format("Select login from Users  where Users.login='{0}'", log);

                                string logg = null;
                                DbDataReader reader = comm.ExecuteReader();
                                while (reader.Read())
                                { logg = reader.GetValue(0).ToString(); }
                                reader.Close();
                                reader = null;

                                if (logg != null)
                                    throw new Exception("Пользователь с таким лоогином уже существует");

                                comm.CommandText = string.Format("INSERT INTO Users (name, surname, login, pass) VALUES ('{0}', '{1}', '{2}', '{3}');",
                                    name, surname, log, pass);
                                DbDataReader reader1 = comm.ExecuteReader();
                                reader1.Close();
                                reader1 = null;

                                comm.CommandText = string.Format("Select id,name,surname from Users  where Users.login='{0}' AND Users.pass = '{1}'", 
                                    log, pass);
                                DbDataReader proverka = comm.ExecuteReader();


                                int id_user = 0;
                                string Name_user = null;
                                string Surname_user = null;

                                while (proverka.Read())
                                {
                                    id_user = int.Parse(proverka.GetValue(0).ToString());
                                    Name_user = proverka.GetValue(1).ToString();
                                    Surname_user = proverka.GetValue(2).ToString();
                                }
                                if (id_user == 0 || Name_user == null || Surname_user == null)
                                    throw new Exception("Не удалось зарегестрировать пользователя");

                                proverka.Close();
                                proverka = null;

                                comm.CommandText = string.Format("CREATE TABLE Friends{0} (id INT NOT NULL PRIMARY KEY,name nchar(15) NOT NULL,surname nchar(15) NOT NULL)", 
                                    id_user);
                                DbDataReader CreatreTable = comm.ExecuteReader();
                                CreatreTable.Close();
                                CreatreTable = null;

                                comm.CommandText = string.Format("CREATE TABLE Rooms{0} (id_Friend INT NOT NULL PRIMARY KEY)", id_user);
                                DbDataReader CreatreTableMessage = comm.ExecuteReader();
                                CreatreTableMessage.Close();
                                CreatreTableMessage = null;

                                buf = Encoding.Unicode.GetBytes("Поздравляем, вы успешно зарегестрировались");
                                client.GetStream().Write(buf, 0, buf.Length);
                                conn.Close();
                            }
                            catch (Exception ex)
                            {
                                buf = Encoding.Unicode.GetBytes(string.Format("Произошла ошибка: {0}", ex.Message));
                                client.GetStream().Write(buf, 0, buf.Length);
                                conn.Close();
                            }
                        }

                        /// Sending a list of friends to the user
                        if (codding == 003)
                        {
                            try
                            {
                                comm.CommandText = string.Format("Select id,name,surname from Friends{0}", str);

                                string lstfriens = null;

                                DbDataReader reader = comm.ExecuteReader();
                                while (reader.Read())
                                {
                                    lstfriens += string.Format("{0} {1} {2}\r\n", reader.GetValue(0).ToString().Trim(' '), reader.GetValue(1).ToString().Trim(' '), reader.GetValue(2).ToString().Trim(' '));
                                }
                                if (lstfriens == null)
                                    lstfriens = "0";
                                buf = Encoding.Unicode.GetBytes(lstfriens);
                                client.GetStream().Write(buf, 0, buf.Length);
                                conn.Close();
                                reader.Close();
                            }
                            catch (Exception ex)
                            {
                                buf = Encoding.Unicode.GetBytes(string.Format("Произошла ошибка: {0}", ex.Message));
                                client.GetStream().Write(buf, 0, buf.Length);
                                conn.Close();
                            }
                        }

                        /// search for new friends
                        if (codding == 004)
                        {
                            try
                            {
                                string[] log_pass = str.Split(stringSeparators, StringSplitOptions.None);
                                int codSerch = int.Parse(log_pass[0]);

                                if (codSerch == 0)
                                {
                                    int id_serch = int.Parse(log_pass[1]);
                                    Console.WriteLine(codSerch + " " + id_serch);

                                    comm.CommandText = string.Format("Select id,name,surname from Users  where Users.id='{0}'", id_serch);

                                }
                                else if (codSerch == 1)
                                {
                                    string name_serch = log_pass[1];
                                    Console.WriteLine(codSerch + " " + name_serch); 

                                    comm.CommandText = string.Format("Select id,name,surname from Users  where Users.name='{0}'", name_serch);

                                }
                                else if (codSerch == 2)
                                {
                                    string surname_serch = log_pass[1];
                                    Console.WriteLine(codSerch + " " + surname_serch);

                                    comm.CommandText = string.Format("Select id,name,surname from Users  where Users.surname='{0}'", surname_serch);

                                }
                                else if (codSerch == 3)
                                {
                                    string[] name_surname = log_pass[1].Split(' ');
                                    string name = name_surname[0];
                                    string surname = name_surname[1];

                                    Console.WriteLine(codSerch + " " + name + " " + surname);

                                    comm.CommandText = string.Format("Select id,name,surname from Users  where Users.name='{0}' AND Users.surname = '{1}'", name, surname);

                                }

                                string lstfriens = null;

                                DbDataReader reader = comm.ExecuteReader();
                                while (reader.Read())
                                {
                                    lstfriens += string.Format("{0} {1} {2}\r\n", reader.GetValue(0).ToString().Trim(' '), reader.GetValue(1).ToString().Trim(' '), reader.GetValue(2).ToString().Trim(' '));
                                }
                                if (lstfriens == null)
                                    lstfriens = "0";
                                buf = Encoding.Unicode.GetBytes(lstfriens);
                                client.GetStream().Write(buf, 0, buf.Length);
                                conn.Close();
                                reader.Close();
                            }
                            catch (Exception ex)
                            {
                                buf = Encoding.Unicode.GetBytes(string.Format("Произошла ошибка: {0}", ex.Message));
                                client.GetStream().Write(buf, 0, buf.Length);
                                conn.Close();
                            }
                        }

                        /// add user to friends
                        if (codding == 005)
                        {
                            try
                            {
                                string[] log_pass = str.Split(stringSeparators, StringSplitOptions.None);
                                string id_friend = log_pass[0];
                                string my_id = log_pass[1];
                                Console.WriteLine(id_friend + " " + my_id);

                                comm.CommandText = string.Format("Select name from Friends{0}  where id='{1}'", my_id, id_friend);
                                string Name_friend = null;
                                string Surname_friend = null;
                                DbDataReader reader = comm.ExecuteReader();
                                while (reader.Read())
                                {
                                    Name_friend = reader.GetValue(0).ToString();
                                }
                                reader.Close();
                                reader = null;
                                if (Name_friend != null)
                                    throw new Exception("Этот пользователь уже находится у вас в списке друзей");

                                comm.CommandText = string.Format("Select id,name,surname from Users  where Users.id='{0}'", id_friend);

                                reader = comm.ExecuteReader();
                                while (reader.Read())
                                {
                                    id_friend = reader.GetValue(0).ToString();
                                    Name_friend = reader.GetValue(1).ToString();
                                    Surname_friend = reader.GetValue(2).ToString();
                                }
                                reader.Close();
                                reader = null;
                                comm.CommandText = string.Format("INSERT INTO Friends{3} (id,name, surname) VALUES ('{0}', '{1}', '{2}');", id_friend, Name_friend, Surname_friend, my_id);
                                reader = comm.ExecuteReader();
                                reader.Close();
                                reader = null;

                                buf = Encoding.Unicode.GetBytes(string.Format("Пользователь {0} {1} успешно добавлен в список друзей", Name_friend.Trim(' '), Surname_friend.Trim(' ')));
                                client.GetStream().Write(buf, 0, buf.Length);
                                conn.Close();
                            }
                            catch (Exception ex)
                            {
                                buf = Encoding.Unicode.GetBytes(string.Format("Произошла ошибка: {0}", ex.Message));
                                client.GetStream().Write(buf, 0, buf.Length);
                                conn.Close();
                            }
                        }

                        /// creating a "room" of two users for communication
                        if (codding == 006)
                        {
                            try
                            {
                                string[] log_pass = str.Split(stringSeparators, StringSplitOptions.None);
                                string my_id = log_pass[0];
                                string friend_id = log_pass[1];
                                Console.WriteLine(my_id + " " + friend_id);
                                string id = null;

                                comm.CommandText = string.Format("Select id_Friend from Rooms{0}  where id_Friend='{1}'", my_id, friend_id);
                                DbDataReader reader = comm.ExecuteReader();
                                while (reader.Read())
                                {
                                    id = reader.GetValue(0).ToString();
                                }
                                reader.Close();
                                reader = null;
                                string name_table_message = null;
                                if (id != null)
                                {
                                    try
                                    {
                                        if (int.Parse(my_id) < int.Parse(friend_id))
                                            throw new Exception(name_table_message = "Message_" + my_id + "_" + friend_id);
                                        else
                                            throw new Exception(name_table_message = "Message_" + friend_id + "_" + my_id);
                                    }
                                    catch (Exception ex)
                                    {
                                        buf = Encoding.Unicode.GetBytes(string.Format(ex.Message));
                                        client.GetStream().Write(buf, 0, buf.Length);
                                        conn.Close();
                                    }

                                }


                                comm.CommandText = string.Format("INSERT INTO Rooms{0} (id_Friend) VALUES ('{1}');", my_id, friend_id);
                                DbDataReader reader1 = comm.ExecuteReader();
                                reader1.Close();
                                reader1 = null;

                                comm.CommandText = string.Format("INSERT INTO Rooms{0} (id_Friend) VALUES ('{1}');", friend_id, my_id);
                                DbDataReader reader2 = comm.ExecuteReader();
                                reader2.Close();
                                reader2 = null;

                                if (int.Parse(my_id) < int.Parse(friend_id))
                                {
                                    comm.CommandText = string.Format("CREATE TABLE Message_{0}_{1} (id_Message INT NOT NULL PRIMARY KEY IDENTITY,Text nvarchar(1000) NOT NULL,id_sender int NOT NULL)", my_id, friend_id);
                                    name_table_message = "Message_" + my_id + "_" + friend_id;
                                }
                                else
                                    comm.CommandText = string.Format("CREATE TABLE Message_{0}_{1} (id_Message INT NOT NULL PRIMARY KEY IDENTITY,Text nvarchar(1000) NOT NULL,id_sender int NOT NULL)", friend_id, my_id);
                                name_table_message = "Message_" + friend_id + "_" + my_id;
                                DbDataReader CreatreTableMessage = comm.ExecuteReader();
                                CreatreTableMessage.Close();
                                CreatreTableMessage = null;

                                buf = Encoding.Unicode.GetBytes(name_table_message);
                                client.GetStream().Write(buf, 0, buf.Length);
                                conn.Close();
                            }
                            catch (Exception ex)
                            {
                                buf = Encoding.Unicode.GetBytes(string.Format("Произошла ошибка: {0}", ex.Message));
                                client.GetStream().Write(buf, 0, buf.Length);
                                conn.Close();
                            }
                        }

                        /// return the user a list of all his chat rooms
                        if (codding == 007)
                        {
                            try
                            {
                                string lstfriens = null;

                                comm.CommandText = string.Format("Select id_Friend from Rooms{0}", str);

                                DbDataReader reader = comm.ExecuteReader();
                                while (reader.Read())
                                {
                                    lstfriens += string.Format("{0} ", reader.GetValue(0).ToString().Trim(' '));
                                }
                                reader.Close();
                                reader = null;
                                if (lstfriens == null)
                                    throw new Exception("0");


                                string[] lstfriendinroom = lstfriens.Split(' ');
                                lstfriens = null;
                                for (int i = 0; i < lstfriendinroom.Length - 1; i++)
                                {
                                    comm.CommandText = string.Format("Select id,name,surname from Users  where id='{0}'", lstfriendinroom[i]);

                                    reader = comm.ExecuteReader();
                                    while (reader.Read())
                                    {
                                        lstfriens += string.Format("{0} {1} {2}\r\n", reader.GetValue(0).ToString().Trim(' '),
                                            reader.GetValue(1).ToString().Trim(' '),
                                            reader.GetValue(2).ToString().Trim(' '));
                                    }
                                    reader.Close();
                                    reader = null;
                                }

                                buf = Encoding.Unicode.GetBytes(lstfriens);
                                client.GetStream().Write(buf, 0, buf.Length);
                                conn.Close();
                            }
                            catch (Exception ex)
                            {
                                buf = Encoding.Unicode.GetBytes(string.Format("Произошла ошибка: {0}", ex.Message));
                                client.GetStream().Write(buf, 0, buf.Length);
                                conn.Close();
                            }
                        }

                        /// send a chat history to the user
                        if (codding == 008)
                        {
                            try
                            {
                                string[] log_pass = str.Split(stringSeparators, StringSplitOptions.None);
                                string NameChat = log_pass[0];
                                string lstMessage = null;

                                comm.CommandText = string.Format("Select Text,id_sender from {0}", NameChat);
                                DbDataReader reader = comm.ExecuteReader();
                                while (reader.Read())
                                {
                                    lstMessage += string.Format("{0}_{1}\\", reader.GetValue(0).ToString(), reader.GetValue(1).ToString());
                                }
                                reader.Close();
                                reader = null;

                                buf = Encoding.Unicode.GetBytes(lstMessage);
                                client.GetStream().Write(buf, 0, buf.Length);
                                conn.Close();
                            }
                            catch (Exception ex)
                            {
                                buf = Encoding.Unicode.GetBytes(string.Format("Произошла ошибка: {0}", ex.Message));
                                client.GetStream().Write(buf, 0, buf.Length);
                                conn.Close();
                            }
                        }

                        /// chat room posting
                        if (codding == 009)
                        {
                            try
                            {
                                string[] log_pass = str.Split('\\');
                                string NameChat = log_pass[0];
                                string id_user = log_pass[1];
                                string text = log_pass[2];
                                Console.WriteLine(codding + " " + NameChat);

                                comm.CommandText = string.Format("INSERT INTO {0} (Text,id_sender) VALUES ('{1}', '{2}');", NameChat, text, id_user);
                                DbDataReader reader = comm.ExecuteReader();
                                reader.Close();
                                reader = null;

                                buf = Encoding.Unicode.GetBytes("1");
                                client.GetStream().Write(buf, 0, buf.Length);
                                conn.Close();
                            }
                            catch (Exception ex)
                            {
                                buf = Encoding.Unicode.GetBytes(string.Format("Произошла ошибка: {0}", ex.Message));
                                client.GetStream().Write(buf, 0, buf.Length);
                                conn.Close();
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            { Console.WriteLine(ex); }
        }
    }
}

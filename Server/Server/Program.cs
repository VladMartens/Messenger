using System;


namespace Server
{
    /// <summary>
    /// main class
    /// </summary>
    class Program
    {
        /// <summary>
        /// The static main function, in which the Server class is initialized and the "Connect" function is called
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            try
            {
                ServConnect sc = new ServConnect();
                sc.Connect();
                Console.ReadLine();
            }
            catch(Exception ex)
            { Console.WriteLine(ex.Message); }
            
        }
    }
}

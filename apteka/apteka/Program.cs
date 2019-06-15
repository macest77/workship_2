using System;
using System.Data.SqlClient;

namespace apteka
{
    class Program
    {
        static void Main(string[] args)
        {
            var command = "";
            do
            {

                Console.WriteLine("Podaj komendę:");
                command = Console.ReadLine();
                var DB = new Database();

                SqlConnection connection = null;
                DB.CheckStructure();
            } while (command != "exit");
        }
    }
}

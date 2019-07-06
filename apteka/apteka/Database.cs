using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;

namespace apteka
{
    class Database
    {
        const string connectionString = "Integrated Security=SSPI;Data Source=.\\SQLEXPRESS;Initial Catalog=drugstore;";

        public static SqlConnection CreateConnection()
        {
            return new SqlConnection(connectionString);
        }

        public void CheckStructure()
        {
            string[] tables = { "Medicines", "Prescriptions",// "DropOrders",
                "Orders" };
            foreach (var table in tables)
            {
                var connection = CreateConnection();
                var sql = "";
                try
                {
                    //SqlCommand sqlCommand = new SqlCommand("DROP TABLE Medicines", connection);
                    Console.WriteLine("SELECT TOP 1 * FROM " + table);
                    SqlCommand sqlCommand = new SqlCommand("SELECT TOP 1 * FROM " + table, connection);
                    /*SqlParameter sqlParameterTable = new SqlParameter();
                    sqlParameterTable.ParameterName = "@table";
                    sqlParameterTable.DbType = System.Data.DbType.AnsiString;
                    sqlParameterTable.Value = table;
                    sqlCommand.Parameters.Add(sqlParameterTable);*/
                    using (connection)
                    {
                        connection.Open();
                        //var sqlCommand = new SqlCommand(sqlCommand, connection);

                        var sqlReader = sqlCommand.ExecuteNonQuery();
                    }
                } catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    if (ex.Message.Substring(0, 14) == "Invalid object")
                    {
                        switch (table)
                        {
                            case "Medicines":
                                sql = "CREATE TABLE Medicines (ID int IDENTITY(1,1)NOT NULL PRIMARY KEY," +
                                        "Name varchar(50) UNIQUE," +
                                        "Manufacturer varchar(50)," +
                                        "Price decimal(7,2)," +
                                        "Amount smallint," +
                                        "WithPrescription tinyint)";
                                break;
                            case "Prescriptions":
                                sql = "CREATE TABLE Prescriptions (ID int IDENTITY(1,1)NOT NULL PRIMARY KEY," +
                                            "CustomerName varchar(100) NOT NULL, " +
                                            "PESEL varchar(11) NOT NULL," +
                                            "PrescriptionNumber varchar(15))";
                                break;
                            case "Orders":
                                sql = "CREATE TABLE Orders (ID int IDENTITY(1,1)NOT NULL PRIMARY KEY," +
                                            "PrescriptionID int NULL," +
                                            "MedicineID int NOT NULL," +
                                            "Date DateTime," +
                                            "Amount smallint," +
                                            "FOREIGN KEY (PrescriptionID) REFERENCES Prescriptions(ID)," +
                                            "FOREIGN KEY (MedicineID) REFERENCES Medicines(ID))";
                                break;
                            case "DropOrders":
                                sql = "DROP TABLE Orders";
                                break;
                        }
                    } 
                    
                }
                if (sql != "")
                {
                    var connection2 = CreateConnection();
                    SqlCommand sqlCommand = new SqlCommand(sql, connection2);
                    using (connection2)
                    {
                        connection2.Open();
                        //var sqlCommand = new SqlCommand(sqlCommand, connection);

                        var sqlReader = sqlCommand.ExecuteReader();
                    }
                }
                Console.ReadKey();
            }
        }

        public SqlDataReader Connect(SqlCommand sql)
        {
            using(var connection = CreateConnection())
            {
                connection.Open();
                return sql.ExecuteReader();
            }
        }
    }
}

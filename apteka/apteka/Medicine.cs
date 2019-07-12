using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace apteka
{
    class Medicine : AciveRecord
    {
        public int ID { get; private set; }
        public string Name { get; set; }
        public string Manufacturer { get; set; }
        public double Price { get; set; }
        public int Amount { get; set; }
        public int WithPrescription { get; set; }

        public override void Reload()
        {
            throw new NotImplementedException();
        }

        public override void Remove()
        {
            throw new NotImplementedException();
        }

        public override void Save()
        {
            try
            {
                var sql = "";
                var kind = "update";
                if (ID > 0)
                {
                    //save
                    sql = "UPDATE Medicines " +
                        "SET Name=@name,Manufacturer=@manufacturer,Price=@price," +
                        "Amount=@amount,WithPrescription=@prescription " +
                        $"WHERE ID={ID}";
                }
                else
                {
                    //insert
                    sql = "INSERT INTO Medicines (Name, Manufacturer, Price, Amount, WithPrescription) " +
                        "VALUES (@name, @manufacturer, @price, @amount, @prescription)";
                    kind = "insert";
                }
                if (sql != "")
                {
                    var connection2 = Database.CreateConnection();
                    SqlCommand sqlCommand = new SqlCommand(sql, connection2);

                    SqlParameter sqlParameterName = new SqlParameter();
                    sqlParameterName.ParameterName = "@name";
                    sqlParameterName.Value = Name;
                    sqlParameterName.DbType = System.Data.DbType.AnsiString;
                    sqlCommand.Parameters.Add(sqlParameterName);

                    SqlParameter sqlParameterManuf = new SqlParameter();
                    sqlParameterManuf.ParameterName = "@manufacturer";
                    sqlParameterManuf.Value = Manufacturer;
                    sqlParameterManuf.DbType = System.Data.DbType.AnsiString;
                    sqlCommand.Parameters.Add(sqlParameterManuf);

                    SqlParameter sqlParameterPrice = new SqlParameter();
                    sqlParameterPrice.ParameterName = "@price";
                    sqlParameterPrice.Value = Price;
                    sqlParameterPrice.DbType = System.Data.DbType.Decimal;
                    sqlCommand.Parameters.Add(sqlParameterPrice);

                    SqlParameter sqlParameterAmount = new SqlParameter();
                    sqlParameterAmount.ParameterName = "@amount";
                    sqlParameterAmount.Value = Amount;
                    sqlParameterAmount.DbType = System.Data.DbType.Int32;
                    sqlCommand.Parameters.Add(sqlParameterAmount);

                    SqlParameter sqlParameterPresc = new SqlParameter();
                    sqlParameterPresc.ParameterName = "@prescription";
                    sqlParameterPresc.Value = WithPrescription;
                    sqlParameterPresc.DbType = System.Data.DbType.Int32;
                    sqlCommand.Parameters.Add(sqlParameterPresc);

                    using (connection2)
                    {
                        connection2.Open();
                        //var sqlCommand = new SqlCommand(sqlCommand, connection);

                        var sqlReader = sqlCommand.ExecuteNonQuery();

                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        protected override void Close()
        {
            throw new NotImplementedException();
        }

        protected override void Open()
        {
            throw new NotImplementedException();
        }

        public static List<string> Validate(string medicineString)
        {
            var toReturn = new List<string>();
            var medicineSplitted = medicineString.Split(',');
            if (medicineSplitted[0].Length > 50)
                toReturn.Add("Nazwa leku nie może być dłuższa niż 50 znaków");
            if (medicineSplitted[1].Length > 50)
                toReturn.Add("Nazwa producenta nie może być dłuższa niż 50 znaków");
            var price = 0D;
            if (!double.TryParse(medicineSplitted[2], out price))
                toReturn.Add("Cena musi być podana w formacie zmiennoprzecinkowym");
            var amount = 0;
            if (!int.TryParse(medicineSplitted[3], out amount))
                toReturn.Add("Ilość musi być podana jako pełna wartość");
            if (toReturn.Count < 1)
            {
                toReturn.Add("OK");
            }
            if (medicineSplitted.Length > 4)
            {
                if (medicineSplitted[4] != "tak" && medicineSplitted[4] != "nie")
                    toReturn.Add("Uwaga - przyjęte zostanie bez recepty");
            }
            return toReturn;
        }

        public static void ShowMedicines()
        {
            try
            {
                var sql = "select * from Medicines order by Name asc";
                var connection2 = Database.CreateConnection();
                SqlCommand sqlCommand = new SqlCommand(sql, connection2);
                using (connection2)
                {
                    connection2.Open();
                    //var sqlCommand = new SqlCommand(sqlCommand, connection);

                    var sqlDataReader = sqlCommand.ExecuteReader();
                    if (sqlDataReader.HasRows)
                    {
                        var orgConsoleFColor = Console.ForegroundColor;
                        var orgConsoleBColor = Console.BackgroundColor;
                        Console.ForegroundColor = ConsoleColor.White;
                        Console.BackgroundColor = ConsoleColor.Blue;
                        while (sqlDataReader.Read())
                        {
                            var showTxt = $"{sqlDataReader.GetInt32(0).ToString().PadRight(4)} | " +
                                $"{sqlDataReader["Name"].ToString().PadRight(50)} | " +
                                $"{sqlDataReader["manufacturer"].ToString().PadRight(50)} | " +
                                $"{sqlDataReader["Price"].ToString().PadLeft(7)} | " +
                                $"{sqlDataReader["Amount"].ToString()}";
                            if (sqlDataReader["WithPrescription"].ToString() == "1")
                            {
                                showTxt += " (lek na receptę)";
                            }
                            Console.WriteLine(showTxt);
                        }
                        Console.ForegroundColor = orgConsoleFColor;
                        Console.BackgroundColor = orgConsoleBColor;
                    }
                    else
                    {
                        Console.WriteLine("Brak leków w bazie");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

            }
        }

        public static Medicine GetMedicine(int ID)
        {
            var MedicineToReturn = new Medicine();
            try
            {
                var sql = "select * from Medicines where ID = @id";
                var connection2 = Database.CreateConnection();
                SqlCommand sqlCommand = new SqlCommand(sql, connection2);

                SqlParameter sqlParameterID = new SqlParameter();
                sqlParameterID.ParameterName = "@id";
                sqlParameterID.Value = ID;
                sqlParameterID.DbType = System.Data.DbType.Int32;
                sqlCommand.Parameters.Add(sqlParameterID);
                using (connection2)
                {
                    connection2.Open();
                    //var sqlCommand = new SqlCommand(sqlCommand, connection);

                    var sqlDataReader = sqlCommand.ExecuteReader();
                    if (sqlDataReader.HasRows)
                    {
                        while (sqlDataReader.Read())
                        {
                            MedicineToReturn.ID = sqlDataReader.GetInt32(0);
                            MedicineToReturn.Name = sqlDataReader.GetString(1);
                            MedicineToReturn.Manufacturer = sqlDataReader.GetString(2);
                            MedicineToReturn.Price = (double)sqlDataReader.GetDecimal(3);
                            MedicineToReturn.Amount = sqlDataReader.GetInt16(4);
                            MedicineToReturn.WithPrescription = sqlDataReader.GetByte(5);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

            }
            return MedicineToReturn;
        }
    }
}

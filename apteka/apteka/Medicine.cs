using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace apteka
{
    class Medicine : AciveRecord
    {
        public override int ID { get;  }
        public string Name { get; set;  }
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
                if (ID > 0)
                {
                    //save
                    sql = "UPDATE Medicines " +
                        $"SET Name={Name},Manufacturer={Manufacturer},Price={Price}," +
                        $"Amount={Amount},WithPrescription={WithPrescription} " +
                        $"WHERE ID={ID}";
                }
                else
                {
                    //insert
                    sql = "INSERT INTO Medicines (Name, Manufacturer, Price, Amount, WithPrescription) " +
                        $"VALUES ({Name},{Manufacturer},{Price},{Amount},{WithPrescription})";
                }
                if (sql != "")
                {
                    var connection2 = Database.CreateConnection();
                    SqlCommand sqlCommand = new SqlCommand(sql, connection2);
                    using (connection2)
                    {
                        connection2.Open();
                        //var sqlCommand = new SqlCommand(sqlCommand, connection);

                        var sqlReader = sqlCommand.ExecuteReader();
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
    }
}

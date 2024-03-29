﻿using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace apteka
{
    class Prescription : AciveRecord
    {
        public int ID { get; private set; }
        public string CustomerName { get; set; }
        public string PESEL { get; set; }
        public string PrescriptionNumber { get; set; }

        public Prescription(int ID)
        {
            if (ID > 0)
            {
                GetPrescription(ID);
            }
        }

        public Prescription(string prescriptionNumber)
        {
            prescriptionNumber = sqlReplace(prescriptionNumber);
            GetPrescription(0, prescriptionNumber);
        }

        public Prescription(int id, string customer, string pesel, string number)
        {
            ID = id;
            CustomerName = customer;
            PESEL = pesel;
            PrescriptionNumber = number;
        }

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
                    sql = "UPDATE Prescriptions " +
                        "SET CustomerName=@name,PESEL=@pesel,PrescriptionNumber=@prescription " +
                        $"WHERE ID={ID}";
                }
                else
                {
                    sql = "INSERT INTO Prescriptions (CustomerName, PESEL, PrescriptionNumber) " +
                        "VALUES (@name, @pesel, @prescription)";
                }
                if (sql != "")
                {
                    var connection2 = Database.CreateConnection();
                    SqlCommand sqlCommand = new SqlCommand(sql, connection2);

                    SqlParameter sqlParameterName = new SqlParameter();
                    sqlParameterName.ParameterName = "@name";
                    sqlParameterName.Value = CustomerName;
                    sqlParameterName.DbType = System.Data.DbType.AnsiString;
                    sqlCommand.Parameters.Add(sqlParameterName);

                    SqlParameter sqlParameterPesel = new SqlParameter();
                    sqlParameterPesel.ParameterName = "@pesel";
                    sqlParameterPesel.Value = PESEL;
                    sqlParameterPesel.DbType = System.Data.DbType.AnsiString;
                    sqlCommand.Parameters.Add(sqlParameterPesel);

                    SqlParameter sqlParameterPresc = new SqlParameter();
                    sqlParameterPresc.ParameterName = "@prescription";
                    sqlParameterPresc.Value = PrescriptionNumber;
                    sqlParameterPresc.DbType = System.Data.DbType.AnsiString;
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

        public List<Prescription> GetPrescription(int ID = 0,  string number = "")
        {
            var returnList = new List<Prescription>();
            var sql = "SELECT * FROM Prescriptions";
            if (ID > 0)
            {
                sql += " WHERE ID = @id";
            }
            else if (number != "")
            {
                sql += " WHERE PrescriptionNumber = @number";
            }
            var connection2 = Database.CreateConnection();
            SqlCommand sqlCommand = new SqlCommand(sql, connection2);

            SqlParameter sqlParameterID = new SqlParameter();
            if (ID > 0)
            {
                sqlParameterID.ParameterName = "@id";
                sqlParameterID.Value = ID;
                sqlParameterID.DbType = System.Data.DbType.Int32;
            }
            else if (number != "")
            {
                sqlParameterID.ParameterName = "@number";
                sqlParameterID.Value = number;
                sqlParameterID.DbType = System.Data.DbType.String;
            }
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
                        if (ID > 0 || number != "")
                        {
                            ID = sqlDataReader.GetInt32(0);
                            CustomerName = sqlDataReader.GetString(1);
                            PESEL = sqlDataReader.GetString(2);
                            PrescriptionNumber = sqlDataReader.GetString(3);
                        }
                        else
                        {
                            var tmp = new Prescription(sqlDataReader.GetInt32(0), sqlDataReader.GetString(1),
                                sqlDataReader.GetString(2), sqlDataReader.GetString(3));
                            returnList.Add(tmp);
                        }
                    }
                }
                else if (number != "")
                {
                    ID = 0;
                    CustomerName = "";
                    PESEL = "";
                    PrescriptionNumber = number;
                }
            }
            return returnList;
        }
    }
}

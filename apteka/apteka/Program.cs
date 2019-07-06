using System;
using System.Data.SqlClient;

namespace apteka
{
    class Program
    {
        static void Main(string[] args)
        {
            var command = "";
            var DB = new Database();
            DB.CheckStructure();
            do
            {
                Console.WriteLine("Podaj komendę:");
                command = Console.ReadLine();
                
                SqlConnection connection = null;
                var PM = new ProgramMedicine();

                switch (command)
                {
                    case "AddMedicine":
                    case "ad":
                        PM.AddMedicine();
                        break;
                    default:
                        break;
                }

            } while (command != "exit");
        }
    }

    public class ProgramMedicine
    {
        public void AddMedicine()
        {
            Console.WriteLine("Podaj dane leku - rozdziel je przecinkiem");
            Console.WriteLine("format: nazwa leku,producent,cena,ilość[,na receptę(tak/nie)]");
            var newMedicineString = Console.ReadLine();
            var medicineValidate = Medicine.Validate(newMedicineString);
            var isContinued = "t";
            if (medicineValidate[0] == "OK")
            {
                if (medicineValidate.Count > 1)
                {
                    Console.WriteLine(medicineValidate[1]);
                    Console.WriteLine("Czy kontynuować? (t/n)");
                    isContinued = Console.ReadLine();

                }
                if (isContinued == "t")
                {
                    var newMedicineSplitted = newMedicineString.Split(',');
                    var newMedicine = new Medicine();
                    newMedicine.Name = newMedicineSplitted[0];
                    newMedicine.Manufacturer = newMedicineSplitted[1];
                    newMedicine.Price = double.Parse(newMedicineSplitted[2]);
                    newMedicine.Amount = int.Parse(newMedicineSplitted[3]);
                    var prescription = 0;
                    if (newMedicineSplitted.Length > 4)
                    {
                        if (newMedicineSplitted[4] == "t")
                            prescription = 1;
                    }
                    newMedicine.WithPrescription = prescription;
                    newMedicine.Save();
                }
            }
        }
    }
}

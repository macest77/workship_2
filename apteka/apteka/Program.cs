using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace apteka
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("Witaj w programie LekoBaza");
            
            var command = "";
            var DB = new Database();
            DB.CheckStructure();
            do
            {
                Console.WriteLine("\r\nPodaj komendę ('-h' - lista komend):");
                command = Console.ReadLine();
                
                SqlConnection connection = null;
                var PM = new ProgramMedicine();

                switch (command)
                {
                    case "AddMedicine":
                    case "ad":
                        PM.AddMedicine();
                        break;
                    case "ShowMedicines":
                    case "sm":
                        Medicine.ShowMedicines();
                        break;
                    case "SellMedicines":
                    case "sl":
                        PM.SellMedicines();
                        break;
                    default:
                        Console.WriteLine("Lista komend:");
                        foreach (KeyValuePair<string, string> commandKV in PM.getCommandList())
                        {
                            Console.WriteLine($" {commandKV.Key.PadRight(13)} - {commandKV.Value}");
                        }
                        break;
                }

            } while (command != "exit");
        }
    }

    public class ProgramMedicine
    {
        public Dictionary<string, string> getCommandList()
        {
            Dictionary<string, string> commandList = new Dictionary<string, string>();
            commandList.Add("AddMedicine", "dodaj lek");
            commandList.Add("ad", "dodaj lek");
            commandList.Add("exit", "wyjście");
            commandList.Add("ShowMedicines", "pokaż leki w bazie");
            commandList.Add("sm", "pokaż leki w bazie");
            commandList.Add("SellMedicines", "księgowanie sprzedaży bez recepty");
            commandList.Add("sl", "księgowanie sprzedaży bez recepty");
            return commandList;
        }

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
                        if (newMedicineSplitted[4][0] == 't')
                            prescription = 1;
                    }
                    newMedicine.WithPrescription = prescription;
                    newMedicine.Save();
                }
            }
        }

        public void SellMedicines()
        {
            Console.Write("Podaj id leku (wpisz 0 jeśli wyświetlić listę leków): ");
            var med = Console.ReadLine();
            var med_id = 0;
            if (!int.TryParse(med, out med_id) || int.Parse(med) == 0)
            {
                Medicine.ShowMedicines();
            }
            else
            {
                var orgConsoleFColor = Console.ForegroundColor;
                var orgConsoleBColor = Console.BackgroundColor;
                Console.ForegroundColor = ConsoleColor.White;
                Console.BackgroundColor = ConsoleColor.Red;

                var medicine = Medicine.GetMedicine(med_id);
                Console.WriteLine($"{medicine.Name} ({medicine.Manufacturer}) - cena: {medicine.Price}");
                Console.WriteLine($"akt. ilość w magazynie: {medicine.Amount}");
                if (medicine.WithPrescription == 1)
                {
                    Console.WriteLine($"UWAGA - lek na receptę");
                }
                Console.ForegroundColor = orgConsoleFColor;
                Console.BackgroundColor = orgConsoleBColor;
                if (medicine.WithPrescription == 1)
                {
                    Console.Write("Podaj numer recepty: ");
                    med = Console.ReadLine();
                    var prescription = new Prescription(med);
                    if (prescription.ID == 0)
                    {
                        Console.Write("Podaj imię i nazwisko klienta: ");
                        prescription.CustomerName = Console.ReadLine();
                        Console.Write("Podaj PESEL klienta: ");
                        prescription.PESEL = Console.ReadLine();
                        prescription.Save();
                    }
                    else
                    {
                        Console.WriteLine($"Recepta na nazwisko '{prescription.CustomerName}' " +
                            $"(PESEL: {prescription.PESEL})");
                    }
                }
                Console.Write("Podaj ilość: ");
                med = Console.ReadLine();
                var amount = 0;
                if (!int.TryParse(med, out amount))
                {
                    Console.WriteLine("Podano błędną wartość");
                }
                else
                {
                    medicine.Sell(amount);
                }
            }
        }
    }
}

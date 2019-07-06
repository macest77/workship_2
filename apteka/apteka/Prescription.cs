using System;
using System.Collections.Generic;
using System.Text;

namespace apteka
{
    class Prescription : AciveRecord
    {
        public override int ID { get; }
        public string CustomerName { get; set; }
        public string PESEL { get; set; }
        public string PrescriptionNumber { get; set; }

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
                if (ID > 0)
                {
                    //save
                }
                else
                {
                    //insert
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
    }
}

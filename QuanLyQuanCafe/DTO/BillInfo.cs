using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyQuanCafe.DTO
{
    public class BillInfo
    {
        private int idBill;
        private int idFood;
        private int count;

        public int IdBill { get => idBill; set => idBill = value; }
        public int IdFood { get => idFood; set => idFood = value; }
        public int Count { get => count; set => count = value; }

        public BillInfo(int idBill, int idFood, int count)
        {
            this.IdBill = idBill;
            this.idFood = idFood;
            this.Count = count;
        }

        public BillInfo(DataRow row)
        {
            this.IdBill = (int)row["idBill"];
            this.idFood = (int)row["idFood"];
            this.Count = (int)row["count"];
        }
    }
}

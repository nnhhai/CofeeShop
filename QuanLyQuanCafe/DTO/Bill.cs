using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyQuanCafe.DTO
{
    public class Bill
    {
        private int id;
        private int idTable;
        private int status;

        public int Id { get => id; set => id = value; }
        public int IdTable { get => idTable; set => idTable = value; }
        public int Status { get => status; set => status = value; }

        public Bill(int id, int idTable, int status)
        {
            this.Id = id;
            this.IdTable = idTable;
            this.Status = status;
        }

        public Bill(DataRow row)
        {
            this.Id = (int)row["id"];
            this.IdTable = (int)row["idTable"];
            this.Status = (int)row["status"];
        }
    }
}

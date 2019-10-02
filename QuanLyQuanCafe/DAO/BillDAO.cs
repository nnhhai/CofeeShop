using QuanLyQuanCafe.DTO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyQuanCafe.DAO
{
    public class BillDAO
    {
        private static BillDAO instance;

        public static BillDAO Instance
        {
            get { if (instance == null) instance = new BillDAO(); return BillDAO.instance; }
            private set { BillDAO.instance = value; }
        }

        private BillDAO() { }

        public int GetUnCheckBillByTableId(int id)
        {
            string query = "SELECT * FROM dbo.Bill WHERE idTable = " + id + " AND status = 0";
            DataTable data = DataProvider.Instance.ExecuteQuery(query);

            if (data.Rows.Count > 0)
            {
                Bill bill = new Bill(data.Rows[0]);
                return bill.Id;
            }
            else return -1;
        }
        public void InsertBill(int id)
        {
            string query = "USP_InsertBill @idTable";
            DataProvider.Instance.ExecuteNonQuery(query,new object[]{ id});
        }
        public int GetMaxIdBill()
        {
                return (int)DataProvider.Instance.ExecuteScalar("SELECT MAX(id) FROM dbo.Bill");
        }
        public void CheckOut(int id, float total)
        {
            string query = "update dbo.Bill set status = 1, total = " + total + "where id = " + id;
            DataProvider.Instance.ExecuteNonQuery(query);
        }

        public DataTable GetListBill()
        {
            string query = "select b.id, t.name, b.total from dbo.Bill as b, dbo.TableFood as t where b.idTable = t.id and b.total > 0";
            return DataProvider.Instance.ExecuteQuery(query);
        }
        public void DeleteBillByIdTable(int idTable)
        {
            BillInfoDAO.Instance.DeleteBillInfoByIdBillOnDeleteTable(idTable);
            string query = string.Format("delete dbo.Bill where idTable = {0} and status = 1", idTable);
            DataProvider.Instance.ExecuteQuery(query);
        }
    }
}

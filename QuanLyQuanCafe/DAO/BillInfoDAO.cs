using QuanLyQuanCafe.DTO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyQuanCafe.DAO
{
    public class BillInfoDAO
    {
        private static BillInfoDAO instance;

        public static BillInfoDAO Instance
        {
            get { if (instance == null) instance = new BillInfoDAO(); return BillInfoDAO.instance; }
            private set { BillInfoDAO.instance = value; }
        }

        private BillInfoDAO() { }

        public List<BillInfo> GetListBillInfo(int id)
        {
            List<BillInfo> listBillInfo = new List<BillInfo>();
            string query = "SELECT * FROM dbo.BillInfo WHERE idBill = ";
            DataTable data = DataProvider.Instance.ExecuteQuery(query + id);

            foreach (DataRow item in data.Rows)
            {
                BillInfo info = new BillInfo(item);
                listBillInfo.Add(info);
            }

            return listBillInfo;
        }

        public void InsertBillInfo(int idBill, int idFood, int count)
        {
            string query = "USP_InsertBillInfo @idBill , @idFood , @count";
            DataProvider.Instance.ExecuteNonQuery(query, new object[] { idBill, idFood, count });
        }

        public void DeleteBillInfoByIdFood(int idFood)
        {
            string query = "delete dbo.BillInfo where idFood = ";
            DataProvider.Instance.ExecuteQuery(query + idFood);
        }
        public void DeleteBillInfoByIdBillOnDeleteTable(int idTable)
        {
            string query = string.Format("delete dbo.BillInfo where idBill in (select id from dbo.Bill where idTable = {0} and status = 1)", idTable);
            DataProvider.Instance.ExecuteQuery(query);
        }
    }
}

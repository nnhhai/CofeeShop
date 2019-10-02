using QuanLyQuanCafe.DTO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyQuanCafe.DAO
{
    public class TableDAO
    {
        private static TableDAO instance;

        public static TableDAO Instance
        {
            get { if (instance == null) instance = new TableDAO(); return TableDAO.instance; }
            private set { TableDAO.instance = value; }
        }

        public static int TableWidth = 100;
        public static int TableHeight = 100;

        private TableDAO() { }

        public List<Table> GetTableList()
        {
            List<Table> tableList = new List<Table>();
            string query = "select * from dbo.TableFood";

            DataTable data = DataProvider.Instance.ExecuteQuery(query);

            foreach (DataRow item in data.Rows)
            {
                Table table = new Table(item);
                tableList.Add(table);
            }

            return tableList;
        }
        public int InsertTable(string name)
        {
            int result = -1;
            string query = string.Format("insert dbo.TableFood (name) values ('{0}')", name);
            result = DataProvider.Instance.ExecuteNonQuery(query);
            return result;
        }
        public int DeleteTable(int id) //references Bill BillInfo
        {
            int result = -1;
            BillDAO.Instance.DeleteBillByIdTable(id);
            string query = string.Format("delete dbo.TableFood where id = {0} and status = 'Free'", id);
            result = DataProvider.Instance.ExecuteNonQuery(query);
            return result;
        }
    }
}

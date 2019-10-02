using QuanLyQuanCafe.DTO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyQuanCafe.DAO
{
    public class AccountDAO
    {
        private static AccountDAO instance;

        public static AccountDAO Instance
        {
            get { if (instance == null) instance = new AccountDAO(); return instance; }
            private set { instance = value; }
        }

        private AccountDAO() { }

        public bool Login(string userName, string passWord)
        {
            string query = "SELECT * FROM dbo.Account WHERE UserName = N'" + userName + "' AND PassWord = N'" + passWord + "'";
            DataTable result = DataProvider.Instance.ExecuteQuery(query);
            return result.Rows.Count > 0;
        }
        public Account GetAccountByUserName(string userName)
        {
            string query = "Select * from dbo.Account where UserName = '" + userName + "'";
            DataTable data = DataProvider.Instance.ExecuteQuery(query);

            foreach (DataRow item in data.Rows)
            {
                Account account = new Account(item);
                return account;
            }
            return null;
        }

        public DataTable GetListAccount()
        {
            string query = "select UserName, Type from dbo.Account";
            return DataProvider.Instance.ExecuteQuery(query);
        }
        public int UpdateAccount(string userName, string passWord, string newPass)
        {
            int result = -1;
            string query = "USP_UpdateAccount @userName , @passWord , @newPass";
            result = DataProvider.Instance.ExecuteNonQuery(query, new object[] { userName, passWord, newPass });
            return result;
        }

        public int InsertAccount(string userName, int type)
        {
            int result = -1;
            string query = string.Format("insert dbo.Account (UserName, Type) values ('{0}', {1})", userName, type);
            result = DataProvider.Instance.ExecuteNonQuery(query);
            return result;
        }
        public int UpdateAccount(string userName, int type)
        {
            int result = -1;
            string query = string.Format("update dbo.Account set Type = {0} where UserName = '{1}'", type, userName);
            result = DataProvider.Instance.ExecuteNonQuery(query);
            return result;
        }
        public int DeleteAccount(string userName)
        {
            int result = -1;
            string query = string.Format("delete dbo.Account where UserName = '{0}'", userName);
            result = DataProvider.Instance.ExecuteNonQuery(query);
            return result;
        }
        public int ResetPassword(string userName)
        {
            int result = -1;
            string query = string.Format("update dbo.Account set PassWord = '0' where UserName = '{0}'", userName);
            result = DataProvider.Instance.ExecuteNonQuery(query);
            return result;
        }
    }
}

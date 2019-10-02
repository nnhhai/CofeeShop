using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyQuanCafe.DTO
{
    public class Account
    {
        private string userName;
        private int type;
        private string passWord;
        public string UserName { get => userName; set => userName = value; }
        public int Type { get => type; set => type = value; }
        public string PassWord { get => passWord; set => passWord = value; }
        public Account(string userName, string passWord,int type)
        {
            this.UserName = userName;
            this.PassWord = passWord;
            this.Type = type;
        }
        public Account(DataRow row)
        {
            this.UserName = row["userName"].ToString();
            this.PassWord = row["passWord"].ToString();
            this.Type = (int)row["type"];
        }
    }
}

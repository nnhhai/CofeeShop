using QuanLyQuanCafe.DAO;
using QuanLyQuanCafe.DTO;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QuanLyQuanCafe
{
    public partial class fAccountProfile : Form
    {
        private Account loginAccount;
        public Account LoginAccount { get => loginAccount; set => loginAccount = value; }
        public fAccountProfile(Account account)
        {
            InitializeComponent();
            this.LoginAccount = account;
            txbUserName.Text = LoginAccount.UserName;
        }
        public void UpdateAccountInfo()
        {
            string userName = txbUserName.Text.ToString();
            string passWord = txbPassword.Text.ToString();
            string newPass = txbNewPass.Text.ToString();

            if(AccountDAO.Instance.UpdateAccount(userName, passWord, newPass) > 0)
            {
                MessageBox.Show("Updated!");
            }
            else MessageBox.Show("Re-type PassWord or UserName!");
        }
        private void BtnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private void BtnUpdate_Click(object sender, EventArgs e)
        {
            UpdateAccountInfo();
        }
    }
}

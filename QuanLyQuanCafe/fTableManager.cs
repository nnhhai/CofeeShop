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
    public partial class fTableManager : Form
    {
        private Account loginAccount;
        public Account LoginAccount { get => loginAccount; set => loginAccount = value; }
        public fTableManager(Account account)
        {
            InitializeComponent();
            this.LoginAccount = account;
            EnableByAccountType(account.Type);
            LoadTable();
            LoadFoodCategory();
        }

        void LoadTable()
        {
            flpTable.Controls.Clear();
            List<Table> tableList = TableDAO.Instance.GetTableList();

            foreach (Table item in tableList)
            {                
                Button btn = new Button() { Width = TableDAO.TableWidth, Height = TableDAO.TableHeight };
                btn.Text = item.Name + Environment.NewLine + item.Status;
                btn.Click += Btn_Click;
                btn.Tag = item;

                switch (item.Status)
                {
                    case "Free":
                        btn.BackColor = Color.Azure;
                        break;
                    default:
                        btn.BackColor = Color.LightPink;
                        break;
                }

                flpTable.Controls.Add(btn);
            }
        }
        void EnableByAccountType(int type)
        {
            if (type == 1) adminToolStripMenuItem.Enabled = true;
            else adminToolStripMenuItem.Enabled = false;
        }
        void LoadFoodCategory()
        {
            List<FoodCategory> listFoodCatagory = new List<FoodCategory>();
            listFoodCatagory = FoodCategoryDAO.Instance.GetListFoodCategorie();
            cbCategory.DataSource = listFoodCatagory;
            cbCategory.DisplayMember = "name";
        }

        void LoadFoodByIdFoodCategory(int id)
        {
            List<Food> listFood = new List<Food>();
            listFood = FoodDAO.Instance.GetListFoodFromIdFoodCategory(id);
            cbFood.DataSource = listFood;
            if (listFood.Count > 0)
            {
                cbFood.DisplayMember = "name";
            }
            else cbFood.DisplayMember = null;

        }
        void ShowBill(int id)
        {
            lsvBill.Items.Clear();
            List<DTO.Menu> listBillInfo = MenuDAO.Instance.GetListMenuByTable(id);
            float total = 0;
            foreach (DTO.Menu item in listBillInfo)
            {
                ListViewItem lsvItem = new ListViewItem(item.Name.ToString());
                lsvItem.SubItems.Add(item.Count.ToString());
                lsvItem.SubItems.Add(item.Price.ToString());
                lsvItem.SubItems.Add(item.Total.ToString());

                total += item.Total;
                lsvBill.Items.Add(lsvItem);
            }
            txbTotal.Text = total.ToString();
        }
        private void Btn_Click(object sender, EventArgs e)
        {
            int tableId = ((sender as Button).Tag as Table).Id;
            lsvBill.Tag = (sender as Button).Tag;
            ShowBill(tableId);
        }

        private void LogOutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void PersonalInformationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            fAccountProfile f = new fAccountProfile(LoginAccount);
            f.ShowDialog();
        }

        private void AdminToolStripMenuItem_Click(object sender, EventArgs e)
        {
            fAdmin f = new fAdmin();
            f.loginAccount = LoginAccount;
            f.ShowDialog();
        }

        private void CbCategory_SelectedIndexChanged(object sender, EventArgs e)
        {
            int id = 0;

            ComboBox cb = sender as ComboBox;

            if (cb.SelectedItem == null)
                return;

            FoodCategory selected = cb.SelectedItem as FoodCategory;
            id = selected.Id;

            LoadFoodByIdFoodCategory(id);
        }

        private void BtnUpdateFood_Click(object sender, EventArgs e)
        {
            Table table = lsvBill.Tag as Table;
            if ((table != null) && (cbFood.SelectedItem != null))
            {
                int idBill = BillDAO.Instance.GetUnCheckBillByTableId(table.Id);
                int idFood = (cbFood.SelectedItem as Food).Id;
                int count = (int)nmFoodCount.Value;

                if (idBill == -1)
                {
                    BillDAO.Instance.InsertBill(table.Id);
                    BillInfoDAO.Instance.InsertBillInfo(BillDAO.Instance.GetMaxIdBill(), idFood, count);
                }
                else
                {
                    BillInfoDAO.Instance.InsertBillInfo(idBill, idFood, count);
                }
                ShowBill(table.Id);
                LoadTable();
            }
            
        }

        private void BtnCheckOut_Click(object sender, EventArgs e)
        {
            Table table = lsvBill.Tag as Table;
            int idBill = -1;
            if (table != null)
                idBill = BillDAO.Instance.GetUnCheckBillByTableId(table.Id);
            float total = (float)Convert.ToDouble(txbTotal.Text.ToString());
            if (idBill != -1)
            {
                if(MessageBox.Show("CheckOut " + table.Name, "CheckOut", MessageBoxButtons.OKCancel) == DialogResult.OK)
                {
                    BillDAO.Instance.CheckOut(idBill, total);
                    ShowBill(table.Id);
                    LoadTable();
                }
            }
        }
    }
}

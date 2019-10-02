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
    public partial class fAdmin : Form
    {
        BindingSource listFood = new BindingSource();
        public Account loginAccount;
        public fAdmin()
        {
            InitializeComponent();
            dgvFood.DataSource = listFood;
            LoadListBill();
            LoadListFood();
            AddFoodBinding();
            LoadListAccount();
            AddAccountBingding();
            LoadListFoodCategory();
            AddCategoryBinding();
            LoadListTable();
            AddTableBinding();
        }
        void LoadListBill()
        {
            dgvBill.DataSource = BillDAO.Instance.GetListBill();
        }
        void LoadListFood()
        {
            listFood.DataSource = FoodDAO.Instance.GetListFood();
        }

        void LoadListAccount()
        {
            dgvAccount.DataSource = AccountDAO.Instance.GetListAccount();
        }
        void LoadListFoodCategory()
        {
            dgvCategory.DataSource = FoodCategoryDAO.Instance.GetListFoodCategorie();
        }
        void LoadListTable()
        {
            dgvTable.DataSource = TableDAO.Instance.GetTableList();
        }
        void AddAccountBingding()
        {
            txbUserName.DataBindings.Add(new Binding("Text", dgvAccount.DataSource, "UserName", true, DataSourceUpdateMode.Never));
            nmAccountType.DataBindings.Add(new Binding("Value", dgvAccount.DataSource, "Type", true, DataSourceUpdateMode.Never));
        }
        void AddFoodBinding()
        {
            txbFoodName.DataBindings.Add(new Binding("Text", dgvFood.DataSource, "Name", true,DataSourceUpdateMode.Never));
            txbFoodID.DataBindings.Add(new Binding("Text", dgvFood.DataSource, "Id", true, DataSourceUpdateMode.Never));
            nmFoodPrice.DataBindings.Add(new Binding("Value", dgvFood.DataSource, "Price", true, DataSourceUpdateMode.Never));

            cbFoodCategory.DataSource = FoodCategoryDAO.Instance.GetListFoodCategorie();
            cbFoodCategory.DisplayMember = "Name";
        }
        void AddCategoryBinding()
        {
            txbCategoryId.DataBindings.Add(new Binding("Text", dgvCategory.DataSource, "id", true, DataSourceUpdateMode.Never));
            txbCategoryName.DataBindings.Add(new Binding("Text", dgvCategory.DataSource, "name", true, DataSourceUpdateMode.Never));
        }
        void AddTableBinding()
        {
            txbTableName.DataBindings.Add(new Binding("Text", dgvTable.DataSource, "name", true, DataSourceUpdateMode.Never));
        }
        private void TxbFoodID_TextChanged(object sender, EventArgs e)
        {
            try
            {
                int id = (int)dgvFood.SelectedCells[0].OwningRow.Cells["IdCategory"].Value;
                FoodCategory foodCategory = FoodCategoryDAO.Instance.GetFoodCategoryById(id);
                cbFoodCategory.Text = foodCategory.Name;
            }
            catch
            {
            }
        }

        private void BtnAddFood_Click(object sender, EventArgs e)
        {
            string name = txbFoodName.Text;
            int idCategory = (cbFoodCategory.SelectedItem as FoodCategory).Id;
            float price = (float)nmFoodPrice.Value;
            if (FoodDAO.Instance.InsertFood(name, idCategory, price) > 0)
            {
                MessageBox.Show("Updated Food!");
                LoadListFood();
            }
            else MessageBox.Show("Wrong!");
        }

        private void BtnEditFood_Click(object sender, EventArgs e)
        {
            string name = txbFoodName.Text;
            int idCategory = (cbFoodCategory.SelectedItem as FoodCategory).Id;
            float price = (float)nmFoodPrice.Value;
            int id = Convert.ToInt32(txbFoodID.Text);
            if (FoodDAO.Instance.UpdateFood(name, idCategory, price, id) > 0)
            {
                MessageBox.Show("Edited Food!");
                LoadListFood();
            }
            else MessageBox.Show("Wrong!");
        }

        private void BtnDeleteFood_Click(object sender, EventArgs e)
        {
            int id = Convert.ToInt32(txbFoodID.Text);
            if (FoodDAO.Instance.DeleteFood(id) > 0)
            {
                MessageBox.Show("Deleted Food!");
                LoadListFood();
            }
            else MessageBox.Show("Wrong!");
        }

        private void BtnSearchFood_Click(object sender, EventArgs e)
        {
            string name = txbSearchFoodName.Text;
            listFood.DataSource = FoodDAO.Instance.GetListSearchFoodByName(name);
        }

        private void TxbSearchFoodName_TextChanged(object sender, EventArgs e)
        {
            string name = txbSearchFoodName.Text;
            listFood.DataSource = FoodDAO.Instance.GetListSearchFoodByName(name);
        }

        private void BtnAddAccount_Click(object sender, EventArgs e)
        {
            string userName = txbUserName.Text;
            int type = (int)nmAccountType.Value;
            if (AccountDAO.Instance.InsertAccount(userName, type) > 0)
            {
                MessageBox.Show("Default Password is 0 !");
                LoadListAccount();
            }
            else MessageBox.Show("Wrong!");
        }

        private void BtnDeleteAccount_Click(object sender, EventArgs e)
        {
            string userName = txbUserName.Text;
            if (loginAccount.UserName.Equals(userName))
            {
                MessageBox.Show("Don't Delete Your Account!");
                return;
            }
            if (AccountDAO.Instance.DeleteAccount(userName) > 0)
            {
                MessageBox.Show("Delete Account Succesfull!");
                LoadListAccount();
            }
            else MessageBox.Show("Wrong!");
        }

        private void BtnEditAccount_Click(object sender, EventArgs e)
        {
            string userName = txbUserName.Text;
            int type = (int)nmAccountType.Value;
            if (AccountDAO.Instance.UpdateAccount(userName, type) > 0)
            {
                MessageBox.Show("Edit Account Succesfull!");
                LoadListAccount();
            }
            else MessageBox.Show("Wrong!");
        }

        private void BtnResetPassword_Click(object sender, EventArgs e)
        {
            string userName = txbUserName.Text;
            if (AccountDAO.Instance.ResetPassword(userName) > 0)
            {
                MessageBox.Show("Default Password is 0 !");
            }
            else MessageBox.Show("Wrong!");
        }

        private void BtnAddCategory_Click(object sender, EventArgs e)
        {
            string name = txbCategoryName.Text;
            if (FoodCategoryDAO.Instance.InsertCategory(name) > 0)
            {
                MessageBox.Show("Add Category Successfull!");
                LoadListFoodCategory();
            }
            else MessageBox.Show("Wrong!");
        }

        private void BtnDeleteCategory_Click(object sender, EventArgs e)
        {
            int id = Convert.ToInt32(txbCategoryId.Text);
            if (FoodCategoryDAO.Instance.DeleteCategory(id) > 0)
            {
                MessageBox.Show("Delete Category Successfull!");
                LoadListFoodCategory();
            }
            else MessageBox.Show("Wrong!");
        }

        private void BtnEditCategory_Click(object sender, EventArgs e)
        {
            string name = txbCategoryName.Text;
            int id = Convert.ToInt32(txbCategoryId.Text);
            if (FoodCategoryDAO.Instance.UpdateCategory(id, name) > 0)
            {
                MessageBox.Show("Edit Category Successfull!");
                LoadListFoodCategory();
            }
            else MessageBox.Show("Wrong!");
        }
        private void TxbTableName_TextChanged(object sender, EventArgs e)
        {
            string status = dgvTable.SelectedCells[0].OwningRow.Cells["status"].Value.ToString();
            cbTableStatus.Text = status;
        }

        private void BtnAddTable_Click(object sender, EventArgs e)
        {
            string name = txbTableName.Text;
            if (TableDAO.Instance.InsertTable(name) > 0)
            {
                MessageBox.Show("Add Table Successfull!");
                LoadListTable();
            }
            else MessageBox.Show("Wrong!");
        }

        private void BtnDeleteTable_Click(object sender, EventArgs e)
        {
            int id = (int)dgvTable.SelectedCells[0].OwningRow.Cells["id"].Value;
            if (TableDAO.Instance.DeleteTable(id) > 0)
            {
                MessageBox.Show("Delete Table Successfull!");
                LoadListTable();
            }
            else MessageBox.Show("Wrong!");
        }
    }
}

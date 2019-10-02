using QuanLyQuanCafe.DTO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyQuanCafe.DAO
{
    public class FoodDAO
    {
        private static FoodDAO instance;

        public static FoodDAO Instance
        {
            get { if (instance == null) instance = new FoodDAO(); return FoodDAO.instance; }
            private set { FoodDAO.instance = value; }
        }

        private FoodDAO() { }

        public List<Food> GetListFoodFromIdFoodCategory(int id)
        {
            List<Food> listFood = new List<Food>();
            string query = "select * from dbo.Food where idCategory = ";
            DataTable data = DataProvider.Instance.ExecuteQuery(query + id);

            foreach (DataRow item in data.Rows)
            {
                Food food = new Food(item);
                listFood.Add(food);
            }
            return listFood;
        }
        public List<Food> GetListFood()
        {
            List<Food> listFood = new List<Food>();
            string query = "select * from dbo.Food  ";
            DataTable data = DataProvider.Instance.ExecuteQuery(query);

            foreach (DataRow item in data.Rows)
            {
                Food food = new Food(item);
                listFood.Add(food);
            }
            return listFood;
        }

        public List<Food> GetListSearchFoodByName(string name)
        {
            List<Food> listFood = new List<Food>();
            string query = string.Format("select * from dbo.Food where name like '{0}%'",name);
            DataTable data = DataProvider.Instance.ExecuteQuery(query);

            foreach (DataRow item in data.Rows)
            {
                Food food = new Food(item);
                listFood.Add(food);
            }
            return listFood;
        }
        public int InsertFood(string name, int idCategory, float price)
        {
            int result = -1;
            string query = string.Format("insert dbo.Food (name, idCategory, price) values ('{0}', {1}, {2})", name, idCategory, price);
            result = DataProvider.Instance.ExecuteNonQuery(query);
            return result;
        }
        public int UpdateFood(string name, int idCategory, float price, int id)
        {
            int result = -1;
            string query = string.Format("update dbo.Food set name = '{0}', idCategory = {1}, price = {2} where id = {3}", name, idCategory, price, id);
            result = DataProvider.Instance.ExecuteNonQuery(query);
            return result;
        }
        public int DeleteFood(int id)
        {
            int result = -1;
            BillInfoDAO.Instance.DeleteBillInfoByIdFood(id);
            string query = string.Format("delete dbo.Food where id = {0}", id);
            result = DataProvider.Instance.ExecuteNonQuery(query);
            return result;
        }
        public void ChangeIdCategoryOnDeleted(int id)
        {
            string query = string.Format("update dbo.Food set idCategory = (select MAX(id) from dbo.FoodCategory where id != {0}) where idCategory = {1}", id, id);
            DataProvider.Instance.ExecuteQuery(query);
        }
    }
}

using QuanLyQuanCafe.DTO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyQuanCafe.DAO
{
    public class FoodCategoryDAO
    {
        private static FoodCategoryDAO instance;

        public static FoodCategoryDAO Instance
        {
            get { if (instance == null) instance = new FoodCategoryDAO(); return FoodCategoryDAO.instance; }
            private set { FoodCategoryDAO.instance = value; }
        }

        private FoodCategoryDAO() { }

        public List<FoodCategory> GetListFoodCategorie()
        {
            List<FoodCategory> listFoodCatagory = new List<FoodCategory>();
            string query = "select * from FoodCategory";
            DataTable data = DataProvider.Instance.ExecuteQuery(query);

            foreach (DataRow item in data.Rows)
            {
                FoodCategory foodCategory = new FoodCategory(item);
                listFoodCatagory.Add(foodCategory);
            }
            return listFoodCatagory;
        }
        public FoodCategory GetFoodCategoryById(int id)
        {
            FoodCategory foodCategory = null;
            string query = "select * from FoodCategory where id = "+ id;
            DataTable data = DataProvider.Instance.ExecuteQuery(query);

            foreach (DataRow item in data.Rows)
            {
                foodCategory = new FoodCategory(item);
            }
            return foodCategory;
        }
        public int InsertCategory(string name)
        {
            int result = -1;
            string query = string.Format("insert dbo.FoodCategory ( name) values ('{0}')", name);
            result = DataProvider.Instance.ExecuteNonQuery(query);
            return result;
        }
        public int UpdateCategory(int id, string name)
        {
            int result = -1;
            string query = string.Format("update dbo.FoodCategory set name = '{0}' where id = {1}", name, id);
            result = DataProvider.Instance.ExecuteNonQuery(query);
            return result;
        }
        public int DeleteCategory(int id)  //references dbo.Food
        {
            int result = -1;
            FoodDAO.Instance.ChangeIdCategoryOnDeleted(id);
            string query = string.Format("delete dbo.FoodCategory where id = {0}", id);
            result = DataProvider.Instance.ExecuteNonQuery(query);
            return result;
        }
    }
}

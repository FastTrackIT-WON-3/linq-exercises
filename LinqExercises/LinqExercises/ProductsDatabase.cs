using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinqExercises
{
    public static class ProductsDatabase
    {
        public static IEnumerable<Category> AllCategories
        {
            get
            {
                yield return new Category(1, "Laptopuri");
                yield return new Category(2, "Telefoane");
                yield return new Category(3, "Tablete");
                yield return new Category(4, "Refrigerators");
            }
        }

        public static IEnumerable<Product> AllProducts
        {
            get
            {
                // Laptopuri
                yield return new Product(1, "Lenovo IdeaPad", 1);
                yield return new Product(2, "HP Envy", 1);
                yield return new Product(3, "Dell Latitude", 1);

                // Telefoane
                yield return new Product(4, "Samsung Galaxy Phone", 2);
                yield return new Product(5, "Huawei Phone", 2);
                yield return new Product(6, "Xiaomi Phone", 2);
                yield return new Product(7, "Nokia Phone", 2);
                yield return new Product(8, "iPhone", 2);

                // Tablete
                yield return new Product(9, "Samsung Galaxy Tab", 3);
                yield return new Product(10, "Huawei Tablet", 3);
                yield return new Product(11, "Lenovo Tablet", 3);
                yield return new Product(12, "iPad", 3);

                // Products with orphan categories
                yield return new Product(13, "Coca Cola", -1);
            }
        }
    }
}

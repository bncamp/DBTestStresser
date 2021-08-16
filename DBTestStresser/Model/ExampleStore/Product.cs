using DBTestStresser.Util;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace DBTestStresser.Model {
    public class Product {
        public int Id { get; set; }
        public Brand Brand { get; set; }
        public string Name { get; set; }

        public double Price { get; set; }

        public int Stock { get; set; }

        public static Product GenerateRandom(int maxBrandId, int id=-1) {
            Product p = new Product();
            p.Id = id;
            p.Name = RandomDB.GenerateRandomString(15);
            p.Price = (double) RandomDB.GenerateRandomInt(0, 500);
            p.Stock = RandomDB.GenerateRandomInt(0, 15);
            int brand_id = RandomDB.GenerateRandomInt(0, maxBrandId);
            p.Brand = Brand.GenerateRandom(brand_id);

            return p;
        }

        public JsonDocument ToJSON() {
            string content = JsonConvert.SerializeObject(this);
            Console.WriteLine(content);
            var json = JsonDocument.Parse(content);
            return json;
        }

        
    }
}

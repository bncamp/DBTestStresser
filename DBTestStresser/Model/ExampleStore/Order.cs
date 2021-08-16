using DBTestStresser.Util;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace DBTestStresser.Model {
    public class Order {
        int Id { get; set; }
        Product Product { get; set; }
        Customer Customer { get; set; }

        public static Order GenerateRandom(int maxProductId, int maxCustomerId, int maxBrandId, int id = -1) {
            Order o = new Order();
            o.Id = id;
            int p_id = RandomDB.GenerateRandomInt(0, maxProductId);
            int c_id = RandomDB.GenerateRandomInt(0, maxCustomerId);
            o.Product = Product.GenerateRandom(maxBrandId,p_id);
            o.Customer = Customer.GenerateRandom(p_id);

            return o;
        }

        public JsonDocument ToJSON() {
            string content = JsonConvert.SerializeObject(this);
            var json = JsonDocument.Parse(content);
            return json;
        }
    }
}

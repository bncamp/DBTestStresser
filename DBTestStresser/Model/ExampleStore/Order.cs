using DBTestStresser.Util;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace DBTestStresser.Model {
    public class Order : ExampleStore.ModelExampleStore{
        public int Id { get; set; }
        public Product Product { get; set; }
        public Customer Customer { get; set; }

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
            Order o = this;
            string content = JsonConvert.SerializeObject(o);
            //Newtonsoft.Json.JsonSerializer serializer = new Newtonsoft.Json.JsonSerializer();

            var json = JsonDocument.Parse(content);
            return json;
        }

        public string ToJsonString() {
            Order o = this;
            string content = JsonConvert.SerializeObject(o);
            return content;
        }
    }
}

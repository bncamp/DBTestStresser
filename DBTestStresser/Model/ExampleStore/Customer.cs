using DBTestStresser.Util;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace DBTestStresser.Model {
    public class Customer : ExampleStore.ModelExampleStore {

        public int Id { get; set; }
        public string Surname { get; set; }
        public string Name { get; set; }

        public string City { get; set; }
        public string Email { get; set; }

        public static Customer GenerateRandom(int id = -1) {
            Customer c = new Customer();
            c.Id = id;
            c.Surname = RandomDB.GenerateRandomString(15);
            c.Name = RandomDB.GenerateRandomString(15);
            c.City = RandomDB.GenerateRandomString(15);
            c.Email = RandomDB.GenerateRandomString(30);

            return c;
        }

        public JsonDocument ToJSON() {
            string content = JsonConvert.SerializeObject(this);
            var json = JsonDocument.Parse(content);
            return json;
        }
    }
}

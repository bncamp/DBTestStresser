using DBTestStresser.Util;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace DBTestStresser.Model {


    public class Brand : ExampleStore.ModelExampleStore {
        
        public int Id { get; set; }
        public string Name { get; set; }

        public static Brand GenerateRandom(int id = -1) {
            Brand b = new Brand();
            b.Id = id;
            b.Name = RandomDB.GenerateRandomString(10);

            return b;
        }

        public JsonDocument ToJSON() {
            string content = JsonConvert.SerializeObject(this);
            var json = JsonDocument.Parse(content);

            return json;
        }

        public string ToCypherJsonString() {
            string json = JsonConvert.SerializeObject(this);
            string regex = "\"([^\"]+)\":"; // capture property
            json = Regex.Replace(json, regex, "$1:");
            return json;
        }
    }
}

using DBTestStresser.Util;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBTestStresser.Model.DBMS {
    public class MySQL : EntityDBMS {

        public MySQL(string ip, string port) {
            this.Ip = ip;
            this.Port = port;
            this.Name = "MySQL";
        }
        public override string BuildConnectionString() {
            return String.Format(
                "Server = {0};" +
                "Port={1};" +
                " Database = {2};" +
                " Uid = {3};" +
                " Pwd = {4};",
                Ip,Port,DB_NAME,USER_NAME,PASSWORD
                );
        }

        public override DbConnection GetConnection() {
            return new MySqlConnection(BuildConnectionString());
        }

        public override void PopulateDB() {
            var cnx = GetConnection();
            
            string wipe = "DELETE FROM orders; DELETE FROM products; DELETE FROM customers;" +
                "DELETE FROM brands;"
                //+
                //"TRUNCATE TABLE orders; TRUNCATE TABLE customers; TRUNCATE TABLE customers;" +
                //"TRUNCATE TABLE orders";
            ;
            //string resetSequences = "ALTER SEQUENCE products_id_seq RESTART WITH 1;" +
            //                   "ALTER SEQUENCE brands_id_seq RESTART WITH 1;" +
            //                   "ALTER SEQUENCE customers_id_seq RESTART WITH 1; ";
            GUI.Log("Connection to DB...");
            cnx.Open();
            GUI.Log("Connected !");
            GUI.Log("Wiping existing data...");
            WriteQuery(cnx, wipe);

            GUI.Log("Resetting auto increments...");
            string reset = "ALTER TABLE brands AUTO_INCREMENT = 1;" +
                "ALTER TABLE products AUTO_INCREMENT = 1;" +
                "ALTER TABLE customers AUTO_INCREMENT = 1; ";
            WriteQuery(cnx, reset);

            // Brands
            GUI.Log("BRANDS - Generating query");
            List<string> types = new List<string> { "string" };
            string insert = "INSERT INTO brands(name) ";
            insert += RandomDB.BuildInsertIntoValues(types, N_BRANDS) + ";";
            GUI.Log("BRANDS - Executing query");
            WriteQuery(cnx, insert);

            // Customers
            GUI.Log("CUSTOMERS - Generating query");
            types = new List<string> { "string", "string", "string", "string" };
            insert = "INSERT INTO customers(surname,name,city,email)";
            insert += RandomDB.BuildInsertIntoValues(types, N_CUSTOMERS) + ";";
            GUI.Log("CUSTOMERS - Executing query");
            WriteQuery(cnx, insert);

            // Products
            GUI.Log("PRODUCTS - Generating query");
            types = new List<string> { "string", "double", "int", "int" };
            var intMins = new int[] { 100, 1, 1 };
            var intMaxs = new int[] { 400, N_BRANDS, 100 };
            insert = "INSERT INTO products(name,price,brand_id,stock)";
            insert += RandomDB.BuildInsertIntoValues(types, N_PRODUCTS, intMins, intMaxs) + ";";
            GUI.Log("PRODUCTS - Executing query");
            WriteQuery(cnx, insert);

            // Orders
            GUI.Log("ORDERS - Generating query");
            types = new List<string> { "string", "int", "int" };
            intMins = new int[] { 1, 1 };
            intMaxs = new int[] { N_CUSTOMERS, N_PRODUCTS };
            insert = "INSERT INTO orders(date,customer_id,product_id)";
            insert += RandomDB.BuildInsertIntoValues(types, N_ORDERS, intMins, intMaxs) + ";";
            GUI.Log("ORDERS - Executing query");
            WriteQuery(cnx, insert);

            GUI.Log("===== END ======");
            cnx.Close();
        }

        public override void ReadQuery(DbConnection cnx, string query) {
            var cmd = new MySqlCommand(query, (MySqlConnection)cnx);
            var reader = cmd.ExecuteReader();
            var p = new Product();
            var b = new Brand();
            while (reader.Read()) {
                p.Id = reader.GetInt32(0);
                p.Name = reader.GetString(1);
                p.Price = reader.GetDouble(2);
                p.Stock = reader.GetInt32(3);
            }
        }

        public override void WriteQuery(DbConnection cnx, string query) {
            var cmd = new MySqlCommand(query, (MySqlConnection)cnx);
            cmd.ExecuteNonQuery();
        }
    }
}

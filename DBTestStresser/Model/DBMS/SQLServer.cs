using DBTestStresser.Util;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBTestStresser.Model.DBMS {
    public class SQLServer : EntityDBMS {

        public SQLServer(string ip, string port) {
            this.Ip = ip;
            this.Port = port;
            this.Name = "SQLServer";
        }

        public override string BuildConnectionString() {

            var ip = Encoding.Unicode.GetBytes(Ip);
            var dbName = Encoding.Unicode.GetBytes(DB_NAME);
            var uname = Encoding.Unicode.GetBytes(USER_NAME);
            //var pass = Encoding.Unicode.GetBytes("sT0R312000 *");
            string password = "sT0R312000*";
            return String.Format(

            "Server = {0};" +            
            //"Data Source={0}\\SQLEXPRESS;" +
            //"Network Library=DBMSSOCN;" +
            " Database={1};" +
            " User ID={2};" +
            " Password={3};"
            //"Trusted_Connection=True;"
            ,
            Ip,DB_NAME,USER_NAME,password
            );

        }

        public override DbConnection GetConnection() {
            return new SqlConnection(BuildConnectionString());
        }

        public override void PopulateDB() {
            
            var cnx = GetConnection();

            string wipe = "DELETE FROM orders; DELETE FROM products; DELETE FROM customers;" +
                "DELETE FROM brands";
            //string resetSequences = "ALTER SEQUENCE products_id_seq RESTART WITH 1;" +
            //                   "ALTER SEQUENCE brands_id_seq RESTART WITH 1;" +
            //                   "ALTER SEQUENCE customers_id_seq RESTART WITH 1; ";
            GUI.Log("Connection to DB...");
            cnx.Open();
            GUI.Log("Connected !");
            GUI.Log("Resetting sequences...");
            string reset = 
                "DBCC CHECKIDENT ('brands', RESEED, 0);" +
                "DBCC CHECKIDENT ('products', RESEED, 0);" +
                "DBCC CHECKIDENT ('customers', RESEED, 0);"
                ;
            WriteQuery(cnx, reset);
            GUI.Log("Wiping existing data...");
            WriteQuery(cnx, wipe);


            // Brands
            GUI.Log("BRANDS - Generating query");
            List<string> types = new List<string> { "string" };
            string insert = "INSERT INTO brands(name) ";
            List<string> query = RandomDB.BuildInsertThousandSplitted(insert,types, N_BRANDS);
            GUI.Log("BRANDS - Executing query");
            foreach(var q in query) {
                WriteQuery(cnx, q);
            }
            

            // Customers
            GUI.Log("CUSTOMERS - Generating query");
            types = new List<string> { "string", "string", "string", "string" };
            insert = "INSERT INTO customers(surname,name,city,email)";
            query = RandomDB.BuildInsertThousandSplitted(insert, types, N_CUSTOMERS);
            GUI.Log("CUSTOMERS - Executing query");
            foreach (var q in query) {
                WriteQuery(cnx, q);
            }

            // Products
            GUI.Log("PRODUCTS - Generating query");
            types = new List<string> { "string", "double", "int", "int" };
            var intMins = new int[] { 100, 1, 1 };
            var intMaxs = new int[] { 400, N_BRANDS, 100 };
            insert = "INSERT INTO products(name,price,brand_id,stock)";
            query = RandomDB.BuildInsertThousandSplitted(insert, types, N_PRODUCTS, intMins, intMaxs);
            GUI.Log("PRODUCTS - Executing query");
            foreach (var q in query) {
                WriteQuery(cnx, q);
            }

            // Orders
            GUI.Log("ORDERS - Generating query");
            types = new List<string> { "string", "int", "int" };
            intMins = new int[] { 1, 1 };
            intMaxs = new int[] { N_CUSTOMERS, N_PRODUCTS };
            insert = "INSERT INTO orders(date,customer_id,product_id)";
            query = RandomDB.BuildInsertThousandSplitted(insert, types, N_ORDERS, intMins, intMaxs);
            GUI.Log("ORDERS - Executing query");
            foreach (var q in query) {
                WriteQuery(cnx, q);
            }

            GUI.Log("===== END ======");
            
            cnx.Close();
        }

        public override void ReadQuery(DbConnection cnx, string query) {
            var cmd = new SqlCommand(query, (SqlConnection) cnx);
            var reader = cmd.ExecuteReader();
            var p = new Product();
            var b = new Brand();
            while (reader.Read()) {
                //p.Id = reader.GetInt32(0);
                //p.Name = reader.GetString(1);
                //p.Price = reader.GetDouble(2);
                //p.Stock = reader.GetInt32(3);
            }
        }

        public override void WriteQuery(DbConnection cnx, string query) {
            var cmd = new SqlCommand(query, (SqlConnection)cnx);
            cmd.ExecuteNonQuery();
        }
    }
}

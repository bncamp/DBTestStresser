using Cassandra;
using DBTestStresser.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBTestStresser.Model.DBMS {

    
    public class Cassandra : EntityDBMS {
        private static PoolingOptions pooling = new PoolingOptions()
            //.SetCoreConnectionsPerHost(HostDistance.Local, 100)
            //.SetMaxConnectionsPerHost(HostDistance.Local, 100)
            //.SetCoreConnectionsPerHost(HostDistance.Remote, 100)
            //.SetMaxConnectionsPerHost(HostDistance.Remote, 100)
            //.SetMaxSimultaneousRequestsPerConnectionTreshold(HostDistance.Local, 10000)
            //.SetMinSimultaneousRequestsPerConnectionTreshold(HostDistance.Remote, 1);
            ;
        public List<string> ContactPoints = new List<string>();

        private PreparedStatement prep;
        public Cassandra(string ip, string port) {
            this.Ip = ip;
            ContactPoints.Add(ip);
            this.Port = !String.IsNullOrEmpty(port) ? port : "9042";
            this.Name = "Cassandra";
        }

        public override string BuildConnectionString() {
            return "";
        }
        public override DatabaseConnection GetConnection() {
            return new DatabaseConnection(
                Cluster.Builder()
               .AddContactPoint(Ip)
               .WithPoolingOptions(pooling)
               .WithQueryOptions(new QueryOptions().SetConsistencyLevel(ConsistencyLevel.One))
               .WithLoadBalancingPolicy(new RoundRobinPolicy())
               .Build()
            );
        }
        public override string TestConnection() {
            string r = "Connexion successful !";

            try {
                var c = (Cluster)GetConnection().GetConnectionInstance();

                c.Connect();
            } catch (Exception e) {
                r = "Connection failed : " + e.Message;
            }

            return r;
        }

        public override void PopulateDB() {
            var cnx = (Cluster) GetConnection().GetConnectionInstance();
            var session = cnx.Connect();
            GUI.Log("Deleting data...");
            string[] del = new string[] {
                "TRUNCATE ExampleStore.Brand;",
                "TRUNCATE ExampleStore.ProductBrand;",
                "TRUNCATE ExampleStore.OrderProduct;",
                "TRUNCATE ExampleStore.OrderCustomer;",
                "TRUNCATE ExampleStore.\"Order\";",
                "TRUNCATE ExampleStore.Customer" 
            };

            foreach (var d in del) {
                session.Execute(d);
            }

            GUI.Log("Generating brands..");
            List<string> types = new List<string> {
                "int","string"
            };
            
            string cql;
            List<string> cqls = new List<string>();
            for (int i = 0; i < N_BRANDS; i++) {
                cql = String.Format("INSERT INTO ExampleStore.Brand(brand_id, name) VALUES ({0},'{1}');",
                    i, RandomDB.GenerateRandomString(10));
                cqls.Add(cql);
            }

            foreach (var c in cqls) {
                session.Execute(c);
            }

            cqls = new List<string>();
            GUI.Log("Generating Products and ProductBrand....");
            for (int i = 0; i < N_PRODUCTS; i++) {
                cql = String.Format("INSERT INTO ExampleStore.Product(product_id, product_name,product_price,product_stock)" +
                    " VALUES ({0},'{1}',{2},{3});",
                    i, 
                    RandomDB.GenerateRandomString(10),
                    (double)RandomDB.GenerateRandomInt(0,500), 
                    RandomDB.GenerateRandomInt(0,20));
                cqls.Add(cql);
                cql = String.Format("INSERT INTO ExampleStore.ProductBrand(product_id, brand_id, product_name,product_price,product_stock)" +
                    " VALUES ({0},{1},'{2}',{3},{4});",
                    i,
                    RandomDB.GenerateRandomInt(0, N_BRANDS),
                    RandomDB.GenerateRandomString(10),
                    (double) RandomDB.GenerateRandomInt(0, 500),
                    RandomDB.GenerateRandomInt(0, 20));
                cqls.Add(cql);
            }
            foreach (var c in cqls) {
                session.Execute(c);
            }

            GUI.Log("Generating Customer...");
            cqls = new List<string>();
            for (int i = 0; i < N_CUSTOMERS; i++) {
                cql = String.Format("INSERT INTO ExampleStore.Customer(customer_id,customer_name,customer_surname,customer_city," +
                    "customer_email)" +
                    " VALUES ({0},'{1}','{2}','{3}','{4}');",
                    i,
                    RandomDB.GenerateRandomString(20),
                    RandomDB.GenerateRandomString(20),
                    RandomDB.GenerateRandomString(20),
                    RandomDB.GenerateRandomString(30));
                cqls.Add(cql);

            }
            foreach (var c in cqls) {
                session.Execute(c);
            }

            GUI.Log("Generating Order, OrderProduct and OrderCustomer...");
            cqls = new List<string>();
            for (int i = 0; i < N_ORDERS; i++) {
                cql = String.Format("INSERT INTO ExampleStore.\"Order\"(order_id,order_date)" +
                    
                   " VALUES ({0},'{1}')",
                   i,
                   RandomDB.GenerateRandomString(10));
                cqls.Add(cql);
                cql = String.Format("INSERT INTO ExampleStore.OrderCustomer(order_id,customer_id,order_date," +
                    "customer_name,customer_surname,customer_city," +
                   "customer_email)" +
                   " VALUES ({0},{1},'{2}','{3}','{4}','{5}','{6}');",
                   i,
                   RandomDB.GenerateRandomInt(0, N_CUSTOMERS),
                   RandomDB.GenerateRandomString(10),
                   RandomDB.GenerateRandomString(20),
                   RandomDB.GenerateRandomString(20),
                   RandomDB.GenerateRandomString(20),
                   RandomDB.GenerateRandomString(30));
                cqls.Add(cql);
                cql = String.Format("INSERT INTO ExampleStore.OrderProduct(order_id,product_id," +
                    "order_date," +
                    "product_name," +
                    "product_price," +
                    "product_stock )" +
                   " VALUES ({0},{1},'{2}','{3}',{4},{5});",
                   i,
                   RandomDB.GenerateRandomInt(0, N_PRODUCTS),
                   RandomDB.GenerateRandomString(10),
                   RandomDB.GenerateRandomString(20),
                   RandomDB.GenerateRandomInt(0,500),
                   RandomDB.GenerateRandomInt(0,20)
                   );
                cqls.Add(cql);
            }

            foreach (var c in cqls) {
                session.Execute(c);
            }
            cnx.Shutdown();
            GUI.Log("Done !");
        }
        public override string[] GenerateRandomReadQueries(int amount) {
            string[] cqls = new string[amount];
            int nbProductPerPage = 100;
            int greater, lower;
            for(int i = 0; i < amount; i++) {
                //greater = RandomDB.GenerateRandomInt(0, N_PRODUCTS - nbProductPerPage);
                //lower = greater + nbProductPerPage;
                cqls[i] = "SELECT * FROM ExampleStore.ProductBrand";
                    //" WHERE product_id BETWEEN " + lower +
                    //" AND " + greater + " allow filtering";
            }

            return cqls;
        }

        public override string[] GenerateRandomWriteQueries(int amount) {
            string cql = "";
            string[] cqls = new string[amount];
            for (int i = 0; i < amount; i++) {
                cql = "BEGIN BATCH ";
                cql += String.Format("INSERT INTO ExampleStore.\"Order\"(order_id,order_date)" +

                   " VALUES ({0},'{1}')",
                   i,
                   RandomDB.GenerateRandomString(10));

                cql += String.Format("INSERT INTO ExampleStore.OrderCustomer(order_id,customer_id,order_date," +
                    "customer_name,customer_surname,customer_city," +
                   "customer_email)" +
                   " VALUES ({0},{1},'{2}','{3}','{4}','{5}','{6}');",
                   i,
                   RandomDB.GenerateRandomInt(0, N_CUSTOMERS),
                   RandomDB.GenerateRandomString(10),
                   RandomDB.GenerateRandomString(20),
                   RandomDB.GenerateRandomString(20),
                   RandomDB.GenerateRandomString(20),
                   RandomDB.GenerateRandomString(30));

                cql += String.Format("INSERT INTO ExampleStore.OrderProduct(order_id,product_id," +
                    "order_date," +
                    "product_name," +
                    "product_price," +
                    "product_stock )" +
                   " VALUES ({0},{1},'{2}','{3}',{4},{5});",
                   i,
                   RandomDB.GenerateRandomInt(0, N_PRODUCTS),
                   RandomDB.GenerateRandomString(10),
                   RandomDB.GenerateRandomString(20),
                   RandomDB.GenerateRandomInt(0, 500),
                   RandomDB.GenerateRandomInt(0, 20)
                   );

                cql += " APPLY BATCH";
                cqls[i] = cql;
            }
            return cqls;
        }

        private void ExecuteCassandraQuery(DatabaseConnection cnx, string query) {
            var c = (Cluster) cnx.GetConnectionInstance();
            //var sess = c.Connect();
            //sess.Execute(query);
            var sess = cnx.CassandraSession;

            if (prep == null) {
                prep = sess.Prepare(query);
            }
            var statement = prep.Bind();
            var r = sess.ExecuteAsync(statement);
            //r.Wait();
        }
        public override void ReadQuery(DatabaseConnection cnx, string query) {
            ExecuteCassandraQuery(cnx, query);
            
        }

        public override void WriteQuery(DatabaseConnection cnx, string query) {
            ExecuteCassandraQuery(cnx, query);
        }

        public override void UpdateQuery(DatabaseConnection cnx, string query) {
            ExecuteCassandraQuery(cnx, query);
        }

        public override string[] GenerateRandomUpdateQueries(int amount) {
            string[] queries = new string[amount];
            string template = "UPDATE examplestore.product SET product_stock=20 WHERE product_id=";
            for (int i = 0; i < amount; i++) {
                queries[i] = template + RandomDB.GenerateRandomInt(0, N_PRODUCTS);
            }

            return queries;
        }
    }
}

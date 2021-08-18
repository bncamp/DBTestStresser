using DBTestStresser.Util;
using Neo4j.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBTestStresser.Model.DBMS {
    public class Neo4J : EntityDBMS {

        public Neo4J (string ip, string port) {
            this.Ip = ip;
            this.Port = !String.IsNullOrEmpty(port) ? port : "7687";
            this.Name = "Neo4j";
        }
        public override string BuildConnectionString() {
            return "bolt://" + Ip + ":" + Port;
        }

        public override DatabaseConnection GetConnection() {
            
            return new DatabaseConnection(GraphDatabase.Driver(
                BuildConnectionString(),AuthTokens.Basic(USER_NAME,PASSWORD)));
        }

        public override void PopulateDB() {
            var sess = (ISession)GetConnection().GetConnectionInstance();
            

            // Delete all nodes & relationships
            var wipeNodesWithRelations = "match (a) -[r] -> () delete a, r";
            var wipeSinglesNodes = "match (a) delete a";
            sess.Run(wipeNodesWithRelations);
            sess.Run(wipeSinglesNodes);

            // Brands
            GUI.Log("Generating brands...");
            string json;
            for (int i = 0; i < N_BRANDS; i++) {
                json = Brand.GenerateRandom(i).ToCypherJsonString();
                sess.Run("CREATE (b:Brand " + json + ")");
            }

            GUI.Log("Generating customers...");
            for (int i = 0; i < N_CUSTOMERS; i++) {
                json = Customer.GenerateRandom(i).ToCypherJsonString();
                sess.Run("CREATE (c:Customer " + json + ")");
            }

            Product p;
            string cyph;
            GUI.Log("Generating products...");
            for (int i = 0; i < N_PRODUCTS; i++) {
                p = Product.GenerateRandom(N_BRANDS, i);
                cyph = "MATCH (b:Brand {Id:" + p.Brand.Id + "}) " +
                "CREATE (p:Product " + p.ToCypherJsonString() + ") -[:OWNED_BY]-> (b)";
                
                sess.Run(cyph);
            }

            Order o;

            GUI.Log("Generating orders...");
            for (int i = 0; i < N_ORDERS; i++) {
                o = Order.GenerateRandom(N_PRODUCTS, N_CUSTOMERS, N_BRANDS, i);
                cyph =
                "MATCH (c:Customer {Id:" + o.Customer.Id + "}) " +
                "MATCH (p:Product {Id:" + o.Product.Id + "}) " +
                "CREATE (o:Order " + o.ToCypherJsonString() + ") -[:CREATED_FOR]-> (c) -[:BOUGHT]-> (p)";

                sess.Run(cyph);
            }

            GUI.Log("Complete !");



        }


        public override string[] GenerateRandomReadQueries(int amount) {
            var queries = new string[amount];

            string cyph = "MATCH (p:Product) WHERE p.Id > {0} AND p.Id < {1} return p";

            int greater, lower;
            int nbProductsPerPage = 100;
            for (int i = 0; i < amount; i++) {
                greater = RandomDB.GenerateRandomInt(0, N_PRODUCTS - nbProductsPerPage);
                lower = RandomDB.GenerateRandomInt(greater, greater + nbProductsPerPage);
                queries[i] = String.Format(cyph, greater, lower);
            }

            return queries;
        }

        public override string[] GenerateRandomWriteQueries(int amount) {
            Order o;
            string[] queries = new string[amount];
            string cyph = "CREATE (o:Order {0})";
            for (int i = 0; i < amount; i++) {
                o = Order.GenerateRandom(N_PRODUCTS,N_CUSTOMERS,N_BRANDS,-i);
                queries[i] = String.Format(cyph, o.ToCypherJsonString());
            }
            return queries;
        }

        

        public override void ReadQuery(DatabaseConnection cnx, string query) {
            var session = (ISession) cnx.GetConnectionInstance();
            session.Run(query);
        }

        public override string TestConnection() {
            var r = "Connected successfully !";
            try {
                var sess = (ISession) GetConnection().GetConnectionInstance();
                sess.Run("match (a) -> () RETURN a");
                
            } catch (Exception e) {
                r = "Connexion error : " + e.Message;
            }

            return r;
            
        }

        public override void WriteQuery(DatabaseConnection cnx, string query) {
            var session = (ISession) cnx.GetConnectionInstance();
            session.Run(query);
        }
    }
}

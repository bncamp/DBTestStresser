using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using MongoDB.Driver;

namespace DBTestStresser.Model.DBMS {
    public class MongoDb : EntityDBMS {
        private MongoDb() {

        }
        public MongoDb(string ip, string port) {
            this.Ip = ip;
            this.Port = port;
            this.Name = "MongoDB";
        }
        public override string BuildConnectionString() {
            return "mongodb://" + Ip + ":" + Port;
        }

        public override DbConnection GetConnection() {
            //return new MongoClient(BuildConnectionString()) as DbConnection;
            return null;
        }

        public override void PopulateDB() {
            MongoClient dbClient = new MongoClient(BuildConnectionString());
            var db = dbClient.GetDatabase(DB_NAME);

            GUI.Log("Generating brands...");
            var cl_brands = db.GetCollection<JsonDocument>(C_BRANDS);
            PopulateCollection(cl_brands,N_BRANDS);

            GUI.Log("Generating customers...");
            var cl_customers = db.GetCollection<JsonDocument>(C_CUSTOMERS);
            PopulateCollection(cl_customers,N_CUSTOMERS);

            GUI.Log("Generating products...");
            var cl_products = db.GetCollection<JsonDocument>(C_PRODUCTS);
            PopulateCollection(cl_products, N_PRODUCTS);

            GUI.Log("Generating orders...");
            var cl_orders = db.GetCollection<JsonDocument>(C_ORDERS);
            PopulateCollection(cl_orders, N_ORDERS);
            
        }

        public void PopulateCollection<T>(IMongoCollection<T> collection, int amount) {
            collection.DeleteMany(Builders<T>.Filter.Empty);

            JsonDocument[] jsons = new JsonDocument[amount];
            for (int i = 0; i < amount; i++) {
                //jsons[i] = T.GenerateRandom(i).ToJSON();
                //Console.WriteLine(Brand.GenerateRandom(i));
            }
            //collection.InsertMany(jsons);
        }



        public override void ReadQuery(DbConnection cnx, string query) {
            var client = new MongoClient(BuildConnectionString());
            var db = client.GetDatabase(DB_NAME);
            var c = db.GetCollection<Customer>(C_CUSTOMERS);
            
        }

        public override void WriteQuery(DbConnection cnx, string query) {
            var client = new MongoClient(BuildConnectionString());
            var db = client.GetDatabase(DB_NAME);
            var c = db.GetCollection<Order>(C_CUSTOMERS);
            Order o = new Order();
            c.InsertOne(o);
        }
    }
}

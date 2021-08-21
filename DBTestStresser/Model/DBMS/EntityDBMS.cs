using DBTestStresser.Model.DBMS;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBTestStresser.Model {
    public abstract class EntityDBMS {

        public string Ip;
        public string DefaultPort;
        public string Port;
        public string Name;
        
        public DatabaseConnection DbConnection;
        public string QueryLangage;

        // DATABASE PROPERTIES
        public const string DB_NAME = "ExampleStore";
        public const string USER_NAME = "u_store";
        public const string PASSWORD = "store";

        // Query Langage List
        public const string QL_SQL = "SQL";
        public const string QL_MONGOSH = "MongoSH";

        // COLUMNS
        public const string C_CUSTOMERS = "customers";
        public const string C_PRODUCTS = "products";
        public const string C_BRANDS = "brands";
        public const string C_ORDERS = "orders";

        // Populate parameters
        public const int N_PRODUCTS = 1000;
        public const int N_BRANDS = 250;
        public const int N_CUSTOMERS = 10000;
        public const int N_ORDERS = 20000;

        public abstract void ReadQuery(DatabaseConnection cnx, string query);
        public abstract void WriteQuery(DatabaseConnection cnx, string query);

        public abstract void UpdateQuery(DatabaseConnection cnx, string query);
        public abstract void PopulateDB();
        public abstract string BuildConnectionString();

        public abstract DatabaseConnection GetConnection();
        public static EntityDBMS CreateDBMS(string name, string ip, string port) {
            EntityDBMS dbms = null;
            switch(name) {
                case "PostgreSQL":
                    dbms = (EntityDBMS) new PostgreSQL(ip, port);
                    break;
                case "MySQL":
                    dbms = (EntityDBMS) new MySQL(ip, port);
                    break;
                case "SQLServer":
                    dbms = (EntityDBMS) new SQLServer(ip, port);
                    break;
                case "MongoDB":
                    dbms = (EntityDBMS)new MongoDb(ip, port);
                    break;
                case "Neo4j":
                    dbms = (EntityDBMS) new Neo4J(ip, port);
                    break;
                case "Cassandra":
                    dbms = (EntityDBMS) new DBMS.Cassandra(ip, port);
                    break;
            }

            return dbms;
            
        }


        public abstract string[] GenerateRandomReadQueries(int amount);
        public abstract string[] GenerateRandomWriteQueries(int amount);
        public abstract string[] GenerateRandomUpdateQueries(int amount);

        public abstract string TestConnection();
    }
}

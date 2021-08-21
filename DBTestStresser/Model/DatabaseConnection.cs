using Cassandra;
using MongoDB.Driver;
using Neo4j.Driver;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBTestStresser.Model {
    public class DatabaseConnection {
        public DbConnection Classic { get; set; }
        public MongoClient MongoClient { get; set; }

        public IDriver Neo4jDriver { get; set; }
        public Neo4j.Driver.ISession Neo4jSession { get; set; }

        public Cluster CassandraCluster { get; set; }
        public Session CassandraSession { get; set; }

        public Boolean NeedConnexionOpening { get; }

        public string ConnectionType { get; set; }


        public static string T_CLASSIC = "Classic";
        public static string T_MONGO_CLIENT = "MongoClient";
        public static string T_NEO4J = "Neo4j";
        public static string T_CASSANDRA = "CassandraCluster";

        public DatabaseConnection(DbConnection cnx) {
            this.Classic = cnx;
            this.NeedConnexionOpening = true;
            this.ConnectionType = T_CLASSIC;
        }

        public DatabaseConnection(MongoClient cnx ) {
            this.MongoClient = cnx;
            this.NeedConnexionOpening = false;
            this.ConnectionType = T_MONGO_CLIENT;
        }

        public DatabaseConnection(IDriver cnx) {
            this.Neo4jDriver = cnx;
            
            
            this.NeedConnexionOpening = true;
            this.ConnectionType = T_NEO4J;
        }

        public DatabaseConnection(Cluster cassandraCluster) {
            this.CassandraCluster = cassandraCluster;
            this.NeedConnexionOpening = true;
            this.ConnectionType = T_CASSANDRA;
        }
        public object GetConnectionInstance() {
            object cnx = null;
            if (ConnectionType == T_CLASSIC) {
                cnx = Classic;
            } else if (ConnectionType == T_MONGO_CLIENT) {
                cnx = MongoClient;
            } else if (ConnectionType == T_NEO4J) {
                //Neo4jSession = Neo4jDriver.Session();
                //Neo4jSession.SessionConfig = SessionConfigBuilder.ForDatabase(EntityDBMS.DB_NAME).;
                //cnx = Neo4jSession;
                cnx = Neo4jDriver;
            } else if (ConnectionType == T_CASSANDRA) {
                CassandraSession = (Session)CassandraCluster.Connect();
                //cnx = CassandraSession;
                cnx = CassandraCluster;
            }

            return cnx;
        }

        public System.Data.ConnectionState GetState() {
            System.Data.ConnectionState r = System.Data.ConnectionState.Closed;
            if (ConnectionType == T_CLASSIC) {
                r = Classic.State;
            } else if (ConnectionType == T_MONGO_CLIENT) {
                
            } else if (ConnectionType == T_NEO4J) {
                
            }

            return r;
        }
        public void Open() {
            if (NeedConnexionOpening) {
                if (ConnectionType == T_CLASSIC ) {
                    Classic.Open();
                } else if (ConnectionType == T_NEO4J) {
                    // TODO BETTER
                } else if (ConnectionType == T_CASSANDRA) {
                    //CassandraCluster.Connect();
                }
                    
            }
            
        }

        public void Close() {
            if (NeedConnexionOpening) {
                if (ConnectionType == T_CLASSIC) {
                    Classic.Close();
                } else if (ConnectionType == T_NEO4J) {
                    
                } else if (ConnectionType == T_CASSANDRA) {
                    CassandraCluster.Shutdown();
                }
            }
                
        }
    }
}

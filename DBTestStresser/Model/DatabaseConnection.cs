using MongoDB.Driver;
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
        public Boolean NeedConnexionOpening { get; }

        public string ConnectionType { get; set; }

        public static string T_CLASSIC = "Classic";
        public static string T_MONGO_CLIENT = "MongoClient";

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

        public object GetConnexion() {
            object cnx = null;
            if (Classic != null) {
                cnx = Classic;
            } else if (MongoClient != null) {
                cnx = MongoClient;
            }

            return cnx;
        }

        public System.Data.ConnectionState GetState() {
            System.Data.ConnectionState r = System.Data.ConnectionState.Closed;
            if (Classic != null) {
                r = Classic.State;
            } else if (MongoClient != null) {
                
            }

            return r;
        }
        public void Open() {
            if (NeedConnexionOpening)
                Classic.Open();
        }

        public void Close() {
            if (NeedConnexionOpening)
                Classic.Close();
        }
    }
}

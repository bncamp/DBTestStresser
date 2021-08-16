using DBTestStresser.Util;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBTestStresser.Model.DBMS {
    public class PostgreSQL : EntityDBMS {

        private PostgreSQL() {

        }
        public PostgreSQL(string ip, string port) {
            this.Ip = ip;
            this.Port = port;
            this.Name = "PostgreSQL";
        }
        public override string BuildConnectionString() {
            string cnx_str = String.Format(
                        " Server = {0};" +
                        " Port = {1};" +
                        " Database = {2};" +
                        " User Id = {3};" +
                        " Password = {4};" +
                        " Command Timeout=0;",
                        Ip, Port, EntityDBMS.DB_NAME, EntityDBMS.USER_NAME, EntityDBMS.PASSWORD
                    );

            return cnx_str;
        }

        public override DbConnection GetConnection() {
            return (DbConnection) new NpgsqlConnection(BuildConnectionString());
        }
        public override void ReadQuery(DbConnection cnx, string query) {
            var cmd = new NpgsqlCommand(query, (NpgsqlConnection)cnx);
            //object r = cmd.ExecuteScalar();   
            Stopwatch s = new Stopwatch();
            s.Start();
            var reader = cmd.ExecuteReader();
            var p = new Product();
            var b = new Brand();
            
            while (reader.Read()) {
                p.Id = reader.GetInt32(0);
                p.Name = reader.GetString(1);
                p.Price = reader.GetDouble(2);
                p.Stock = reader.GetInt32(3);
            }
            s.Stop();
            Console.WriteLine("mesure ds query : " + s.ElapsedMilliseconds + "ms");
        }

        public override void WriteQuery(DbConnection cnx, string query) {
            var cmd = new NpgsqlCommand(query, (NpgsqlConnection)cnx);
            cmd.ExecuteNonQuery();
        }

        public override void PopulateDB() {
            GUI.Log("Building insert queries...");
            var cnx = new NpgsqlConnection(BuildConnectionString());
            
            cnx.Close();
        }

        
    }
}

using DBTestStresser.Model;
using DBTestStresser.Util;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DBTestStresser {
    public partial class GUI : Form {

        public static GUI INSTANCE = null;
        public static EntityDBMS DBMS = null;
        public GUI() {
            InitializeComponent();

            cb_operation.Items.Add("Read");
            cb_operation.Items.Add("Write");
            cb_operation.Items.Add("Update");
            cb_operation.SelectedIndex = 0;

            cb_dbms.Items.Add("PostgreSQL");
            cb_dbms.Items.Add("MySQL");
            cb_dbms.Items.Add("SQLServer");
            cb_dbms.Items.Add("MongoDB");
            cb_dbms.Items.Add("Neo4j");
            cb_dbms.Items.Add("Cassandra");
            cb_dbms.SelectedIndex = 0;

            tb_ip.Text = ConfigurationManager.AppSettings.Get("server-ip");
            tb_port.Text = ConfigurationManager.AppSettings.Get("server-port");
            int indexDBMS;
            if (Int32.TryParse(ConfigurationManager.AppSettings.Get("indexDMBS"),out indexDBMS)
                && indexDBMS < cb_dbms.Items.Count) {
                cb_dbms.SelectedIndex = indexDBMS;
            }

            if (INSTANCE == null) {
                INSTANCE = this;
            }
            
        }

        private void btn_launch_Click(object sender, EventArgs e) {
            string ip = tb_ip.Text;
            string port = tb_port.Text;
            string dbmsName = cb_dbms.Items[cb_dbms.SelectedIndex].ToString();
            tb_log.Text = "";
            if (!String.IsNullOrEmpty(ip) && !String.IsNullOrEmpty(dbmsName)) {
                TestSeries ts = new TestSeries();
                SaveInputsToConfig(ip, port);
                ts.OperationType = cb_operation.Items[cb_operation.SelectedIndex].ToString();
                ts.DBMS = EntityDBMS.CreateDBMS(dbmsName,ip,port);
                //ts.Connection_String = Db.BuildConnectionString(ts.DBMS, ip, port);
                Log("Running " + ts.OperationType + " ( " + dbmsName + ") at " + ip + ":" + port);

                ts.Execute();
            } else {
                Log("ERROR : Please mention DMBS & Server IP.");
            }
        }

        public static void Log(string s) {
            INSTANCE.tb_log.AppendText(s + "\r\n");
            //File.AppendAllText(@".\log.txt", s + '\n');
        }

        private void btn_populate_Click(object sender, EventArgs e) {
            Log("DB POPULATION");
            
            string dbmsName = cb_dbms.Items[cb_dbms.SelectedIndex].ToString();
            string ip = tb_ip.Text;
            string port = tb_port.Text;

            if (!String.IsNullOrEmpty(dbmsName) && !String.IsNullOrEmpty(ip)) {
                var dbms = EntityDBMS.CreateDBMS(dbmsName,ip, port); ;
                SaveInputsToConfig(ip, port);
                dbms.PopulateDB();
            } else {
                Log("ERROR : Please mention DMBS & Server IP.");
            }
        }

        private void SaveInputsToConfig(string ip, string port) {
            var config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            
            config.AppSettings.Settings["server-ip"].Value = ip;
            config.AppSettings.Settings["server-port"].Value = port;
            config.AppSettings.Settings["indexDMBS"].Value = cb_dbms.SelectedIndex.ToString();
            config.Save();
            ConfigurationManager.RefreshSection("appSettings");
        }

        private void btn_test_Click(object sender, EventArgs e) {
            string dbmsName = cb_dbms.Items[cb_dbms.SelectedIndex].ToString();
            string ip = tb_ip.Text;
            string port = tb_port.Text;
            if (!String.IsNullOrEmpty(dbmsName) && !String.IsNullOrEmpty(ip)) {
                var dbms = EntityDBMS.CreateDBMS(dbmsName, ip, port);
                GUI.Log("Testing connection to DB...");
                string res = dbms.TestConnection();
                GUI.Log(res);
            } else {
                Log("ERROR : Please mention DMBS & Server IP.");
            }
            
        }
    }
}

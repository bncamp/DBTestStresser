
namespace DBTestStresser {
    partial class GUI {
        /// <summary>
        /// Variable nécessaire au concepteur.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Nettoyage des ressources utilisées.
        /// </summary>
        /// <param name="disposing">true si les ressources managées doivent être supprimées ; sinon, false.</param>
        protected override void Dispose(bool disposing) {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Code généré par le Concepteur Windows Form

        /// <summary>
        /// Méthode requise pour la prise en charge du concepteur - ne modifiez pas
        /// le contenu de cette méthode avec l'éditeur de code.
        /// </summary>
        private void InitializeComponent() {
            this.tb_ip = new System.Windows.Forms.TextBox();
            this.tb_port = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label2 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.cb_dbms = new System.Windows.Forms.ComboBox();
            this.cb_operation = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.tb_log = new System.Windows.Forms.TextBox();
            this.btn_launch = new System.Windows.Forms.Button();
            this.btn_populate = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // tb_ip
            // 
            this.tb_ip.Location = new System.Drawing.Point(85, 25);
            this.tb_ip.Name = "tb_ip";
            this.tb_ip.Size = new System.Drawing.Size(214, 26);
            this.tb_ip.TabIndex = 0;
            // 
            // tb_port
            // 
            this.tb_port.Location = new System.Drawing.Point(85, 57);
            this.tb_port.Name = "tb_port";
            this.tb_port.Size = new System.Drawing.Size(214, 26);
            this.tb_port.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(18, 28);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(24, 20);
            this.label1.TabIndex = 2;
            this.label1.Text = "IP";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.tb_ip);
            this.groupBox1.Controls.Add(this.tb_port);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(316, 115);
            this.groupBox1.TabIndex = 3;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Server Infos";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(18, 60);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(38, 20);
            this.label2.TabIndex = 3;
            this.label2.Text = "Port";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.cb_dbms);
            this.groupBox2.Controls.Add(this.cb_operation);
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Controls.Add(this.label4);
            this.groupBox2.Location = new System.Drawing.Point(12, 133);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(316, 115);
            this.groupBox2.TabIndex = 4;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Test Parameters";
            // 
            // cb_dbms
            // 
            this.cb_dbms.FormattingEnabled = true;
            this.cb_dbms.Location = new System.Drawing.Point(148, 60);
            this.cb_dbms.Name = "cb_dbms";
            this.cb_dbms.Size = new System.Drawing.Size(151, 28);
            this.cb_dbms.TabIndex = 6;
            // 
            // cb_operation
            // 
            this.cb_operation.FormattingEnabled = true;
            this.cb_operation.Location = new System.Drawing.Point(148, 25);
            this.cb_operation.Name = "cb_operation";
            this.cb_operation.Size = new System.Drawing.Size(151, 28);
            this.cb_operation.TabIndex = 5;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(18, 63);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(56, 20);
            this.label3.TabIndex = 3;
            this.label3.Text = "DBMS";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(18, 28);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(113, 20);
            this.label4.TabIndex = 2;
            this.label4.Text = "Operation type";
            // 
            // tb_log
            // 
            this.tb_log.Location = new System.Drawing.Point(12, 284);
            this.tb_log.Multiline = true;
            this.tb_log.Name = "tb_log";
            this.tb_log.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.tb_log.Size = new System.Drawing.Size(426, 132);
            this.tb_log.TabIndex = 5;
            // 
            // btn_launch
            // 
            this.btn_launch.Location = new System.Drawing.Point(334, 142);
            this.btn_launch.Name = "btn_launch";
            this.btn_launch.Size = new System.Drawing.Size(104, 106);
            this.btn_launch.TabIndex = 6;
            this.btn_launch.Text = "LAUNCH";
            this.btn_launch.UseVisualStyleBackColor = true;
            this.btn_launch.Click += new System.EventHandler(this.btn_launch_Click);
            // 
            // btn_populate
            // 
            this.btn_populate.Location = new System.Drawing.Point(334, 21);
            this.btn_populate.Name = "btn_populate";
            this.btn_populate.Size = new System.Drawing.Size(104, 106);
            this.btn_populate.TabIndex = 7;
            this.btn_populate.Text = "POPULATE DB";
            this.btn_populate.UseVisualStyleBackColor = true;
            this.btn_populate.Click += new System.EventHandler(this.btn_populate_Click);
            // 
            // GUI
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(450, 428);
            this.Controls.Add(this.btn_populate);
            this.Controls.Add(this.btn_launch);
            this.Controls.Add(this.tb_log);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Name = "GUI";
            this.Text = "DBTestStresser";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox tb_ip;
        private System.Windows.Forms.TextBox tb_port;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.ComboBox cb_dbms;
        private System.Windows.Forms.ComboBox cb_operation;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox tb_log;
        private System.Windows.Forms.Button btn_launch;
        private System.Windows.Forms.Button btn_populate;
    }
}


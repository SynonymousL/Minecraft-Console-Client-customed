namespace MinecraftClientGUI
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.box_output = new System.Windows.Forms.RichTextBox();
            this.box_input = new System.Windows.Forms.TextBox();
            this.btn_send = new System.Windows.Forms.Button();
            this.box_Login = new System.Windows.Forms.TextBox();
            this.box_password = new System.Windows.Forms.TextBox();
            this.box_ip = new System.Windows.Forms.TextBox();
            this.btn_connect = new System.Windows.Forms.Button();
            this.sel_Login = new System.Windows.Forms.ComboBox();
            this.radio_1 = new System.Windows.Forms.RadioButton();
            this.radio_2 = new System.Windows.Forms.RadioButton();
            this.label1 = new System.Windows.Forms.Label();
            this.sel_password = new System.Windows.Forms.TextBox();
            this.groupBox_Login = new System.Windows.Forms.GroupBox();
            this.sel_ip = new System.Windows.Forms.ComboBox();
            this.groupBox_Login.SuspendLayout();
            this.SuspendLayout();
            // 
            // box_output
            // 
            this.box_output.Location = new System.Drawing.Point(13, 114);
            this.box_output.Name = "box_output";
            this.box_output.ReadOnly = true;
            this.box_output.Size = new System.Drawing.Size(520, 222);
            this.box_output.TabIndex = 1;
            this.box_output.Text = "";
            this.box_output.LinkClicked += new System.Windows.Forms.LinkClickedEventHandler(this.LinkClicked);
            // 
            // box_input
            // 
            this.box_input.AcceptsTab = true;
            this.box_input.Font = new System.Drawing.Font("MS UI Gothic", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.box_input.Location = new System.Drawing.Point(13, 341);
            this.box_input.MaxLength = 100;
            this.box_input.Multiline = true;
            this.box_input.Name = "box_input";
            this.box_input.Size = new System.Drawing.Size(474, 20);
            this.box_input.TabIndex = 2;
            this.box_input.KeyDown += new System.Windows.Forms.KeyEventHandler(this.inputBox_KeyDown);
            // 
            // btn_send
            // 
            this.btn_send.Location = new System.Drawing.Point(493, 341);
            this.btn_send.Name = "btn_send";
            this.btn_send.Size = new System.Drawing.Size(40, 20);
            this.btn_send.TabIndex = 3;
            this.btn_send.Text = "送信";
            this.btn_send.UseVisualStyleBackColor = true;
            this.btn_send.Click += new System.EventHandler(this.btn_send_Click);
            // 
            // box_Login
            // 
            this.box_Login.Location = new System.Drawing.Point(135, 18);
            this.box_Login.Name = "box_Login";
            this.box_Login.Size = new System.Drawing.Size(130, 19);
            this.box_Login.TabIndex = 1;
            this.box_Login.KeyUp += new System.Windows.Forms.KeyEventHandler(this.loginBox_KeyUp);
            // 
            // box_password
            // 
            this.box_password.Location = new System.Drawing.Point(271, 18);
            this.box_password.Name = "box_password";
            this.box_password.PasswordChar = '•';
            this.box_password.Size = new System.Drawing.Size(154, 19);
            this.box_password.TabIndex = 3;
            this.box_password.KeyUp += new System.Windows.Forms.KeyEventHandler(this.loginBox_KeyUp);
            // 
            // box_ip
            // 
            this.box_ip.Location = new System.Drawing.Point(135, 68);
            this.box_ip.Name = "box_ip";
            this.box_ip.Size = new System.Drawing.Size(290, 19);
            this.box_ip.TabIndex = 5;
            this.box_ip.KeyUp += new System.Windows.Forms.KeyEventHandler(this.loginBox_KeyUp);
            // 
            // btn_connect
            // 
            this.btn_connect.Font = new System.Drawing.Font("MS UI Gothic", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.btn_connect.Location = new System.Drawing.Point(431, 18);
            this.btn_connect.Name = "btn_connect";
            this.btn_connect.Size = new System.Drawing.Size(81, 69);
            this.btn_connect.TabIndex = 6;
            this.btn_connect.Text = "接続";
            this.btn_connect.UseVisualStyleBackColor = true;
            this.btn_connect.Click += new System.EventHandler(this.btn_connect_Click);
            // 
            // sel_Login
            // 
            this.sel_Login.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.sel_Login.FormattingEnabled = true;
            this.sel_Login.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.sel_Login.Location = new System.Drawing.Point(135, 43);
            this.sel_Login.Name = "sel_Login";
            this.sel_Login.Size = new System.Drawing.Size(130, 20);
            this.sel_Login.TabIndex = 7;
            this.sel_Login.SelectedValueChanged += new System.EventHandler(this.sel_Login_SelectedValueChanged);
            // 
            // radio_1
            // 
            this.radio_1.AutoSize = true;
            this.radio_1.Checked = true;
            this.radio_1.Location = new System.Drawing.Point(15, 19);
            this.radio_1.Name = "radio_1";
            this.radio_1.Size = new System.Drawing.Size(90, 16);
            this.radio_1.TabIndex = 8;
            this.radio_1.TabStop = true;
            this.radio_1.Text = "直接入力する";
            this.radio_1.UseVisualStyleBackColor = true;
            this.radio_1.CheckedChanged += new System.EventHandler(this.radio_1_CheckedChanged);
            // 
            // radio_2
            // 
            this.radio_2.AutoSize = true;
            this.radio_2.Location = new System.Drawing.Point(15, 44);
            this.radio_2.Name = "radio_2";
            this.radio_2.Size = new System.Drawing.Size(114, 16);
            this.radio_2.TabIndex = 9;
            this.radio_2.Text = "登録済みアカウント";
            this.radio_2.UseVisualStyleBackColor = true;
            this.radio_2.CheckedChanged += new System.EventHandler(this.radio_2_CheckedChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 71);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(81, 12);
            this.label1.TabIndex = 10;
            this.label1.Text = "サーバーアドレス";
            // 
            // sel_password
            // 
            this.sel_password.BackColor = System.Drawing.Color.Silver;
            this.sel_password.Enabled = false;
            this.sel_password.ForeColor = System.Drawing.Color.Gray;
            this.sel_password.Location = new System.Drawing.Point(271, 43);
            this.sel_password.Name = "sel_password";
            this.sel_password.Size = new System.Drawing.Size(154, 19);
            this.sel_password.TabIndex = 11;
            this.sel_password.Text = "パスワードは自動入力されます";
            // 
            // groupBox_Login
            // 
            this.groupBox_Login.BackColor = System.Drawing.Color.Transparent;
            this.groupBox_Login.Controls.Add(this.sel_ip);
            this.groupBox_Login.Controls.Add(this.sel_password);
            this.groupBox_Login.Controls.Add(this.label1);
            this.groupBox_Login.Controls.Add(this.radio_2);
            this.groupBox_Login.Controls.Add(this.radio_1);
            this.groupBox_Login.Controls.Add(this.btn_connect);
            this.groupBox_Login.Controls.Add(this.box_ip);
            this.groupBox_Login.Controls.Add(this.sel_Login);
            this.groupBox_Login.Controls.Add(this.box_password);
            this.groupBox_Login.Controls.Add(this.box_Login);
            this.groupBox_Login.Location = new System.Drawing.Point(13, 10);
            this.groupBox_Login.Name = "groupBox_Login";
            this.groupBox_Login.Size = new System.Drawing.Size(518, 98);
            this.groupBox_Login.TabIndex = 0;
            this.groupBox_Login.TabStop = false;
            this.groupBox_Login.Text = "接続情報   ";
            // 
            // sel_ip
            // 
            this.sel_ip.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.sel_ip.FormattingEnabled = true;
            this.sel_ip.Location = new System.Drawing.Point(135, 68);
            this.sel_ip.Name = "sel_ip";
            this.sel_ip.Size = new System.Drawing.Size(290, 20);
            this.sel_ip.TabIndex = 12;
            this.sel_ip.Visible = false;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.ClientSize = new System.Drawing.Size(546, 369);
            this.Controls.Add(this.btn_send);
            this.Controls.Add(this.box_input);
            this.Controls.Add(this.box_output);
            this.Controls.Add(this.groupBox_Login);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Minecraft Console Client GUI 改造版";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.onClose);
            this.Load += new System.EventHandler(this.Form1_Load);
            this.groupBox_Login.ResumeLayout(false);
            this.groupBox_Login.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.RichTextBox box_output;
        private System.Windows.Forms.TextBox box_input;
        private System.Windows.Forms.Button btn_send;
        private System.Windows.Forms.TextBox box_Login;
        private System.Windows.Forms.TextBox box_password;
        private System.Windows.Forms.TextBox box_ip;
        private System.Windows.Forms.Button btn_connect;
        private System.Windows.Forms.ComboBox sel_Login;
        private System.Windows.Forms.RadioButton radio_1;
        private System.Windows.Forms.RadioButton radio_2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox sel_password;
        private System.Windows.Forms.GroupBox groupBox_Login;
        private System.Windows.Forms.ComboBox sel_ip;
    }
}


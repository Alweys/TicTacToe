
namespace TicTacToe_Client
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
            this.IP = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.port = new System.Windows.Forms.TextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.listBox1 = new System.Windows.Forms.ListBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.box8 = new System.Windows.Forms.Button();
            this.box7 = new System.Windows.Forms.Button();
            this.box6 = new System.Windows.Forms.Button();
            this.box5 = new System.Windows.Forms.Button();
            this.box4 = new System.Windows.Forms.Button();
            this.box3 = new System.Windows.Forms.Button();
            this.box2 = new System.Windows.Forms.Button();
            this.box1 = new System.Windows.Forms.Button();
            this.box0 = new System.Windows.Forms.Button();
            this.readybutton = new System.Windows.Forms.Button();
            this.readyPlayer2 = new System.Windows.Forms.Label();
            this.readyPlayer1 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.label7 = new System.Windows.Forms.Label();
            this.username = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.button4 = new System.Windows.Forms.Button();
            this.label9 = new System.Windows.Forms.Label();
            this.timeLeftLabel = new System.Windows.Forms.Label();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // IP
            // 
            this.IP.Location = new System.Drawing.Point(382, 418);
            this.IP.Name = "IP";
            this.IP.Size = new System.Drawing.Size(100, 20);
            this.IP.TabIndex = 0;
            this.IP.Text = "192.168.1.11";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(356, 421);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(20, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "IP:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(488, 421);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(29, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Port:";
            // 
            // port
            // 
            this.port.Location = new System.Drawing.Point(523, 418);
            this.port.Name = "port";
            this.port.Size = new System.Drawing.Size(100, 20);
            this.port.TabIndex = 3;
            this.port.Text = "50000";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(632, 416);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 4;
            this.button1.Text = "Connect";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(464, 373);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(159, 35);
            this.button2.TabIndex = 5;
            this.button2.Text = "Test connection";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(713, 415);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(75, 23);
            this.button3.TabIndex = 6;
            this.button3.Text = "Disconnect";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(204, 421);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(67, 13);
            this.label3.TabIndex = 7;
            this.label3.Text = "Client status:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(277, 421);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(73, 13);
            this.label4.TabIndex = 8;
            this.label4.Text = "Disconnected";
            // 
            // listBox1
            // 
            this.listBox1.FormattingEnabled = true;
            this.listBox1.Location = new System.Drawing.Point(599, 34);
            this.listBox1.Name = "listBox1";
            this.listBox1.Size = new System.Drawing.Size(189, 199);
            this.listBox1.TabIndex = 9;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.box8);
            this.panel1.Controls.Add(this.box7);
            this.panel1.Controls.Add(this.box6);
            this.panel1.Controls.Add(this.box5);
            this.panel1.Controls.Add(this.box4);
            this.panel1.Controls.Add(this.box3);
            this.panel1.Controls.Add(this.box2);
            this.panel1.Controls.Add(this.box1);
            this.panel1.Controls.Add(this.box0);
            this.panel1.Location = new System.Drawing.Point(12, 34);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(317, 320);
            this.panel1.TabIndex = 10;
            // 
            // box8
            // 
            this.box8.Location = new System.Drawing.Point(215, 215);
            this.box8.Name = "box8";
            this.box8.Size = new System.Drawing.Size(100, 100);
            this.box8.TabIndex = 8;
            this.box8.Tag = "none";
            this.box8.UseVisualStyleBackColor = true;
            this.box8.Click += new System.EventHandler(this.box8_Click);
            // 
            // box7
            // 
            this.box7.Location = new System.Drawing.Point(109, 215);
            this.box7.Name = "box7";
            this.box7.Size = new System.Drawing.Size(100, 102);
            this.box7.TabIndex = 7;
            this.box7.Tag = "none";
            this.box7.UseVisualStyleBackColor = true;
            this.box7.Click += new System.EventHandler(this.box7_Click);
            // 
            // box6
            // 
            this.box6.Location = new System.Drawing.Point(3, 215);
            this.box6.Name = "box6";
            this.box6.Size = new System.Drawing.Size(100, 100);
            this.box6.TabIndex = 6;
            this.box6.Tag = "none";
            this.box6.UseVisualStyleBackColor = true;
            this.box6.Click += new System.EventHandler(this.box6_Click);
            // 
            // box5
            // 
            this.box5.Location = new System.Drawing.Point(215, 109);
            this.box5.Name = "box5";
            this.box5.Size = new System.Drawing.Size(100, 100);
            this.box5.TabIndex = 5;
            this.box5.Tag = "none";
            this.box5.UseVisualStyleBackColor = true;
            this.box5.Click += new System.EventHandler(this.box5_Click);
            // 
            // box4
            // 
            this.box4.Location = new System.Drawing.Point(109, 109);
            this.box4.Name = "box4";
            this.box4.Size = new System.Drawing.Size(100, 100);
            this.box4.TabIndex = 4;
            this.box4.Tag = "none";
            this.box4.UseVisualStyleBackColor = true;
            this.box4.Click += new System.EventHandler(this.box4_Click);
            // 
            // box3
            // 
            this.box3.Location = new System.Drawing.Point(3, 109);
            this.box3.Name = "box3";
            this.box3.Size = new System.Drawing.Size(100, 100);
            this.box3.TabIndex = 3;
            this.box3.Tag = "none";
            this.box3.UseVisualStyleBackColor = true;
            this.box3.Click += new System.EventHandler(this.box3_Click);
            // 
            // box2
            // 
            this.box2.Location = new System.Drawing.Point(215, 3);
            this.box2.Name = "box2";
            this.box2.Size = new System.Drawing.Size(100, 100);
            this.box2.TabIndex = 2;
            this.box2.Tag = "none";
            this.box2.UseVisualStyleBackColor = true;
            this.box2.Click += new System.EventHandler(this.box2_Click);
            // 
            // box1
            // 
            this.box1.Location = new System.Drawing.Point(109, 3);
            this.box1.Name = "box1";
            this.box1.Size = new System.Drawing.Size(100, 100);
            this.box1.TabIndex = 1;
            this.box1.Tag = "none";
            this.box1.UseVisualStyleBackColor = true;
            this.box1.Click += new System.EventHandler(this.box1_Click);
            // 
            // box0
            // 
            this.box0.Location = new System.Drawing.Point(3, 3);
            this.box0.Name = "box0";
            this.box0.Size = new System.Drawing.Size(100, 100);
            this.box0.TabIndex = 0;
            this.box0.Tag = "none";
            this.box0.UseVisualStyleBackColor = true;
            this.box0.Click += new System.EventHandler(this.box0_Click);
            // 
            // readybutton
            // 
            this.readybutton.Location = new System.Drawing.Point(632, 373);
            this.readybutton.Name = "readybutton";
            this.readybutton.Size = new System.Drawing.Size(156, 36);
            this.readybutton.TabIndex = 11;
            this.readybutton.Text = "Ready";
            this.readybutton.UseVisualStyleBackColor = true;
            this.readybutton.Click += new System.EventHandler(this.button4_Click);
            // 
            // readyPlayer2
            // 
            this.readyPlayer2.AutoSize = true;
            this.readyPlayer2.Location = new System.Drawing.Point(729, 9);
            this.readyPlayer2.Name = "readyPlayer2";
            this.readyPlayer2.Size = new System.Drawing.Size(58, 13);
            this.readyPlayer2.TabIndex = 24;
            this.readyPlayer2.Text = "Not Ready";
            // 
            // readyPlayer1
            // 
            this.readyPlayer1.AutoSize = true;
            this.readyPlayer1.Location = new System.Drawing.Point(608, 9);
            this.readyPlayer1.Name = "readyPlayer1";
            this.readyPlayer1.Size = new System.Drawing.Size(58, 13);
            this.readyPlayer1.TabIndex = 23;
            this.readyPlayer1.Text = "Not Ready";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(675, 9);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(48, 13);
            this.label12.TabIndex = 22;
            this.label12.Text = "Player 2:";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(557, 9);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(48, 13);
            this.label11.TabIndex = 21;
            this.label11.Text = "Player 1:";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(12, 18);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(32, 13);
            this.label5.TabIndex = 25;
            this.label5.Text = "Turn:";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(50, 18);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(46, 13);
            this.label6.TabIndex = 26;
            this.label6.Text = "Player X";
            // 
            // pictureBox1
            // 
            this.pictureBox1.Location = new System.Drawing.Point(645, 237);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(130, 130);
            this.pictureBox1.TabIndex = 27;
            this.pictureBox1.TabStop = false;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(12, 421);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(58, 13);
            this.label7.TabIndex = 28;
            this.label7.Text = "Username:";
            // 
            // username
            // 
            this.username.Location = new System.Drawing.Point(71, 419);
            this.username.Name = "username";
            this.username.Size = new System.Drawing.Size(127, 20);
            this.username.TabIndex = 29;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(12, 385);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(80, 13);
            this.label8.TabIndex = 30;
            this.label8.Text = "Send message:";
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(102, 382);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(146, 20);
            this.textBox1.TabIndex = 31;
            this.textBox1.KeyDown += new System.Windows.Forms.KeyEventHandler(this.textBox1_KeyDown);
            // 
            // button4
            // 
            this.button4.Location = new System.Drawing.Point(254, 380);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(75, 23);
            this.button4.TabIndex = 32;
            this.button4.Text = "Send";
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Click += new System.EventHandler(this.button4_Click_1);
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(277, 18);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(33, 13);
            this.label9.TabIndex = 33;
            this.label9.Text = "Time:";
            // 
            // timeLeftLabel
            // 
            this.timeLeftLabel.AutoSize = true;
            this.timeLeftLabel.Location = new System.Drawing.Point(312, 18);
            this.timeLeftLabel.Name = "timeLeftLabel";
            this.timeLeftLabel.Size = new System.Drawing.Size(0, 13);
            this.timeLeftLabel.TabIndex = 34;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.timeLeftLabel);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.button4);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.username);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.readyPlayer2);
            this.Controls.Add(this.readyPlayer1);
            this.Controls.Add(this.label12);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.readybutton);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.listBox1);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.port);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.IP);
            this.Name = "Form1";
            this.Text = "Form1";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.Form1_FormClosed);
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox IP;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox port;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ListBox listBox1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button box8;
        private System.Windows.Forms.Button box7;
        private System.Windows.Forms.Button box6;
        private System.Windows.Forms.Button box5;
        private System.Windows.Forms.Button box4;
        private System.Windows.Forms.Button box3;
        private System.Windows.Forms.Button box2;
        private System.Windows.Forms.Button box1;
        private System.Windows.Forms.Button box0;
        private System.Windows.Forms.Button readybutton;
        private System.Windows.Forms.Label readyPlayer2;
        private System.Windows.Forms.Label readyPlayer1;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox username;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label timeLeftLabel;
    }
}


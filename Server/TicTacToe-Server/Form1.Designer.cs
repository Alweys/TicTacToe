
namespace TicTacToe_Server
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
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.listBox1 = new System.Windows.Forms.ListBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.clientListPanel = new System.Windows.Forms.Panel();
            this.button3 = new System.Windows.Forms.Button();
            this.playerturnlabel = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.readyPlayer2 = new System.Windows.Forms.Label();
            this.readyPlayer1 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.player2status = new System.Windows.Forms.Label();
            this.player1status = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.button4 = new System.Windows.Forms.Button();
            this.gameListPanel = new System.Windows.Forms.Panel();
            this.label6 = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.label7 = new System.Windows.Forms.Label();
            this.gameSelection = new System.Windows.Forms.ComboBox();
            this.gamePreview = new System.Windows.Forms.Label();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(12, 12);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 0;
            this.button1.Text = "Start Server";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(12, 41);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 1;
            this.button2.Text = "Stop Server";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(9, 428);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(72, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Server status:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(87, 428);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(37, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "Offline";
            // 
            // listBox1
            // 
            this.listBox1.FormattingEnabled = true;
            this.listBox1.Location = new System.Drawing.Point(505, 12);
            this.listBox1.Name = "listBox1";
            this.listBox1.Size = new System.Drawing.Size(283, 251);
            this.listBox1.TabIndex = 4;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(233, 428);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(95, 13);
            this.label3.TabIndex = 5;
            this.label3.Text = "Connected clients:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(334, 428);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(13, 13);
            this.label4.TabIndex = 6;
            this.label4.Text = "0";
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.clientListPanel);
            this.panel1.Controls.Add(this.button3);
            this.panel1.Controls.Add(this.playerturnlabel);
            this.panel1.Controls.Add(this.label10);
            this.panel1.Controls.Add(this.readyPlayer2);
            this.panel1.Controls.Add(this.readyPlayer1);
            this.panel1.Controls.Add(this.label9);
            this.panel1.Controls.Add(this.player2status);
            this.panel1.Controls.Add(this.player1status);
            this.panel1.Controls.Add(this.label8);
            this.panel1.Controls.Add(this.label5);
            this.panel1.Location = new System.Drawing.Point(236, 12);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(263, 413);
            this.panel1.TabIndex = 7;
            // 
            // clientListPanel
            // 
            this.clientListPanel.Location = new System.Drawing.Point(6, 21);
            this.clientListPanel.Name = "clientListPanel";
            this.clientListPanel.Size = new System.Drawing.Size(252, 360);
            this.clientListPanel.TabIndex = 9;
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(183, 387);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(75, 23);
            this.button3.TabIndex = 11;
            this.button3.Text = "Stop Games";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // playerturnlabel
            // 
            this.playerturnlabel.AutoSize = true;
            this.playerturnlabel.Location = new System.Drawing.Point(77, 392);
            this.playerturnlabel.Name = "playerturnlabel";
            this.playerturnlabel.Size = new System.Drawing.Size(0, 13);
            this.playerturnlabel.TabIndex = 10;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(7, 392);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(64, 13);
            this.label10.TabIndex = 9;
            this.label10.Text = "Player Turn:";
            // 
            // readyPlayer2
            // 
            this.readyPlayer2.AutoSize = true;
            this.readyPlayer2.Location = new System.Drawing.Point(180, 312);
            this.readyPlayer2.Name = "readyPlayer2";
            this.readyPlayer2.Size = new System.Drawing.Size(21, 13);
            this.readyPlayer2.TabIndex = 8;
            this.readyPlayer2.Text = "No";
            // 
            // readyPlayer1
            // 
            this.readyPlayer1.AutoSize = true;
            this.readyPlayer1.Location = new System.Drawing.Point(180, 285);
            this.readyPlayer1.Name = "readyPlayer1";
            this.readyPlayer1.Size = new System.Drawing.Size(21, 13);
            this.readyPlayer1.TabIndex = 7;
            this.readyPlayer1.Text = "No";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(180, 261);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(41, 13);
            this.label9.TabIndex = 6;
            this.label9.Text = "Ready:";
            // 
            // player2status
            // 
            this.player2status.AutoSize = true;
            this.player2status.Location = new System.Drawing.Point(76, 312);
            this.player2status.Name = "player2status";
            this.player2status.Size = new System.Drawing.Size(78, 13);
            this.player2status.TabIndex = 5;
            this.player2status.Text = "Not connected";
            // 
            // player1status
            // 
            this.player1status.AutoSize = true;
            this.player1status.Location = new System.Drawing.Point(76, 285);
            this.player1status.Name = "player1status";
            this.player1status.Size = new System.Drawing.Size(78, 13);
            this.player1status.TabIndex = 4;
            this.player1status.Text = "Not connected";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(76, 261);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(40, 13);
            this.label8.TabIndex = 3;
            this.label8.Text = "Status:";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(3, 5);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(44, 13);
            this.label5.TabIndex = 0;
            this.label5.Text = "Players:";
            // 
            // button4
            // 
            this.button4.Location = new System.Drawing.Point(12, 394);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(75, 23);
            this.button4.TabIndex = 8;
            this.button4.Text = "button4";
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Click += new System.EventHandler(this.button4_Click);
            // 
            // gameListPanel
            // 
            this.gameListPanel.Location = new System.Drawing.Point(505, 285);
            this.gameListPanel.Name = "gameListPanel";
            this.gameListPanel.Size = new System.Drawing.Size(283, 153);
            this.gameListPanel.TabIndex = 9;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(505, 266);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(43, 13);
            this.label6.TabIndex = 10;
            this.label6.Text = "Games:";
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.gamePreview);
            this.panel2.Controls.Add(this.gameSelection);
            this.panel2.Location = new System.Drawing.Point(12, 106);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(216, 282);
            this.panel2.TabIndex = 12;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(12, 90);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(77, 13);
            this.label7.TabIndex = 0;
            this.label7.Text = "Preview game:";
            // 
            // gameSelection
            // 
            this.gameSelection.FormattingEnabled = true;
            this.gameSelection.Location = new System.Drawing.Point(3, 3);
            this.gameSelection.Name = "gameSelection";
            this.gameSelection.Size = new System.Drawing.Size(121, 21);
            this.gameSelection.TabIndex = 13;
            this.gameSelection.SelectedIndexChanged += new System.EventHandler(this.gameSelection_SelectedIndexChanged);
            // 
            // gamePreview
            // 
            this.gamePreview.AutoSize = true;
            this.gamePreview.Location = new System.Drawing.Point(3, 45);
            this.gamePreview.Name = "gamePreview";
            this.gamePreview.Size = new System.Drawing.Size(88, 13);
            this.gamePreview.TabIndex = 14;
            this.gamePreview.Text = "Game not started";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.gameListPanel);
            this.Controls.Add(this.button4);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.listBox1);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Name = "Form1";
            this.Text = "Form1";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.Form1_FormClosed);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ListBox listBox1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label playerturnlabel;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.Label readyPlayer2;
        private System.Windows.Forms.Label readyPlayer1;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label player2status;
        private System.Windows.Forms.Label player1status;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Panel clientListPanel;
        private System.Windows.Forms.Panel gameListPanel;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.ComboBox gameSelection;
        private System.Windows.Forms.Label gamePreview;
    }
}


using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;
using System.IO;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;

namespace TicTacToe_Client
{
    public partial class Form1 : Form
    {
        TcpClient client = null;
        Thread client_thread;
        Thread game_thread;
        bool client_connected = false;
        bool clientReady = false;
        bool opponentReady = false;
        bool game_started = false;
        int clientID = -1;
        bool myTurn = false;
        string path = @"D:\3.RT\Programiranje\TicTacToe\images\";
        int zeroCount = 0;
        int crossCount = 0;
        float turnTime = 2;
        float timerCounter = 0; 
        bool startTurnTime = true;
        static Form f0;
        System.Timers.Timer myTimer;

        void TimerEventProcessor(Object myObject, EventArgs myEventArgs)
        {
            timerCounter--;
            //Console.WriteLine(timerCounter);
            if (timerCounter > 0)
                timeLeftLabel.Invoke((MethodInvoker)(() => timeLeftLabel.Text = "" + timerCounter));
            if (timerCounter == 0)
            {
                myTimer.Stop();
                myTimer.Enabled = false;
                timeLeftLabel.Invoke((MethodInvoker)(() => timeLeftLabel.Text = ""));
                startTurnTime = true;
                myTurn = false;
                try
                {
                    string textToSend = "nextTurn=" + clientID;
                    NetworkStream nwStream = client.GetStream();
                    byte[] bytesToSend = ASCIIEncoding.ASCII.GetBytes(textToSend);
                    nwStream.Write(bytesToSend, 0, bytesToSend.Length);
                }
                catch { }
            }
            if(timerCounter < 0)
                timeLeftLabel.Invoke((MethodInvoker)(() => timeLeftLabel.Text = ""));
        }

        public Form1()
        {
            InitializeComponent();
            button2.Visible = false;
            f0 = this;
            this.Text = "TicTacToe Client";
            myTimer = new System.Timers.Timer();
            //myTimer.Interval = 1000;
            //myTimer.Elapsed += new System.Timers.ElapsedEventHandler(TimerEventProcessor);
            //game_thread = new Thread(GameThread);
            //game_thread.Start();
            
        }

        void GameThread(/*System.Timers.Timer timer*/)
        {
            while (true)
            {
                if (myTurn && startTurnTime)
                {
                    myTimer.Stop();
                    myTimer.Enabled = false;
                    Thread.Sleep(50);
                    myTimer.Start();
                    myTimer.Enabled = true;
                    timerCounter = turnTime+1;
                    startTurnTime = false;
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if(!client_connected)
            {
                try
                {
                    client = new TcpClient();
                    client.Connect(IP.Text.ToString(), int.Parse(port.Text));
                    client_connected = true;
                    client_thread = new Thread(Client);
                    client_thread.IsBackground = true;
                    client_thread.Start();
                }
                catch
                {
                    string caption = "Error";
                    string message = "Failed connecting to the server";
                    MessageBoxButtons buttons = MessageBoxButtons.OK;
                    DialogResult result;
                    result = MessageBox.Show(message, caption, buttons);

                }
            }
        }

        int player;

        void Client()
        {
            try
            { 
                while (client_connected)
                {
                    String data = null;
                    NetworkStream nwStream = client.GetStream();
                    byte[] bytesToRead = new byte[client.ReceiveBufferSize];
                    int bytesRead = nwStream.Read(bytesToRead, 0, client.ReceiveBufferSize);
                    data = Encoding.ASCII.GetString(bytesToRead, 0, bytesRead);
                    if (data.ToString().StartsWith("id="))
                    {
                        string newData = data.Replace("id=", "");
                        clientID = int.Parse(newData);
                        label4.Invoke((MethodInvoker)(() => label4.Text = "Connected!"));
                        listBox1.Invoke((MethodInvoker)(() => listBox1.Items.Add("Your id is: " + clientID)));
                        listBox1.Invoke((MethodInvoker)(() => listBox1.SelectedIndex = listBox1.Items.Count - 1));
                        if (username.Text.Length == 0)
                            username.Invoke((MethodInvoker)(() => username.Text = "CID" + clientID));
                        string textToSend = "username="/* + clientID + "="*/ + username.Text;
                        byte[] bytesToSend = ASCIIEncoding.ASCII.GetBytes(textToSend);
                        nwStream.Write(bytesToSend, 0, bytesToSend.Length);
                        player = (clientID % 2 + 1);
                        if (player == 1)
                            player = 2;
                        else
                            player = 1;
                        f0.Invoke((MethodInvoker)(() => f0.Text = "TicTacToe Client - Player " + player));
                        if(player == 1)
                        {
                            pictureBox1.Image = Image.FromFile(path + @"zero.png");
                            pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
                        }
                        else if (player == 2)
                        {
                            pictureBox1.Image = Image.FromFile(path + @"cross.png");
                            pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
                        }
                    }
                    if (data.ToString().Contains("full"))
                    {
                        listBox1.Invoke((MethodInvoker)(() => listBox1.Items.Add("Server is full!")));
                        listBox1.Invoke((MethodInvoker)(() => listBox1.SelectedIndex = listBox1.Items.Count - 1));
                        client_connected = false;
                        client.Close();
                        client_thread.Join();
                    }
                    if(data.ToString().StartsWith("opponentReady="))
                    {
                        string newData = data.Replace("opponentReady=", "");
                        int opponentID = int.Parse(newData);
                        if ((int)(opponentID % 2 + 1) == 1)
                            readyPlayer1.Invoke((MethodInvoker)(() => readyPlayer1.Text = "Ready"));
                        else if ((int)(opponentID % 2 + 1) == 2)
                            readyPlayer2.Invoke((MethodInvoker)(() => readyPlayer2.Text = "Ready"));
                        opponentReady = true;
                        //listBox1.Invoke((MethodInvoker)(() => listBox1.Items.Add((int)(opponentID % 2 + 1))));
                        listBox1.Invoke((MethodInvoker)(() => listBox1.Items.Add("Opponent ready!")));
                        listBox1.Invoke((MethodInvoker)(() => listBox1.SelectedIndex = listBox1.Items.Count - 1));
                    }
                    if (data.ToString().StartsWith("turn="))
                    {
                        string newData = data.Replace("turn=", "");
                        int turnID = int.Parse(newData);
                        if (turnID == player)
                        {
                            myTurn = true;
                            foreach(Button btn in panel1.Controls)
                            {
                                btn.Enabled = true;
                            }
                            listBox1.Invoke((MethodInvoker)(() => listBox1.Items.Add("Your turn!")));
                            listBox1.Invoke((MethodInvoker)(() => listBox1.SelectedIndex = listBox1.Items.Count - 1));
                        }
                        if(myTurn)
                            label6.Invoke((MethodInvoker)(() => label6.Text = "You"));
                        else
                            label6.Invoke((MethodInvoker)(() => label6.Text = "Opponent"));
                    }
                    if (data.ToString().StartsWith("update="))
                    {
                        string newData = data.Replace("update=", "");
                        string id = newData.Substring(0, 1);
                        newData = newData.Remove(0, 1);
                        string status = newData;

                        if (id.Equals("0"))
                        {
                            if (!status.Equals("none"))
                                box0.Invoke((MethodInvoker)(() => box0.BackgroundImage = Image.FromFile(path + status + @".png")));
                            else
                                box0.Invoke((MethodInvoker)(() => box0.BackgroundImage = null));
                            box0.Invoke((MethodInvoker)(() => box0.BackgroundImageLayout = ImageLayout.Zoom));
                            box0.Invoke((MethodInvoker)(() => box0.Tag = status));
                        }
                        else if (id.Equals("1"))
                        {
                            if (!status.Equals("none"))
                                box1.Invoke((MethodInvoker)(() => box1.BackgroundImage = Image.FromFile(path + status + @".png")));
                            else
                                box1.Invoke((MethodInvoker)(() => box1.BackgroundImage = null));
                            box1.Invoke((MethodInvoker)(() => box1.BackgroundImageLayout = ImageLayout.Zoom));
                            box1.Invoke((MethodInvoker)(() => box1.Tag = status));
                        }
                        else if (id.Equals("2"))
                        {
                            if (!status.Equals("none"))
                                box2.Invoke((MethodInvoker)(() => box2.BackgroundImage = Image.FromFile(path + status + @".png")));
                            else
                                box2.Invoke((MethodInvoker)(() => box2.BackgroundImage = null));
                            box2.Invoke((MethodInvoker)(() => box2.BackgroundImageLayout = ImageLayout.Zoom));
                            box2.Invoke((MethodInvoker)(() => box2.Tag = status));
                        }
                        else if (id.Equals("3"))
                        {
                            if (!status.Equals("none"))
                                box3.Invoke((MethodInvoker)(() => box3.BackgroundImage = Image.FromFile(path + status + @".png")));
                            else
                                box3.Invoke((MethodInvoker)(() => box3.BackgroundImage = null));
                            box3.Invoke((MethodInvoker)(() => box3.BackgroundImageLayout = ImageLayout.Zoom));
                            box3.Invoke((MethodInvoker)(() => box3.Tag = status));
                        }
                        else if (id.Equals("4"))
                        {
                            if (!status.Equals("none"))
                                box4.Invoke((MethodInvoker)(() => box4.BackgroundImage = Image.FromFile(path + status + @".png")));
                            else
                                box4.Invoke((MethodInvoker)(() => box4.BackgroundImage = null));
                            box4.Invoke((MethodInvoker)(() => box4.BackgroundImageLayout = ImageLayout.Zoom));
                            box4.Invoke((MethodInvoker)(() => box4.Tag = status));
                        }
                        else if (id.Equals("5"))
                        {
                            if (!status.Equals("none"))
                                box5.Invoke((MethodInvoker)(() => box5.BackgroundImage = Image.FromFile(path + status + @".png")));
                            else
                                box5.Invoke((MethodInvoker)(() => box5.BackgroundImage = null));
                            box5.Invoke((MethodInvoker)(() => box5.BackgroundImageLayout = ImageLayout.Zoom));
                            box5.Invoke((MethodInvoker)(() => box5.Tag = status));
                        }
                        else if (id.Equals("6"))
                        {
                            if (!status.Equals("none"))
                                box6.Invoke((MethodInvoker)(() => box6.BackgroundImage = Image.FromFile(path + status + @".png")));
                            else
                                box6.Invoke((MethodInvoker)(() => box6.BackgroundImage = null));
                            box6.Invoke((MethodInvoker)(() => box6.BackgroundImageLayout = ImageLayout.Zoom));
                            box6.Invoke((MethodInvoker)(() => box6.Tag = status));
                        }
                        else if (id.Equals("7"))
                        {
                            if (!status.Equals("none"))
                                box7.Invoke((MethodInvoker)(() => box7.BackgroundImage = Image.FromFile(path + status + @".png")));
                            else
                                box7.Invoke((MethodInvoker)(() => box7.BackgroundImage = null));
                            box7.Invoke((MethodInvoker)(() => box7.BackgroundImageLayout = ImageLayout.Zoom));
                            box7.Invoke((MethodInvoker)(() => box7.Tag = status));
                        }
                        else if (id.Equals("8"))
                        {
                            if (!status.Equals("none"))
                                box8.Invoke((MethodInvoker)(() => box8.BackgroundImage = Image.FromFile(path + status + @".png")));
                            else
                                box8.Invoke((MethodInvoker)(() => box8.BackgroundImage = null));
                            box8.Invoke((MethodInvoker)(() => box8.BackgroundImageLayout = ImageLayout.Zoom));
                            box8.Invoke((MethodInvoker)(() => box8.Tag = status));
                        }
                        myTurn = true;
                    }
                    if(data.ToString().Equals("reset"))
                    {
                        readyPlayer1.Invoke((MethodInvoker)(() => readyPlayer1.Text = "Not Ready"));
                        readyPlayer2.Invoke((MethodInvoker)(() => readyPlayer2.Text = "Not Ready"));
                        foreach (Button btn in panel1.Controls)
                        {
                            btn.Invoke((MethodInvoker)(() => btn.BackgroundImage = null));
                            btn.Invoke((MethodInvoker)(() => btn.Tag = "none"));
                            btn.Invoke((MethodInvoker)(() => btn.Enabled = true));
                        }
                        clientReady = false;
                        opponentReady = false;
                        zeroCount = 0;
                        crossCount = 0;
                        myTurn = false;
                        listBox1.Invoke((MethodInvoker)(() => listBox1.Items.Add("Game reset!")));
                        readybutton.Invoke((MethodInvoker)(() => readybutton.Enabled = true));
                    }
                    if (data.ToString().StartsWith("winner="))
                    {
                        foreach (Button btn in panel1.Controls)
                        {
                            btn.Invoke((MethodInvoker)(() => btn.Enabled = false));
                        }
                        string newData = data.Replace("winner=", "");
                        //int winnerID = int.Parse(newData);
                        string caption = "Game over!";
                        string message = newData + " won!";
                        listBox1.Invoke((MethodInvoker)(() => listBox1.Items.Clear()));
                        listBox1.Invoke((MethodInvoker)(() => listBox1.Items.Add(message)));
                        listBox1.Invoke((MethodInvoker)(() => listBox1.SelectedIndex = listBox1.Items.Count - 1));
                        MessageBoxButtons buttons = MessageBoxButtons.OK;
                        DialogResult result;
                        result = MessageBox.Show(message, caption, buttons);
                        if (result == System.Windows.Forms.DialogResult.OK)
                        {
                            readyPlayer1.Invoke((MethodInvoker)(() => readyPlayer1.Text = "Not Ready"));
                            readyPlayer2.Invoke((MethodInvoker)(() => readyPlayer2.Text = "Not Ready"));
                            label6.Invoke((MethodInvoker)(() => label6.Text = ""));
                            foreach (Button btn in panel1.Controls)
                            {
                                btn.Invoke((MethodInvoker)(() => btn.BackgroundImage = null));
                                btn.Invoke((MethodInvoker)(() => btn.Tag = "none"));
                                btn.Invoke((MethodInvoker)(() => btn.Enabled = true));
                            }
                            clientReady = false;
                            opponentReady = false;
                            zeroCount = 0;
                            crossCount = 0;
                            myTurn = false;
                            readybutton.Invoke((MethodInvoker)(() => readybutton.Enabled = true));
                            listBox1.Invoke((MethodInvoker)(() => listBox1.Items.Add("Your id is: " + clientID)));
                            listBox1.Invoke((MethodInvoker)(() => listBox1.SelectedIndex = listBox1.Items.Count - 1));
                        }
                    }
                    if (data.ToString().StartsWith("opponentDisconnected"))
                    {
                        readyPlayer1.Invoke((MethodInvoker)(() => readyPlayer1.Text = "Not Ready"));
                        readyPlayer2.Invoke((MethodInvoker)(() => readyPlayer2.Text = "Not Ready"));
                        label6.Invoke((MethodInvoker)(() => label6.Text = ""));
                        foreach (Button btn in panel1.Controls)
                        {
                            btn.Invoke((MethodInvoker)(() => btn.BackgroundImage = null));
                            btn.Invoke((MethodInvoker)(() => btn.Tag = "none"));
                            btn.Invoke((MethodInvoker)(() => btn.Enabled = true));
                        }
                        clientReady = false;
                        opponentReady = false;
                        zeroCount = 0;
                        crossCount = 0;
                        readybutton.Invoke((MethodInvoker)(() => readybutton.Enabled = true));
                        listBox1.Invoke((MethodInvoker)(() => listBox1.Items.Add("Opponent disconnected!")));
                        listBox1.Invoke((MethodInvoker)(() => listBox1.SelectedIndex = listBox1.Items.Count - 1));
                    }
                    if (data.StartsWith("chat="))
                    {
                        string message = data.Replace("chat=", "");
                        listBox1.Invoke((MethodInvoker)(() => listBox1.Items.Add(message)));
                    }
                }
            }
            catch
            {

            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (client_connected)
            {
                try
                {
                    string textToSend = "disconnect=" + clientID;
                    NetworkStream nwStream = client.GetStream();
                    byte[] bytesToSend = ASCIIEncoding.ASCII.GetBytes(textToSend);
                    nwStream.Write(bytesToSend, 0, bytesToSend.Length);
                }
                catch { }
                client_connected = false;
                client.Close();
                client_thread.Join();
                this.Text = "TicTacToe Client";
                label4.Text = "Disconnected";
                readyPlayer1.Text = "Not Ready";
                readyPlayer2.Text = "Not Ready";
                foreach(Button btn in panel1.Controls)
                {
                    btn.BackgroundImage = null;
                    btn.Tag = "none";
                    btn.Enabled = true;
                }
                pictureBox1.Image = null;
                clientReady = false;
                opponentReady = false;
                zeroCount = 0;
                crossCount = 0;
                myTurn = false;
                readybutton.Invoke((MethodInvoker)(() => readybutton.Enabled = true));
                listBox1.Invoke((MethodInvoker)(() => listBox1.Items.Clear()));
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (client_connected)
            {
                try
                {
                    string textToSend = "My id is " + clientID;
                    NetworkStream nwStream = client.GetStream();
                    byte[] bytesToSend = ASCIIEncoding.ASCII.GetBytes(textToSend);
                    nwStream.Write(bytesToSend, 0, bytesToSend.Length);
                }
                catch { }
            }
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            try
            {
                string textToSend = "disconnect=" + clientID;
                NetworkStream nwStream = client.GetStream();
                byte[] bytesToSend = ASCIIEncoding.ASCII.GetBytes(textToSend);
                nwStream.Write(bytesToSend, 0, bytesToSend.Length);
            }
            catch { }
            Environment.Exit(Environment.ExitCode);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if(!clientReady && client_connected)
            {
                try
                {
                    string textToSend = "ready="+clientID;
                    NetworkStream nwStream = client.GetStream();
                    byte[] bytesToSend = ASCIIEncoding.ASCII.GetBytes(textToSend);
                    nwStream.Write(bytesToSend, 0, bytesToSend.Length);
                }
                catch { }
                if ((clientID % 2 + 1) == 1)
                    readyPlayer1.Text = "Ready";
                else if((clientID % 2 + 1) == 2)
                    readyPlayer2.Text = "Ready";
                //readybutton.Enabled = false;
                clientReady = true;
            }
        }

        private void box0_Click(object sender, EventArgs e)
        {
            if(myTurn)
            {
                if (player == 1)
                {
                    if(zeroCount < 3 && box0.Tag.Equals("none"))
                    {
                        box0.BackgroundImage = Image.FromFile(path + "zero.png");
                        box0.BackgroundImageLayout = ImageLayout.Zoom;
                        box0.Tag = "zero";
                        zeroCount++;
                        try
                        {
                            string textToSend = "update=" + clientID + "=0zero";
                            NetworkStream nwStream = client.GetStream();
                            byte[] bytesToSend = ASCIIEncoding.ASCII.GetBytes(textToSend);
                            nwStream.Write(bytesToSend, 0, bytesToSend.Length);
                        }
                        catch { }
                        myTurn = false;
                        try
                        {
                            string textToSend = "nextTurn="+clientID;
                            NetworkStream nwStream = client.GetStream();
                            byte[] bytesToSend = ASCIIEncoding.ASCII.GetBytes(textToSend);
                            nwStream.Write(bytesToSend, 0, bytesToSend.Length);
                        }
                        catch { }
                        myTimer.Enabled = false;
                        startTurnTime = true;
                        myTimer.Stop();
                        timeLeftLabel.Invoke((MethodInvoker)(() => timeLeftLabel.Text = ""));
                    }
                    else if (zeroCount >= 3 && box0.Tag.Equals("zero"))
                    {
                        box0.BackgroundImage  = null;
                        box0.Tag = "none";
                        box0.Enabled = false;
                        zeroCount--;
                        try
                        {
                            string textToSend = "update=" + clientID + "=0none";
                            NetworkStream nwStream = client.GetStream();
                            byte[] bytesToSend = ASCIIEncoding.ASCII.GetBytes(textToSend);
                            nwStream.Write(bytesToSend, 0, bytesToSend.Length);
                        }
                        catch { }
                    }
                    
                }
                if (player == 2)
                {
                    if(crossCount < 3 && box0.Tag.Equals("none"))
                    {
                        box0.BackgroundImage = Image.FromFile(path + "cross.png");
                        box0.BackgroundImageLayout = ImageLayout.Zoom;
                        box0.Tag = "cross";
                        crossCount++;
                        try
                        {
                            string textToSend = "update=" + clientID + "=0cross";
                            NetworkStream nwStream = client.GetStream();
                            byte[] bytesToSend = ASCIIEncoding.ASCII.GetBytes(textToSend);
                            nwStream.Write(bytesToSend, 0, bytesToSend.Length);
                        }
                        catch { }
                        myTurn = false;
                        try
                        {
                            string textToSend = "nextTurn="+clientID;;
                            NetworkStream nwStream = client.GetStream();
                            byte[] bytesToSend = ASCIIEncoding.ASCII.GetBytes(textToSend);
                            nwStream.Write(bytesToSend, 0, bytesToSend.Length);
                        }
                        catch { }
                        myTimer.Enabled = false;
                        startTurnTime = true;
                        myTimer.Stop();
                        timeLeftLabel.Invoke((MethodInvoker)(() => timeLeftLabel.Text = ""));
                    }
                    else if (crossCount >= 3 && box0.Tag.Equals("cross"))
                    {
                        box0.BackgroundImage = null;
                        box0.Tag = "none";
                        box0.Enabled = false;
                        crossCount--;
                        try
                        {
                            string textToSend = "update=" + clientID + "=0none";
                            NetworkStream nwStream = client.GetStream();
                            byte[] bytesToSend = ASCIIEncoding.ASCII.GetBytes(textToSend);
                            nwStream.Write(bytesToSend, 0, bytesToSend.Length);
                        }
                        catch { }
                    }
                }

            }
        }

        private void box1_Click(object sender, EventArgs e)
        {
            if (myTurn)
            {
                if (player == 1)
                {
                    if (zeroCount < 3 && box1.Tag.Equals("none"))
                    {
                        box1.BackgroundImage = Image.FromFile(path + "zero.png");
                        box1.BackgroundImageLayout = ImageLayout.Zoom;
                        box1.Tag = "zero";
                        zeroCount++;
                        try
                        {
                            string textToSend = "update=" + clientID + "=1zero";
                            NetworkStream nwStream = client.GetStream();
                            byte[] bytesToSend = ASCIIEncoding.ASCII.GetBytes(textToSend);
                            nwStream.Write(bytesToSend, 0, bytesToSend.Length);
                        }
                        catch { }
                        myTurn = false;
                        try
                        {
                            string textToSend = "nextTurn="+clientID;;
                            NetworkStream nwStream = client.GetStream();
                            byte[] bytesToSend = ASCIIEncoding.ASCII.GetBytes(textToSend);
                            nwStream.Write(bytesToSend, 0, bytesToSend.Length);
                        }
                        catch { }
                        myTimer.Enabled = false;
                        startTurnTime = true;
                        myTimer.Stop();
                        timeLeftLabel.Invoke((MethodInvoker)(() => timeLeftLabel.Text = ""));
                    }
                    else if (zeroCount >= 3 && box1.Tag.Equals("zero"))
                    {
                        box1.BackgroundImage = null;
                        box1.Tag = "none";
                        box1.Enabled = false;
                        zeroCount--;
                        try
                        {
                            string textToSend = "update=" + clientID + "=1none";
                            NetworkStream nwStream = client.GetStream();
                            byte[] bytesToSend = ASCIIEncoding.ASCII.GetBytes(textToSend);
                            nwStream.Write(bytesToSend, 0, bytesToSend.Length);
                        }
                        catch { }
                    }
                    
                }
                if (player == 2)
                {
                    if (crossCount < 3 && box1.Tag.Equals("none"))
                    {
                        box1.BackgroundImage = Image.FromFile(path + "cross.png");
                        box1.BackgroundImageLayout = ImageLayout.Zoom;
                        box1.Tag = "cross";
                        crossCount++;
                        try
                        {
                            string textToSend = "update=" + clientID + "=1cross";
                            NetworkStream nwStream = client.GetStream();
                            byte[] bytesToSend = ASCIIEncoding.ASCII.GetBytes(textToSend);
                            nwStream.Write(bytesToSend, 0, bytesToSend.Length);
                        }
                        catch { }
                        myTurn = false;
                        try
                        {
                            string textToSend = "nextTurn="+clientID;;
                            NetworkStream nwStream = client.GetStream();
                            byte[] bytesToSend = ASCIIEncoding.ASCII.GetBytes(textToSend);
                            nwStream.Write(bytesToSend, 0, bytesToSend.Length);
                        }
                        catch { }
                        myTimer.Enabled = false;
                        myTimer.Stop();
                        startTurnTime = true;
                        timeLeftLabel.Invoke((MethodInvoker)(() => timeLeftLabel.Text = ""));
                    }
                    else if (crossCount >= 3 && box1.Tag.Equals("cross"))
                    {
                        box1.BackgroundImage = null;
                        box1.Tag = "none";
                        box1.Enabled = false;
                        crossCount--;
                        try
                        {
                            string textToSend = "update=" + clientID + "=1none";
                            NetworkStream nwStream = client.GetStream();
                            byte[] bytesToSend = ASCIIEncoding.ASCII.GetBytes(textToSend);
                            nwStream.Write(bytesToSend, 0, bytesToSend.Length);
                        }
                        catch { }
                    }
                }
            }
        }

        private void box2_Click(object sender, EventArgs e)
        {
            if (myTurn)
            {
                if (player == 1)
                {
                    if (zeroCount < 3 && box2.Tag.Equals("none"))
                    {
                        box2.BackgroundImage = Image.FromFile(path + "zero.png");
                        box2.BackgroundImageLayout = ImageLayout.Zoom;
                        box2.Tag = "zero";
                        zeroCount++;
                        try
                        {
                            string textToSend = "update=" + clientID + "=2zero";
                            NetworkStream nwStream = client.GetStream();
                            byte[] bytesToSend = ASCIIEncoding.ASCII.GetBytes(textToSend);
                            nwStream.Write(bytesToSend, 0, bytesToSend.Length);
                        }
                        catch { }
                        myTurn = false;
                        try
                        {
                            string textToSend = "nextTurn="+clientID;;
                            NetworkStream nwStream = client.GetStream();
                            byte[] bytesToSend = ASCIIEncoding.ASCII.GetBytes(textToSend);
                            nwStream.Write(bytesToSend, 0, bytesToSend.Length);
                        }
                        catch { }
                        myTimer.Enabled = false;
                        startTurnTime = true;
                        myTimer.Stop();
                        timeLeftLabel.Invoke((MethodInvoker)(() => timeLeftLabel.Text = ""));
                    }
                    else if (zeroCount >= 3 && box2.Tag.Equals("zero"))
                    {
                        box2.BackgroundImage = null;
                        box2.Tag = "none";
                        box2.Enabled = false;
                        zeroCount--;
                        try
                        {
                            string textToSend = "update=" + clientID + "=2none";
                            NetworkStream nwStream = client.GetStream();
                            byte[] bytesToSend = ASCIIEncoding.ASCII.GetBytes(textToSend);
                            nwStream.Write(bytesToSend, 0, bytesToSend.Length);
                        }
                        catch { }
                    }
                }
                if (player == 2)
                {
                    if (crossCount < 3 && box2.Tag.Equals("none"))
                    {
                        box2.BackgroundImage = Image.FromFile(path + "cross.png");
                        box2.BackgroundImageLayout = ImageLayout.Zoom;
                        box2.Tag = "cross";
                        crossCount++;
                        try
                        {
                            string textToSend = "update=" + clientID + "=2cross";
                            NetworkStream nwStream = client.GetStream();
                            byte[] bytesToSend = ASCIIEncoding.ASCII.GetBytes(textToSend);
                            nwStream.Write(bytesToSend, 0, bytesToSend.Length);
                        }
                        catch { }
                        myTurn = false;
                        try
                        {
                            string textToSend = "nextTurn="+clientID;;
                            NetworkStream nwStream = client.GetStream();
                            byte[] bytesToSend = ASCIIEncoding.ASCII.GetBytes(textToSend);
                            nwStream.Write(bytesToSend, 0, bytesToSend.Length);
                        }
                        catch { }
                        myTimer.Enabled = false;
                        startTurnTime = true;
                        myTimer.Stop();
                        timeLeftLabel.Invoke((MethodInvoker)(() => timeLeftLabel.Text = ""));
                    }
                    else if (crossCount >= 3 && box2.Tag.Equals("cross"))
                    {
                        box2.BackgroundImage = null;
                        box2.Tag = "none";
                        box2.Enabled = false;
                        crossCount--;
                        try
                        {
                            string textToSend = "update=" + clientID + "=2none";
                            NetworkStream nwStream = client.GetStream();
                            byte[] bytesToSend = ASCIIEncoding.ASCII.GetBytes(textToSend);
                            nwStream.Write(bytesToSend, 0, bytesToSend.Length);
                        }
                        catch { }
                    }
                }
            }
        }

        private void box3_Click(object sender, EventArgs e)
        {
            if (myTurn)
            {
                if (player == 1)
                {
                    if (zeroCount < 3 && box3.Tag.Equals("none"))
                    {
                        box3.BackgroundImage = Image.FromFile(path + "zero.png");
                        box3.BackgroundImageLayout = ImageLayout.Zoom;
                        box3.Tag = "zero";
                        zeroCount++;
                        try
                        {
                            string textToSend = "update=" + clientID + "=3zero";
                            NetworkStream nwStream = client.GetStream();
                            byte[] bytesToSend = ASCIIEncoding.ASCII.GetBytes(textToSend);
                            nwStream.Write(bytesToSend, 0, bytesToSend.Length);
                        }
                        catch { }
                        myTurn = false;
                        try
                        {
                            string textToSend = "nextTurn="+clientID;;
                            NetworkStream nwStream = client.GetStream();
                            byte[] bytesToSend = ASCIIEncoding.ASCII.GetBytes(textToSend);
                            nwStream.Write(bytesToSend, 0, bytesToSend.Length);
                        }
                        catch { }
                        myTimer.Enabled = false;
                        startTurnTime = true;
                        myTimer.Stop();
                        timeLeftLabel.Invoke((MethodInvoker)(() => timeLeftLabel.Text = ""));
                    }
                    else if (zeroCount >= 3 && box3.Tag.Equals("zero"))
                    {
                        box3.BackgroundImage = null;
                        box3.Tag = "none";
                        box3.Enabled = false;
                        zeroCount--;
                        try
                        {
                            string textToSend = "update=" + clientID + "=3none";
                            NetworkStream nwStream = client.GetStream();
                            byte[] bytesToSend = ASCIIEncoding.ASCII.GetBytes(textToSend);
                            nwStream.Write(bytesToSend, 0, bytesToSend.Length);
                        }
                        catch { }
                    }
                }
                if (player == 2)
                {
                    if (crossCount < 3 && box3.Tag.Equals("none"))
                    {
                        box3.BackgroundImage = Image.FromFile(path + "cross.png");
                        box3.BackgroundImageLayout = ImageLayout.Zoom;
                        box3.Tag = "cross";
                        crossCount++;
                        try
                        {
                            string textToSend = "update=" + clientID + "=3cross";
                            NetworkStream nwStream = client.GetStream();
                            byte[] bytesToSend = ASCIIEncoding.ASCII.GetBytes(textToSend);
                            nwStream.Write(bytesToSend, 0, bytesToSend.Length);
                        }
                        catch { }
                        myTurn = false;
                        try
                        {
                            string textToSend = "nextTurn="+clientID;;
                            NetworkStream nwStream = client.GetStream();
                            byte[] bytesToSend = ASCIIEncoding.ASCII.GetBytes(textToSend);
                            nwStream.Write(bytesToSend, 0, bytesToSend.Length);
                        }
                        catch { }
                        myTimer.Enabled = false;
                        startTurnTime = true;
                        myTimer.Stop();
                        timeLeftLabel.Invoke((MethodInvoker)(() => timeLeftLabel.Text = ""));
                    }
                    else if (crossCount >= 3 && box3.Tag.Equals("cross"))
                    {
                        box3.BackgroundImage = null;
                        box3.Tag = "none";
                        box3.Enabled = false;
                        crossCount--;
                        try
                        {
                            string textToSend = "update=" + clientID + "=3none";
                            NetworkStream nwStream = client.GetStream();
                            byte[] bytesToSend = ASCIIEncoding.ASCII.GetBytes(textToSend);
                            nwStream.Write(bytesToSend, 0, bytesToSend.Length);
                        }
                        catch { }
                    }
                }
            }
        }

        private void box4_Click(object sender, EventArgs e)
        {
            if (myTurn)
            {
                if (player == 1)
                {
                    if (zeroCount < 3 && box4.Tag.Equals("none"))
                    {
                        box4.BackgroundImage = Image.FromFile(path + "zero.png");
                        box4.BackgroundImageLayout = ImageLayout.Zoom;
                        box4.Tag = "zero";
                        zeroCount++;
                        try
                        {
                            string textToSend = "update=" + clientID + "=4zero";
                            NetworkStream nwStream = client.GetStream();
                            byte[] bytesToSend = ASCIIEncoding.ASCII.GetBytes(textToSend);
                            nwStream.Write(bytesToSend, 0, bytesToSend.Length);
                        }
                        catch { }
                        myTurn = false;
                        try
                        {
                            string textToSend = "nextTurn="+clientID;;
                            NetworkStream nwStream = client.GetStream();
                            byte[] bytesToSend = ASCIIEncoding.ASCII.GetBytes(textToSend);
                            nwStream.Write(bytesToSend, 0, bytesToSend.Length);
                        }
                        catch { }
                        myTimer.Enabled = false;
                        startTurnTime = true;
                        myTimer.Stop();
                        timeLeftLabel.Invoke((MethodInvoker)(() => timeLeftLabel.Text = ""));
                    }
                    else if (zeroCount >= 3 && box4.Tag.Equals("zero"))
                    {
                        box4.BackgroundImage = null;
                        box4.Tag = "none";
                        box4.Enabled = false;
                        zeroCount--;
                        try
                        {
                            string textToSend = "update=" + clientID + "=4none";
                            NetworkStream nwStream = client.GetStream();
                            byte[] bytesToSend = ASCIIEncoding.ASCII.GetBytes(textToSend);
                            nwStream.Write(bytesToSend, 0, bytesToSend.Length);
                        }
                        catch { }
                    }
                }
                if (player == 2)
                {
                    if (crossCount < 3 && box4.Tag.Equals("none"))
                    {
                        box4.BackgroundImage = Image.FromFile(path + "cross.png");
                        box4.BackgroundImageLayout = ImageLayout.Zoom;
                        box4.Tag = "cross";
                        crossCount++;
                        try
                        {
                            string textToSend = "update=" + clientID + "=4cross";
                            NetworkStream nwStream = client.GetStream();
                            byte[] bytesToSend = ASCIIEncoding.ASCII.GetBytes(textToSend);
                            nwStream.Write(bytesToSend, 0, bytesToSend.Length);
                        }
                        catch { }
                        myTurn = false;
                        try
                        {
                            string textToSend = "nextTurn="+clientID;;
                            NetworkStream nwStream = client.GetStream();
                            byte[] bytesToSend = ASCIIEncoding.ASCII.GetBytes(textToSend);
                            nwStream.Write(bytesToSend, 0, bytesToSend.Length);
                        }
                        catch { }
                        myTimer.Enabled = false;
                        myTimer.Stop();
                        startTurnTime = true;
                        timeLeftLabel.Invoke((MethodInvoker)(() => timeLeftLabel.Text = ""));
                    }
                    else if (crossCount >= 3 && box4.Tag.Equals("cross"))
                    {
                        box4.BackgroundImage = null;
                        box4.Tag = "none";
                        box4.Enabled = false;
                        crossCount--;
                        try
                        {
                            string textToSend = "update=" + clientID + "=4none";
                            NetworkStream nwStream = client.GetStream();
                            byte[] bytesToSend = ASCIIEncoding.ASCII.GetBytes(textToSend);
                            nwStream.Write(bytesToSend, 0, bytesToSend.Length);
                        }
                        catch { }
                    }
                }
            }
        }

        private void box5_Click(object sender, EventArgs e)
        {
            if (myTurn)
            {
                if (player == 1)
                {
                    if (zeroCount < 3 && box5.Tag.Equals("none"))
                    {
                        box5.BackgroundImage = Image.FromFile(path + "zero.png");
                        box5.BackgroundImageLayout = ImageLayout.Zoom;
                        box5.Tag = "zero";
                        zeroCount++;
                        try
                        {
                            string textToSend = "update=" + clientID + "=5zero";
                            NetworkStream nwStream = client.GetStream();
                            byte[] bytesToSend = ASCIIEncoding.ASCII.GetBytes(textToSend);
                            nwStream.Write(bytesToSend, 0, bytesToSend.Length);
                        }
                        catch { }
                        myTurn = false;
                        try
                        {
                            string textToSend = "nextTurn="+clientID;;
                            NetworkStream nwStream = client.GetStream();
                            byte[] bytesToSend = ASCIIEncoding.ASCII.GetBytes(textToSend);
                            nwStream.Write(bytesToSend, 0, bytesToSend.Length);
                        }
                        catch { }
                        myTimer.Enabled = false;
                        startTurnTime = true;
                        myTimer.Stop();
                        timeLeftLabel.Invoke((MethodInvoker)(() => timeLeftLabel.Text = ""));
                    }
                    else if (zeroCount >= 3 && box5.Tag.Equals("zero"))
                    {
                        box5.BackgroundImage = null;
                        box5.Tag = "none";
                        box5.Enabled = false;
                        zeroCount--;
                        try
                        {
                            string textToSend = "update=" + clientID + "=5none";
                            NetworkStream nwStream = client.GetStream();
                            byte[] bytesToSend = ASCIIEncoding.ASCII.GetBytes(textToSend);
                            nwStream.Write(bytesToSend, 0, bytesToSend.Length);
                        }
                        catch { }
                    }
                }
                if (player == 2)
                {
                    if (crossCount < 3 && box5.Tag.Equals("none"))
                    {
                        box5.BackgroundImage = Image.FromFile(path + "cross.png");
                        box5.BackgroundImageLayout = ImageLayout.Zoom;
                        box5.Tag = "cross";
                        crossCount++;
                        try
                        {
                            string textToSend = "update=" + clientID + "=5cross";
                            NetworkStream nwStream = client.GetStream();
                            byte[] bytesToSend = ASCIIEncoding.ASCII.GetBytes(textToSend);
                            nwStream.Write(bytesToSend, 0, bytesToSend.Length);
                        }
                        catch { }
                        myTurn = false;
                        try
                        {
                            string textToSend = "nextTurn="+clientID;;
                            NetworkStream nwStream = client.GetStream();
                            byte[] bytesToSend = ASCIIEncoding.ASCII.GetBytes(textToSend);
                            nwStream.Write(bytesToSend, 0, bytesToSend.Length);
                        }
                        catch { }
                        myTimer.Enabled = false;
                        startTurnTime = true;
                        myTimer.Stop();
                        timeLeftLabel.Invoke((MethodInvoker)(() => timeLeftLabel.Text = ""));
                    }
                    else if (crossCount >= 3 && box5.Tag.Equals("cross"))
                    {
                        box5.BackgroundImage = null;
                        box5.Tag = "none";
                        box5.Enabled = false;
                        crossCount--;
                        try
                        {
                            string textToSend = "update=" + clientID + "=5none";
                            NetworkStream nwStream = client.GetStream();
                            byte[] bytesToSend = ASCIIEncoding.ASCII.GetBytes(textToSend);
                            nwStream.Write(bytesToSend, 0, bytesToSend.Length);
                        }
                        catch { }
                    }
                }
            }
        }

        private void box6_Click(object sender, EventArgs e)
        {
            if (myTurn)
            {
                if (player == 1)
                {
                    if (zeroCount < 3 && box6.Tag.Equals("none"))
                    {
                        box6.BackgroundImage = Image.FromFile(path + "zero.png");
                        box6.BackgroundImageLayout = ImageLayout.Zoom;
                        box6.Tag = "zero";
                        zeroCount++;
                        try
                        {
                            string textToSend = "update=" + clientID + "=6zero";
                            NetworkStream nwStream = client.GetStream();
                            byte[] bytesToSend = ASCIIEncoding.ASCII.GetBytes(textToSend);
                            nwStream.Write(bytesToSend, 0, bytesToSend.Length);
                        }
                        catch { }
                        myTurn = false;
                        try
                        {
                            string textToSend = "nextTurn="+clientID;;
                            NetworkStream nwStream = client.GetStream();
                            byte[] bytesToSend = ASCIIEncoding.ASCII.GetBytes(textToSend);
                            nwStream.Write(bytesToSend, 0, bytesToSend.Length);
                        }
                        catch { }
                        myTimer.Enabled = false;
                        startTurnTime = true;
                        myTimer.Stop();
                        timeLeftLabel.Invoke((MethodInvoker)(() => timeLeftLabel.Text = ""));
                    }
                    else if (zeroCount >= 3 && box6.Tag.Equals("zero"))
                    {
                        box6.BackgroundImage = null;
                        box6.Tag = "none";
                        box6.Enabled = false;
                        zeroCount--;
                        try
                        {
                            string textToSend = "update=" + clientID + "=6none";
                            NetworkStream nwStream = client.GetStream();
                            byte[] bytesToSend = ASCIIEncoding.ASCII.GetBytes(textToSend);
                            nwStream.Write(bytesToSend, 0, bytesToSend.Length);
                        }
                        catch { }
                    }
                }
                if (player == 2)
                {
                    if (crossCount < 3 && box6.Tag.Equals("none"))
                    {
                        box6.BackgroundImage = Image.FromFile(path + "cross.png");
                        box6.BackgroundImageLayout = ImageLayout.Zoom;
                        box6.Tag = "cross";
                        crossCount++;
                        try
                        {
                            string textToSend = "update=" + clientID + "=6cross";
                            NetworkStream nwStream = client.GetStream();
                            byte[] bytesToSend = ASCIIEncoding.ASCII.GetBytes(textToSend);
                            nwStream.Write(bytesToSend, 0, bytesToSend.Length);
                        }
                        catch { }
                        myTurn = false;
                        try
                        {
                            string textToSend = "nextTurn="+clientID;;
                            NetworkStream nwStream = client.GetStream();
                            byte[] bytesToSend = ASCIIEncoding.ASCII.GetBytes(textToSend);
                            nwStream.Write(bytesToSend, 0, bytesToSend.Length);
                        }
                        catch { }
                        myTimer.Enabled = false;
                        startTurnTime = true;
                        myTimer.Stop();
                        timeLeftLabel.Invoke((MethodInvoker)(() => timeLeftLabel.Text = ""));
                    }
                    else if (crossCount >= 3 && box6.Tag.Equals("cross"))
                    {
                        box6.BackgroundImage = null;
                        box6.Tag = "none";
                        box6.Enabled = false;
                        crossCount--;
                        try
                        {
                            string textToSend = "update=" + clientID + "=6none";
                            NetworkStream nwStream = client.GetStream();
                            byte[] bytesToSend = ASCIIEncoding.ASCII.GetBytes(textToSend);
                            nwStream.Write(bytesToSend, 0, bytesToSend.Length);
                        }
                        catch { }
                    }
                }
            }
        }

        private void box7_Click(object sender, EventArgs e)
        {
            if (myTurn)
            {
                if (player == 1)
                {
                    if (zeroCount < 3 && box7.Tag.Equals("none"))
                    {
                        box7.BackgroundImage = Image.FromFile(path + "zero.png");
                        box7.BackgroundImageLayout = ImageLayout.Zoom;
                        box7.Tag = "zero";
                        zeroCount++;
                        try
                        {
                            string textToSend = "update=" + clientID + "=7zero";
                            NetworkStream nwStream = client.GetStream();
                            byte[] bytesToSend = ASCIIEncoding.ASCII.GetBytes(textToSend);
                            nwStream.Write(bytesToSend, 0, bytesToSend.Length);
                        }
                        catch { }
                        myTurn = false;
                        try
                        {
                            string textToSend = "nextTurn="+clientID;;
                            NetworkStream nwStream = client.GetStream();
                            byte[] bytesToSend = ASCIIEncoding.ASCII.GetBytes(textToSend);
                            nwStream.Write(bytesToSend, 0, bytesToSend.Length);
                        }
                        catch { }
                        myTimer.Enabled = false;
                        startTurnTime = true;
                        myTimer.Stop();
                        timeLeftLabel.Invoke((MethodInvoker)(() => timeLeftLabel.Text = ""));
                    }
                    else if (zeroCount >= 3 && box7.Tag.Equals("zero"))
                    {
                        box7.BackgroundImage = null;
                        box7.Tag = "none";
                        box7.Enabled = false;
                        zeroCount--;
                        try
                        {
                            string textToSend = "update=" + clientID + "=7none";
                            NetworkStream nwStream = client.GetStream();
                            byte[] bytesToSend = ASCIIEncoding.ASCII.GetBytes(textToSend);
                            nwStream.Write(bytesToSend, 0, bytesToSend.Length);
                        }
                        catch { }
                    }
                }
                if (player == 2)
                {
                    if (crossCount < 3 && box7.Tag.Equals("none"))
                    {
                        box7.BackgroundImage = Image.FromFile(path + "cross.png");
                        box7.BackgroundImageLayout = ImageLayout.Zoom;
                        box7.Tag = "cross";
                        crossCount++;
                        try
                        {
                            string textToSend = "update=" + clientID + "=7cross";
                            NetworkStream nwStream = client.GetStream();
                            byte[] bytesToSend = ASCIIEncoding.ASCII.GetBytes(textToSend);
                            nwStream.Write(bytesToSend, 0, bytesToSend.Length);
                        }
                        catch { }
                        myTurn = false;
                        try
                        {
                            string textToSend = "nextTurn="+clientID;;
                            NetworkStream nwStream = client.GetStream();
                            byte[] bytesToSend = ASCIIEncoding.ASCII.GetBytes(textToSend);
                            nwStream.Write(bytesToSend, 0, bytesToSend.Length);
                        }
                        catch { }
                        myTimer.Enabled = false;
                        startTurnTime = true;
                        myTimer.Stop();
                        timeLeftLabel.Invoke((MethodInvoker)(() => timeLeftLabel.Text = ""));
                    }
                    else if (crossCount >= 3 && box7.Tag.Equals("cross"))
                    {
                        box7.BackgroundImage = null;
                        box7.Tag = "none";
                        box7.Enabled = false;
                        crossCount--;
                        try
                        {
                            string textToSend = "update=" + clientID + "=7none";
                            NetworkStream nwStream = client.GetStream();
                            byte[] bytesToSend = ASCIIEncoding.ASCII.GetBytes(textToSend);
                            nwStream.Write(bytesToSend, 0, bytesToSend.Length);
                        }
                        catch { }
                    }
                }
            }
        }

        private void box8_Click(object sender, EventArgs e)
        {
            if (myTurn)
            {
                if (player == 1)
                {
                    if (zeroCount < 3 && box8.Tag.Equals("none"))
                    {
                        box8.BackgroundImage = Image.FromFile(path + "zero.png");
                        box8.BackgroundImageLayout = ImageLayout.Zoom;
                        box8.Tag = "zero";
                        zeroCount++;
                        try
                        {
                            string textToSend = "update=" + clientID + "=8zero";
                            NetworkStream nwStream = client.GetStream();
                            byte[] bytesToSend = ASCIIEncoding.ASCII.GetBytes(textToSend);
                            nwStream.Write(bytesToSend, 0, bytesToSend.Length);
                        }
                        catch { }
                        myTurn = false;
                        try
                        {
                            string textToSend = "nextTurn="+clientID;;
                            NetworkStream nwStream = client.GetStream();
                            byte[] bytesToSend = ASCIIEncoding.ASCII.GetBytes(textToSend);
                            nwStream.Write(bytesToSend, 0, bytesToSend.Length);
                        }
                        catch { }
                        myTimer.Enabled = false;
                        startTurnTime = true;
                        myTimer.Stop();
                        timeLeftLabel.Invoke((MethodInvoker)(() => timeLeftLabel.Text = ""));
                    }
                    else if (zeroCount >= 3 && box8.Tag.Equals("zero"))
                    {
                        box8.BackgroundImage = null;
                        box8.Tag = "none";
                        box8.Enabled = false;
                        zeroCount--;
                        try
                        {
                            string textToSend = "update=" + clientID + "=8none";
                            NetworkStream nwStream = client.GetStream();
                            byte[] bytesToSend = ASCIIEncoding.ASCII.GetBytes(textToSend);
                            nwStream.Write(bytesToSend, 0, bytesToSend.Length);
                        }
                        catch { }
                    }
                }
                if (player == 2)
                {
                    if (crossCount < 3 && box8.Tag.Equals("none"))
                    {
                        box8.BackgroundImage = Image.FromFile(path + "cross.png");
                        box8.BackgroundImageLayout = ImageLayout.Zoom;
                        box8.Tag = "cross";
                        crossCount++;
                        try
                        {
                            string textToSend = "update=" + clientID + "=8cross";
                            NetworkStream nwStream = client.GetStream();
                            byte[] bytesToSend = ASCIIEncoding.ASCII.GetBytes(textToSend);
                            nwStream.Write(bytesToSend, 0, bytesToSend.Length);
                        }
                        catch { }
                        myTurn = false;
                        try
                        {
                            string textToSend = "nextTurn="+clientID;;
                            NetworkStream nwStream = client.GetStream();
                            byte[] bytesToSend = ASCIIEncoding.ASCII.GetBytes(textToSend);
                            nwStream.Write(bytesToSend, 0, bytesToSend.Length);
                        }
                        catch { }
                        myTimer.Enabled = false;
                        startTurnTime = true;
                        myTimer.Stop();
                        timeLeftLabel.Invoke((MethodInvoker)(() => timeLeftLabel.Text = ""));
                    }
                    else if (crossCount >= 3 && box8.Tag.Equals("cross"))
                    {
                        box8.BackgroundImage = null;
                        box8.Tag = "none";
                        box8.Enabled = false;
                        crossCount--;
                        try
                        {
                            string textToSend = "update=" + clientID + "=8none";
                            NetworkStream nwStream = client.GetStream();
                            byte[] bytesToSend = ASCIIEncoding.ASCII.GetBytes(textToSend);
                            nwStream.Write(bytesToSend, 0, bytesToSend.Length);
                        }
                        catch { }
                    }
                }
            }
        }

        private void button4_Click_1(object sender, EventArgs e)
        {
            if(client_connected)
            {
                string textToSend = "chat=" + username.Text + ": " + textBox1.Text;
                NetworkStream nwStream = client.GetStream();
                byte[] bytesToSend = ASCIIEncoding.ASCII.GetBytes(textToSend);
                nwStream.Write(bytesToSend, 0, bytesToSend.Length);
                textBox1.Text = "";
            }
        }

        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if(client_connected)
                {
                    string textToSend = "chat=" + username.Text + ": " + textBox1.Text;
                    NetworkStream nwStream = client.GetStream();
                    byte[] bytesToSend = ASCIIEncoding.ASCII.GetBytes(textToSend);
                    nwStream.Write(bytesToSend, 0, bytesToSend.Length);
                    textBox1.Text = "";
                }
            }
        }
    }
}

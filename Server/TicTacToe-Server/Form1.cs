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

namespace TicTacToe_Server
{
    public partial class Form1 : Form
    {
        Int32 port = 50000;
        //static string IP = "127.0.0.1";
        static string IP = "192.168.1.11";
        IPAddress localAddr = IPAddress.Parse(IP);
        bool server_started = false;
        TcpListener server = null;
        Thread server_thread;
        Thread game_thread;
        static int maxClients = 10;
        public enum Token { X, O, Empty }
        public Token[,] _board = new Token[3, 3];
        struct GameInfo
        {
            public int id;
            public int playerID1;
            public int playerID2;
            public bool setTurn;
            public Thread[] threads;
            public int playerTurn;
            public bool game_started;
            public Token[,] _board;
        }

        struct ClientInfo
        {
            public int id;
            public int gameID;
            public int opponentID;
            public NetworkStream stream;
            public bool ready;
            public bool connected;
            public bool inGame;
            public string name;
            public bool Pota;
            public bool verified;
        };

        int connected_clients = 0;

        Thread[] threadsArray = new Thread[maxClients + 1];
        ClientInfo[] clients = new ClientInfo[maxClients + 1];
        GameInfo[] games = new GameInfo[(maxClients / 2) + 1];
        int started_games = 0;

        void init()
        {
            connected_clients = 0;
            for (int i = 1; i < clients.Length; i++)
            {
                clients[i].id = -1;
                clients[i].gameID = -1;
                clients[i].opponentID = -1;
                clients[i].name = "";
                clients[i].ready = false;
                clients[i].inGame = false;
                clients[i].Pota = false;
                clients[i].verified = false;
            }
            for (int i = 0; i < games.Length; i++)
            {
                games[i].id = -1;
                games[i]._board = new Token[3, 3];
                games[i].threads = new Thread[maxClients / 2];
                games[i].game_started = false;
                games[i].playerTurn = -1;
                games[i].playerID1 = -1;
                games[i].playerID2 = -1;
                games[i].setTurn = false;
                for (int j = 0; j < 3; j++)
                {
                    for (int k = 0; k < 3; k++)
                    {
                        games[i]._board[j, k] = Token.Empty;
                    }
                }
            }

            /*
            player1status.Text = "Not connected";
            player2status.Text = "Not connected";
            readyPlayer1.Text = "No";
            readyPlayer2.Text = "No";*/
        }

        void updateClientList()
        {
            clientListPanel.Invoke((MethodInvoker)(() => clientListPanel.Controls.Clear()));
            for (int i = 1; i < maxClients + 1; i++)
            {
                if (clients[i].connected)
                {
                    Label textControl = new Label();
                    textControl.AutoSize = true;
                    textControl.Text = "[" + i + "]:    ";
                    textControl.Text += clients[i].name + "    ";
                    if (clients[i].ready)
                        textControl.Text += "Ready" + "    ";
                    else
                        textControl.Text += "Not Ready" + "    ";
                    if (clients[i].inGame)
                        textControl.Text += "In Game" + "    ";
                    else
                        textControl.Text += "Not In Game" + "    ";
                    textControl.Location = new Point(5, 5 + (i * 15));
                    clientListPanel.Invoke((MethodInvoker)(() => clientListPanel.Controls.Add(textControl)));
                }
            }
        }

        void updateGameList()
        {
            Font MediumFont = new Font("Courier", 15);
            Font LargeFont = new Font("Courier", 20);
            gameListPanel.Invoke((MethodInvoker)(() => gameListPanel.Controls.Clear()));
            for (int i = 0; i < games.Length; i++)
            {
                if (games[i].game_started)
                {
                    Label textControl = new Label();
                    textControl.AutoSize = true;
                    textControl.Text = "Game [" + games[i].id + "]:    ";
                    textControl.Text += "Player 1: [" + games[i].playerID1 + "]    ";
                    textControl.Text += "Player 2: [" + games[i].playerID2 + "]    ";
                    textControl.Text += "Turn: [" + games[i].playerTurn + "]    ";
                    textControl.Location = new Point(5, 5 + (i * 15));
                    gameListPanel.Invoke((MethodInvoker)(() => gameListPanel.Controls.Add(textControl)));
                    if (gameSelection.SelectedIndex == i)
                    {
                        string board = "";
                        for (int k = 0; k < games[i]._board.GetLength(0); k++)
                        {
                            for (int j = 0; j < games[i]._board.GetLength(1); j++)
                            {
                                if (!games[i]._board[k, j].Equals(Token.Empty))
                                    board += (games[i]._board[k, j] + " ");
                                else
                                    board += ("  ");
                            }
                            board += "\n";
                        }
                        gamePreview.Invoke((MethodInvoker)(() => gamePreview.Font = LargeFont));
                        gamePreview.Invoke((MethodInvoker)(() => gamePreview.Text = board));
                    }
                }
                else
                {
                    if (gameSelection.SelectedIndex == i)
                    {
                        gamePreview.Invoke((MethodInvoker)(() => gamePreview.Font = MediumFont));
                        gamePreview.Invoke((MethodInvoker)(() => gamePreview.Text = "Game not started"));
                    }
                }
            }
        }

        public class Item
        {
            public Item() { }

            public string Value { set; get; }
            public string Text { set; get; }
        }
        public Form1()
        {
            InitializeComponent();
            this.Text = "TicTacToe Server";
            init();
            game_thread = new Thread(mainGame);
            game_thread.IsBackground = true;
            game_thread.Start();
            button3.Visible = false;
            button4.Visible = false;
            playerturnlabel.Visible = false;
            label10.Visible = false;
            List<Item> items = new List<Item>();
            for (int i = 0; i < games.Length - 1; i++)
            {
                items.Add(new Item() { Text = "Game " + i, Value = "" + i });
            }

            gameSelection.DataSource = items;
            gameSelection.DisplayMember = "Text";
            gameSelection.ValueMember = "Value";

        }

        void mainGame()
        {
            while (true)
            {
                /*for (int i = 1; i < clients.Length; i++)
                {
                    Console.WriteLine("[" + i + "] " + clients[i].id);
                    Thread.Sleep(1000);
                }*/

                for (int i = 0; i < games.Length; i++)
                {
                    if (games[i].id == -1 && !games[i].game_started)
                    {
                        for (int j = 1; j < connected_clients; j += 2)
                        {
                            if (clients[j].connected && clients[j + 1].connected)
                            {
                                if (!clients[j].inGame && !clients[j + 1].inGame)
                                    if (clients[j].ready && clients[j + 1].ready)
                                    {
                                        games[i].id = i;
                                        games[i].playerID1 = j;
                                        games[i].playerID2 = j + 1;
                                        //games[i].game_started = true;
                                        games[i].setTurn = true;
                                        clients[j].inGame = true;
                                        clients[j].gameID = games[i].id;
                                        clients[j + 1].inGame = true;
                                        clients[j + 1].gameID = games[i].id;
                                        started_games++;
                                        break;
                                    }
                            }
                        }

                    }
                }

                //Console.WriteLine(started_games);
                Thread.Sleep(100);

                for (int i = 0; i < started_games; i++)
                {
                    if (!games[i].game_started)
                    {
                        if (games[i].setTurn)
                        {
                            listBox1.Invoke((MethodInvoker)(() => listBox1.Items.Add("Set turn for game " + games[i].id)));
                            listBox1.Invoke((MethodInvoker)(() => listBox1.SelectedIndex = listBox1.Items.Count - 1));
                            Random rand = new Random();
                            games[i].playerTurn = rand.Next(1, 3);
                            games[i].setTurn = false;
                            string textToSend = "turn=" + games[i].playerTurn;
                            byte[] bytesToSend = ASCIIEncoding.ASCII.GetBytes(textToSend);
                            clients[games[i].playerID1].stream.Write(bytesToSend, 0, bytesToSend.Length);
                            clients[games[i].playerID2].stream.Write(bytesToSend, 0, bytesToSend.Length);
                            games[i].game_started = true;
                            updateClientList();
                            updateGameList();
                        }
                    }
                    else if (games[i].game_started)
                    {
                        if (HasWon(Token.O, games[i].id))
                        {
                            games[i].id = -1;
                            games[i].game_started = false;
                            clients[games[i].playerID1].ready = false;
                            clients[games[i].playerID2].ready = false;
                            clients[games[i].playerID1].inGame = false;
                            clients[games[i].playerID2].inGame = false;
                            clients[games[i].playerID1].gameID = -1;
                            clients[games[i].playerID2].gameID = -1;
                            games[i].setTurn = true;
                            started_games--;
                            Console.WriteLine("Player 1 Won!");
                            int winnerID = -1;
                            if ((clients[games[i].playerID1].id % 2) == 0)
                                winnerID = games[i].playerID2;
                            else
                                winnerID = games[i].playerID1;
                            string textToSend = "winner=" + clients[winnerID].name;
                            byte[] bytesToSend = ASCIIEncoding.ASCII.GetBytes(textToSend);
                            clients[games[i].playerID1].stream.Write(bytesToSend, 0, bytesToSend.Length);
                            Thread.Sleep(100);
                            clients[games[i].playerID2].stream.Write(bytesToSend, 0, bytesToSend.Length);
                            for (int j = 0; j < 3; j++)
                            {
                                for (int k = 0; k < 3; k++)
                                {
                                    games[i]._board[j, k] = Token.Empty;
                                }
                            }
                            updateClientList();
                            updateGameList();
                        }
                        else if (HasWon(Token.X, games[i].id))
                        {
                            games[i].id = -1;
                            games[i].game_started = false;
                            clients[games[i].playerID1].ready = false;
                            clients[games[i].playerID2].ready = false;
                            clients[games[i].playerID1].inGame = false;
                            clients[games[i].playerID2].inGame = false;
                            clients[games[i].playerID1].gameID = -1;
                            clients[games[i].playerID2].gameID = -1;
                            games[i].setTurn = true;
                            started_games--;
                            Console.WriteLine("Player 2 Won!");
                            int winnerID = -1;
                            if ((clients[games[i].playerID1].id % 2) == 0)
                                winnerID = games[i].playerID1;
                            else
                                winnerID = games[i].playerID2;
                            string textToSend = "winner=" + clients[winnerID].name;
                            byte[] bytesToSend = ASCIIEncoding.ASCII.GetBytes(textToSend);
                            clients[games[i].playerID1].stream.Write(bytesToSend, 0, bytesToSend.Length);
                            Thread.Sleep(100);
                            clients[games[i].playerID2].stream.Write(bytesToSend, 0, bytesToSend.Length);
                            for (int j = 0; j < 3; j++)
                            {
                                for (int k = 0; k < 3; k++)
                                {
                                    games[i]._board[j, k] = Token.Empty;
                                }
                            }
                            updateClientList();
                            updateGameList();
                        }
                    }
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (!server_started)
            {
                label2.Text = "Waiting for a connection...";
                server_thread = new Thread(serverListen);
                server_thread.Start();
                server_started = true;
            }
        }

        public bool HasWon(Token token, int game)
        {
            return IsHorizontalVictory(token, game) || IsVerticalVictory(token, game) || IsDiagonalVictory(token, game);
        }

        private bool IsHorizontalVictory(Token token, int game)
        {
            for (int y = 0; y <= 2; y++)
            {
                if (games[game]._board[0, y] == token && games[game]._board[1, y] == token && games[game]._board[2, y] == token)
                    return true;
            }

            return false;
        }

        private bool IsVerticalVictory(Token token, int game)
        {
            for (int x = 0; x <= 2; x++)
            {
                if (games[game]._board[x, 0] == token && games[game]._board[x, 1] == token && games[game]._board[x, 2] == token)
                    return true;
            }

            return false;
        }

        private bool IsDiagonalVictory(Token token, int game)
        {
            if (games[game]._board[0, 0] == token && games[game]._board[1, 1] == token && games[game]._board[2, 2] == token)
                return true;

            if (games[game]._board[0, 2] == token && games[game]._board[1, 1] == token && games[game]._board[2, 0] == token)
                return true;

            return false;
        }

        void HandleClient(TcpClient obj)
        {
            Byte[] bytes = new Byte[256];
            String data = null;
            int thisClientId = -1;
            TcpClient client = obj;
            //stream = client.GetStream();
            if (client.Connected)
            {

                for (int j = 1; j < clients.Length; j++)
                {
                    if (clients[j].id == -1)
                    {
                        clients[j].id = j;
                        thisClientId = clients[j].id;
                        //thisClientId = connected_clients;
                        break;
                    }
                }
                try
                {
                    clients[thisClientId].stream = client.GetStream();
                    string textToSend = "id=" + thisClientId;
                    byte[] bytesToSend = ASCIIEncoding.ASCII.GetBytes(textToSend);
                    clients[thisClientId].stream.Write(bytesToSend, 0, bytesToSend.Length);
                }
                catch { }
                listBox1.Invoke((MethodInvoker)(() => listBox1.Items.Add("Client with id " + thisClientId + " connected to the Server!")));
                listBox1.Invoke((MethodInvoker)(() => listBox1.SelectedIndex = listBox1.Items.Count - 1));
                clients[thisClientId].connected = true;
                clients[thisClientId].id = thisClientId;
                updateClientList();
            }

            int i;

            while ((i = clients[thisClientId].stream.Read(bytes, 0, bytes.Length)) != 0)
            {
                data = System.Text.Encoding.ASCII.GetString(bytes, 0, i);
                if (data.StartsWith("username=") && !clients[thisClientId].verified)
                {
                    string newData = data.Replace("username=", "");
                    string strID = newData.Substring(0, 1);
                    int clientID = int.Parse(strID);
                    string username = newData.Replace(strID + "=", "");
                    clients[clientID].name = username;
                    clients[clientID].verified = true;
                    updateClientList();
                }
                else if (!data.StartsWith("username=") && !clients[thisClientId].verified)
                {
                    clients[thisClientId].Pota = true;
                }
                if (data.StartsWith("chat="))
                {
                    if (clients[thisClientId].verified)
                    {
                        string message = data.Replace("chat=", "");
                        int opponentID = -1;
                        int offset = thisClientId % 2;
                        opponentID = (thisClientId % 2) == 1 ? thisClientId + offset : thisClientId - 1;

                        string textToSend = data;
                        byte[] bytesToSend = ASCIIEncoding.ASCII.GetBytes(textToSend);
                        if (clients[opponentID].stream != null)
                            clients[opponentID].stream.Write(bytesToSend, 0, bytesToSend.Length);
                    }
                }
                if (data.Contains("disconnect="))
                {
                    string newData = data.Replace("disconnect=", "");
                    int clientID = int.Parse(newData);
                    if (clients[clientID].verified)
                    {
                        listBox1.Invoke((MethodInvoker)(() => listBox1.Items.Add("Client with id " + clientID + " disconnected from the Server!")));
                        listBox1.Invoke((MethodInvoker)(() => listBox1.SelectedIndex = listBox1.Items.Count - 1));
                        clients[clientID].connected = false;
                        clients[clientID].ready = false;
                        clients[clientID].id = -1;
                        clients[clientID].verified = false;
                        clients[clientID].Pota = false;
                        clients[clientID].name = "";
                        int gameID = clients[clientID].gameID;
                        if (gameID != -1 && games[gameID].game_started)
                        {
                            int opponentID = -1;
                            if (games[gameID].playerID1 == clientID)
                                opponentID = games[gameID].playerID2;
                            else
                                opponentID = games[gameID].playerID1;
                            string textToSend = "opponentDisconnected";
                            byte[] bytesToSend = ASCIIEncoding.ASCII.GetBytes(textToSend);
                            clients[opponentID].stream.Write(bytesToSend, 0, bytesToSend.Length);
                            games[gameID].id = -1;
                            games[gameID].game_started = false;
                            clients[games[gameID].playerID1].ready = false;
                            clients[games[gameID].playerID2].ready = false;
                            clients[games[gameID].playerID1].inGame = false;
                            clients[games[gameID].playerID2].inGame = false;
                            clients[games[gameID].playerID1].gameID = -1;
                            clients[games[gameID].playerID2].gameID = -1;
                            games[gameID].setTurn = true;
                            for (int j = 0; j < 3; j++)
                            {
                                for (int k = 0; k < 3; k++)
                                {
                                    games[gameID]._board[j, k] = Token.Empty;
                                }
                            }
                            started_games--;
                        }
                        client.Close();
                        updateClientList();
                        updateGameList();
                        connected_clients--;
                        if (connected_clients == 0)
                            label2.Invoke((MethodInvoker)(() => label2.Text = "Waiting for a connection..."));
                        label4.Invoke((MethodInvoker)(() => label4.Text = "" + connected_clients));
                        playerturnlabel.Invoke((MethodInvoker)(() => playerturnlabel.Text = ""));
                        threadsArray[clientID].Join();
                    }
                }
                if (data.Contains("ready="))
                {
                    string newData = data.Replace("ready=", "");
                    int clientID = int.Parse(newData);
                    if (clients[clientID].verified)
                    {
                        clients[clientID].ready = true;
                        if (connected_clients <= 1)
                            return;
                        int opponentID = -1;
                        int offset = clientID % 2;
                        opponentID = (clientID % 2) == 1 ? clientID + offset : clientID - 1;
                        //listBox1.Invoke((MethodInvoker)(() => listBox1.Items.Add("clientID " + clientID + " opponentID " + opponentID)));
                        string textToSend = "opponentReady=" + thisClientId;
                        byte[] bytesToSend = ASCIIEncoding.ASCII.GetBytes(textToSend);
                        clients[opponentID].stream.Write(bytesToSend, 0, bytesToSend.Length);

                        updateClientList();
                        updateGameList();
                    }
                }
                for (int game = 0; game < started_games; game++)
                {
                    if (data.Contains("update=") && games[game].game_started)
                    {
                        string newData = data.Replace("update=", "");
                        string strID = newData.Substring(0, 1);
                        //listBox1.Invoke((MethodInvoker)(() => listBox1.Items.Add(strID)));
                        int clientID = int.Parse(strID);
                        if (clients[clientID].verified)
                        {
                            if (clientID == games[game].playerID1 || clientID == games[game].playerID2)
                            {
                                newData = newData.Remove(0, 2);
                                //listBox1.Invoke((MethodInvoker)(() => listBox1.Items.Add("dada " + newData)));
                                int opponentID = -1;
                                if (clientID == games[game].playerID1)
                                    opponentID = games[game].playerID2;
                                else if (clientID == games[game].playerID2)
                                    opponentID = games[game].playerID1;
                                string textToSend = "update=" + newData;
                                byte[] bytesToSend = ASCIIEncoding.ASCII.GetBytes(textToSend);
                                clients[opponentID].stream.Write(bytesToSend, 0, bytesToSend.Length);

                                string id = newData.Substring(0, 1);
                                newData = newData.Remove(0, 1);
                                string status = newData;
                                if (id.Contains("0"))
                                {
                                    if (status.Contains("zero"))
                                    {
                                        games[game]._board[0, 0] = Token.O;
                                    }
                                    if (status.Contains("cross"))
                                    {
                                        games[game]._board[0, 0] = Token.X;
                                    }
                                    if (status.Contains("none"))
                                    {
                                        games[game]._board[0, 0] = Token.Empty;
                                    }
                                }
                                if (id.Contains("1"))
                                {
                                    if (status.Contains("zero"))
                                    {
                                        games[game]._board[0, 1] = Token.O;
                                    }
                                    if (status.Contains("cross"))
                                    {
                                        games[game]._board[0, 1] = Token.X;
                                    }
                                    if (status.Contains("none"))
                                    {
                                        games[game]._board[0, 1] = Token.Empty;
                                    }
                                }
                                if (id.Contains("2"))
                                {
                                    if (status.Contains("zero"))
                                    {
                                        games[game]._board[0, 2] = Token.O;
                                    }
                                    if (status.Contains("cross"))
                                    {
                                        games[game]._board[0, 2] = Token.X;
                                    }
                                    if (status.Contains("none"))
                                    {
                                        games[game]._board[0, 2] = Token.Empty;
                                    }
                                }
                                if (id.Contains("3"))
                                {
                                    if (status.Contains("zero"))
                                    {
                                        games[game]._board[1, 0] = Token.O;
                                    }
                                    if (status.Contains("cross"))
                                    {
                                        games[game]._board[1, 0] = Token.X;
                                    }
                                    if (status.Contains("none"))
                                    {
                                        games[game]._board[1, 0] = Token.Empty;
                                    }
                                }
                                if (id.Contains("4"))
                                {
                                    if (status.Contains("zero"))
                                    {
                                        games[game]._board[1, 1] = Token.O;
                                    }
                                    if (status.Contains("cross"))
                                    {
                                        games[game]._board[1, 1] = Token.X;
                                    }
                                    if (status.Contains("none"))
                                    {
                                        games[game]._board[1, 1] = Token.Empty;
                                    }
                                }
                                if (id.Contains("5"))
                                {
                                    if (status.Contains("zero"))
                                    {
                                        games[game]._board[1, 2] = Token.O;
                                    }
                                    if (status.Contains("cross"))
                                    {
                                        games[game]._board[1, 2] = Token.X;
                                    }
                                    if (status.Contains("none"))
                                    {
                                        games[game]._board[1, 2] = Token.Empty;
                                    }
                                }
                                if (id.Contains("6"))
                                {
                                    if (status.Contains("zero"))
                                    {
                                        games[game]._board[2, 0] = Token.O;
                                    }
                                    if (status.Contains("cross"))
                                    {
                                        games[game]._board[2, 0] = Token.X;
                                    }
                                    if (status.Contains("none"))
                                    {
                                        games[game]._board[2, 0] = Token.Empty;
                                    }
                                }
                                if (id.Contains("7"))
                                {
                                    if (status.Contains("zero"))
                                    {
                                        games[game]._board[2, 1] = Token.O;
                                    }
                                    if (status.Contains("cross"))
                                    {
                                        games[game]._board[2, 1] = Token.X;
                                    }
                                    if (status.Contains("none"))
                                    {
                                        games[game]._board[2, 1] = Token.Empty;
                                    }
                                }
                                if (id.Contains("8"))
                                {
                                    if (status.Contains("zero"))
                                    {
                                        games[game]._board[2, 2] = Token.O;
                                    }
                                    if (status.Contains("cross"))
                                    {
                                        games[game]._board[2, 2] = Token.X;
                                    }
                                    if (status.Contains("none"))
                                    {
                                        games[game]._board[2, 2] = Token.Empty;
                                    }
                                }
                                Console.WriteLine("Game " + games[game].id + ":");
                                for (int k = 0; k < games[game]._board.GetLength(0); k++)
                                {
                                    for (int j = 0; j < games[game]._board.GetLength(1); j++)
                                    {
                                        Console.Write(games[game]._board[k, j] + "\t");
                                    }
                                    Console.WriteLine();
                                }
                            }
                            updateGameList();
                        }
                    }
                    if (data.StartsWith("nextTurn=") && games[game].game_started)
                    {
                        string newData = data.Replace("nextTurn=", "");
                        string strID = newData.Substring(0, 1);
                        int clientID = int.Parse(strID);
                        if (clients[clientID].verified)
                        {
                            if (games[game].game_started && (clientID == games[game].playerID1 || clientID == games[game].playerID2))
                            {
                                if (games[game].playerTurn == 1)
                                    games[game].playerTurn = 2;
                                else if (games[game].playerTurn == 2)
                                    games[game].playerTurn = 1;
                                string textToSend = "turn=" + games[game].playerTurn;
                                byte[] bytesToSend = ASCIIEncoding.ASCII.GetBytes(textToSend);
                                clients[games[game].playerID1].stream.Write(bytesToSend, 0, bytesToSend.Length);
                                Thread.Sleep(100);
                                clients[games[game].playerID2].stream.Write(bytesToSend, 0, bytesToSend.Length);
                            }
                            updateGameList();
                        }
                    }
                }
                if (clients[thisClientId].verified)
                {
                    listBox1.Invoke((MethodInvoker)(() => listBox1.Items.Add("[" + thisClientId + "]: " + data)));
                    listBox1.Invoke((MethodInvoker)(() => listBox1.SelectedIndex = listBox1.Items.Count - 1));
                }
                else if (clients[thisClientId].Pota)
                {
                    listBox1.Invoke((MethodInvoker)(() => listBox1.Items.Add("Bravo Pota!")));
                    listBox1.Invoke((MethodInvoker)(() => listBox1.SelectedIndex = listBox1.Items.Count - 1));
                    int clientID = thisClientId;
                    listBox1.Invoke((MethodInvoker)(() => listBox1.Items.Add("Client with id " + clientID + " disconnected from the Server!")));
                    listBox1.Invoke((MethodInvoker)(() => listBox1.SelectedIndex = listBox1.Items.Count - 1));
                    clients[clientID].connected = false;
                    clients[clientID].ready = false;
                    clients[clientID].id = -1;
                    int gameID = clients[clientID].gameID;
                    if (gameID != -1 && games[gameID].game_started)
                    {
                        int opponentID = -1;
                        if (games[gameID].playerID1 == clientID)
                            opponentID = games[gameID].playerID2;
                        else
                            opponentID = games[gameID].playerID1;
                        string textToSend = "opponentDisconnected";
                        byte[] bytesToSend = ASCIIEncoding.ASCII.GetBytes(textToSend);
                        clients[opponentID].stream.Write(bytesToSend, 0, bytesToSend.Length);
                        games[gameID].id = -1;
                        games[gameID].game_started = false;
                        clients[games[gameID].playerID1].ready = false;
                        clients[games[gameID].playerID2].ready = false;
                        clients[games[gameID].playerID1].inGame = false;
                        clients[games[gameID].playerID2].inGame = false;
                        clients[games[gameID].playerID1].gameID = -1;
                        clients[games[gameID].playerID2].gameID = -1;
                        games[gameID].setTurn = true;
                        started_games--;
                    }
                    client.Close();
                    updateClientList();
                    updateGameList();
                    connected_clients--;
                    if (connected_clients == 0)
                        label2.Invoke((MethodInvoker)(() => label2.Text = "Waiting for a connection..."));
                    label4.Invoke((MethodInvoker)(() => label4.Text = "" + connected_clients));
                    playerturnlabel.Invoke((MethodInvoker)(() => playerturnlabel.Text = ""));
                    threadsArray[clientID].Join();
                }
            }

            client.Close();
        }

        public Thread StartTheThread(TcpClient obj)
        {
            var t = new Thread(() => HandleClient(obj));
            t.IsBackground = true;
            t.Start();
            return t;
        }

        void serverListen()
        {
            server = new TcpListener(localAddr, port);
            server.Start();
            while (server_started)
            {
                try
                {
                    if (connected_clients == 0)
                        label2.Invoke((MethodInvoker)(() => label2.Text = "Waiting for a connection..."));
                    else
                        label2.Invoke((MethodInvoker)(() => label2.Text = "Clients connected."));
                    if (connected_clients < maxClients)
                    {
                        TcpClient client = server.AcceptTcpClient();
                        if (client.Connected)
                        {
                            connected_clients++;
                            label4.Invoke((MethodInvoker)(() => label4.Text = "" + connected_clients));
                            threadsArray[connected_clients] = StartTheThread(client);
                        }
                    }
                    else
                    {
                        TcpClient client = server.AcceptTcpClient();
                        NetworkStream stream;
                        stream = client.GetStream();
                        string textToSend = "full";
                        byte[] bytesToSend = ASCIIEncoding.ASCII.GetBytes(textToSend);
                        stream.Write(bytesToSend, 0, bytesToSend.Length);
                        stream.Close();
                        client.Close();
                    }

                }
                catch
                {

                }
            }
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            Environment.Exit(Environment.ExitCode);

            /*if (server_started)
            {
                server.Stop();
                server_started = false;
                server_thread.Join();
            }*/
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (server_started)
            {
                listBox1.Items.Clear();
                server.Stop();
                server_started = false;
                server_thread.Join();
                label2.Text = "Offline";
                init();
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            clients[1].ready = false;
            clients[2].ready = false;
            readyPlayer1.Invoke((MethodInvoker)(() => readyPlayer1.Text = "No"));
            readyPlayer2.Invoke((MethodInvoker)(() => readyPlayer2.Text = "No"));
            string textToSend = "reset";
            byte[] bytesToSend = ASCIIEncoding.ASCII.GetBytes(textToSend);
            if (clients[1].connected)
                clients[1].stream.Write(bytesToSend, 0, bytesToSend.Length);
            if (clients[2].connected)
                clients[2].stream.Write(bytesToSend, 0, bytesToSend.Length);
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    _board[i, j] = Token.Empty;
                }
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            updateClientList();
        }

        private void gameSelection_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.Handle != null)
                updateGameList();
        }
    }
}

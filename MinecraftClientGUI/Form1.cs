using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using System.Threading;
using System.IO;

namespace MinecraftClientGUI
{
    /// <summary>
    /// The main graphical user interface
    /// </summary>

    public partial class Form1 : Form
    {
        private LinkedList<string> previous = new LinkedList<string>();
        private MinecraftClient Client;
        private Thread t_clientread;

        // 2019-10-26 Act1. 接続方法切替式実装に伴うグローバル変数追加
        // 既存コードでは接続ボタン押下時、ローカル変数に直接代入していたが、
        // 新しい方法「セレクトボックス選択式」の実装によりコンポーネント直接指定が不可となったため、
        // 新しい方法の値は変数をグローバル化し、接続時に渡す。（従来の値は従来通り渡す）
        // 画面ロード時およびラジオボタンの切替時、これらを初期化する。

        // グローバル変数 (1) ログインメールアドレス保持
        private String tmpLoginStr;
        // グローバル変数 (2) パスワード保持
        private String tmpPassStr;

        // 2019-10-26 Act2. 外部ファイル読込機能追加に伴うグローバル変数追加
        // 適切な2ファイルがある場合、内容を配列に保持して各コンポーネントに渡す。
        // ない場合は従来の機能（入力式）のみ有効とする。
        // なお、プログラム起動中に外部ファイルが破損または削除されても、
        // プログラムが起動している限り下記変数に保持され、初期化されないため、再接続時に落ちる事はない。

        // グローバル変数 (3) アカウント情報ファイル 保持配列
        private String[] tmpAllAccountLst;
        // グローバル変数 (4) サーバー情報ファイル 保持配列
        private String[] tmpAllServerLst;
        // グローバル変数 (5) 画面で選択したアカウント名と、3つに切り出した情報を紐付ける多次元配列
        private String[,] accountConnectionLst;

        #region Aero Glass Low-level Windows API

        [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Sequential)]
        public struct MARGINS
        {
            public int Left;
            public int Right;
            public int Top;
            public int Bottom;
        }

        [System.Runtime.InteropServices.DllImport("dwmapi.dll")]
        public static extern int DwmExtendFrameIntoClientArea(IntPtr hWnd, ref MARGINS pMargins);

        #endregion

        public Form1(string[] args)
        {
            InitializeComponent();
            if (args.Length > 0) { initClient(new MinecraftClient(args)); }
        }

        /// <summary>
        /// Define some element properties and init Aero Glass if using Vista or newer
        /// </summary>

        private void Form1_Load(object sender, EventArgs e)
        {
            box_output.ScrollBars = RichTextBoxScrollBars.None;
            // 環境によってはconsolasでの日本語が文字化けする可能性あり。MSPゴシックに変更。
            box_output.Font = new Font("MS PGothic", 8);
            box_output.BackColor = Color.White;

            if (Environment.OSVersion.Version.Major >= 6 && Environment.OSVersion.Version.Minor == 1)
            {
                this.BackColor = Color.DarkMagenta; this.TransparencyKey = Color.DarkMagenta;
                MARGINS marg = new MARGINS() { Left = -1, Right = -1, Top = -1, Bottom = -1 };
                DwmExtendFrameIntoClientArea(this.Handle, ref marg);
            }

            // Act1. 初期表示準備

            // グローバル変数初期化
            initializeTempValues();
            // 初期カーソルはラジオボタン
            radio_1.Select();
            // 初期選択は「直接入力側」（利便性考えるとあとで変えるかも）
            radio_1.Checked = true;
            // 初期状態では選択側を操作できない
            sel_Login.Enabled = false;
            // ファイルチェック前は選択式のラジオボタンを非活性とする
            radio_2.Enabled = false;

            // Act2. 外部ファイル読込機能

            // 実行ファイルパス取得
            string filePath = Application.StartupPath;
            string accountListFilePath = filePath + "\\accounts.txt";
            string serverListFilePath = filePath + "\\servers.txt";

            // ファイルが存在する場合のみ、配列に情報を格納
            if (File.Exists(accountListFilePath))
            {
                tmpAllAccountLst = initializeFileFormatCheck(accountListFilePath, "accountlist");
                if (tmpAllAccountLst != null)
                {
                    // 戻り値がnull以外の場合のみ、選択式のラジオボタンを有効化
                    radio_2.Enabled = true;
                    
                    string[] arr;
                    int len = tmpAllAccountLst.Length;
                    // 3項ずつ、ファイルの総行数で多次元配列を初期化
                    accountConnectionLst = new string[tmpAllAccountLst.Length,3];

                    // ファイルの内容（配列）をセレクトボックス要素に詰め込む
                    // なお、パスワードは内部で保持するだけで画面に表示はしない
                    for (int i=0; i<tmpAllAccountLst.Length; i++)
                    {
                        // 全アカウント情報のリストの1行をカンマで区切り、処理用の配列に格納
                        arr = tmpAllAccountLst[i].Split(',');

                        // 表示用アカウント名（エイリアス）としてセレクトボックスに詰め込む
                        sel_Login.Items.Add(arr[0]);
                        // エイリアス名・メールアドレス・パスワードを結びつける内部配列にも格納しておく
                        accountConnectionLst[i, 0] = arr[0];
                        accountConnectionLst[i, 1] = arr[1];
                        accountConnectionLst[i, 2] = arr[2];
                    }
                }
            }
            if (File.Exists(serverListFilePath))
            {
                tmpAllServerLst = initializeFileFormatCheck(serverListFilePath, "serverlist");
                if (tmpAllServerLst != null)
                {
                    // 戻り値がnull以外の場合のみ
                    // 選択式のサーバーセレクトボックスを有効化、
                    // 入力式サーバーテキストボックスを無効化
                    box_ip.Enabled = false;
                    box_ip.Visible = false;
                    sel_ip.Enabled = true;
                    sel_ip.Visible = true;

                    // ファイルの内容（配列）をセレクトボックス要素に詰め込む
                    for (int i=0; i < tmpAllServerLst.Length; i++)
                    {
                        sel_ip.Items.Add(tmpAllServerLst[i]);
                    }
                } else
                {
                    // 戻り値がnullの場合は選択式ラジオボタンを無効化
                    radio_2.Enabled = false;
                    // サーバーセレクトボックスは初期値非表示なので操作不要
                }
            }
        }

        /// <summary>
        /// Launch the Minecraft Client by clicking the "Go!" button.
        /// If a client is already running, it will be closed.
        /// </summary>

        private void btn_connect_Click(object sender, EventArgs e)
        {
            if (Client != null)
            {
                Client.Close();
                t_clientread.Abort();
                box_output.Text = "";
            }

            // 2019-10-26 機能追加に伴い、ラジオボタンどちらの値も変数に格納するよう分岐を実装
            string username = "";
            string password = "";
            string serverip = "";

            if (radio_1.Checked)
            {
                username = box_Login.Text;
                password = box_password.Text;
                serverip = box_ip.Text;

            } else if (radio_2.Checked)
            {
                if (tmpLoginStr == "" || tmpPassStr == "")
                {
                    MessageBox.Show("アカウントを選択してください");
                }
                username = tmpLoginStr;
                password = tmpPassStr;
            }

            if (box_ip.Visible && box_ip.Text != "")
            {
                serverip = box_ip.Text;
            }
            else if (sel_ip.Visible && sel_ip.Text != "")
            {
                serverip = sel_ip.Text;
            }

            if (password == "") { password = "-"; }
            if (username != "" && serverip != "")
            {
                initClient(new MinecraftClient(username, password, serverip));
            }
        }

        /// <summary>
        /// Handle a new Minecraft Client
        /// </summary>
        /// <param name="client">Client to handle</param>

        private void initClient(MinecraftClient client)
        {
            Client = client;
            t_clientread = new Thread(new ThreadStart(t_clientread_loop));
            t_clientread.Start();
        }

        /// <summary>
        /// Thread reading output from the Minecraft Client
        /// </summary>

        private void t_clientread_loop()
        {
            while (true && !Client.Disconnected)
            {
                printstring(Client.ReadLine());
            }
        }

        /// <summary>
        /// Print a Minecraft-Formatted string to the console area
        /// </summary>
        /// <param name="str">String to print</param>

        private void printstring(string str)
        {
            if (!String.IsNullOrEmpty(str))
            {
                Color color = Color.Black;
                FontStyle style = FontStyle.Regular;
                string[] subs = str.Split('§');
                if (subs[0].Length > 0) { AppendTextBox(box_output, subs[0], Color.Black, FontStyle.Regular); }
                for (int i = 1; i < subs.Length; i++)
                {
                    if (subs[i].Length > 0)
                    {
                        if (subs[i].Length > 1)
                        {
                            switch (subs[i][0])
                            {
                                //Font colors
                                case '0': color = Color.Black; break;
                                case '1': color = Color.DarkBlue; break;
                                case '2': color = Color.DarkGreen; break;
                                case '3': color = Color.DarkCyan; break;
                                case '4': color = Color.DarkRed; break;
                                case '5': color = Color.DarkMagenta; break;
                                case '6': color = Color.DarkGoldenrod; break;
                                case '7': color = Color.DimGray; break;
                                case '8': color = Color.Gray; break;
                                case '9': color = Color.Blue; break;
                                case 'a': color = Color.Green; break;
                                case 'b': color = Color.CornflowerBlue; break;
                                case 'c': color = Color.Red; break;
                                case 'd': color = Color.Magenta; break;
                                case 'e': color = Color.Goldenrod; break;

                                //White on white = invisible so use gray instead
                                case 'f': color = Color.DimGray; break;

                                //Font styles. Can use several styles eg Bold + Underline
                                case 'l': style = style | FontStyle.Bold; break;
                                case 'm': style = style | FontStyle.Strikeout; break;
                                case 'n': style = style | FontStyle.Underline; break;
                                case 'o': style = style | FontStyle.Italic; break;

                                //Reset font color & style
                                case 'r': color = Color.Black; style = FontStyle.Regular; break;
                            }

                            AppendTextBox(box_output, subs[i].Substring(1, subs[i].Length - 1), color, style);
                        }
                    }
                }
                AppendTextBox(box_output, "\n", Color.Black, FontStyle.Regular);
            }
            Console.ForegroundColor = ConsoleColor.Gray;
        }

        /// <summary>
        /// Append text to a RichTextBox with font customization
        /// </summary>
        /// <param name="box">Target RichTextBox</param>
        /// <param name="text">Text to add</param>
        /// <param name="color">Color of the text</param>
        /// <param name="style">Font style of the text</param>

        private void AppendTextBox(RichTextBox box, string text, Color color, FontStyle style)
        {
            if (InvokeRequired)
            {
                this.Invoke(new Action<RichTextBox, string, Color, FontStyle>(AppendTextBox), new object[] { box, text, color, style });
            }
            else
            {
                box.SelectionStart = box.TextLength;
                box.SelectionLength = 0;
                box.SelectionColor = color;
                box.SelectionFont = new Font(box.Font, style);
                box.AppendText(text);
                box.SelectionColor = box.ForeColor;
                box.SelectionStart = box.Text.Length;
                box.ScrollToCaret();
            }
        }

        /// <summary>
        /// Properly disconnect the client when clicking the [X] close button
        /// </summary>

        protected void onClose(object sender, EventArgs e)
        {
            if (t_clientread != null) { t_clientread.Abort(); }
            if (Client != null) { new Thread(new ThreadStart(Client.Close)).Start(); }
        }

        /// <summary>
        /// Allows an Enter keypress in "Login", "Password" or "Server IP" box to be considered as a click on the "Go!" button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>

        public void loginBox_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                btn_connect_Click(sender, e);
                e.Handled = true;
            }
        }

        /// <summary>
        /// Handle special functions in the input box : send with Enter key, command history and tab-complete
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>

        public void inputBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                btn_send_Click(sender, e);
                e.Handled = true;
            }
            else if (e.KeyCode == Keys.Down)
            {
                if (previous.Count > 0)
                {
                    box_input.Text = previous.First.Value;
                    previous.AddLast(box_input.Text);
                    previous.RemoveFirst();
                    box_input.Select(box_input.Text.Length, 0);
                }
                e.Handled = true;
            }
            else if (e.KeyCode == Keys.Up)
            {
                if (previous.Count > 0)
                {
                    box_input.Text = previous.Last.Value;
                    previous.AddFirst(box_input.Text);
                    previous.RemoveLast();
                    box_input.Select(box_input.Text.Length, 0);
                }
                e.Handled = true;
            }
            else if (e.KeyCode == Keys.Tab)
            {
                if (box_input.SelectionStart > 0)
                {
                    string behind_cursor = box_input.Text.Substring(0, box_input.SelectionStart);
                    string after_cursor = box_input.Text.Substring(box_input.SelectionStart);
                    string[] behind_temp = behind_cursor.Split(' ');
                    string autocomplete = Client.tabAutoComplete(behind_temp[behind_temp.Length - 1]);
                    if (!String.IsNullOrEmpty(autocomplete))
                    {
                        behind_temp[behind_temp.Length - 1] = autocomplete;
                        behind_cursor = String.Join(" ", behind_temp);
                        box_input.Text = behind_cursor + after_cursor;
                        box_input.SelectionStart = behind_cursor.Length;
                    }
                }
                e.SuppressKeyPress = true;
                e.Handled = true;
            }
        }

        /// <summary>
        /// Send the input in the input box, if any, by pressing the "Send" button.
        /// Handle "/quit" command to properly disconnect and close the GUI.
        /// </summary>

        private void btn_send_Click(object sender, EventArgs e)
        {
            if (Client != null)
            {
                if (box_input.Text.Trim().ToLower() == "/quit")
                {
                    Close();
                }
                else
                {
                    Client.SendText(box_input.Text);
                    previous.AddLast(box_input.Text);
                    box_input.Text = "";
                }
            }
        }

        /// <summary>
        /// Draw text on glass pane without ClearType, only black pixels
        /// *unused*
        /// </summary>

        protected override void OnPaint(PaintEventArgs e)
        {
            //e.Graphics.TextRenderingHint = System.Drawing.Text.TextRenderingHint.SingleBitPerPixelGridFit;
            //e.Graphics.DrawString("Login Details", this.Font, Brushes.Black, 20, 11);
            //e.Graphics.DrawString("Username:", this.Font, Brushes.Black, 20, 31);
            //e.Graphics.DrawString("Password:", this.Font, Brushes.Black, 191, 31);
            //e.Graphics.DrawString("Server IP:", this.Font, Brushes.Black, 355, 31);
        }

        /// <summary>
        /// Open a link located in the console window
        /// </summary>

        private void LinkClicked(object sender, LinkClickedEventArgs e)
        {
            try { System.Diagnostics.Process.Start(e.LinkText); }
            catch (Exception ex) { MessageBox.Show("An error occured while opening the link :\n" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error); }
        }

        // ------------------------------------------------ //
        //              ここから独自コード                  //
        // ------------------------------------------------ //

        /// <summary>
        /// ラジオボタン1 : 直接入力する（従来の機能）
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void radio_1_CheckedChanged(object sender, EventArgs e)
        {
            // 直接入力側の有効化（アカウント名とパス両方）
            box_Login.Enabled = true;
            box_password.Enabled = true;

            // 選択側の無効化
            sel_Login.Enabled = false;
        }

        /// <summary>
        /// ラジオボタン2 : 登録済みアカウントから選択する（新機能）
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void radio_2_CheckedChanged(object sender, EventArgs e)
        {
            // 選択側の有効化（アカウント名のみ）
            sel_Login.Enabled = true;

            // 直接入力側の無効化と初期化
            box_Login.Text = "";
            box_Login.Enabled = false;
            box_password.Text = "";
            box_password.Enabled = false;
        }

        /// <summary>
        /// 登録済みアカウント > セレクトボックス値変更時のメソッド
        /// 
        /// 　※一般公開する時はファイルからの読込などを考える‥
        /// 　
        /// 例：起動時にファイル有無を確認、ファイルがない場合は選択側のラジオボタン自体を無効化
        /// 　　ファイルがある場合は起動時に書式チェック、不正書式はダイアログ表示、有効書式のみセレクトボックスに反映
        /// 　　
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void sel_Login_SelectedValueChanged(object sender, EventArgs e)
        {
            string loginText = sel_Login.Text;

            if (loginText != "")
            {
                // 画面で選択したアカウント名（エイリアス名）から、メールアドレスとパスワードを、
                // 初期表示時にファイルから取得し内部保持用に格納した多次元配列 accountConnectionLst から取得
                // 各要素[x,0]番目のエイリアス名をfor文で全行数分回して、該当行のメールアドレスとパスワードを内部保持変数に代入
                for (int i=0; i<tmpAllAccountLst.Length;i++)
                {
                    if (accountConnectionLst[i,0] == loginText)
                    {
                        // 内部保持値 メールアドレス
                        tmpLoginStr = accountConnectionLst[i, 1];
                        // 内部保持値 パスワード
                        tmpPassStr = accountConnectionLst[i, 2];
                    }
                }
                // パスワードの表示を一旦初期化
                sel_password.Text = "";
                // また、パスワードの文字数に応じたアスタリスクを画面のsel_password.Textに表示させる
                for (int i = 0; i < tmpPassStr.Length; i++)
                {
                    sel_password.Text += "*";
                }

            } else
            {
                // ラジオボタン切替時など、そもそもエイリアス名が空欄の場合は、
                // メールアドレス、パスワードを初期化。パスワードは初期表示文言を表示
                initializeTempValues();
                sel_password.Text = "パスワードは自動入力されます";
            }
        }

        /// <summary>
        /// 独自メソッド : グローバル変数を初期化する際に呼出
        /// </summary>
        private void initializeTempValues()
        {
            tmpLoginStr = "";
            tmpPassStr = "";
        }

        /// <summary>
        /// 独自メソッド : 起動時設定ファイルチェック
        /// 　アカウント情報ファイル(accounts.txt)およびサーバー情報ファイル(servers.txt)が
        /// 　2ファイル、実行ファイルと同ディレクトリにある場合のみ、本メソッドを呼び出し、形式チェックを行う。
        /// 　内容が適切な場合のみ、各コンポーネントに値を反映するが、
        /// 　不正な形式の場合は読み込まず、起動時にエラーダイアログを表示し「選択式」のラジオボタン以下を無効化する。
        /// 　 ※ つまり、適切な2ファイルを配置しないと「選択式」は選べない。
        ///   
        /// 　サーバーアドレス入力欄は、適切なファイルがある場合はセレクトボックスで表示、
        ///   ファイルがない場合は自由入力のテキストボックスで表示する。（接続ボタン押下時に片方を代入させる）
        /// </summary>
        private string[] initializeFileFormatCheck(string filepath, string filetype)
        {
            string[] returnVal = new string[1];
            StreamReader sr = new StreamReader(filepath, System.Text.Encoding.GetEncoding("Shift_JIS"));
            int i = 0;
            string[] arr;

            while (sr.EndOfStream == false)
            {
                
                string line = sr.ReadLine();

                arr = line.Split(',');

                if (filetype.Equals("accountllist"))
                {
                    // 内容の切り出しと妥当性チェックまでは行わない。
                    // とりあえず戻り値の配列に取得行の内容を詰め込む。
                    // アカウント情報は、カンマ区切りではない場合、1行3項目以外の場合にnullを返す。
                    if (arr.Length != 3 || !System.Text.RegularExpressions.Regex.IsMatch(line, @"^([^,]+,)+[^,]+$"))
                    {
                        // アカウント情報の項目数エラー
                        MessageBox.Show("アカウント情報ファイルの書式が正しくありません。[" + i + 1 + "行目]\r"
                                + "「表示名」「メールアドレス」「パスワード」の3項をカンマ区切りで1行に記載してください。",
                            "アカウント情報 ファイルフォーマットエラー",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Warning);
                        return null;
                    }

                } else if (filetype.Equals("serverlist") && (arr.Length != 1))
                {
                    // サーバー情報の項目数エラー
                    MessageBox.Show("サーバー情報ファイルの書式が正しくありません。[" + i + 1 + "行目]\r"
                                + "　・各行にサーバーアドレスのみ記載（例 localhost）\r"
                                + "　・ポート番号は省略してください。25565以外の指定は出来ません（仕様上の問題）\r\r"
                                + "お手数ですが、サーバーアドレスは直接入力してください。",
                        "サーバー情報 ファイル形式エラー",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);
                    return null;
                }
                // 消去法で正常系処理はこちらへ
                // 初期配列長1だが、ファイルの行数が未知数のため、動的可変長配列である必要がある。
                // 2行以上の場合はそのつど戻り値の配列長を継ぎ足して値を格納する。
                if (i < returnVal.Length)
                {
                    returnVal[i] = line;
                } else
                {
                    Array.Resize(ref returnVal, returnVal.Length + 1);
                    returnVal[i] = line;
                }
                i++;
                }
            // エラーに該当しなければ配列を戻す
            return returnVal;
        }
    }
}

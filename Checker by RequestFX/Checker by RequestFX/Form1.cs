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
            if (Program.rainbow == false)
            {
                Rainbowcycle.Start();
                Program.rainbow = true;
            }
            else if (Program.rainbow == true)
            {
                Rainbowcycle.Stop();
                Program.rainbow = false;
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
                if (!Directory.Exists(Program.Date))
                {
                    Directory.CreateDirectory(Program.Date);
                }
                if (!File.Exists(Program.Date + "\\Good.txt"))
                {
                    using (StreamWriter sw = File.CreateText(Program.Date + "\\Good.txt"))
                        sw.WriteLine("Coded By RequestFX :(\n");
                }
                for (int i = 0; i <= Program.thread; ++i)
                    new Thread(new ThreadStart(Program.Check)).Start();
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
            Environment.Exit(Environment.ExitCode);
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
            Error.Text = Program.error.ToString();
            Checked.Text = Program.check.ToString() + " / " + Program.ccount.ToString();
        }

        private void NordVPN_Click(object sender, EventArgs e)
        {
            //new Form1().AccountList.Rows.Add(
            //        new object[]
            //        {
            //            Program.AccountDetails[0],
            //            Program.AccountDetails[1],
            //        });
        }

        private void Exit_Click(object sender, EventArgs e)
        {
            Environment.Exit(Environment.ExitCode);
        }
    }
}
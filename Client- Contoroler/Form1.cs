using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing.Drawing2D;
using System.Runtime.InteropServices;
using System.Diagnostics;
using System.ServiceModel;
using Client.ChatService;
using System.Drawing.Imaging;
using Microsoft.Win32;
using System.ComponentModel.Design;
using System.Net.Mail;

using System.IO;

namespace Client
{
    public partial class frmClient : Form
    {
        bool login;
        int t;
        ReceiveClient rc = null;
        string myName;
       
        public frmClient()
        {
            InitializeComponent();
            this.FormClosing+=new FormClosingEventHandler(frmClient_FormClosing);
            this.txtSend.KeyPress += new KeyPressEventHandler(txtSend_KeyPress);
            
        }

        void txtSend_KeyPress(object sender, KeyPressEventArgs e)
        {
            int keyValue = (int)e.KeyChar;

            if (keyValue == 13)
                SendMessage();
            
        }

        private void frmClient_FormClosing(object sender, EventArgs e)
        {
            txtSend.Text = "[" + txtUserName.Text + "]" + "has been left";
            

            if(login==true)
            rc.Stop(myName);
        }
        private void Main_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (MessageBox.Show("Are you realy want to exit?", "question?", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.Yes)
                e.Cancel = false;
            else e.Cancel = true;
        }

        private void frmClient_Load(object sender, EventArgs e)
        {
            timer4.Start();
            timer3.Start();
            timer2.Start();
            // this.Hide();
           // MessageBox.Show("              !!خوش آمدید            ", "Control App <By ma.orojloo>", MessageBoxButtons.OK, MessageBoxIcon.Information);

           // listBox1.Visible = false;
            Random R_n = new Random();
            int random = R_n.Next(100,99999);


            txtUserName.Text += Environment.UserName + "_" + Environment.MachineName+"."+random;

           // txtUserName.Enabled = false;
           // txtMsgs.Enabled = false;
            txtSend.Enabled = false;
            btnSend.Enabled = false;
        }

        void rc_ReceiveMsg(string sender, string msg)
        {
            if (msg.Length > 0)
                txtMsgs.Text +=Environment.NewLine + sender +">"+ msg;
          

        }
        //-------------
        //----------------
        // قفل کردن کامپیوتر از طریق کدنویسی 
        [DllImport("user32.dll")]
        public static extern void LockWorkStation();
        ///-
        //خالی کردن سطل بازیافت ویندوز
        enum RecycleFlags : uint
        {
            SHERB_NOCONFIRMATION = 0x00000001,
            SHERB_NOPROGRESSUI = 0x00000001,
            SHERB_NOSOUND = 0x00000004
        }
        [DllImport("Shell32.dll", CharSet = CharSet.Unicode)]
        static extern uint SHEmptyRecycleBin(IntPtr hwnd, string pszRootPath,
        RecycleFlags dwFlags);
        ///-
        // ترسیم و نوشتن مستقیم بر دسکتاپ ویندوز 
        [DllImport("user32")]
        internal static extern IntPtr GetDC(IntPtr hwnd);
        [DllImport("User32.dll")]
        internal static extern void ReleaseDC(IntPtr dc);
        public void PaintRectangleToScreen()
        {
            IntPtr deskDC = GetDC(IntPtr.Zero);
            Graphics g = Graphics.FromHdc(deskDC);
            Font font = new Font("Arial Black", 100);
            DrawStringOnCenter(g, "BY MA.OROJLOO", font, new Point(0, -25));
            font = new Font("Arial Black", 50);
            DrawStringOnCenter(g, "J.D.E.V.S", font, new Point(-15, 110));
            Rectangle rect = new Rectangle(25, 50,
                Screen.PrimaryScreen.Bounds.Width - 50,
                Screen.PrimaryScreen.Bounds.Height - 125);
            g.FillRectangle(new SolidBrush(Color.FromArgb(100, Color.DodgerBlue))
                , rect);
            g.DrawRectangle(new Pen(Color.DeepPink, 3), rect);
            g.Dispose();
           // ReleaseDC(deskDC);
        }
        private void DrawStringOnCenter(Graphics g, string str, Font font, Point offset)
        {
            SizeF size = g.MeasureString(str, font);
            g.DrawString(str, font, Brushes.White,
                      new PointF(
                                  (Screen.PrimaryScreen.Bounds.Width - size.Width) / 2 + offset.X,
                                  (Screen.PrimaryScreen.Bounds.Height - size.Height) / 2 + offset.Y)
                                 );
        }
        //-------------------
        //----------------------
        //-------------------------
       

        //----

        void rc_NewNames(object sender, List<string> names)
        {
            lstClients.Items.Clear();
            foreach (string name in names)
            {
                if (name!=myName)
                    lstClients.Items.Add(name);
            }
        }

        private void btnSend_Click(object sender, EventArgs e)
        {
            
            SendMessage();
        }

        private void SendMessage()
        {
            if (lstClients.Items.Count != 0)
            {
                txtMsgs.Text += Environment.NewLine + myName + ">" + txtSend.Text;
                if (lstClients.SelectedItems.Count == 0)
                    rc.SendMessage(txtSend.Text, myName, lstClients.Items[0].ToString());
                else
                    if (!string.IsNullOrEmpty(lstClients.SelectedItem.ToString()))
                        rc.SendMessage(txtSend.Text, myName, lstClients.SelectedItem.ToString());

                txtSend.Clear();
            }
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
           // lstClients.Items.Add("You");

            if (txtUserName.Text.Length > 0)
            {
                login = true;
                timer5.Start();
                btnLogin.Enabled = false;
                txtMsgs.Enabled = true;
                txtSend.Enabled = true;
                btnSend.Enabled = true;

                myName = txtUserName.Text.Trim();

                rc = new ReceiveClient();
                rc.Start(rc, myName);

                rc.NewNames += new GotNames(rc_NewNames);
                rc.ReceiveMsg += new ReceviedMessage(rc_ReceiveMsg);
            }
            else
            {
                txtMsgs.Enabled = false;
                txtSend.Enabled = false;
                btnSend.Enabled = false;
            }


            timer1.Start();

            
        }
        private void login_()
        {
            txtSend.Text = "joined";
            SendMessage();

        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            t++;
            if (t == 3)
            {
                login_();
                txtSend.Text = string.Empty;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {

            txtMsgs.Text = string.Empty;
        }

        private void label3_Click(object sender, System.EventArgs e)
        {
           //// uint result = SHEmptyRecycleBin(IntPtr.Zero, null, 0);
        }

        private void label3_Click_1(object sender, System.EventArgs e)
        {
          
        }

        private void label3_Click_2(object sender, System.EventArgs e)
        {
           
        }

        private void linkLabel1_LinkClicked(object sender, System.Windows.Forms.LinkLabelLinkClickedEventArgs e)
        {
          
        }

        private void txtMsgs_TextChanged(object sender, System.EventArgs e)
        {
            
        }
        int time;
        private void timer2_Tick(object sender, System.EventArgs e)
        {
            time++;
           
            if(time==5)
            {
                label3.Text = lstClients.Items.Count.ToString();
                time = 0;
            }
        }
        int ddd;
        private void timer3_Tick(object sender, System.EventArgs e)
        {


            int tg;
        }
        private void timer4_Tick(object sender, System.EventArgs e)
        {
           
        }

        private void txtUserName_TextChanged(object sender, System.EventArgs e)
        {

        }

        private void label5_Click(object sender, System.EventArgs e)
        {
         
        }

        private void lstClients_SelectedIndexChanged(object sender, System.EventArgs e)
        {

        }

        private void button2_Click(object sender, System.EventArgs e)
        {
            if (MessageBox.Show("Are you realy want to send this massage?", "question?", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) == DialogResult.Yes)
            {
                txtSend.Text = textBox1.Text;
                SendMessage();
                textBox1.Text = string.Empty;
            }
            else
            {
                textBox1.Text = string.Empty;
            }
        }

        private void label7_Click(object sender, System.EventArgs e)
        {
            txtSend.Text = "OFF";
        }

        private void label8_Click(object sender, System.EventArgs e)
        {
            txtSend.Text = label8.Text;
        }

        private void label9_Click(object sender, System.EventArgs e)
        {
            txtSend.Text = label9.Text;
        }

        private void label10_Click(object sender, System.EventArgs e)
        {
            txtSend.Text = label10.Text;
        }

        private void label11_Click(object sender, System.EventArgs e)
        {
            txtSend.Text = label11.Text;
        }

        private void label12_Click(object sender, System.EventArgs e)
        {
            txtSend.Text = label12.Text;
        }

        private void label13_Click(object sender, System.EventArgs e)
        {
            txtSend.Text = label13.Text;
        }

        private void label14_Click(object sender, System.EventArgs e)
        {
            txtSend.Text = label14.Text;
        }

        private void label15_Click(object sender, System.EventArgs e)
        {
            txtSend.Text = label15.Text;
        }

        private void label16_Click(object sender, System.EventArgs e)
        {
            txtSend.Text = label16.Text;
        }

        private void label17_Click(object sender, System.EventArgs e)
        {
            txtSend.Text = label17.Text;
        }

        private void label18_Click(object sender, System.EventArgs e)
        {
            txtSend.Text = label18.Text;
        }

        private void label19_Click(object sender, System.EventArgs e)
        {
            txtSend.Text = label19.Text;
        }

        private void label20_Click(object sender, System.EventArgs e)
        {
            txtSend.Text = label20.Text;
        }

        private void label21_Click(object sender, System.EventArgs e)
        {
            txtSend.Text = label21.Text;
        }

        private void label22_Click(object sender, System.EventArgs e)
        {
            txtSend.Text = label22.Text;
        }

        private void label23_Click(object sender, System.EventArgs e)
        {
            txtSend.Text = label23.Text;
        }

        private void label24_Click(object sender, System.EventArgs e)
        {
            txtSend.Text = label24.Text;
        }

        private void label25_Click(object sender, System.EventArgs e)
        {
            txtSend.Text = label25.Text;
        }

        private void label27_Click(object sender, System.EventArgs e)
        {
            txtSend.Text = label27.Text;
        }

        private void label26_Click(object sender, System.EventArgs e)
        {
            txtSend.Text = label26.Text;
        }

        private void label28_Click(object sender, System.EventArgs e)
        {
            txtSend.Text = label28.Text;
        }

        private void label29_Click(object sender, System.EventArgs e)
        {
            txtSend.Text = label29.Text;
        }

        private void label31_Click(object sender, System.EventArgs e)
        {
            txtSend.Text = label31.Text;
        }

        private void label30_Click(object sender, System.EventArgs e)
        {
            txtSend.Text = label30.Text;
        }

        private void label32_Click(object sender, System.EventArgs e)
        {
            txtSend.Text = label32.Text;
        }

        private void label34_Click(object sender, System.EventArgs e)
        {
            txtSend.Text = label34.Text;
        }

        private void label33_Click(object sender, System.EventArgs e)
        {
            txtSend.Text = label33.Text;
        }

        private void label35_Click(object sender, System.EventArgs e)
        {
            txtSend.Text = label35.Text;
        }

        private void label38_Click(object sender, System.EventArgs e)
        {
            txtSend.Text = label38.Text;
        }

        private void label37_Click(object sender, System.EventArgs e)
        {
            txtSend.Text = label37.Text;
        }

        private void label39_Click(object sender, System.EventArgs e)
        {
            txtSend.Text = label39.Text;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (login == true)
                rc.Stop(myName);
        }

        private void toolStripStatusLabel1_Click(object sender, EventArgs e)
        {
           
        }
            int s_s;
            int s_m;
            int s_h;
        private void timer4_Tick_1(object sender, EventArgs e)
        {
            s_s++;
            if (s_s==60)
            {
                s_m++;
                s_s = 0;
            }
            if(s_m==60)
            {
                s_m = 0;
                s_h++;
               
            }
            if (s_h == 24)
                Application.Restart();



            label42.Text = s_h+ ":" + s_m + ":" +  s_s;

        }
        int l_s;
        int l_m;
        int l_h;
        private void timer5_Tick(object sender, EventArgs e)
        {
            l_s++;
            if (l_s == 60)
            {
                l_m++;
                l_s = 0;
            }
            if (l_m == 60)
            {
                l_m = 0;
                l_h++;

            }
            label43.Text = l_h + ":" + l_m + ":" + l_s;
            

        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AboutBox1 a = new AboutBox1();
            a.Show();
        }

        private void txtMsgs_TextChanged_1(object sender, EventArgs e)
        {
            

        }

    }
   
}

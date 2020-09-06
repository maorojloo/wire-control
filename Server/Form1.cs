using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.ServiceModel;

namespace Server
{
    public partial class Form1 : Form
    {
        bool blnStartStop;
        ServiceHost host;
        ChatService cs = new ChatService();
        

        public Form1()
        {
            InitializeComponent();
            blnStartStop = true;
        }

        void cs_ChatListOfNames(List<string> names, object sender)
        {
            lstUser.Items.Clear();
            foreach (string s in names)
            {
                lstUser.Items.Add(s);
            }
        }

        private void btnStartStop_Click()
        {
            
            
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            Environment.Exit(0);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            if (blnStartStop)
            {
                host = new ServiceHost(typeof(Server.ChatService));
                host.Open();
                // btnStartStop.Text = "Stop";
                ChatService.ChatListOfNames += new ListOfNames(cs_ChatListOfNames);
            }
            else
            {
                cs.Close();
                host.Close();
                // btnStartStop.Text = "Start";
            }

            blnStartStop = !blnStartStop;


            timer1.Start();
        }
        private void Main_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (MessageBox.Show("Are you realy want to exit?", "question?", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.Yes)
                e.Cancel = false;
            else e.Cancel = true;
        }
        int l_s;
        int l_m;
        int l_h;
        private void timer1_Tick(object sender, EventArgs e)
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
            label1.Text = l_h + ":" + l_m + ":" + l_s;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Application.Restart();
        }
    }
}

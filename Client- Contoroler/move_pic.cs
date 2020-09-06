using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Client
{
    public partial class move_pic : Form
    {
        string s;
        string path;
        public Boolean top = true;
        public Boolean left = true;
        public int Speed = 20;
        public move_pic()
        {
            InitializeComponent();
        }
        private void Form1_Deactivate(object sender, EventArgs e)
        {
           this.Hide();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
        private void Form1_Activated(object sender, EventArgs e)
        {
            timer1.Enabled = true;
        }
        private void move_pic_Load(object sender, EventArgs e)
        {
            timer1.Start();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            try
            {
                if (top == true) move_pic.ActiveForm.Top += Speed; else move_pic.ActiveForm.Top -= Speed;
                if (left == true) move_pic.ActiveForm.Left += Speed; else move_pic.ActiveForm.Left -= Speed;

                if (move_pic.ActiveForm.Top >= Screen.PrimaryScreen.Bounds.Height - 40) top = false;
                if (move_pic.ActiveForm.Left >= Screen.PrimaryScreen.Bounds.Width - 37) left = false;
                if (move_pic.ActiveForm.Top < -5) top = true;
                if (move_pic.ActiveForm.Left < -5) left = true;
            }
            catch
            {
                timer1.Enabled = false;
            }
        }
    }
}

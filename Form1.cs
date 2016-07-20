using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading.Tasks;

using System.Runtime.InteropServices;

namespace WindowsFormsApplication1
{
    public partial class Form1 : Form
    {
        [DllImport("Gdi32.dll", EntryPoint = "CreateRoundRectRgn")]
        private static extern IntPtr CreateRoundRectRgn
        (
            int nLeftRect, // x-coordinate of upper-left corner
            int nTopRect, // y-coordinate of upper-left corner
            int nRightRect, // x-coordinate of lower-right corner
            int nBottomRect, // y-coordinate of lower-right corner
            int nWidthEllipse, // height of ellipse
            int nHeightEllipse // width of ellipse
        );

        public const int WM_NCLBUTTONDOWN = 0x00A1;
        public const int HT_CAPTION = 0x2;

        [DllImportAttribute("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);
        [DllImportAttribute("user32.dll")]
        public static extern bool ReleaseCapture();


        // Hotkeys
        [DllImport("user32.dll")]
        private static extern bool RegisterHotKey(IntPtr hWnd, int id, int fsModifiers, int vlc);

        [DllImport("user32.dll")]
        private static extern bool UnregisterHotKey(IntPtr hWnd, int id);

        const int MOD_ALT = 0x1;
        const int MOD_CONTROL = 0x2;
        const int MOD_SHIFT = 0x4;
        const int MOD_WIN = 0x8;
        const int WM_HOTKEY = 0x312;

        protected override void WndProc(ref Message m)
        {
            base.WndProc(ref m);

            if (m.Msg == WM_HOTKEY)
            {
                //WindowState = FormWindowState.Normal;
                Activate();
                btnOk.PerformClick();
            }
        }
        //Ya. Seguimo       

        public static Bitmap BM = new Bitmap(Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height); // variable a utilizar para el capture

        public Form1()
        {
            InitializeComponent();
            this.Text = "WinCapture v1.0.0";  // Titulo 
            Region = System.Drawing.Region.FromHrgn(CreateRoundRectRgn(0, 0, Width, Height, 900, 500));

        }

        private void Form1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            System.Media.SoundPlayer player = new System.Media.SoundPlayer();
            player.SoundLocation = "Shutter.wav";
            player.Play();

            Opacity = 0;
            Graphics GH = Graphics.FromImage(BM as Image);
            GH.CopyFromScreen(0, 0, 0, 0, BM.Size);


            SaveFileDialog SFD = new SaveFileDialog();
            SFD.Filter = "PNG |*.png|JPEG |*.jpg;*jpeg;*.jpe;*.jfif";
            SFD.FileName = "";
            if (SFD.ShowDialog() == DialogResult.OK)
            {
                Form1.BM.Save(SFD.FileName);
            }

            Opacity = 100;
        }

        private void btnCerrar_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void label1_Click(object sender, EventArgs e)
        {
            System.Media.SoundPlayer player = new System.Media.SoundPlayer();
            player.SoundLocation = "min.wav";
            player.Play();

            this.WindowState = FormWindowState.Minimized;
        }

        private void btnCerrar_MouseHover(object sender, EventArgs e)
        {
            btnCerrar.ForeColor = Color.Purple;
        }

        private void label1_MouseHover(object sender, EventArgs e)
        {
            label1.ForeColor = Color.MediumPurple;
        }

        private void label1_MouseLeave(object sender, EventArgs e)
        {
            label1.ForeColor = Color.Red;
        }

        private void btnCerrar_MouseHover_1(object sender, EventArgs e)
        {
            btnCerrar.ForeColor = Color.MediumPurple;
        }

        private void btnCerrar_MouseLeave(object sender, EventArgs e)
        {
            btnCerrar.ForeColor = Color.Red;
        }

        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                ReleaseCapture();
                SendMessage(Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
            }
        }

        private void Form1_SizeChanged(object sender, EventArgs e)
        {
            if (this.WindowState == FormWindowState.Normal)
            {
                System.Media.SoundPlayer player = new System.Media.SoundPlayer();
                player.SoundLocation = "max.wav";
                player.Play();

                this.ShowInTaskbar = true;
                btnOk.Focus();
            }
        }


        private void Form1_Click_1(object sender, EventArgs e)
        {
            System.Media.SoundPlayer player = new System.Media.SoundPlayer();
            player.SoundLocation = "pre.wav";
            player.Play();
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            System.Media.SoundPlayer player = new System.Media.SoundPlayer();
            player.SoundLocation = "Shutter.wav";
            player.Play();

            Opacity = 0;
            Graphics GH = Graphics.FromImage(BM as Image);
            GH.CopyFromScreen(0, 0, 0, 0, BM.Size);


            SaveFileDialog SFD = new SaveFileDialog();
            SFD.Filter = "PNG |*.png|JPEG |*.jpg;*jpeg;*.jpe;*.jfif";
            SFD.FileName = "";
            if (SFD.ShowDialog() == DialogResult.OK)
            {
                Form1.BM.Save(SFD.FileName);
            }

            Opacity = 100;
        }

        private void label2_Click(object sender, EventArgs e)
        {
            FrmInfo abrir = new FrmInfo();
            abrir.ShowDialog();
        }

        private void Form1_Shown(object sender, EventArgs e)
        {

            int duration = 100;//in milliseconds
            int steps = 10;
            Timer timer = new Timer();
            timer.Interval = duration / steps;

            int currentStep = 0;
            timer.Tick += (arg1, arg2) =>
            {
                Opacity = ((double)currentStep) / steps;
                currentStep++;

                if (currentStep >= steps)
                {
                    timer.Stop();
                    timer.Dispose();
                }
            };

            timer.Start();
        }

        private void button1_Enter(object sender, EventArgs e)
        {
            btnOk.Focus();
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Alt && e.KeyCode == Keys.P)
            {
                btnOk.PerformClick();
            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            UnregisterHotKey((IntPtr)Handle, 1).ToString();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            RegisterHotKey((IntPtr)Handle, 1, MOD_ALT, (int)Keys.P).ToString();
        }

        }
}

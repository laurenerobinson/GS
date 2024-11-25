using System;
using System.Drawing;
using System.Windows.Forms;

namespace GstartApp
{
    public partial class SplashScreen : Form
    {
        private Timer fadeTimer;
        private float opacity = 0;

        public SplashScreen()
        {
            InitializeComponent();
            InitializeCustomComponents();
        }

        private void InitializeCustomComponents()
        {
            // Form settings
            this.BackColor = Color.FromArgb(18, 18, 18);
            this.TransparencyKey = Color.FromArgb(18, 18, 18);
            this.Opacity = 0;
            this.Size = new Size(800, 600);
            this.StartPosition = FormStartPosition.CenterScreen;

            // Logo PictureBox
            PictureBox logoPictureBox = new PictureBox
            {
                Size = new Size(600, 400),
                SizeMode = PictureBoxSizeMode.Zoom,
                BackColor = Color.Transparent,
                Dock = DockStyle.None
            };

            // Center the PictureBox
            logoPictureBox.Location = new Point(
                (this.ClientSize.Width - logoPictureBox.Width) / 2,
                (this.ClientSize.Height - logoPictureBox.Height) / 2
            );

            try
            {
                logoPictureBox.Image = Image.FromFile(@"logo/path.png");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading logo: " + ex.Message);
            }

            // Timer for fade effect
            fadeTimer = new Timer
            {
                Interval = 50
            };
            fadeTimer.Tick += FadeTimer_Tick;

            this.Controls.Add(logoPictureBox);
            this.Load += SplashScreen_Load;

            // Handle window resize to keep PictureBox centered
            this.Resize += (sender, e) =>
            {
                logoPictureBox.Location = new Point(
                    (this.ClientSize.Width - logoPictureBox.Width) / 2,
                    (this.ClientSize.Height - logoPictureBox.Height) / 2
                );
            };
        }

        private void SplashScreen_Load(object sender, EventArgs e)
        {
            fadeTimer.Start();
        }

        private void FadeTimer_Tick(object sender, EventArgs e)
        {
            opacity += 0.05f;
            this.Opacity = Math.Min(opacity, 1);

            if (opacity >= 1)
            {
                fadeTimer.Stop();
                System.Threading.Thread.Sleep(2000);

                using (var loginForm = new LoginForm())
                //{
                //    this.Hide();
                //    loginForm.ShowDialog();
                //    this.Close();
                //}
                using (var mainMenu = new MainMenuForm())
                {
                    this.Hide();
                    mainMenu.ShowDialog();
                    this.Close();
                }
            }
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            fadeTimer.Dispose();
            base.OnFormClosing(e);
        }
    }
}

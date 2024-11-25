using System;
using System.Drawing;
using System.Windows.Forms;

namespace GstartApp
{
    public partial class MainMenuForm : Form
    {
        private PictureBox logoBox;
        private Button adminLoginButton;
        private Button openLibraryButton;

        public MainMenuForm()
        {
            InitializeComponent();
            InitializeCustomComponents();
        }

        private void InitializeCustomComponents()
        {
            // Form settings
            this.Text = "GameStart";
            this.Size = new Size(500, 400);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = Color.FromArgb(18, 18, 18);
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;

            // Logo PictureBox
            logoBox = new PictureBox
            {
                Size = new Size(300, 120),
                Location = new Point(100, 40),
                SizeMode = PictureBoxSizeMode.Zoom,
                BackColor = Color.Transparent
            };

            try
            {
                logoBox.Image = Image.FromFile(@"logo/image/path.png");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading logo: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }

            // Admin Login Button
            adminLoginButton = new Button
            {
                Text = "Admin Login",
                Location = new Point(100, 200),
                Size = new Size(300, 45),
                FlatStyle = FlatStyle.Flat,
                BackColor = Color.FromArgb(150, 0, 0),
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 12F, FontStyle.Regular),
                Cursor = Cursors.Hand
            };
            adminLoginButton.FlatAppearance.BorderSize = 0;
            adminLoginButton.Click += AdminLoginButton_Click;

            // Open Library Button
            openLibraryButton = new Button
            {
                Text = "Open Library",
                Location = new Point(100, 270),
                Size = new Size(300, 45),
                FlatStyle = FlatStyle.Flat,
                BackColor = Color.FromArgb(150, 0, 0),
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 12F, FontStyle.Regular),
                Cursor = Cursors.Hand
            };
            openLibraryButton.FlatAppearance.BorderSize = 0;
            openLibraryButton.Click += OpenLibraryButton_Click;

            // Add hover effects for buttons
            adminLoginButton.MouseEnter += (s, e) => {
                adminLoginButton.BackColor = Color.FromArgb(180, 0, 0);
            };
            adminLoginButton.MouseLeave += (s, e) => {
                adminLoginButton.BackColor = Color.FromArgb(150, 0, 0);
            };

            openLibraryButton.MouseEnter += (s, e) => {
                openLibraryButton.BackColor = Color.FromArgb(180, 0, 0);
            };
            openLibraryButton.MouseLeave += (s, e) => {
                openLibraryButton.BackColor = Color.FromArgb(150, 0, 0);
            };

            // Add controls to form
            this.Controls.AddRange(new Control[] {
                logoBox,
                adminLoginButton,
                openLibraryButton
            });
        }

        private void AdminLoginButton_Click(object sender, EventArgs e)
        {
            using (var loginForm = new LoginForm())
            {
                this.Hide();
                loginForm.ShowDialog();
                this.Close();
            }
        }

        private void OpenLibraryButton_Click(object sender, EventArgs e)
        {
            using (var searchForm = new GameSearchForm())
            {
                this.Hide();
                searchForm.ShowDialog();
                this.Close();
            }
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            base.OnFormClosing(e);
            if (logoBox.Image != null)
            {
                logoBox.Image.Dispose();
            }
        }
    }
}

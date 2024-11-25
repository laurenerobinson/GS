using System;
using System.Data.SqlClient;
using System.Drawing;
using System.Windows.Forms;

namespace GstartApp
{
    public partial class LoginForm : Form
    {
        private TextBox usernameTextBox;
        private TextBox passwordTextBox;
        private Button loginButton;
        private Button menuButton;
        private Label lblUsername;
        private Label lblPassword;
        private PictureBox logoBox;

        public LoginForm()
        {
            InitializeComponent();
            InitializeCustomComponents();
        }

        private void InitializeCustomComponents()
        {
            // Form settings - Made taller to accommodate logo
            this.Text = "GameStart";
            this.Size = new Size(400, 400);  // Increased height
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = Color.FromArgb(18, 18, 18);
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;

            // Logo PictureBox
            logoBox = new PictureBox
            {
                Size = new Size(200, 80),  // Adjust size as needed
                Location = new Point(100, 20),  // Centered at top
                SizeMode = PictureBoxSizeMode.Zoom,
                BackColor = Color.Transparent
            };

            try
            {
                // Load the image - replace this path with your project's resources path
                logoBox.Image = Image.FromFile(@"logo/image/path.png");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading logo: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }

            // Adjusted Y positions to accommodate logo
            // Username Label
            lblUsername = new Label
            {
                Text = "Username",
                Location = new Point(50, 120),  // Moved down
                Size = new Size(100, 20),
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 10F)
            };

            // Username TextBox
            usernameTextBox = new TextBox
            {
                Location = new Point(50, 145),  // Moved down
                Size = new Size(280, 30),
                Font = new Font("Segoe UI", 12F),
                BackColor = Color.FromArgb(45, 45, 45),
                ForeColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle
            };

            // Password Label
            lblPassword = new Label
            {
                Text = "Password",
                Location = new Point(50, 190),  // Moved down
                Size = new Size(100, 20),
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 10F)
            };

            // Password TextBox
            passwordTextBox = new TextBox
            {
                Location = new Point(50, 215),  // Moved down
                Size = new Size(280, 30),
                PasswordChar = '•',
                Font = new Font("Segoe UI", 12F),
                BackColor = Color.FromArgb(45, 45, 45),
                ForeColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle
            };

            // Login Button
            loginButton = new Button
            {
                Text = "Login",
                Location = new Point(50, 265),  // Moved down
                Size = new Size(130, 35),
                FlatStyle = FlatStyle.Flat,
                BackColor = Color.FromArgb(150, 0, 0),
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 10F)
            };
            loginButton.Click += LoginButton_Click;

            // Menu Button
            menuButton = new Button
            {
                Text = "Main Menu",
                Location = new Point(200, 265),  // Moved down
                Size = new Size(130, 35),
                FlatStyle = FlatStyle.Flat,
                BackColor = Color.FromArgb(150, 0, 0),
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 10F)
            };
            menuButton.Click += MenuButton_Click;

            // Add controls to form
            this.Controls.AddRange(new Control[] {
                logoBox,
                lblUsername, usernameTextBox,
                lblPassword, passwordTextBox,
                loginButton, menuButton
            });
        }


        private void LoginButton_Click(object sender, EventArgs e)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(@"sqlserver/connection/path"))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand("SELECT * FROM LOGIN WHERE UserID = @UserID AND Password = @Password", conn);
                    cmd.Parameters.AddWithValue("@UserID", usernameTextBox.Text);
                    cmd.Parameters.AddWithValue("@Password", passwordTextBox.Text);

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            MessageBox.Show("Login successful!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            using (var crudForm = new CRUDForm())
                            {
                                this.Hide();
                                crudForm.ShowDialog();
                                this.Close();
                            }
                        }
                        else
                        {
                            MessageBox.Show("Invalid username or password!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Database error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void MenuButton_Click(object sender, EventArgs e)
        {
            using (var menuForm = new MainMenuForm())
            {
                this.Hide();
                menuForm.ShowDialog();
                this.Close();
            }
        }

        private void LoginForm_Load(object sender, EventArgs e)
        {
            // Add any initialization code needed when the form loads
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

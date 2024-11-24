using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Windows.Forms;

namespace GstartApp
{
    public partial class GameSearchForm : Form
    {
        public SqlConnection DBConnection;
        private TextBox searchBox;
        private Button searchButton;
        private DataGridView gameGrid;
        private Panel searchPanel;
        private ComboBox categoryComboBox;
        private Button logoutButton;

        public GameSearchForm()
        {
            InitializeComponent();
            InitializeFormComponents();
        }

        private void InitializeFormComponents()
        {
            // Form settings
            this.Text = "Guest Search";
            this.Size = new Size(1000, 600); // Increased width to accommodate new column
            this.MinimumSize = new Size(900, 500);
            this.BackColor = Color.FromArgb(18, 18, 18);
            this.ForeColor = Color.White;

            // Search panel
            searchPanel = new Panel
            {
                Dock = DockStyle.Top,
                Height = 80,
                BackColor = Color.FromArgb(28, 28, 28),
                Padding = new Padding(10, 5, 10, 5)
            };

            // Logout Button - Added before the logo
            logoutButton = new Button
            {
                Text = "Logout",
                Location = new Point(searchPanel.Width - 220, 25), // Positioned to the left of the logo
                Size = new Size(90, 30),
                Name = "logoutButton",
                Font = new Font("Segoe UI", 10F),
                FlatStyle = FlatStyle.Flat,
                BackColor = Color.FromArgb(214, 65, 60),
                ForeColor = Color.White,
                Cursor = Cursors.Hand,
                Anchor = AnchorStyles.Top | AnchorStyles.Right // Anchoring to keep position relative to right side
            };
            logoutButton.FlatAppearance.BorderSize = 0;
            logoutButton.Click += LogoutButton_Click;
            searchPanel.Controls.Add(logoutButton);

            // Logo PictureBox
            PictureBox logoPictureBox = new PictureBox
            {
                Size = new Size(90, 60),
                Location = new Point(searchPanel.Width - 120, 10),
                SizeMode = PictureBoxSizeMode.Zoom,
                Anchor = AnchorStyles.Top | AnchorStyles.Right,
                BackColor = Color.Transparent
            };

            try
            {
                logoPictureBox.Image = Image.FromFile(@"C:\Users\robin\Downloads\[removal.ai]_c54a08f5-4839-43a5-aba1-43c768c8d40a-screenshot-2024-11-14-112021.png");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading image: " + ex.Message);
            }

            searchPanel.Controls.Add(logoPictureBox);

            // Updated category combo box with Console
            categoryComboBox = new ComboBox
            {
                Location = new Point(20, 25),
                Size = new Size(150, 30),
                Name = "categoryComboBox",
                DropDownStyle = ComboBoxStyle.DropDownList,
                Font = new Font("Segoe UI", 10F),
                BackColor = Color.FromArgb(45, 45, 45),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };

            // Updated categories with Console
            categoryComboBox.Items.AddRange(new string[] {
                "All Categories",
                "Title",
                "Company",
                "Age Rating",
                "Release Year",
                "Shelf ID",
                "Console"  // Added new category
            });
            categoryComboBox.SelectedIndex = 0;

            // ComboBox styling
            categoryComboBox.DrawMode = DrawMode.OwnerDrawFixed;
            categoryComboBox.DrawItem += new DrawItemEventHandler((sender, e) =>
            {
                if (e.Index < 0) return;
                if ((e.State & DrawItemState.Selected) == DrawItemState.Selected)
                {
                    e.Graphics.FillRectangle(new SolidBrush(Color.FromArgb(150, 0, 0)), e.Bounds);
                }
                else
                {
                    e.Graphics.FillRectangle(new SolidBrush(Color.FromArgb(45, 45, 45)), e.Bounds);
                }
                e.Graphics.DrawString(categoryComboBox.Items[e.Index].ToString(),
                    e.Font, new SolidBrush(Color.White), e.Bounds, StringFormat.GenericDefault);
            });

            // Search controls
            searchBox = new TextBox
            {
                Location = new Point(180, 25),
                Size = new Size(300, 30),
                Name = "searchBox",
                Font = new Font("Segoe UI", 10F),
                BackColor = Color.FromArgb(45, 45, 45),
                ForeColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle
            };

            searchButton = new Button
            {
                Text = "Search",
                Location = new Point(490, 25),
                Size = new Size(100, 30),
                Name = "searchButton",
                Font = new Font("Segoe UI", 10F),
                FlatStyle = FlatStyle.Flat,
                BackColor = Color.FromArgb(214, 65, 60),
                ForeColor = Color.White,
                Cursor = Cursors.Hand
            };
            searchButton.FlatAppearance.BorderSize = 0;
            searchButton.Click += SearchButton_Click;

            // DataGridView setup
            gameGrid = new DataGridView
            {
                Name = "gameGrid",
                Location = new Point(20, 100),
                Size = new Size(945, 440), // Increased width
                Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right,
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                ReadOnly = true,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                BackgroundColor = Color.FromArgb(28, 28, 28),
                GridColor = Color.FromArgb(70, 70, 70),
                BorderStyle = BorderStyle.None,
                CellBorderStyle = DataGridViewCellBorderStyle.Single,
                ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single,
                RowHeadersVisible = false,
                Font = new Font("Segoe UI", 9.5F),
                EnableHeadersVisualStyles = false,
                RowTemplate = { Height = 35 }
            };

            // DataGridView styling
            gameGrid.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(45, 45, 45);
            gameGrid.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            gameGrid.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI Semibold", 10F);
            gameGrid.ColumnHeadersDefaultCellStyle.Padding = new Padding(8);
            gameGrid.ColumnHeadersHeight = 40;

            gameGrid.DefaultCellStyle.BackColor = Color.FromArgb(28, 28, 28);
            gameGrid.DefaultCellStyle.ForeColor = Color.White;
            gameGrid.DefaultCellStyle.SelectionBackColor = Color.FromArgb(150, 0, 0);
            gameGrid.DefaultCellStyle.SelectionForeColor = Color.White;
            gameGrid.DefaultCellStyle.Padding = new Padding(8);

            // Updated column handling to include Console
            gameGrid.ColumnAdded += (sender, e) =>
            {
                e.Column.HeaderCell.Style.Font = new Font("Segoe UI Semibold", 10F);
                e.Column.HeaderCell.Style.BackColor = Color.FromArgb(45, 45, 45);
                e.Column.HeaderCell.Style.ForeColor = Color.White;
                e.Column.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleLeft;
                e.Column.MinimumWidth = 100;

                switch (e.Column.Name)
                {
                    case "Title":
                        e.Column.HeaderText = "GAME TITLE";
                        e.Column.MinimumWidth = 200;
                        break;
                    case "Company":
                        e.Column.HeaderText = "PUBLISHER";
                        e.Column.MinimumWidth = 150;
                        break;
                    case "Age_Rating":
                        e.Column.HeaderText = "AGE RATING";
                        break;
                    case "Release_Year":
                        e.Column.HeaderText = "RELEASE YEAR";
                        break;
                    case "ShelfID":
                        e.Column.HeaderText = "SHELF ID";
                        break;
                    case "Console":  // Added new column
                        e.Column.HeaderText = "CONSOLE";
                        e.Column.MinimumWidth = 120;
                        break;
                }
            };

            // Add controls to form
            searchPanel.Controls.Add(categoryComboBox);
            searchPanel.Controls.Add(searchBox);
            searchPanel.Controls.Add(searchButton);
            this.Controls.Add(searchPanel);
            this.Controls.Add(gameGrid);

            this.Load += GameSearchForm_Load;
        }

      

        private void LoadGameData(string searchTerm, string category)
        {
            try
            {
                SqlCommand cmdSearchGames = DBConnection.CreateCommand();
                string whereClause;

                switch (category)
                {
                    case "Title":
                        whereClause = "WHERE g.Title LIKE @SearchTerm";
                        break;
                    case "Company":
                        whereClause = "WHERE g.Company LIKE @SearchTerm";
                        break;
                    case "Age Rating":
                        whereClause = "WHERE g.Age_Rating LIKE @SearchTerm";
                        break;
                    case "Release Year":
                        whereClause = "WHERE g.Release_Year LIKE @SearchTerm";
                        break;
                    case "Shelf ID":
                        whereClause = "WHERE g.ShelfID LIKE @SearchTerm";
                        break;
                    case "Console":
                        whereClause = "WHERE s.Console LIKE @SearchTerm";
                        break;
                    default:
                        whereClause = @"WHERE g.Title LIKE @SearchTerm 
                    OR g.Company LIKE @SearchTerm 
                    OR g.Age_Rating LIKE @SearchTerm 
                    OR s.Console LIKE @SearchTerm
                    OR CAST(g.Release_Year AS VARCHAR) LIKE @SearchTerm
                    OR CAST(g.ShelfID AS VARCHAR) LIKE @SearchTerm";
                        break;
                }

                cmdSearchGames.CommandText = $@"
            SELECT g.Title, g.Company, g.Age_Rating, g.Release_Year, g.ShelfID, s.Console 
            FROM GAME g
            LEFT JOIN SHELF s ON g.ShelfID = s.Shelf_ID
            {whereClause}";

                cmdSearchGames.Parameters.AddWithValue("@SearchTerm", $"%{searchTerm}%");

                SqlDataReader reader = cmdSearchGames.ExecuteReader();
                DataTable tempTable = new DataTable();
                tempTable.Load(reader);
                gameGrid.DataSource = tempTable;
                reader.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading data: {ex.Message}",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Event handlers remain the same
        private void GameSearchForm_Load(object sender, EventArgs e)
        {
            try
            {
                DBConnection = new SqlConnection(@"Data Source=LAUS_DELL;Initial Catalog=GameStartDB;Integrated Security=True");
                DBConnection.Open();
                LoadGameData("", "All Categories");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error connecting to database: {ex.Message}",
                    "Connection Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void SearchButton_Click(object sender, EventArgs e)
        {
            string searchTerm = searchBox.Text.Trim();
            string category = categoryComboBox.SelectedItem.ToString();
            LoadGameData(searchTerm, category);
        }
        // New Logout Button Click Handler
        private void LogoutButton_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to logout?", "Confirm Logout",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
           
            using (var MainMenu = new MainMenuForm())
            {
                this.Hide();
                MainMenu.ShowDialog();
                this.Close();
            }
        }


        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            if (DBConnection != null && DBConnection.State == ConnectionState.Open)
            {
                DBConnection.Close();
            }
            base.OnFormClosing(e);
        }
    }
}
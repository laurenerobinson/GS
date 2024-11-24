using System.Data;
using System;
using System.Data.SqlClient;
using System.Drawing;
using System.Windows.Forms;

namespace GstartApp
{
    public partial class CRUDForm : Form
    {
        private SqlConnection DBConnection;
        private DataGridView gameGrid;
        private TextBox titleBox;
        private TextBox companyBox;
        private TextBox ageRatingBox;
        private TextBox yearBox;
        private TextBox shelfIDBox;
        private TextBox gameIDBox;  // Added Game ID TextBox
        private ComboBox consoleComboBox;
        private Button addButton;
        private Button updateButton;
        private Button deleteButton;
        private Button clearButton;
        private Button menuButton;
        private Label titleLabel;
        private Label companyLabel;
        private Label ageLabel;
        private Label yearLabel;
        private Label shelfLabel;
        private Label consoleLabel;
        private Label gameIDLabel;  // Added Game ID Label
        private Panel inputPanel;
        private Panel buttonPanel;

        public CRUDForm()
        {
            InitializeComponent();
            InitializeFormComponents();
            ConnectToDatabase();
        }

        private void InitializeFormComponents()
        {
            // Form settings
            this.Text = "Game Management";
            this.Size = new Size(1000, 700);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = Color.FromArgb(18, 18, 18);
            this.ForeColor = Color.White;
            this.MinimumSize = new Size(1000, 700);

            // Input Panel
            inputPanel = new Panel
            {
                Dock = DockStyle.Top,
                Height = 200,
                BackColor = Color.FromArgb(28, 28, 28),
                Padding = new Padding(20)
            };

            // Create and style input fields
            gameIDLabel = CreateLabel("Game ID:", new Point(20, 20));  // Added Game ID Label
            gameIDBox = CreateTextBox(new Point(120, 20), 100);       // Added Game ID TextBox
            gameIDBox.ReadOnly = true;  // Make it read-only since it's auto-generated

            titleLabel = CreateLabel("Game Title:", new Point(400, 20));  // Adjusted position
            titleBox = CreateTextBox(new Point(500, 20), 250);           // Adjusted position

            companyLabel = CreateLabel("Company:", new Point(20, 60));    // Adjusted position
            companyBox = CreateTextBox(new Point(120, 60), 200);         // Adjusted position

            ageLabel = CreateLabel("Age Rating:", new Point(400, 60));    // Adjusted position
            ageRatingBox = CreateTextBox(new Point(500, 60), 100);       // Adjusted position

            yearLabel = CreateLabel("Release Year:", new Point(20, 100)); // Adjusted position
            yearBox = CreateTextBox(new Point(120, 100), 100);           // Adjusted position

            shelfLabel = CreateLabel("Shelf ID:", new Point(400, 100));   // Adjusted position
            shelfIDBox = CreateTextBox(new Point(500, 100), 100);        // Adjusted position

            // Console ComboBox
            consoleLabel = CreateLabel("Console:", new Point(20, 140));   // Adjusted position
            consoleComboBox = new ComboBox
            {
                Location = new Point(120, 140),                          // Adjusted position
                Width = 200,
                DropDownStyle = ComboBoxStyle.DropDownList,
                BackColor = Color.FromArgb(45, 45, 45),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };

            // Button Panel
            buttonPanel = new Panel
            {
                Height = 50,
                Dock = DockStyle.Bottom,
                BackColor = Color.FromArgb(28, 28, 28),
                Padding = new Padding(10)
            };

            // Create and style buttons
            addButton = CreateButton("Add Game", new Point(20, 10));
            updateButton = CreateButton("Update", new Point(150, 10));
            deleteButton = CreateButton("Delete", new Point(280, 10));
            clearButton = CreateButton("Clear", new Point(410, 10));
            menuButton = CreateButton("Back to Menu", new Point(540, 10));

            // Add button click events
            addButton.Click += AddButton_Click;
            updateButton.Click += UpdateButton_Click;
            deleteButton.Click += DeleteButton_Click;
            clearButton.Click += ClearButton_Click;
            menuButton.Click += MenuButton_Click;

            // Create DataGridView with updated styling
            gameGrid = new DataGridView
            {
                Name = "gameGrid",
                Location = new Point(20, 210),
                Size = new Size(945, 370),
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
            gameGrid.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            gameGrid.CellClick += GameGrid_CellClick;

            // Style the DataGridView
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

            // Add column styling handler
            gameGrid.ColumnAdded += (sender, e) =>
            {
                e.Column.HeaderCell.Style.Font = new Font("Segoe UI Semibold", 10F);
                e.Column.HeaderCell.Style.BackColor = Color.FromArgb(45, 45, 45);
                e.Column.HeaderCell.Style.ForeColor = Color.White;
                e.Column.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleLeft;
                e.Column.MinimumWidth = 100;

                switch (e.Column.Name)
                {
                    case "Game_ID":
                        e.Column.HeaderText = "GAME ID";
                        e.Column.MinimumWidth = 80;
                        break;
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
                    case "Console":
                        e.Column.HeaderText = "CONSOLE";
                        e.Column.MinimumWidth = 120;
                        break;
                }
            };

            // Add DataGridView selection event
            gameGrid.SelectionChanged += GameGrid_SelectionChanged;

            // Add controls to panels
            inputPanel.Controls.AddRange(new Control[] {
                gameIDLabel, gameIDBox,      // Added Game ID controls
                titleLabel, titleBox,
                companyLabel, companyBox,
                ageLabel, ageRatingBox,
                yearLabel, yearBox,
                shelfLabel, shelfIDBox,
                consoleLabel, consoleComboBox
            });

            buttonPanel.Controls.AddRange(new Control[] {
                addButton, updateButton, deleteButton, clearButton, menuButton
            });

            // Add panels to form
            this.Controls.AddRange(new Control[] { inputPanel, buttonPanel, gameGrid });
        }

        private Label CreateLabel(string text, Point location)
        {
            return new Label
            {
                Text = text,
                Location = location,
                AutoSize = true,
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 10F)
            };
        }

        private TextBox CreateTextBox(Point location, int width)
        {
            return new TextBox
            {
                Location = location,
                Width = width,
                BackColor = Color.FromArgb(45, 45, 45),
                ForeColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle,
                Font = new Font("Segoe UI", 10F)
            };
        }

        private Button CreateButton(string text, Point location)
        {
            var button = new Button
            {
                Text = text,
                Location = location,
                Size = new Size(110, 30),
                FlatStyle = FlatStyle.Flat,
                BackColor = Color.FromArgb(150, 0, 0),
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 10F)
            };
            button.FlatAppearance.BorderSize = 0;
            return button;
        }

        private void ConnectToDatabase()
        {
            try
            {
                DBConnection = new SqlConnection(@"Data Source=LAUS_DELL;Initial Catalog=GameStartDB;Integrated Security=True");
                DBConnection.Open();
                LoadConsoles();
                LoadGameData();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Database connection error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadConsoles()
        {
            try
            {
                using (var command = new SqlCommand("SELECT DISTINCT Console FROM SHELF ORDER BY Console", DBConnection))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        consoleComboBox.Items.Clear();
                        while (reader.Read())
                        {
                            consoleComboBox.Items.Add(reader["Console"].ToString());
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading consoles: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadGameData()
        {
            try
            {
                using (var command = new SqlCommand(@"
                    SELECT g.Game_ID, g.Title, g.Company, g.Age_Rating, g.Release_Year, g.ShelfID, s.Console
                    FROM GAME g
                    LEFT JOIN SHELF s ON g.ShelfID = s.Shelf_ID", DBConnection))
                {
                    var adapter = new SqlDataAdapter(command);
                    var table = new DataTable();
                    adapter.Fill(table);
                    gameGrid.DataSource = table;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading data: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ClearFields()
        {
            gameIDBox.Clear();  // Added clearing Game ID
            titleBox.Clear();
            companyBox.Clear();
            ageRatingBox.Clear();
            yearBox.Clear();
            shelfIDBox.Clear();
            if (consoleComboBox.Items.Count > 0)
                consoleComboBox.SelectedIndex = -1;
            gameGrid.ClearSelection();
        }
        private void GameGrid_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0) // Ensure we didn't click on the header row
            {
                gameGrid.Rows[e.RowIndex].Selected = true;
            }
        }

        private void AddButton_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(titleBox.Text) || string.IsNullOrWhiteSpace(companyBox.Text))
                {
                    MessageBox.Show("Title and Company are required fields!", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                using (var command = new SqlCommand(
                    @"INSERT INTO GAME (Game_ID, Title, Company, Age_Rating, Release_Year, ShelfID) 
                    VALUES ((SELECT ISNULL(MAX(Game_ID), 0) + 1 FROM GAME), @Title, @Company, @AgeRating, @Year, @ShelfID)",
                    DBConnection))
                {
                    command.Parameters.AddWithValue("@Title", titleBox.Text);
                    command.Parameters.AddWithValue("@Company", companyBox.Text);
                    command.Parameters.AddWithValue("@AgeRating", ageRatingBox.Text);
                    command.Parameters.AddWithValue("@Year", Convert.ToInt32(yearBox.Text));
                    command.Parameters.AddWithValue("@ShelfID", Convert.ToInt32(shelfIDBox.Text));

                    command.ExecuteNonQuery();
                    MessageBox.Show("Game added successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadGameData();
                    ClearFields();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error adding game: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void UpdateButton_Click(object sender, EventArgs e)
        {
            if (gameGrid.SelectedRows.Count == 0)
            {
                MessageBox.Show("Please select a game to update.", "Selection Required", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                var row = gameGrid.SelectedRows[0];
                using (var command = new SqlCommand(
                    @"UPDATE GAME 
                    SET Title = @Title, Company = @Company, Age_Rating = @AgeRating, 
                        Release_Year = @Year, ShelfID = @ShelfID 
                    WHERE Game_ID = @GameID",
                    DBConnection))
                {
                    command.Parameters.AddWithValue("@GameID", Convert.ToInt32(gameIDBox.Text));
                    command.Parameters.AddWithValue("@Title", titleBox.Text);
                    command.Parameters.AddWithValue("@Company", companyBox.Text);
                    command.Parameters.AddWithValue("@AgeRating", ageRatingBox.Text);
                    command.Parameters.AddWithValue("@Year", Convert.ToInt32(yearBox.Text));
                    command.Parameters.AddWithValue("@ShelfID", Convert.ToInt32(shelfIDBox.Text));

                    command.ExecuteNonQuery();
                    MessageBox.Show("Game updated successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadGameData();
                    ClearFields();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error updating game: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void DeleteButton_Click(object sender, EventArgs e)
        {
            if (gameGrid.SelectedRows.Count == 0)
            {
                MessageBox.Show("Please select a game to delete.", "Selection Required", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (MessageBox.Show("Are you sure you want to delete this game?", "Confirm Delete",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                try
                {
                    using (var command = new SqlCommand("DELETE FROM GAME WHERE Game_ID = @GameID", DBConnection))
                    {
                        command.Parameters.AddWithValue("@GameID", Convert.ToInt32(gameIDBox.Text));
                        command.ExecuteNonQuery();
                        MessageBox.Show("Game deleted successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        LoadGameData();
                        ClearFields();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error deleting game: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void ClearButton_Click(object sender, EventArgs e)
        {
            ClearFields();
        }

        private void MenuButton_Click(object sender, EventArgs e)
        {
            using (var menuForm = new GameSearchForm())
            {
                this.Hide();
                menuForm.ShowDialog();
                this.Close();
            }
        }

        private void GameGrid_SelectionChanged(object sender, EventArgs e)
        {
            if (gameGrid.SelectedRows.Count > 0)
            {
                var row = gameGrid.SelectedRows[0];
                gameIDBox.Text = row.Cells["Game_ID"].Value.ToString();  // Added Game ID
                titleBox.Text = row.Cells["Title"].Value.ToString();
                companyBox.Text = row.Cells["Company"].Value.ToString();
                ageRatingBox.Text = row.Cells["Age_Rating"].Value.ToString();
                yearBox.Text = row.Cells["Release_Year"].Value.ToString();
                shelfIDBox.Text = row.Cells["ShelfID"].Value.ToString();
                consoleComboBox.Text = row.Cells["Console"].Value?.ToString() ?? "";
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
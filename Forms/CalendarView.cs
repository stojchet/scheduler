using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Scheduler.Forms
{
    public class CalendarView : Form
    {
        /* ------------------------------ Private Widgets ------------------------------*/
        private TableLayoutPanel TitlePanel;
        private Label TitleLabel;
        private Controls.DatePicker DatePicker;
        private FlowLayoutPanel DatePanel;
        private Label DateLabel;
        private Label CurrentDateLabel;
        private Button SearchButton;
        private FlowLayoutPanel ButtonsPanel;
        private Button AddTaskButton;

        /* ------------------------------ Private Variables ------------------------------*/
        private Day CurrentDay;
        private Button NextDayButton;
        private Button PreviousDayButton;
        private MenuStrip MainMenu;
        private ToolStripMenuItem fIleToolStripMenuItem;
        private ToolStripMenuItem loadDataToolStripMenuItem;
        private ToolStripMenuItem saveDataToolStripMenuItem;
        private ToolStripMenuItem preferencesToolStripMenuItem;
        private ToolStripMenuItem setWorkingHoursToolStripMenuItem;
        private DataGridView TaskList;
        private DataGridViewTextBoxColumn Time;
        private DataGridViewTextBoxColumn TaskName;
        private DataGridViewTextBoxColumn Duration;
        private ToolStripMenuItem addTaskToolStripMenuItem;
        private ContextMenuStrip ItemMenuStrip;

        /* ------------------------------ Private Methods ------------------------------*/
        private void LoadTasks()
        {
            this.TaskList.Rows.Clear();

            if (CurrentDay != null && CurrentDay.Tasks.Count > 0)
            {
                TaskList.Rows.Add(CurrentDay.Tasks.Count);
                for(int i = 0; i < TaskList.RowCount; ++i)
                {
                    TaskList.Rows[i].Cells["Time"].Value = CurrentDay.getTimeRangeForTask(CurrentDay[i]);
                    TaskList.Rows[i].Cells["TaskName"].Value = CurrentDay[i].Name;
                    TaskList.Rows[i].Cells["Duration"].Value = CurrentDay[i].Duration;
                    TaskList.Rows[i].Tag = CurrentDay[i];
                }
            }
        }

        private void InitializeComponent()
        {
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            this.TitlePanel = new System.Windows.Forms.TableLayoutPanel();
            this.TitleLabel = new System.Windows.Forms.Label();
            this.DatePanel = new System.Windows.Forms.FlowLayoutPanel();
            this.DateLabel = new System.Windows.Forms.Label();
            this.SearchButton = new System.Windows.Forms.Button();
            this.CurrentDateLabel = new System.Windows.Forms.Label();
            this.ButtonsPanel = new System.Windows.Forms.FlowLayoutPanel();
            this.AddTaskButton = new System.Windows.Forms.Button();
            this.TaskList = new System.Windows.Forms.DataGridView();
            this.Time = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.TaskName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Duration = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.NextDayButton = new System.Windows.Forms.Button();
            this.PreviousDayButton = new System.Windows.Forms.Button();
            this.MainMenu = new System.Windows.Forms.MenuStrip();
            this.fIleToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveDataToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.loadDataToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.preferencesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.setWorkingHoursToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.addTaskToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.DatePicker = new Scheduler.Controls.DatePicker();
            this.TitlePanel.SuspendLayout();
            this.DatePanel.SuspendLayout();
            this.ButtonsPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.TaskList)).BeginInit();
            this.MainMenu.SuspendLayout();
            this.SuspendLayout();
            // 
            // TitlePanel
            // 
            this.TitlePanel.ColumnCount = 1;
            this.TitlePanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.TitlePanel.Controls.Add(this.TitleLabel, 0, 0);
            this.TitlePanel.Controls.Add(this.DatePanel, 0, 1);
            this.TitlePanel.Controls.Add(this.CurrentDateLabel, 0, 2);
            this.TitlePanel.Controls.Add(this.ButtonsPanel, 0, 4);
            this.TitlePanel.Controls.Add(this.TaskList, 0, 3);
            this.TitlePanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.TitlePanel.Location = new System.Drawing.Point(0, 28);
            this.TitlePanel.Margin = new System.Windows.Forms.Padding(0);
            this.TitlePanel.Name = "TitlePanel";
            this.TitlePanel.RowCount = 5;
            this.TitlePanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 75F));
            this.TitlePanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 50F));
            this.TitlePanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 50F));
            this.TitlePanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.TitlePanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 50F));
            this.TitlePanel.Size = new System.Drawing.Size(605, 923);
            this.TitlePanel.TabIndex = 0;
            // 
            // TitleLabel
            // 
            this.TitleLabel.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.TitleLabel.AutoSize = true;
            this.TitleLabel.Font = new System.Drawing.Font("Segoe UI", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.TitleLabel.Location = new System.Drawing.Point(192, 14);
            this.TitleLabel.Margin = new System.Windows.Forms.Padding(0);
            this.TitleLabel.Name = "TitleLabel";
            this.TitleLabel.Size = new System.Drawing.Size(221, 46);
            this.TitleLabel.TabIndex = 0;
            this.TitleLabel.Text = "My Calendar";
            // 
            // DatePanel
            // 
            this.DatePanel.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.DatePanel.AutoSize = true;
            this.DatePanel.Controls.Add(this.DateLabel);
            this.DatePanel.Controls.Add(this.DatePicker);
            this.DatePanel.Controls.Add(this.SearchButton);
            this.DatePanel.Location = new System.Drawing.Point(134, 77);
            this.DatePanel.Margin = new System.Windows.Forms.Padding(0);
            this.DatePanel.Name = "DatePanel";
            this.DatePanel.Size = new System.Drawing.Size(337, 46);
            this.DatePanel.TabIndex = 4;
            // 
            // DateLabel
            // 
            this.DateLabel.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.DateLabel.AutoSize = true;
            this.DateLabel.Location = new System.Drawing.Point(0, 9);
            this.DateLabel.Margin = new System.Windows.Forms.Padding(0);
            this.DateLabel.Name = "DateLabel";
            this.DateLabel.Size = new System.Drawing.Size(57, 28);
            this.DateLabel.TabIndex = 3;
            this.DateLabel.Text = "Date:";
            // 
            // SearchButton
            // 
            this.SearchButton.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.SearchButton.AutoSize = true;
            this.SearchButton.Location = new System.Drawing.Point(243, 3);
            this.SearchButton.Margin = new System.Windows.Forms.Padding(6, 0, 0, 0);
            this.SearchButton.Name = "SearchButton";
            this.SearchButton.Size = new System.Drawing.Size(94, 39);
            this.SearchButton.TabIndex = 4;
            this.SearchButton.Text = "Search";
            this.SearchButton.UseVisualStyleBackColor = true;
            this.SearchButton.Click += new System.EventHandler(this.SearchButton_Click);
            // 
            // CurrentDateLabel
            // 
            this.CurrentDateLabel.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.CurrentDateLabel.AutoSize = true;
            this.CurrentDateLabel.Font = new System.Drawing.Font("Segoe UI", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.CurrentDateLabel.Location = new System.Drawing.Point(4, 131);
            this.CurrentDateLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.CurrentDateLabel.Name = "CurrentDateLabel";
            this.CurrentDateLabel.Size = new System.Drawing.Size(0, 37);
            this.CurrentDateLabel.TabIndex = 5;
            // 
            // ButtonsPanel
            // 
            this.ButtonsPanel.Controls.Add(this.AddTaskButton);
            this.ButtonsPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ButtonsPanel.FlowDirection = System.Windows.Forms.FlowDirection.RightToLeft;
            this.ButtonsPanel.Location = new System.Drawing.Point(0, 873);
            this.ButtonsPanel.Margin = new System.Windows.Forms.Padding(0);
            this.ButtonsPanel.Name = "ButtonsPanel";
            this.ButtonsPanel.Size = new System.Drawing.Size(605, 50);
            this.ButtonsPanel.TabIndex = 7;
            // 
            // AddTaskButton
            // 
            this.AddTaskButton.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.AddTaskButton.AutoSize = true;
            this.AddTaskButton.Location = new System.Drawing.Point(498, 6);
            this.AddTaskButton.Margin = new System.Windows.Forms.Padding(6);
            this.AddTaskButton.Name = "AddTaskButton";
            this.AddTaskButton.Size = new System.Drawing.Size(101, 39);
            this.AddTaskButton.TabIndex = 0;
            this.AddTaskButton.Text = "Add Task";
            this.AddTaskButton.UseVisualStyleBackColor = true;
            this.AddTaskButton.Click += new System.EventHandler(this.AddTaskButton_Click);
            // 
            // TaskList
            // 
            this.TaskList.AllowUserToAddRows = false;
            this.TaskList.AllowUserToDeleteRows = false;
            this.TaskList.AllowUserToOrderColumns = true;
            this.TaskList.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.TaskList.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Time,
            this.TaskName,
            this.Duration});
            this.TaskList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.TaskList.Location = new System.Drawing.Point(0, 175);
            this.TaskList.Margin = new System.Windows.Forms.Padding(0);
            this.TaskList.MultiSelect = false;
            this.TaskList.Name = "TaskList";
            this.TaskList.ReadOnly = true;
            this.TaskList.RowHeadersWidth = 51;
            this.TaskList.RowTemplate.Height = 24;
            this.TaskList.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.TaskList.Size = new System.Drawing.Size(605, 698);
            this.TaskList.TabIndex = 8;
            this.TaskList.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.TaskList_CellDoubleClick);
            this.TaskList.MouseDown += new System.Windows.Forms.MouseEventHandler(this.TaskList_MouseDown);
            // 
            // Time
            // 
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.Time.DefaultCellStyle = dataGridViewCellStyle1;
            this.Time.HeaderText = "Time";
            this.Time.MinimumWidth = 6;
            this.Time.Name = "Time";
            this.Time.ReadOnly = true;
            this.Time.Width = 125;
            // 
            // TaskName
            // 
            this.TaskName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            this.TaskName.DefaultCellStyle = dataGridViewCellStyle2;
            this.TaskName.HeaderText = "Task Name";
            this.TaskName.MinimumWidth = 6;
            this.TaskName.Name = "TaskName";
            this.TaskName.ReadOnly = true;
            // 
            // Duration
            // 
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.Duration.DefaultCellStyle = dataGridViewCellStyle3;
            this.Duration.HeaderText = "Duration";
            this.Duration.MinimumWidth = 6;
            this.Duration.Name = "Duration";
            this.Duration.ReadOnly = true;
            this.Duration.Width = 125;
            // 
            // NextDayButton
            // 
            this.NextDayButton.AutoEllipsis = true;
            this.NextDayButton.Location = new System.Drawing.Point(559, 515);
            this.NextDayButton.Name = "NextDayButton";
            this.NextDayButton.Size = new System.Drawing.Size(40, 48);
            this.NextDayButton.TabIndex = 1;
            this.NextDayButton.Text = ">";
            this.NextDayButton.UseVisualStyleBackColor = true;
            this.NextDayButton.Click += new System.EventHandler(this.rightButtonClick);
            // 
            // PreviousDayButton
            // 
            this.PreviousDayButton.Location = new System.Drawing.Point(6, 515);
            this.PreviousDayButton.Name = "PreviousDayButton";
            this.PreviousDayButton.Size = new System.Drawing.Size(37, 48);
            this.PreviousDayButton.TabIndex = 3;
            this.PreviousDayButton.Text = "<";
            this.PreviousDayButton.UseVisualStyleBackColor = true;
            this.PreviousDayButton.Click += new System.EventHandler(this.leftButtonClick);
            // 
            // MainMenu
            // 
            this.MainMenu.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.MainMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fIleToolStripMenuItem,
            this.preferencesToolStripMenuItem});
            this.MainMenu.Location = new System.Drawing.Point(0, 0);
            this.MainMenu.Name = "MainMenu";
            this.MainMenu.Size = new System.Drawing.Size(605, 28);
            this.MainMenu.TabIndex = 4;
            this.MainMenu.Text = "menuStrip1";
            // 
            // fIleToolStripMenuItem
            // 
            this.fIleToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.saveDataToolStripMenuItem,
            this.loadDataToolStripMenuItem,
            this.addTaskToolStripMenuItem});
            this.fIleToolStripMenuItem.Name = "fIleToolStripMenuItem";
            this.fIleToolStripMenuItem.Size = new System.Drawing.Size(46, 24);
            this.fIleToolStripMenuItem.Text = "File";
            // 
            // saveDataToolStripMenuItem
            // 
            this.saveDataToolStripMenuItem.Name = "saveDataToolStripMenuItem";
            this.saveDataToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.S)));
            this.saveDataToolStripMenuItem.Size = new System.Drawing.Size(224, 26);
            this.saveDataToolStripMenuItem.Text = "Save Data";
            this.saveDataToolStripMenuItem.Click += new System.EventHandler(this.saveDataToolStripMenuItem_Click);
            // 
            // loadDataToolStripMenuItem
            // 
            this.loadDataToolStripMenuItem.Name = "loadDataToolStripMenuItem";
            this.loadDataToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.L)));
            this.loadDataToolStripMenuItem.Size = new System.Drawing.Size(224, 26);
            this.loadDataToolStripMenuItem.Text = "Load Data";
            this.loadDataToolStripMenuItem.Click += new System.EventHandler(this.loadDataToolStripMenuItem_Click);
            // 
            // preferencesToolStripMenuItem
            // 
            this.preferencesToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.setWorkingHoursToolStripMenuItem});
            this.preferencesToolStripMenuItem.Name = "preferencesToolStripMenuItem";
            this.preferencesToolStripMenuItem.Size = new System.Drawing.Size(99, 24);
            this.preferencesToolStripMenuItem.Text = "Preferences";
            // 
            // setWorkingHoursToolStripMenuItem
            // 
            this.setWorkingHoursToolStripMenuItem.Name = "setWorkingHoursToolStripMenuItem";
            this.setWorkingHoursToolStripMenuItem.Size = new System.Drawing.Size(215, 26);
            this.setWorkingHoursToolStripMenuItem.Text = "Set Working Hours";
            this.setWorkingHoursToolStripMenuItem.Click += new System.EventHandler(this.setWorkingHoursToolStripMenuItem_Click);
            // 
            // addTaskToolStripMenuItem
            // 
            this.addTaskToolStripMenuItem.Name = "addTaskToolStripMenuItem";
            this.addTaskToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.T)));
            this.addTaskToolStripMenuItem.Size = new System.Drawing.Size(224, 26);
            this.addTaskToolStripMenuItem.Text = "Add Task";
            this.addTaskToolStripMenuItem.Click += new System.EventHandler(this.addTaskToolStripMenuItem_Click);
            // 
            // DatePicker
            // 
            this.DatePicker.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.DatePicker.AutoSize = true;
            this.DatePicker.BackColor = System.Drawing.Color.Transparent;
            this.DatePicker.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.DatePicker.Location = new System.Drawing.Point(57, 0);
            this.DatePicker.Margin = new System.Windows.Forms.Padding(0);
            this.DatePicker.Name = "DatePicker";
            this.DatePicker.Size = new System.Drawing.Size(180, 46);
            this.DatePicker.TabIndex = 2;
            // 
            // CalendarView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(120F, 120F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.ClientSize = new System.Drawing.Size(605, 951);
            this.Controls.Add(this.PreviousDayButton);
            this.Controls.Add(this.NextDayButton);
            this.Controls.Add(this.TitlePanel);
            this.Controls.Add(this.MainMenu);
            this.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.MainMenuStrip = this.MainMenu;
            this.Margin = new System.Windows.Forms.Padding(5, 6, 5, 6);
            this.Name = "CalendarView";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Calendar View";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.CalendarView_FormClosing);
            this.TitlePanel.ResumeLayout(false);
            this.TitlePanel.PerformLayout();
            this.DatePanel.ResumeLayout(false);
            this.DatePanel.PerformLayout();
            this.ButtonsPanel.ResumeLayout(false);
            this.ButtonsPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.TaskList)).EndInit();
            this.MainMenu.ResumeLayout(false);
            this.MainMenu.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        private void CalendarView_FormClosing(object sender, FormClosingEventArgs e)
        {
            Settings.Save();
        }

        private void PerformSearch(DateTime date)
        {
            CurrentDay = Settings.MyCalendar.getDayByDate(date.Date);
            this.CurrentDateLabel.Text = date.ToShortDateString();
            LoadTasks();
        }

        private void SearchButton_Click(object sender, EventArgs e)
        {
            PerformSearch(DatePicker.GetDate());
        }

        private void AddTaskButton_Click(object sender, EventArgs e)
        {
            TaskView createTask = new TaskView((t, oldTask) => {
                if (!Settings.MyCalendar.doesTaskExist(t)) { Settings.MyCalendar.addTask(t); return true; }
                return false;
            });

            createTask.ShowDialog();
            DateTime d = DateTime.Parse(CurrentDateLabel.Text);
            PerformSearch(d.Date);
        }

        private void leftButtonClick(object sender, EventArgs e)
        {
            DateTime date = DateTime.Parse(CurrentDateLabel.Text).AddDays(-1);
            PerformSearch(date);
        }

        private void rightButtonClick(object sender, EventArgs e)
        {
            DateTime date = DateTime.Parse(CurrentDateLabel.Text).AddDays(1);
            PerformSearch(date);
        }

        private void TaskList_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                int index = TaskList.HitTest(e.X, e.Y).RowIndex;

                if (index > -1)
                {
                    TaskList.Rows[index].Selected = true;
                    ItemMenuStrip.Show(Cursor.Position);
                    ItemMenuStrip.Visible = true;
                }
                else
                {
                    ItemMenuStrip.Visible = false;
                }
            }
        }

        private void OnLoadFile()
        {
            PerformSearch(DateTime.Today);
        }

        /* ------------------------------ Constructors ------------------------------*/
        public CalendarView()
        {
            this.CurrentDay = Settings.MyCalendar.getDayByDate(DateTime.Today);
            this.ItemMenuStrip = new ContextMenuStrip();

            this.ItemMenuStrip.Items.Add("Modify", null, (sender, e) => {
                TaskView view = new TaskView((Task)TaskList.SelectedRows[0].Tag, (t, oldTask) => {
                    if (!Settings.MyCalendar.doesTaskExist(t))
                    {
                        Settings.MyCalendar.deleteTask(CurrentDay, oldTask);
                        Settings.MyCalendar.addTask(t);
                        return true;
                    }
                    return false;
                });
                view.ShowDialog();
                LoadTasks();
            });

            this.ItemMenuStrip.Items.Add("Delete", null, (sender, e) => {
                Settings.MyCalendar.deleteTask(CurrentDay, (Task)TaskList.SelectedRows[0].Tag);
                LoadTasks();
            });

            InitializeComponent();
            PerformSearch(DateTime.Today);
            Settings.newFileLoaded += OnLoadFile;
        }

        private void loadDataToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog fileDialog = new OpenFileDialog();
            fileDialog.Filter = "Binary File (*.bin)|*.bin";
            fileDialog.Multiselect = false;
            fileDialog.CheckFileExists = true;
            fileDialog.CheckPathExists = true;

            if (fileDialog.ShowDialog() == DialogResult.OK)
            {
                Settings.Load(fileDialog.FileName);
            }
        }

        private void saveDataToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveDialog = new SaveFileDialog();
            saveDialog.Filter = "Binary File (*.bin)|*.bin";
            saveDialog.InitialDirectory = Environment.SpecialFolder.UserProfile.ToString();
            saveDialog.FileName = "data";
            saveDialog.CheckPathExists = true;


            if (saveDialog.ShowDialog() == DialogResult.OK)
            {
                Settings.Save(saveDialog.FileName);
            }
        }

        private void setWorkingHoursToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if(CurrentDay == null)
            {
                Settings.MyCalendar.addDaysUpToDate(DateTime.Parse(CurrentDateLabel.Text));
                CurrentDay = Settings.MyCalendar.getDayByDate(DateTime.Parse(CurrentDateLabel.Text));
            }

            SetWorkingHoursView setWorkingHoursView = new SetWorkingHoursView((from, to) => {
                (int, int) prev = CurrentDay.WorkingHoursInterval;
                CurrentDay.WorkingHoursInterval = (from, to);
                Settings.MyCalendar.changeWorkingHours(CurrentDay.Date, prev);
            }, CurrentDay.WorkingHoursInterval);

            if(setWorkingHoursView.ShowDialog() == DialogResult.OK)
            {
                LoadTasks();
            } 
        }

        private void addTaskToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AddTaskButton.PerformClick();
        }

        private void TaskList_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            this.ItemMenuStrip.Items[0].PerformClick();
        }
    }
}
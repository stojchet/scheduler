﻿using System;
using System.Windows.Forms;

namespace Scheduler.Forms
{
    public partial class CalendarView : Form
    {
        /* ------------------------------ Private Widgets ------------------------------*/
        private TableLayoutPanel TitlePanel;
        private Label TitleLabel;
        private Controls.DatePicker DatePicker;
        private FlowLayoutPanel DatePanel;
        private Label DateLabel;
        private Label CurrentDateLabel;
        private CheckedListBox TaskList;
        private Button SearchButton;
        private FlowLayoutPanel ButtonsPanel;
        private Button AddTaskButton;

        /* ------------------------------ Private Variables ------------------------------*/
        private Day CurrentDay;
        private Button NextDayButton;
        private Button PreviousDayButton;
        private Controls.HintTextBox setWorkingHours;
        private Label workingHours;
        private ContextMenuStrip ItemMenuStrip;

        /* ------------------------------ Private Methods ------------------------------*/
        private void LoadTasks()
        {
            this.TaskList.Items.Clear();

            if (CurrentDay != null)
            {
                foreach (Task task in CurrentDay.tasks)
                {
                    this.TaskList.Items.Add(task);
                }
            }
        }

        private void InitializeComponent()
        {
            this.TitlePanel = new System.Windows.Forms.TableLayoutPanel();
            this.TitleLabel = new System.Windows.Forms.Label();
            this.DatePanel = new System.Windows.Forms.FlowLayoutPanel();
            this.DateLabel = new System.Windows.Forms.Label();
            this.DatePicker = new Scheduler.Controls.DatePicker();
            this.SearchButton = new System.Windows.Forms.Button();
            this.CurrentDateLabel = new System.Windows.Forms.Label();
            this.TaskList = new System.Windows.Forms.CheckedListBox();
            this.ButtonsPanel = new System.Windows.Forms.FlowLayoutPanel();
            this.AddTaskButton = new System.Windows.Forms.Button();
            this.setWorkingHours = new Scheduler.Controls.HintTextBox();
            this.workingHours = new System.Windows.Forms.Label();
            this.NextDayButton = new System.Windows.Forms.Button();
            this.PreviousDayButton = new System.Windows.Forms.Button();
            this.TitlePanel.SuspendLayout();
            this.DatePanel.SuspendLayout();
            this.ButtonsPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // TitlePanel
            // 
            this.TitlePanel.ColumnCount = 1;
            this.TitlePanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.TitlePanel.Controls.Add(this.TitleLabel, 0, 0);
            this.TitlePanel.Controls.Add(this.DatePanel, 0, 1);
            this.TitlePanel.Controls.Add(this.CurrentDateLabel, 0, 2);
            this.TitlePanel.Controls.Add(this.TaskList, 0, 3);
            this.TitlePanel.Controls.Add(this.ButtonsPanel, 0, 4);
            this.TitlePanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.TitlePanel.Location = new System.Drawing.Point(0, 0);
            this.TitlePanel.Margin = new System.Windows.Forms.Padding(0);
            this.TitlePanel.Name = "TitlePanel";
            this.TitlePanel.RowCount = 5;
            this.TitlePanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 75F));
            this.TitlePanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 50F));
            this.TitlePanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 50F));
            this.TitlePanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.TitlePanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 50F));
            this.TitlePanel.Size = new System.Drawing.Size(605, 951);
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
            // TaskList
            // 
            this.TaskList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.TaskList.Font = new System.Drawing.Font("Courier New", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.TaskList.FormattingEnabled = true;
            this.TaskList.Location = new System.Drawing.Point(6, 175);
            this.TaskList.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.TaskList.Name = "TaskList";
            this.TaskList.Size = new System.Drawing.Size(593, 726);
            this.TaskList.TabIndex = 6;
            this.TaskList.MouseDown += new System.Windows.Forms.MouseEventHandler(this.TaskList_MouseDown);
            // 
            // ButtonsPanel
            // 
            this.ButtonsPanel.Controls.Add(this.AddTaskButton);
            this.ButtonsPanel.Controls.Add(this.setWorkingHours);
            this.ButtonsPanel.Controls.Add(this.workingHours);
            this.ButtonsPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ButtonsPanel.FlowDirection = System.Windows.Forms.FlowDirection.RightToLeft;
            this.ButtonsPanel.Location = new System.Drawing.Point(0, 901);
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
            // setWorkingHours
            // 
            this.setWorkingHours.ForeColor = System.Drawing.Color.DarkGray;
            this.setWorkingHours.Hint = null;
            this.setWorkingHours.Location = new System.Drawing.Point(400, 3);
            this.setWorkingHours.Name = "setWorkingHours";
            this.setWorkingHours.Size = new System.Drawing.Size(89, 34);
            this.setWorkingHours.TabIndex = 1;
            this.setWorkingHours.TextChanged += new System.EventHandler(this.setWorkingHours_TextChanged);
            // 
            // workingHours
            // 
            this.workingHours.AutoSize = true;
            this.workingHours.Location = new System.Drawing.Point(217, 0);
            this.workingHours.Name = "workingHours";
            this.workingHours.Size = new System.Drawing.Size(177, 28);
            this.workingHours.TabIndex = 2;
            this.workingHours.Text = "Set Working Hours";
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
            // CalendarView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(120F, 120F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.ClientSize = new System.Drawing.Size(605, 951);
            this.Controls.Add(this.PreviousDayButton);
            this.Controls.Add(this.NextDayButton);
            this.Controls.Add(this.TitlePanel);
            this.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Margin = new System.Windows.Forms.Padding(5, 6, 5, 6);
            this.Name = "CalendarView";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Calendar View";
            this.TitlePanel.ResumeLayout(false);
            this.TitlePanel.PerformLayout();
            this.DatePanel.ResumeLayout(false);
            this.DatePanel.PerformLayout();
            this.ButtonsPanel.ResumeLayout(false);
            this.ButtonsPanel.PerformLayout();
            this.ResumeLayout(false);

        }

        private void CalendarView_FormClosing(object sender, FormClosingEventArgs e)
        {
            Settings.Save();
        }

        private void PerformSearch(DateTime date)
        {
            CurrentDay = Settings.MyCalendar.getDayByDate(date);
            this.CurrentDateLabel.Text = date.ToShortDateString();
            LoadTasks();
        }

        private void SearchButton_Click(object sender, EventArgs e)
        {
            PerformSearch(DatePicker.GetDate());
        }

        private void AddTaskButton_Click(object sender, EventArgs e)
        {
            TaskView createTask = new TaskView((t) => {
                Settings.MyCalendar.addTask(t);
            });

            createTask.ShowDialog();
            PerformSearch(DateTime.Parse(CurrentDateLabel.Text));
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

        private void setWorkingHours_TextChanged(object sender, EventArgs e)
        {
            Day day = Settings.MyCalendar.getDayByDate(DateTime.Parse(CurrentDateLabel.Text));
            // day.workingHours = ;
        }

        private void TaskList_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right && CurrentDay.date >= DateTime.Now)
            {
                int index = TaskList.IndexFromPoint(e.Location);

                if (index != CheckedListBox.NoMatches)
                {
                    TaskList.SetSelected(index, true);
                    ItemMenuStrip.Show(Cursor.Position);
                    ItemMenuStrip.Visible = true;
                }
                else
                {
                    ItemMenuStrip.Visible = false;
                }
            }
        }

        /* ------------------------------ Constructors ------------------------------*/
        public CalendarView()
        {
            this.CurrentDay = Settings.MyCalendar.getDayByDate(DateTime.Now);
            this.ItemMenuStrip = new ContextMenuStrip();

            this.ItemMenuStrip.Items.Add("Modify", null, (sender, e) => {
                TaskView view = new TaskView((Task)TaskList.SelectedItem, (t) => {
                    this.CurrentDay.removeTask(t);
                    Settings.MyCalendar.addTask(t);
                });
                view.ShowDialog();
                LoadTasks();
            });

            this.ItemMenuStrip.Items.Add("Delete", null, (sender, e) => {
                Settings.MyCalendar.deleteTask(CurrentDay, (Task)TaskList.SelectedItem);
                LoadTasks();
            });

            InitializeComponent();
            PerformSearch(DateTime.Now);
        }
    }
}
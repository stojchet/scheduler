using System;
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
        private Calendar CurrentCalendar;
        private Day CurrentDay;
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
            this.TitlePanel = new TableLayoutPanel();
            this.TitleLabel = new Label();
            this.DatePanel = new FlowLayoutPanel();
            this.DateLabel = new Label();
            this.DatePicker = new Controls.DatePicker();
            this.SearchButton = new Button();
            this.CurrentDateLabel = new Label();
            this.TaskList = new CheckedListBox();
            this.ButtonsPanel = new FlowLayoutPanel();
            this.AddTaskButton = new Button();

            this.TitlePanel.SuspendLayout();
            this.DatePanel.SuspendLayout();
            this.ButtonsPanel.SuspendLayout();
            this.SuspendLayout();

            // 
            // TitlePanel
            // 
            this.TitlePanel.ColumnCount = 1;
            this.TitlePanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            this.TitlePanel.Controls.Add(this.TitleLabel, 0, 0);
            this.TitlePanel.Controls.Add(this.DatePanel, 0, 1);
            this.TitlePanel.Controls.Add(this.CurrentDateLabel, 0, 2);
            this.TitlePanel.Controls.Add(this.TaskList, 0, 3);
            this.TitlePanel.Controls.Add(this.ButtonsPanel, 0, 4);
            this.TitlePanel.Dock = DockStyle.Fill;
            this.TitlePanel.Location = new System.Drawing.Point(0, 0);
            this.TitlePanel.Margin = new Padding(0);
            this.TitlePanel.Name = "TitlePanel";
            this.TitlePanel.RowCount = 5;
            this.TitlePanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 60F));
            this.TitlePanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 40F));
            this.TitlePanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 40F));
            this.TitlePanel.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            this.TitlePanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 40F));
            this.TitlePanel.Size = new System.Drawing.Size(484, 761);
            this.TitlePanel.TabIndex = 0;

            // 
            // TitleLabel
            // 
            this.TitleLabel.Anchor = AnchorStyles.None;
            this.TitleLabel.AutoSize = true;
            this.TitleLabel.Font = new System.Drawing.Font("Segoe UI", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
            this.TitleLabel.Location = new System.Drawing.Point(152, 11);
            this.TitleLabel.Margin = new Padding(0);
            this.TitleLabel.Name = "TitleLabel";
            this.TitleLabel.Size = new System.Drawing.Size(179, 37);
            this.TitleLabel.TabIndex = 0;
            this.TitleLabel.Text = "My Calendar";

            // 
            // DatePanel
            // 
            this.DatePanel.Anchor = AnchorStyles.None;
            this.DatePanel.AutoSize = true;
            this.DatePanel.Controls.Add(this.DateLabel);
            this.DatePanel.Controls.Add(this.DatePicker);
            this.DatePanel.Controls.Add(this.SearchButton);
            this.DatePanel.Location = new System.Drawing.Point(107, 60);
            this.DatePanel.Margin = new Padding(0);
            this.DatePanel.Name = "DatePanel";
            this.DatePanel.Size = new System.Drawing.Size(270, 39);
            this.DatePanel.TabIndex = 4;

            // 
            // DateLabel
            // 
            this.DateLabel.Anchor = AnchorStyles.Right;
            this.DateLabel.AutoSize = true;
            this.DateLabel.Location = new System.Drawing.Point(0, 9);
            this.DateLabel.Margin = new Padding(0);
            this.DateLabel.Name = "DateLabel";
            this.DateLabel.Size = new System.Drawing.Size(45, 21);
            this.DateLabel.TabIndex = 3;
            this.DateLabel.Text = "Date:";

            // 
            // DatePicker
            // 
            this.DatePicker.Anchor = AnchorStyles.Left;
            this.DatePicker.AutoSize = true;
            this.DatePicker.BackColor = System.Drawing.Color.Transparent;
            this.DatePicker.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
            this.DatePicker.Location = new System.Drawing.Point(45, 0);
            this.DatePicker.Margin = new Padding(0);
            this.DatePicker.Name = "DatePicker";
            this.DatePicker.Size = new System.Drawing.Size(145, 39);
            this.DatePicker.TabIndex = 2;

            // 
            // SearchButton
            // 
            this.SearchButton.Anchor = AnchorStyles.None;
            this.SearchButton.AutoSize = true;
            this.SearchButton.Location = new System.Drawing.Point(195, 4);
            this.SearchButton.Margin = new Padding(5, 0, 0, 0);
            this.SearchButton.Name = "SearchButton";
            this.SearchButton.Size = new System.Drawing.Size(75, 31);
            this.SearchButton.TabIndex = 4;
            this.SearchButton.Text = "Search";
            this.SearchButton.UseVisualStyleBackColor = true;
            this.SearchButton.Click += new EventHandler(this.SearchButton_Click);

            // 
            // CurrentDateLabel
            // 
            this.CurrentDateLabel.Anchor = AnchorStyles.Left;
            this.CurrentDateLabel.AutoSize = true;
            this.CurrentDateLabel.Font = new System.Drawing.Font("Segoe UI", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
            this.CurrentDateLabel.Location = new System.Drawing.Point(3, 105);
            this.CurrentDateLabel.Name = "CurrentDateLabel";
            this.CurrentDateLabel.Size = new System.Drawing.Size(0, 30);
            this.CurrentDateLabel.TabIndex = 5;

            // 
            // TaskList
            // 
            this.TaskList.Dock = DockStyle.Fill;
            this.TaskList.Font = new System.Drawing.Font("Courier New", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
            this.TaskList.FormattingEnabled = true;
            this.TaskList.Location = new System.Drawing.Point(5, 140);
            this.TaskList.Margin = new Padding(5, 0, 5, 0);
            this.TaskList.Name = "TaskList";
            this.TaskList.Size = new System.Drawing.Size(474, 581);
            this.TaskList.TabIndex = 6;
            this.TaskList.MouseDown += new MouseEventHandler(this.TaskList_MouseDown);

            // 
            // ButtonsPanel
            // 
            this.ButtonsPanel.Controls.Add(this.AddTaskButton);
            this.ButtonsPanel.Dock = DockStyle.Fill;
            this.ButtonsPanel.FlowDirection = FlowDirection.RightToLeft;
            this.ButtonsPanel.Location = new System.Drawing.Point(0, 721);
            this.ButtonsPanel.Margin = new Padding(0);
            this.ButtonsPanel.Name = "ButtonsPanel";
            this.ButtonsPanel.Size = new System.Drawing.Size(484, 40);
            this.ButtonsPanel.TabIndex = 7;

            // 
            // AddTaskButton
            // 
            this.AddTaskButton.Anchor = AnchorStyles.None;
            this.AddTaskButton.AutoSize = true;
            this.AddTaskButton.Location = new System.Drawing.Point(398, 5);
            this.AddTaskButton.Margin = new Padding(5);
            this.AddTaskButton.Name = "AddTaskButton";
            this.AddTaskButton.Size = new System.Drawing.Size(81, 31);
            this.AddTaskButton.TabIndex = 0;
            this.AddTaskButton.Text = "Add Task";
            this.AddTaskButton.UseVisualStyleBackColor = true;
            this.AddTaskButton.Click += new EventHandler(this.AddTaskButton_Click);

            // 
            // CalendarView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = AutoScaleMode.Dpi;
            this.ClientSize = new System.Drawing.Size(484, 761);
            this.Controls.Add(this.TitlePanel);
            this.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
            this.Margin = new Padding(4, 5, 4, 5);
            this.Name = "CalendarView";
            this.ShowIcon = false;
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Text = "Calendar View";


            this.TitlePanel.ResumeLayout(false);
            this.TitlePanel.PerformLayout();
            this.DatePanel.ResumeLayout(false);
            this.DatePanel.PerformLayout();
            this.ButtonsPanel.ResumeLayout(false);
            this.ButtonsPanel.PerformLayout();
            this.ResumeLayout(false);
        }

        private void PerformSearch(DateTime date)
        {
            if (Calendar.isDateValid(date.ToShortDateString()))
            {
                CurrentDay = CurrentCalendar.getDayByDate(date);
                this.CurrentDateLabel.Text = date.ToShortDateString();
                LoadTasks();
            }
        }

        private void SearchButton_Click(object sender, EventArgs e)
        {
            PerformSearch(DatePicker.GetDate());
        }

        private void AddTaskButton_Click(object sender, EventArgs e)
        {
            TaskView createTask = new TaskView((t) => {
                CurrentCalendar.addTask(t);
            });
            createTask.ShowDialog();
            LoadTasks();
        }

        private void TaskList_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
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
            this.ItemMenuStrip = new ContextMenuStrip();

            this.ItemMenuStrip.Items.Add("Modify", null, (sender, e) => {
                TaskView view = new TaskView((Task)TaskList.SelectedItem);
                view.ShowDialog();
                LoadTasks();
            });

            this.ItemMenuStrip.Items.Add("Delete", null, (sender, e) => {
                CurrentCalendar.deleteTask(CurrentDay, (Task)TaskList.SelectedItem);
                LoadTasks();
            });

            InitializeComponent();
            
            this.CurrentCalendar = new Calendar();
            this.CurrentCalendar.defaultWorkingHours = 12;
            PerformSearch(DateTime.Now);
        }
    }
}

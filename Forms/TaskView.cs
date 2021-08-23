using System;
using System.Windows.Forms;

namespace Scheduler.Forms
{
    public class TaskView : Form
    {
        /* ------------------------------ Private Widgets ------------------------------*/
        private TableLayoutPanel MainLayout;
        private Label TitleLabel;
        private GroupBox TaskInformationGroup;
        private Label TaskTypeLabel;
        private Controls.DatePicker DatePickerDeadline;
        private Label TaskDeadlineLabel;
        private TextBox TaskName;
        private Label TaskNameLabel;
        private RadioButton TaskTypeNormal;
        private RadioButton TaskTypeFixed;
        private TextBox Duration;
        private Label TaskDurationLabel;
        private Button FormButton;

        /* ------------------------------ Public Variables ------------------------------*/
        public delegate bool TaskAction(Task task, Task oldTask);

        /* ------------------------------ Private Variables ------------------------------*/
        private TaskAction CallerAction;
        private Task CurrentTask = null;
        private bool close = true;

        /* ------------------------------ Private Methods ------------------------------*/
        private void InitializeComponent()
        {
            this.MainLayout = new TableLayoutPanel();
            this.TitleLabel = new Label();
            this.TaskInformationGroup = new GroupBox();
            this.Duration = new TextBox();
            this.TaskDurationLabel = new Label();
            this.TaskTypeFixed = new RadioButton();
            this.TaskTypeNormal = new RadioButton();
            this.TaskTypeLabel = new Label();
            this.DatePickerDeadline = new Controls.DatePicker();
            this.TaskDeadlineLabel = new Label();
            this.TaskName = new TextBox();
            this.TaskNameLabel = new Label();
            this.FormButton = new Button();

            this.MainLayout.SuspendLayout();
            this.TaskInformationGroup.SuspendLayout();
            this.SuspendLayout();

            // 
            // MainLayout
            // 
            this.MainLayout.ColumnCount = 1;
            this.MainLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            this.MainLayout.Controls.Add(this.TitleLabel, 0, 0);
            this.MainLayout.Controls.Add(this.TaskInformationGroup, 0, 1);
            this.MainLayout.Controls.Add(this.FormButton, 0, 2);
            this.MainLayout.Dock = DockStyle.Fill;
            this.MainLayout.Location = new System.Drawing.Point(0, 0);
            this.MainLayout.Margin = new Padding(0);
            this.MainLayout.Name = "MainLayout";
            this.MainLayout.RowCount = 3;
            this.MainLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 40F));
            this.MainLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            this.MainLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 40F));
            this.MainLayout.Size = new System.Drawing.Size(370, 331);
            this.MainLayout.TabIndex = 0;

            // 
            // TitleLabel
            // 
            this.TitleLabel.Anchor = AnchorStyles.None;
            this.TitleLabel.AutoSize = true;
            this.TitleLabel.Font = new System.Drawing.Font("Segoe UI", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
            this.TitleLabel.Location = new System.Drawing.Point(153, 4);
            this.TitleLabel.Margin = new Padding(0);
            this.TitleLabel.Name = "TitleLabel";
            this.TitleLabel.Size = new System.Drawing.Size(64, 32);
            this.TitleLabel.TabIndex = 0;
            this.TitleLabel.Text = "Task";

            // 
            // TaskInformationGroup
            // 
            this.TaskInformationGroup.Controls.Add(this.Duration);
            this.TaskInformationGroup.Controls.Add(this.TaskDurationLabel);
            this.TaskInformationGroup.Controls.Add(this.TaskTypeFixed);
            this.TaskInformationGroup.Controls.Add(this.TaskTypeNormal);
            this.TaskInformationGroup.Controls.Add(this.TaskTypeLabel);
            this.TaskInformationGroup.Controls.Add(this.DatePickerDeadline);
            this.TaskInformationGroup.Controls.Add(this.TaskDeadlineLabel);
            this.TaskInformationGroup.Controls.Add(this.TaskName);
            this.TaskInformationGroup.Controls.Add(this.TaskNameLabel);
            this.TaskInformationGroup.Dock = DockStyle.Fill;
            this.TaskInformationGroup.Location = new System.Drawing.Point(5, 45);
            this.TaskInformationGroup.Margin = new Padding(5);
            this.TaskInformationGroup.Name = "TaskInformationGroup";
            this.TaskInformationGroup.Padding = new Padding(0);
            this.TaskInformationGroup.Size = new System.Drawing.Size(360, 241);
            this.TaskInformationGroup.TabIndex = 1;
            this.TaskInformationGroup.TabStop = false;
            this.TaskInformationGroup.Text = "Task Information";

            // 
            // Duration
            // 
            this.Duration.Location = new System.Drawing.Point(101, 130);
            this.Duration.Margin = new Padding(5);
            this.Duration.Name = "Duration";
            this.Duration.Size = new System.Drawing.Size(100, 29);
            this.Duration.TabIndex = 5;
            this.Duration.KeyPress += Duration_KeyPress;

            // 
            // TaskDurationLabel
            // 
            this.TaskDurationLabel.AutoSize = true;
            this.TaskDurationLabel.Location = new System.Drawing.Point(21, 133);
            this.TaskDurationLabel.Name = "TaskDurationLabel";
            this.TaskDurationLabel.Size = new System.Drawing.Size(74, 21);
            this.TaskDurationLabel.TabIndex = 4;
            this.TaskDurationLabel.Text = "Duration:";

            // 
            // TaskTypeFixed
            // 
            this.TaskTypeFixed.AutoSize = true;
            this.TaskTypeFixed.Location = new System.Drawing.Point(101, 204);
            this.TaskTypeFixed.Margin = new Padding(5);
            this.TaskTypeFixed.Name = "TaskTypeFixed";
            this.TaskTypeFixed.Size = new System.Drawing.Size(64, 25);
            this.TaskTypeFixed.TabIndex = 8;
            this.TaskTypeFixed.Text = "Fixed";
            this.TaskTypeFixed.UseVisualStyleBackColor = true;

            // 
            // TaskTypeNormal
            // 
            this.TaskTypeNormal.AutoSize = true;
            this.TaskTypeNormal.Checked = true;
            this.TaskTypeNormal.Location = new System.Drawing.Point(101, 169);
            this.TaskTypeNormal.Margin = new Padding(5);
            this.TaskTypeNormal.Name = "TaskTypeNormal";
            this.TaskTypeNormal.Size = new System.Drawing.Size(81, 25);
            this.TaskTypeNormal.TabIndex = 7;
            this.TaskTypeNormal.TabStop = true;
            this.TaskTypeNormal.Text = "Normal";
            this.TaskTypeNormal.UseVisualStyleBackColor = true;

            // 
            // TaskTypeLabel
            // 
            this.TaskTypeLabel.AutoSize = true;
            this.TaskTypeLabel.Location = new System.Drawing.Point(17, 169);
            this.TaskTypeLabel.Name = "TaskTypeLabel";
            this.TaskTypeLabel.Size = new System.Drawing.Size(78, 21);
            this.TaskTypeLabel.TabIndex = 6;
            this.TaskTypeLabel.Text = "Task Type:";

            // 
            // DatePickerDeadline
            // 
            this.DatePickerDeadline.BackColor = System.Drawing.Color.Transparent;
            this.DatePickerDeadline.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.DatePickerDeadline.Location = new System.Drawing.Point(96, 80);
            this.DatePickerDeadline.Margin = new Padding(5);
            this.DatePickerDeadline.Name = "DatePickerDeadline";
            this.DatePickerDeadline.Size = new System.Drawing.Size(147, 40);
            this.DatePickerDeadline.TabIndex = 3;

            // 
            // TaskDeadlineLabel
            // 
            this.TaskDeadlineLabel.AutoSize = true;
            this.TaskDeadlineLabel.Location = new System.Drawing.Point(21, 90);
            this.TaskDeadlineLabel.Name = "TaskDeadlineLabel";
            this.TaskDeadlineLabel.Size = new System.Drawing.Size(74, 21);
            this.TaskDeadlineLabel.TabIndex = 2;
            this.TaskDeadlineLabel.Text = "Deadline:";

            // 
            // TaskName
            // 
            this.TaskName.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            this.TaskName.Location = new System.Drawing.Point(101, 41);
            this.TaskName.Margin = new Padding(5);
            this.TaskName.Name = "TaskName";
            this.TaskName.Size = new System.Drawing.Size(252, 29);
            this.TaskName.TabIndex = 1;

            // 
            // TaskNameLabel
            // 
            this.TaskNameLabel.AutoSize = true;
            this.TaskNameLabel.Location = new System.Drawing.Point(7, 44);
            this.TaskNameLabel.Name = "TaskNameLabel";
            this.TaskNameLabel.Size = new System.Drawing.Size(88, 21);
            this.TaskNameLabel.TabIndex = 0;
            this.TaskNameLabel.Text = "Task Name:";

            // 
            // FormButton
            // 
            this.FormButton.Anchor = AnchorStyles.Right;
            this.FormButton.AutoSize = true;
            this.FormButton.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            this.FormButton.Location = new System.Drawing.Point(359, 308);
            this.FormButton.Margin = new Padding(5);
            this.FormButton.Name = "FormButton";
            this.FormButton.Size = new System.Drawing.Size(6, 6);
            this.FormButton.TabIndex = 2;
            this.FormButton.UseVisualStyleBackColor = true;
            this.FormButton.Click += new EventHandler(this.FormButton_Click);

            // 
            // TaskView
            // 
            this.AcceptButton = this.FormButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = AutoScaleMode.Dpi;
            this.ClientSize = new System.Drawing.Size(370, 331);
            this.Controls.Add(this.MainLayout);
            this.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
            this.Name = "TaskView";
            this.ShowIcon = false;
            this.StartPosition = FormStartPosition.CenterParent;
            this.FormClosing += TaskView_FormClosing;
            this.Text = "Task View";

            this.MainLayout.ResumeLayout(false);
            this.MainLayout.PerformLayout();
            this.TaskInformationGroup.ResumeLayout(false);
            this.TaskInformationGroup.PerformLayout();
            this.ResumeLayout(false);
        }

        private void TaskView_FormClosing(object sender, FormClosingEventArgs e)
        {
            Settings.MyCalendar.ErrorInTaskParameters -= showErrorInTaskMessage;
        }

        private void Duration_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);
        }

        private bool AllFieldsCorrect()
        {
            DateTime val;
            if(DateTime.TryParse(DatePickerDeadline.GetDate().ToString(), out val))
            {
                return this.TaskName.Text.Length > 0 && DateTime.Today <= val && Duration.Text.Length > 0;
            }
            return false;
        }

        private bool hasDataBeenChanged() => CurrentTask.Name != this.TaskName.Text 
            || CurrentTask.Deadline != this.DatePickerDeadline.GetDate() 
            || CurrentTask.getFullTaskDuration() != int.Parse(this.Duration.Text) 
            || CurrentTask.Type != (this.TaskTypeNormal.Checked ? Type.NORMAL : Type.FIXED);

        private void FormButton_Click(object sender, EventArgs e)
        {
            if (AllFieldsCorrect())
            {
                if (CurrentTask == null)
                {
                    CurrentTask = new Task(this.TaskName.Text, this.DatePickerDeadline.GetDate(), 
                        this.Duration.Text.Length > 0 ? int.Parse(this.Duration.Text) : 0,
                        this.TaskTypeNormal.Checked ? Type.NORMAL : Type.FIXED);
                    if (CallerAction != null && !CallerAction.Invoke(CurrentTask, null))
                    {
                        showErrorInTaskMessage();
                    }
                }
                else
                {
                    if (hasDataBeenChanged())
                    {
                        Task modifiedTask = new Task(this.TaskName.Text, this.DatePickerDeadline.GetDate(), 
                        this.Duration.Text.Length > 0 ? int.Parse(this.Duration.Text) : 0,
                        this.TaskTypeNormal.Checked ? Type.NORMAL : Type.FIXED);

                        if (CallerAction != null && !CallerAction.Invoke(modifiedTask, CurrentTask))
                        {
                            showErrorInTaskMessage();
                        }
                    }
                }

                (close ? (Action)(() => this.Close()) : () => close = true)();
            }
            else
            {
                MessageBox.Show("Inavild Arguments", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void showErrorInTaskMessage()
        {
            MessageBox.Show("Error while adding task.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            close = false;
        }

        /* ------------------------------ Constructors ------------------------------*/
        public TaskView()
        {
            InitializeComponent();
            Settings.MyCalendar.ErrorInTaskParameters += showErrorInTaskMessage;
        }

        public TaskView(TaskAction createTaskAction) : this()
        {
            CallerAction = createTaskAction;
            this.TitleLabel.Text = "Create Task";
            this.FormButton.Text = "Create";
        }

        public TaskView(Task task, TaskAction reorderAction) : this()
        {
            CurrentTask = task;
            CallerAction = reorderAction;
            this.TaskName.Text = task.Name;
            this.DatePickerDeadline.SetDate(task.Deadline);
            this.Duration.Text = task.getFullTaskDuration().ToString();

            if (task.Type == Type.NORMAL)
            {
                this.TaskTypeNormal.Checked = true;
            }
            else
            {
                this.TaskTypeFixed.Checked = true;
            }

            this.TitleLabel.Text = "Modify Task";
            this.FormButton.Text = "Save";
        }
    }
}
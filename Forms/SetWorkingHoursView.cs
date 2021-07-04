using System;
using System.Windows.Forms;

namespace Scheduler.Forms
{
    public class SetWorkingHoursView : Form
    {
        /* ------------------------------ Widgets ------------------------------ */
        private TableLayoutPanel MainLayout;
        private Label TitleLabel;
        private FlowLayoutPanel mainFlowPanel;
        private Label FromLabel;
        private TextBox from;
        private Label ToLabel;
        private TextBox to;
        private FlowLayoutPanel buttonsFlowPanel;
        private Button Set;
        private Button Cancel;

        public delegate void setWorkingHoursHandler(int from, int to);
        public event setWorkingHoursHandler setWorkingHoursFunc;

        /* ------------------------------ Private Methods ------------------------------ */
        private void InitializeComponent()
        {
            this.MainLayout = new System.Windows.Forms.TableLayoutPanel();
            this.TitleLabel = new System.Windows.Forms.Label();
            this.mainFlowPanel = new System.Windows.Forms.FlowLayoutPanel();
            this.FromLabel = new System.Windows.Forms.Label();
            this.from = new System.Windows.Forms.TextBox();
            this.ToLabel = new System.Windows.Forms.Label();
            this.to = new System.Windows.Forms.TextBox();
            this.buttonsFlowPanel = new System.Windows.Forms.FlowLayoutPanel();
            this.Set = new System.Windows.Forms.Button();
            this.Cancel = new System.Windows.Forms.Button();
            this.MainLayout.SuspendLayout();
            this.mainFlowPanel.SuspendLayout();
            this.buttonsFlowPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // MainLayout
            // 
            this.MainLayout.ColumnCount = 1;
            this.MainLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.MainLayout.Controls.Add(this.TitleLabel, 0, 0);
            this.MainLayout.Controls.Add(this.mainFlowPanel, 0, 1);
            this.MainLayout.Controls.Add(this.buttonsFlowPanel, 0, 2);
            this.MainLayout.Dock = System.Windows.Forms.DockStyle.Fill;
            this.MainLayout.Location = new System.Drawing.Point(0, 0);
            this.MainLayout.Name = "MainLayout";
            this.MainLayout.RowCount = 3;
            this.MainLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.MainLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.MainLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 45F));
            this.MainLayout.Size = new System.Drawing.Size(344, 194);
            this.MainLayout.TabIndex = 0;
            // 
            // TitleLabel
            // 
            this.TitleLabel.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.TitleLabel.AutoSize = true;
            this.TitleLabel.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.TitleLabel.Location = new System.Drawing.Point(77, 6);
            this.TitleLabel.Margin = new System.Windows.Forms.Padding(0);
            this.TitleLabel.Name = "TitleLabel";
            this.TitleLabel.Size = new System.Drawing.Size(190, 28);
            this.TitleLabel.TabIndex = 0;
            this.TitleLabel.Text = "Set Working Hours";
            // 
            // mainFlowPanel
            // 
            this.mainFlowPanel.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.mainFlowPanel.AutoSize = true;
            this.mainFlowPanel.Controls.Add(this.FromLabel);
            this.mainFlowPanel.Controls.Add(this.from);
            this.mainFlowPanel.Controls.Add(this.ToLabel);
            this.mainFlowPanel.Controls.Add(this.to);
            this.mainFlowPanel.Location = new System.Drawing.Point(81, 50);
            this.mainFlowPanel.Margin = new System.Windows.Forms.Padding(0);
            this.mainFlowPanel.Name = "mainFlowPanel";
            this.mainFlowPanel.Size = new System.Drawing.Size(182, 88);
            this.mainFlowPanel.TabIndex = 1;
            // 
            // FromLabel
            // 
            this.FromLabel.AutoSize = true;
            this.FromLabel.Location = new System.Drawing.Point(5, 5);
            this.FromLabel.Margin = new System.Windows.Forms.Padding(5);
            this.FromLabel.Name = "FromLabel";
            this.FromLabel.Size = new System.Drawing.Size(62, 28);
            this.FromLabel.TabIndex = 0;
            this.FromLabel.Text = "From:";
            // 
            // from
            // 
            this.mainFlowPanel.SetFlowBreak(this.from, true);
            this.from.Location = new System.Drawing.Point(77, 5);
            this.from.Margin = new System.Windows.Forms.Padding(5);
            this.from.MaxLength = 2;
            this.from.Name = "from";
            this.from.Size = new System.Drawing.Size(100, 34);
            this.from.TabIndex = 1;
            this.from.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.Field_KeyPress);
            // 
            // ToLabel
            // 
            this.ToLabel.AutoSize = true;
            this.ToLabel.Location = new System.Drawing.Point(30, 49);
            this.ToLabel.Margin = new System.Windows.Forms.Padding(30, 5, 5, 5);
            this.ToLabel.Name = "ToLabel";
            this.ToLabel.Size = new System.Drawing.Size(36, 28);
            this.ToLabel.TabIndex = 2;
            this.ToLabel.Text = "To:";
            // 
            // to
            // 
            this.to.Location = new System.Drawing.Point(76, 49);
            this.to.Margin = new System.Windows.Forms.Padding(5);
            this.to.MaxLength = 2;
            this.to.Name = "to";
            this.to.Size = new System.Drawing.Size(100, 34);
            this.to.TabIndex = 3;
            this.to.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.Field_KeyPress);
            // 
            // buttonsFlowPanel
            // 
            this.buttonsFlowPanel.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.buttonsFlowPanel.AutoSize = true;
            this.buttonsFlowPanel.Controls.Add(this.Set);
            this.buttonsFlowPanel.Controls.Add(this.Cancel);
            this.buttonsFlowPanel.FlowDirection = System.Windows.Forms.FlowDirection.RightToLeft;
            this.buttonsFlowPanel.Location = new System.Drawing.Point(175, 152);
            this.buttonsFlowPanel.Margin = new System.Windows.Forms.Padding(0);
            this.buttonsFlowPanel.Name = "buttonsFlowPanel";
            this.buttonsFlowPanel.Size = new System.Drawing.Size(169, 38);
            this.buttonsFlowPanel.TabIndex = 2;
            // 
            // Set
            // 
            this.Set.AutoSize = true;
            this.Set.Location = new System.Drawing.Point(89, 0);
            this.Set.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.Set.Name = "Set";
            this.Set.Size = new System.Drawing.Size(75, 38);
            this.Set.TabIndex = 1;
            this.Set.Text = "Set";
            this.Set.UseVisualStyleBackColor = true;
            this.Set.Click += new System.EventHandler(this.SetButton_Click);
            // 
            // Cancel
            // 
            this.Cancel.AutoSize = true;
            this.Cancel.Location = new System.Drawing.Point(0, 0);
            this.Cancel.Margin = new System.Windows.Forms.Padding(0, 0, 5, 0);
            this.Cancel.Name = "Cancel";
            this.Cancel.Size = new System.Drawing.Size(79, 38);
            this.Cancel.TabIndex = 0;
            this.Cancel.Text = "Cancel";
            this.Cancel.UseVisualStyleBackColor = true;
            this.Cancel.Click += new System.EventHandler(this.CancelButton_Click);
            // 
            // SetWorkingHoursView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(120F, 120F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.ClientSize = new System.Drawing.Size(344, 194);
            this.Controls.Add(this.MainLayout);
            this.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "SetWorkingHoursView";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Set Working Hours";
            this.MainLayout.ResumeLayout(false);
            this.MainLayout.PerformLayout();
            this.mainFlowPanel.ResumeLayout(false);
            this.mainFlowPanel.PerformLayout();
            this.buttonsFlowPanel.ResumeLayout(false);
            this.buttonsFlowPanel.PerformLayout();
            this.ResumeLayout(false);

        }


        public SetWorkingHoursView(setWorkingHoursHandler func)
        {
            InitializeComponent();
            this.setWorkingHoursFunc = func;
        }

        private void SetButton_Click(object sender, EventArgs e)
        {
            if (from.Text.Length > 0 || to.Text.Length > 0)
            {
                setWorkingHoursFunc?.Invoke(int.Parse(from.Text), int.Parse(to.Text));
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            else
            {
                MessageBox.Show("Not valid values!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void CancelButton_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private bool isValueValid(string value)
        {
            int val;
            bool result = int.TryParse(value, out val);
            return result && val >= 0 && val <= 24;
        }

        private void Field_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = (!char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar)) || !isValueValid(((TextBox)sender).Text + e.KeyChar) && e.KeyChar != '\b';
        }
    }
}

using System.Windows.Forms;

namespace Scheduler.Controls
{
    public partial class DatePicker : UserControl
    {
        /* ------------------------------ Private Variables ------------------------------*/
        private HintTextBox day;
        private HintTextBox month;
        private HintTextBox year;

        /* ------------------------------ Public Variables ------------------------------*/
        public int Day { get => day.IsHintDisplayed ? 0 : int.Parse(day.Text); }
        public int Month { get => month.IsHintDisplayed ? 0 : int.Parse(month.Text); }
        public int Year { get => year.IsHintDisplayed ? 0 : int.Parse(year.Text); }
        public string Date { get => $"{this.Day}/{this.Month}/{this.Year}";  }

        /* ------------------------------ Private Methods ------------------------------*/
        private void InitializeComponent()
        {
            this.day = new HintTextBox();
            this.month = new HintTextBox();
            this.year = new HintTextBox();

            this.SuspendLayout();

            // 
            // day
            // 
            this.day.ForeColor = System.Drawing.Color.DarkGray;
            this.day.Hint = "dd";
            this.day.Location = new System.Drawing.Point(5, 5);
            this.day.Margin = new Padding(5);
            this.day.MaxLength = 2;
            this.day.Name = "day";
            this.day.Size = new System.Drawing.Size(35, 29);
            this.day.TabIndex = 0;
            this.day.Text = "dd";
            this.day.TextAlign = HorizontalAlignment.Center;

            // 
            // month
            // 
            this.month.ForeColor = System.Drawing.Color.DarkGray;
            this.month.Hint = "mm";
            this.month.Location = new System.Drawing.Point(50, 5);
            this.month.Margin = new Padding(5);
            this.month.MaxLength = 2;
            this.month.Name = "month";
            this.month.Size = new System.Drawing.Size(35, 29);
            this.month.TabIndex = 1;
            this.month.Text = "mm";
            this.month.TextAlign = HorizontalAlignment.Center;

            // 
            // year
            // 
            this.year.ForeColor = System.Drawing.Color.DarkGray;
            this.year.Hint = "yyyy";
            this.year.Location = new System.Drawing.Point(95, 5);
            this.year.Margin = new Padding(5);
            this.year.MaxLength = 4;
            this.year.Name = "year";
            this.year.Size = new System.Drawing.Size(45, 29);
            this.year.TabIndex = 2;
            this.year.Text = "yyyy";
            this.year.TextAlign = HorizontalAlignment.Center;

            // 
            // DatePicker
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = AutoScaleMode.Dpi;
            this.BackColor = System.Drawing.Color.Transparent;
            this.Controls.Add(this.year);
            this.Controls.Add(this.month);
            this.Controls.Add(this.day);
            this.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.Margin = new Padding(0);
            this.Name = "DatePicker";
            this.Size = new System.Drawing.Size(147, 40);

            this.ResumeLayout();
        }

        private void HintTextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);

            if(e.Handled)
            {
                return;
            }

            HintTextBox box = (HintTextBox)sender;

            if (!box.IsHintDisplayed && e.KeyChar != (char)Keys.Back && e.KeyChar != (char)Keys.Delete)
            {
                if (box.Text.Length + 1 == box.MaxLength)
                {
                    this.SelectNextControl(box, true, true, true, false);
                }
            }
        }

        /* ------------------------------ Constructors ------------------------------*/
        public DatePicker()
        {
            InitializeComponent();

            this.day.KeyPress += HintTextBox_KeyPress;
            this.month.KeyPress += HintTextBox_KeyPress;
            this.year.KeyPress += HintTextBox_KeyPress;
        }

        /* ------------------------------ Public Methods ------------------------------*/
        public System.DateTime GetDate()
        {
            try
            {
                return new System.DateTime(this.Year, this.Month, this.Day);
            }
            catch(System.Exception)
            {
                return new System.DateTime();
            }
        }

        public void SetDate(System.DateTime date)
        {
            this.day.SetText(date.Day.ToString());
            this.month.SetText(date.Month.ToString());
            this.year.SetText(date.Year.ToString());
        }
    }
}

namespace Scheduler.Controls
{
    public class HintTextBox : System.Windows.Forms.TextBox
    {
        private enum TextBoxStyle { Normal, Hint }

        /* ------------------------------ Private Variables ------------------------------ */
        private string hint;

        public string Hint
        {
            get
            {
                return hint;
            }
            set
            {
                hint = value;

                if(this.Text.Length == 0)
                {
                    IsHintDisplayed = true;
                    SetStyle(TextBoxStyle.Hint);
                    this.Text = value;
                }
            }
        }

        /* ------------------------------ Public Variables ------------------------------ */
        public bool IsHintDisplayed { get; private set; } = false;

        /* ------------------------------ Private Methods ------------------------------ */
        private void SetStyle(TextBoxStyle style)
        {
            switch (style)
            {
                case TextBoxStyle.Normal:
                    this.ForeColor = System.Drawing.Color.Black;
                    break;

                case TextBoxStyle.Hint:
                    this.ForeColor = System.Drawing.Color.DarkGray;
                    break;
            }
        }
        private void HintTextBox_Enter(object sender, System.EventArgs e)
        {
            if(this.IsHintDisplayed)
            {
                this.Text = "";
                this.IsHintDisplayed = false;
                SetStyle(TextBoxStyle.Normal);
            }
        }

        private void HintTextBox_Leave(object sender, System.EventArgs e)
        {
            if(this.Text.Length == 0 && this.Hint.Length > 0)
            {
                this.IsHintDisplayed = true;
                SetStyle(TextBoxStyle.Hint);
                this.Text = Hint;
            }
        }

        /* ------------------------------ Constructors ------------------------------ */
        public HintTextBox()
        {
            this.Enter += HintTextBox_Enter;
            this.Leave += HintTextBox_Leave;
        }

        /* ------------------------------ Public Methods ------------------------------ */
        public void SetText(string text)
        {
            IsHintDisplayed = false;
            SetStyle(TextBoxStyle.Normal);
            this.Text = text;
        }
    }
}

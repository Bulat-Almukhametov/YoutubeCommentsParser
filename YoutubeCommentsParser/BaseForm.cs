using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using YoutubeCommentsParser.Models;

namespace YoutubeCommentsParser
{
    public partial class BaseForm : Form
    {
        public BaseForm()
        {
            InitializeComponent();
        }
        
        public BaseForm NextForm { get; set; }
        public BaseForm PreviousForm { get; set; }

        public bool SkipSaveFile { get; set; }

        private void BaseForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (NextForm != null || SkipSaveFile)
                return;

            if (AppDataRepository.YoutubeKey == null)
                return;

            var result = MessageBox.Show("Хотите сохранить внесенные изменения?",
                "Сохранение",
                MessageBoxButtons.YesNoCancel);

            switch (result)
            {
                case DialogResult.Yes:
                    AppDataRepository.Save();
                    break;

                case DialogResult.No:
                    break;

                default:
                    e.Cancel = true;
                    break;
            }

            
            PreviousForm?.Close();
        }

        protected void backButton_Click(object sender, EventArgs e)
        {
            Hide();
            PreviousForm.Show();
            PreviousForm.NextForm = null;

            SkipSaveFile = true;
            Close();

        }
    }
}

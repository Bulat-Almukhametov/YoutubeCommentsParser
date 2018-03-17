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
    public partial class ProjectsForm : Form
    {
        public Form KeyForm;
        public ProjectsForm()
        {
            InitializeComponent();
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            string url = Properties.Settings.Default["DeveloperSiteUrl"].ToString();
            System.Diagnostics.Process.Start(url);
        }

        private void AddRowButton_Click(object sender, EventArgs e)
        {
            int i = ProjectsDataGridView.Rows.Add();
            var cell = ProjectsDataGridView[0, i];
            ProjectsDataGridView.CurrentCell = cell;
            ProjectsDataGridView.BeginEdit(false);

            cell.Tag = Guid.NewGuid();
        }

        private void ProjectsDataGridView_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            ProjectsDataGridView.CurrentCell = ProjectsDataGridView[e.ColumnIndex, e.RowIndex];
            ProjectsDataGridView.BeginEdit(true);
        }

        private void ProjectsDataGridView_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            var value = (String)ProjectsDataGridView[e.ColumnIndex, e.RowIndex].Value;
            if (value == null || value.Trim().Length == 0)
            {
                ProjectsDataGridView.Rows.RemoveAt(e.RowIndex);
            }
            else
            {
                NextButton.Visible = true;
            }
        }

        private void panel1_MouseClick(object sender, MouseEventArgs e)
        {
            ProjectsDataGridView.EndEdit();
        }

        private void ProjectsDataGridView_CurrentCellChanged(object sender, EventArgs e)
        {
            var cell = ProjectsDataGridView.CurrentCell;
            if (cell != null && !cell.IsInEditMode && cell.Value != null)
            {
                NextButton.Visible = true;
            }
        }

        private void NextButton_Click(object sender, EventArgs e)
        {

        }

        private void ProjectsForm_Load(object sender, EventArgs e)
        {
            if (AppDataRepository.YoutubeKey == null)
            {
                KeyForm = new YoutubeKeyForm(this);
                KeyForm.Show();
            }
        }

        private void ProjectsForm_Activated(object sender, EventArgs e)
        {
            if (KeyForm != null)
            {
                
                KeyForm.Activate();
            }
        }

        private void ProjectsForm_FormClosing(object sender, FormClosingEventArgs e)
        {
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
        }
    }
}

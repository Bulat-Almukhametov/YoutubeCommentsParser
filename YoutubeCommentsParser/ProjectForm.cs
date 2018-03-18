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
    public partial class ProjectsForm : BaseForm
    {
        public Form KeyForm;
        public ProjectsForm()
        {
            InitializeComponent();
            FillProjectGridView();
        }

        private void FillProjectGridView()
        {
            foreach (var sProject in AppDataRepository.SearchProjects)
            {
                var i = ProjectsDataGridView.Rows.Add();
                var cell = ProjectsDataGridView[0, i];

                cell.Value = sProject.Value.Name;
                cell.Tag = sProject.Key;
            }
            
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
            var cell = ProjectsDataGridView[e.ColumnIndex, e.RowIndex];
            var value = (String)cell.Value;
            var id = (Guid)cell.Tag;
            if (value == null || value.Trim().Length == 0)
            {
                ProjectsDataGridView.Rows.RemoveAt(e.RowIndex);
                AppDataRepository.SearchProjects.Remove(id);
            }
            else
            {
                NextButton.Visible = true;
                NextButton.Tag = id;

                if (AppDataRepository.SearchProjects.ContainsKey(id))
                {
                    AppDataRepository.SearchProjects[id].Name = value;
                }
                else
                {
                    AppDataRepository.SearchProjects.Add(id, new SearchProject { Name = value });
                }
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
                NextButton.Tag = cell.Tag;
            }
        }

        private void NextButton_Click(object sender, EventArgs e)
        {
            var id = (Guid)NextButton.Tag;
            var project = AppDataRepository.SearchProjects[id];
            NextForm = new VideoSearchParametersForm(project);
            NextForm.PreviousForm = this;
            NextForm.StartPosition = StartPosition;
            NextForm.Size = Size;

            Hide();
            NextForm.Show();
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
            
        }
    }
}

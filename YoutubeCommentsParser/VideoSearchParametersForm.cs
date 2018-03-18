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
    public partial class VideoSearchParametersForm : BaseForm
    {
        private SearchProject _Project { get; set; }
        public VideoSearchParametersForm(SearchProject project)
        {
            InitializeComponent();

            _Project = project;
            QueryComboBox.Text = project.Query;
            LoadVideos();
        }

        private void LoadVideos()
        {
            NextButton.Visible = _Project.Videos != null && _Project.Videos.Count > 0;

            videosDataGridView.Rows.Clear();
            if (_Project.Videos != null)
                foreach (var video in _Project.Videos)
                {
                    var i = videosDataGridView.Rows.Add();
                    videosDataGridView["Caption", i].Value = video.Value;
                }
        }

        private void QueryComboBox_TextUpdate(object sender, EventArgs e)
        {
            //QueryComboBox.Items.Clear();
            //var videos = YoutubeHelper.VideoSuggestions(QueryComboBox.Text);
            //foreach (var video in videos)
            //{
            //    QueryComboBox.Items.Add(video);
            //}
            //QueryComboBox.DroppedDown = true;

            //QueryComboBox.SelectionLength = 0;
            //QueryComboBox.SelectionStart = QueryComboBox.Text.Length;
        }

        private void backButton_Click(object sender, EventArgs e)
        {
            Hide();
            PreviousForm.Show();
            PreviousForm.NextForm = null;

            SkipSaveFile = true;
            Close();

        }

        private void LoadButton_Click(object sender, EventArgs e)
        {
            LoadPictureBox.Visible = true;

            string query = QueryComboBox.Text;
            _Project.Query = query;

            backgroundWorker.RunWorkerAsync(query);
            
        }

        private void backgroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            _Project.Videos = YoutubeHelper.GetVideos((string)e.Argument);
        }

        private void backgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            LoadVideos();

            LoadPictureBox.Visible = false;
        }
    }
}

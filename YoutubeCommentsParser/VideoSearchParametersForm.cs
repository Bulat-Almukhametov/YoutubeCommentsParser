using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using OpenXMLExcel.SLExcelUtility;
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

            backButton.Click += backButton_Click;
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
                    videosDataGridView["Caption", i].Value = video.Value.Caption;
                    videosDataGridView["Likes", i].Value = video.Value.Likes.ToString();
                    videosDataGridView["Dislikes", i].Value = video.Value.Dislikes.ToString();
                    videosDataGridView["ViewCount", i].Value = video.Value.ViewedCount.ToString();
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

        private void button1_Click(object sender, EventArgs e)
        {
            var dialog = new SaveFileDialog();
            dialog.DefaultExt = "xls";
            dialog.Filter = "Excell (*.xlsx)|*.xlsx";
            dialog.FileName = _Project.Name;
            var result = dialog.ShowDialog();

            if (result != DialogResult.OK)
                return;

            string filepath = dialog.FileName;
            var headers = new List<string>();
            foreach (var column in videosDataGridView.Columns)
            {
                headers.Add(((DataGridViewColumn)column).HeaderText);
            }

            var rows = new List<List<string>>();
            for (int row = 0; row < videosDataGridView.RowCount; row++)
            {
                var cells = new List<string>();
                for (int column = 0; column < videosDataGridView.ColumnCount; column++)
                {
                    cells.Add(videosDataGridView[column, row].Value.ToString());
                }
                rows.Add(cells);
            }
            
            SLExcelWriter.Export(new SLExcelData
            {
                SheetName = "Фильмы",
                Headers = headers,
                DataRows = rows
            }, filepath);
        }
    }
}

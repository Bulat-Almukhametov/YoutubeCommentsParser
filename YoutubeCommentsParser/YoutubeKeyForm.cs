using Google.Apis.Services;
using Google.Apis.YouTube.v3;
using log4net;
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
    public partial class YoutubeKeyForm : Form
    {
        private readonly ProjectsForm _StartForm;
        private static ILog _Log = LogManager.GetLogger("FileLogger");
        public YoutubeKeyForm(ProjectsForm startForm)
        {
            _StartForm = startForm;
            InitializeComponent();
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            string url = Properties.Settings.Default["GetGoogleApiKeyUrl"].ToString();
            System.Diagnostics.Process.Start(url);
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

            NextButton.Visible = textBox1.Text.Trim().Length > 0;

        }

        private void YoutubeKeyForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            _StartForm.KeyForm = null;

            if (AppDataRepository.YoutubeKey == null)
                _StartForm.Close();
        }

        private void NextButton_Click(object sender, EventArgs e)
        {
            var key = textBox1.Text;
            if (!VerifyGoogleKey(key))
            {
                MessageBox.Show("Неправильно введен ключ");
                return;
            }

            AppDataRepository.YoutubeKey = key;
            Close();
        }

        private bool VerifyGoogleKey(string key)
        {
            var result = false;

            try
            {
                var youtubeService = new YouTubeService(new BaseClientService.Initializer()
                {
                    ApiKey = key
                });

                var searchListRequest = youtubeService.Search.List("id, snippet");
                searchListRequest.Q = "Google";
                searchListRequest.MaxResults = 1;

                var response = searchListRequest.Execute();
                result = response.Items.Count > 0;
            }
            catch(Exception ex)
            {
                _Log.Error(ex.ToString());
            }

            return result;
        }

        private void YoutubeKeyForm_Load(object sender, EventArgs e)
        {

        }
    }
}

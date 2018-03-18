using Google.Apis.Services;
using Google.Apis.YouTube.v3;
using Google.Apis.YouTube.v3.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YoutubeCommentsParser.Models
{
    public static class YoutubeHelper
    {
        public static string[] VideoSuggestions(string query)
        {
            var youtubeService = new YouTubeService(new BaseClientService.Initializer()
            {
                ApiKey = AppDataRepository.YoutubeKey
            });

            var searchListRequest = youtubeService.Search.List("id, snippet");
            searchListRequest.Q = query;
            searchListRequest.MaxResults = 10;

            var response = searchListRequest.Execute();

            return response.Items.Select(item => item.Snippet.Title).ToArray();
        }

        public static Dictionary<string, string> GetVideos(string query, int count = 400)
        {
            if (query == null || query.Trim().Length == 0)
                return null;

            var youtubeService = new YouTubeService(new BaseClientService.Initializer()
            {
                ApiKey = AppDataRepository.YoutubeKey
            });

            var searchListRequest = youtubeService.Search.List("id, snippet");
            searchListRequest.Q = query;
            searchListRequest.Order = SearchResource.ListRequest.OrderEnum.Rating;

            var items = new List<SearchResult>();
            string pageToken = null;
            while (count > 0)
            {
                var itemsInPage = (count > 50) ? 50 : count;
                count -= itemsInPage;
                searchListRequest.MaxResults = itemsInPage;

                searchListRequest.PageToken = pageToken;

                var response = searchListRequest.Execute();
                pageToken = response.NextPageToken;
                items.AddRange(response.Items);
            }


            var result = new Dictionary<string, string>();

            foreach (var item in items)
            {
                var id = item.Id.VideoId;
                if (id != null && !result.ContainsKey(id))
                    result.Add(id, item.Snippet.Title);
            }

            return result;
        }
    }
}

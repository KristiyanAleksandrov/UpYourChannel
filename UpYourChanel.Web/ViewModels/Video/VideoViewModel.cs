﻿using System.Text.RegularExpressions;

namespace UpYourChannel.Web.ViewModels.Video
{
    public class VideoViewModel
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string Link { get; set; }

        public string Description { get; set; }

        // MAYBE REMOVE UserId
        public string UserId { get; set; }

        public string IFrameSource
        {
            get
            {
                if (this.Link.Contains("youtube"))
                {
                    var regex = new Regex(@"youtu(?:\.be|be\.com)/(?:(.*)v(/|=)|(.*/)?)(?<id>[a-zA-Z0-9-_]+)", RegexOptions.IgnoreCase);
                    var videoId = regex.Match(this.Link).Groups["id"];
                    return $"https://www.youtube.com/embed/{videoId}";
                }
                else
                {
                    return this.Link;
                }
            }
        }
    }
}

﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using UpYourChannel.Data.Data;
using UpYourChannel.Data.Models;
using UpYourChannel.Web.Paging;
using UpYourChannel.Web.Services;
using UpYourChannel.Web.ViewModels.Video;

namespace UpYourChannel.Web.Areas.Administration.Controllers
{
    [Area("Administration")]
    [Authorize(Roles = GlobalConstants.AdminRoleName)]
    public class VideoController : Controller
    {
        private readonly IVideoService videoService;

        public VideoController(IVideoService videoService)
        {
            this.videoService = videoService;
        }

       //public IActionResult AllVideos()
       //{
       //
       //    return this.View(videoService.AllVideos());
       //}

        public async Task<IActionResult> RemoveVideoById(int id)
        {
            await videoService.RemoveVideoByIdAsync(id);
            return Redirect("/Administration/Video/AllVideos");
        }

        [HttpPost]
        public async Task<IActionResult> EditVideoTitle(int videoId,string newTitle)
        {
            await videoService.EditVideoTitleAsync(videoId, newTitle);
            return Redirect("/Administration/Video/AllVideos");
        }

        public IActionResult AllVideos(string searchString, int? pageNumber)
        {
            ViewData["CurrentFilter"] = searchString;
            var videos = string.IsNullOrEmpty(searchString) ? videoService.AllVideos()
                : videoService.VideosBySearch(searchString);
            return View(PaginatedList<VideoViewModel>.Create(videos.AllVideos, pageNumber ?? 1, GlobalConstants.PageSize));
        }
    }
}

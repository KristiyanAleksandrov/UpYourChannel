﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using UpYourChannel.Data.Models.Enums;

namespace UpYourChannel.Data.Models
{
    public class Post
    {
        public Post()
        {
            Comments = new HashSet<Comment>();
            CreatedOn = DateTime.UtcNow;
        }
        public int Id { get; set; }

        public string Title { get; set; }

        public string Content { get; set; }

        public string UserId { get; set; }

        [ForeignKey("UserId")]
        public virtual User User { get; set; }

        public virtual IEnumerable<Comment> Comments { get; set; }

        public virtual IEnumerable<Vote> Votes { get; set; }

        public CategoryType Category { get; set; }

        public int CommentsCount => Comments.Count();

        public DateTime CreatedOn { get; set; }
    }
}

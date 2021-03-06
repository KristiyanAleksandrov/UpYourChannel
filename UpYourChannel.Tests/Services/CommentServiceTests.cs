﻿using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using UpYourChannel.Data.Data;
using UpYourChannel.Web.Services;
using Xunit;

namespace UpYourChannel.Tests.Services
{
    public class CommentServiceTests
    {
        [Fact]
        public async Task CreateComment()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                    .UseInMemoryDatabase(databaseName: "CreateComment_Database")
                    .Options;
            var dbContext = new ApplicationDbContext(options);
            var commentService = new CommentService(dbContext);

            await commentService.CreateCommentAsync(1,"Tweets", "Hello i am tweet",null,false);
            await commentService.CreateCommentAsync(1,"Tweets2", "Hello i am tweet2", 1,false);

            var comentsCount = await dbContext.Comments.CountAsync();
            var comment = await dbContext.Comments.FirstAsync();

            Assert.Equal(1, comment.Id);
            Assert.Equal("Tweets", comment.UserId);
            Assert.Equal("Hello i am tweet", comment.Content);
            Assert.Null(comment.ParentId);
            Assert.Equal(2, comentsCount);
        }

        [Fact]
        public async Task EditComment()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                    .UseInMemoryDatabase(databaseName: "EditComment_Database")
                    .Options;
            var dbContext = new ApplicationDbContext(options);
            var commentService = new CommentService(dbContext);

            await commentService.CreateCommentAsync(1, "u1", "Hello i am tweet", null,false);
            await commentService.CreateCommentAsync(1, "u2", "Hello i am tweet2", 1,false);
            await commentService.EditCommentAsync(2, "Hell i am new tweet","u2");

            var comentsCount = await dbContext.Comments.CountAsync();
            var comment = await commentService.GetCommentByIdAsync(2);

            Assert.Equal(2, comment.Id);
            Assert.Equal("u2", comment.UserId);
            Assert.Equal("Hell i am new tweet", comment.Content);
            Assert.Equal(1,comment.ParentId);
            Assert.Equal(2, comentsCount);
        }

        [Fact]
        public async Task DeleteComment()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                    .UseInMemoryDatabase(databaseName: "DeleteComment_Database")
                    .Options;
            var dbContext = new ApplicationDbContext(options);
            var commentService = new CommentService(dbContext);
            var voteService = new VoteService(dbContext);

            await commentService.CreateCommentAsync(1, "u1", "Hello i am tweet", null,false);
            await commentService.CreateCommentAsync(1, "u2", "Hello i am tweet2", 1,false);
            await voteService.VoteForCommentAsync("u1",1,true);
            await commentService.EditCommentAsync(2, "Hello i am new tweet2","u2");
            await commentService.DeleteCommentByIdAsync(1,1,"u1");

            var comentsCount = await dbContext.Comments.CountAsync();
            var comment = await dbContext.Comments.LastOrDefaultAsync();

            Assert.Equal(2, comment.Id);
            Assert.Equal("u2", comment.UserId);
            Assert.Equal("Hello i am new tweet2", comment.Content);
            Assert.Null(comment.ParentId);
            Assert.Equal(1, comentsCount);
            Assert.Equal(0, await dbContext.Votes.CountAsync());
        }
        [Fact]
        public async Task AllCommentsForPost()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                    .UseInMemoryDatabase(databaseName: "AllComments_Database")
                    .Options;
            var dbContext = new ApplicationDbContext(options);
            var commentService = new CommentService(dbContext);
            var postService = new PostService(dbContext);

            await postService.CreatePostAsync("Hello","I am Kris", "Kris",1);
            await commentService.CreateCommentAsync(1, "u1", "Hello i am tweet", null,false);
            await commentService.CreateCommentAsync(1, "u2", "Hello i am tweet2", 1,false);
            var commentsForPost = commentService.AllCommentsForPost(1);

            Assert.Equal(2, commentsForPost.Count());
        }

        [Fact]
        public async Task Top3CommentsForPost()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                    .UseInMemoryDatabase(databaseName: "TopComments_Database")
                    .Options;
            var dbContext = new ApplicationDbContext(options);
            var commentService = new CommentService(dbContext);
            var postService = new PostService(dbContext);
            var voteService = new VoteService(dbContext);

            await postService.CreatePostAsync("Hello", "I am Kris", "Kris",1);
            await commentService.CreateCommentAsync(1, "u1", "Hello i am tweet", null,false);
            await commentService.CreateCommentAsync(1, "u2", "Hello i am tweet2", 1,false);
            await commentService.CreateCommentAsync(1, "u3", "Hello i am tweet3", 1,false);
            await commentService.CreateCommentAsync(1, "u4", "Hello i am tweet4", 1,false);
            await voteService.VoteForCommentAsync("Kris",1,true);
            
            var commentsForPost = commentService.Top3CommentsForPost(1);
            var commentWithMostLikes = commentsForPost.FirstOrDefault();
            
            Assert.Equal(1,commentWithMostLikes.Id);
            Assert.Equal("u1", commentWithMostLikes.UserId);
            Assert.Equal("Hello i am tweet", commentWithMostLikes.Content);
            Assert.Null(commentWithMostLikes.ParentId);
            Assert.Equal(3, commentsForPost.Count());
        }
        [Fact]
        public async Task Top3AnswersForPost()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                    .UseInMemoryDatabase(databaseName: "TopAnswers_Database")
                    .Options;
            var dbContext = new ApplicationDbContext(options);
            var commentService = new CommentService(dbContext);
            var postService = new PostService(dbContext);
            var voteService = new VoteService(dbContext);

            await postService.CreatePostAsync("Hello", "I am Kris", "Kris", 1);
            await commentService.CreateCommentAsync(1, "u1", "Hello i am tweet", null, true);
            await commentService.CreateCommentAsync(1, "u2", "Hello i am tweet2", 1, true);
            await commentService.CreateCommentAsync(1, "u3", "Hello i am tweet3", 1, true);
            await commentService.CreateCommentAsync(1, "u4", "Hello i am tweet4", 1, true);
            await voteService.VoteForCommentAsync("Kris", 1, true);

            var commentsForPost = commentService.Top3AnswersForPost(1);
            var commentWithMostLikes = commentsForPost.FirstOrDefault();

            Assert.Equal(1, commentWithMostLikes.Id);
            Assert.Equal("u1", commentWithMostLikes.UserId);
            Assert.Equal("Hello i am tweet", commentWithMostLikes.Content);
            Assert.Null(commentWithMostLikes.ParentId);
            Assert.Equal(3, commentsForPost.Count());
        }
    }
}

using DnlForumsData;
using DnlForumsData.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DnlForumsService
{
    public class PostService : IPost
    {
        private readonly ApplicationDbContext context;

        public PostService(ApplicationDbContext context)
        {
            this.context = context;
        }

        public async Task Add(Post post)
        {
            this.context.Add(post);
            await this.context.SaveChangesAsync();
        }

        public async Task AddReply(PostReply reply)
        {
            await this.context.AddAsync(reply);
            await this.context.SaveChangesAsync();
        }

        public Task Delete(int id)
        {
            throw new NotImplementedException();
        }

        public Task EditPostContent(int id, string newContent)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Post> GetAll()
        {
            return this.context.Posts.Include(post => post.User)
                                     .Include(post => post.Replies).ThenInclude(reply => reply.User)
                                     .Include(post => post.Forum);
        }

        public Post GetById(int id)
        {
            return this.context.Posts.Where(post => post.Id == id)
                                     .Include(post => post.User)
                                     .Include(post => post.Replies).ThenInclude(reply => reply.User)
                                     .Include(post => post.Forum)
                                     .First();
        }

        public IEnumerable<Post> GetFilteredPosts(Forum forum, string searchQuery)
        {
            var query = searchQuery.ToLower();
            return String.IsNullOrEmpty(searchQuery) ? forum.Posts
                                                     : forum.Posts.Where(post => post.Title.ToLower().Contains(query)
                                                     || post.Content.ToLower().Contains(query));
        }

        public IEnumerable<Post> GetFilteredPosts(string searchQuery)
        {
            var query = searchQuery.ToLower();
            return this.GetAll().Where(post => post.Title.ToLower().Contains(query)
                                                     || post.Content.ToLower().Contains(query));
        }

        public IEnumerable<Post> GetLatestPost(int n)
        {
            return this.GetAll().OrderByDescending(post => post.Created).Take(n);
        }

        public IEnumerable<Post> GetPostsByForum(int id)
        {
            return this.context.Forums.Where(forum => forum.Id == id).First().Posts;
        }
    }
}

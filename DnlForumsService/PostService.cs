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

        public Task Add(Post post)
        {
            throw new NotImplementedException();
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
            throw new NotImplementedException();
        }

        public Post GetById(int id)
        {
            return this.context.Posts.Where(post => post.Id == id)
                                     .Include(post => post.User)
                                     .Include(post => post.Replies).ThenInclude(reply => reply.User)
                                     .Include(post => post.Forum)
                                     .First();
        }

        public IEnumerable<Post> GetFilteredPosts(string searchQuery)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Post> GetPostsByForum(int id)
        {
            return this.context.Forums.Where(forum => forum.Id == id).First().Posts;
        }
    }
}

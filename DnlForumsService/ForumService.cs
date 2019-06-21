﻿using DnlForumsData;
using DnlForumsData.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DnlForumsService
{
    public class ForumService : IForum
    {
        private readonly ApplicationDbContext context;

        public ForumService(ApplicationDbContext context)
        {
            this.context = context;
        }

        public Task Create(Forum forum)
        {
            throw new NotImplementedException();
        }

        public Task Delete(int forumId)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Forum> GetAll()
        {
            return this.context.Forums.Include(forum => forum.Posts);
        }

        public IEnumerable<ApplicationUser> GetAllActiveUsers()
        {
            throw new NotImplementedException();
        }

        public Forum GetById(int id)
        {
            var forum = this.context.Forums.Where(f => f.Id == id)
                                           .Include(f => f.Posts).ThenInclude(p => p.User)
                                           .Include(f => f.Posts).ThenInclude(p => p.Replies).ThenInclude(r => r.User)
                                           .FirstOrDefault();
            return forum;
        }

        public Task UpdateForumDescription(int forumId, string newDescription)
        {
            throw new NotImplementedException();
        }

        public Task UpdateForumTitle(int forumId, string newTitle)
        {
            throw new NotImplementedException();
        }
    }
}
using DnlForumsData;
using DnlForumsData.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DnlForumsService
{
    public class ApplicationUserService : IApplicationUser
    {
        private readonly ApplicationDbContext context;

        public ApplicationUserService(ApplicationDbContext context)
        {
            this.context = context;
        }

        public IEnumerable<ApplicationUser> GetAll()
        {
            return this.context.ApplicationUsers;
        }

        public ApplicationUser GetById(string id)
        {
            return this.GetAll().FirstOrDefault(user => user.Id == id);
        }

        public async Task UpdateUserRating(string id, Type type)
        {
            var user = this.GetById(id);
            user.Rating = this.CalculateUserRating(type, user.Rating);
            await this.context.SaveChangesAsync();
        }

        private int CalculateUserRating(Type type, int userRating)
        {
            var inc = 0;
            if (type == typeof(Post))
                inc = 1;
            if (type == typeof(PostReply))
                inc = 3;

            return userRating + inc;
        }

        public async Task SetProfileImage(string id, string imageUrl)
        {
            var user = this.GetById(id);
            user.ProfileUrl = imageUrl;
            this.context.Update(user);
            await this.context.SaveChangesAsync();
        }
    }
}

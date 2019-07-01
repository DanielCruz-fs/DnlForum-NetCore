using DnlForumsData.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DnlForumsData
{
    public interface IApplicationUser
    {
        ApplicationUser GetById(string id);
        IEnumerable<ApplicationUser> GetAll();

        Task SetProfileImage(string id, string imageUrl);
        Task IncrementRating(string id, Type type);
    }
}

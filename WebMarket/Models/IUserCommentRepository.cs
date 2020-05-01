using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebMarket.Models
{
    public interface IUserCommentRepository
    {
        UserComment GetUserComment(int id);
        UserComment GetUserCommentByIndex(int index);
        IEnumerable<UserComment> GetAllUserComments();
        UserComment Add(UserComment comment);
        UserComment Update(UserComment commentChanges);
        UserComment Delete(int id);
    }
}

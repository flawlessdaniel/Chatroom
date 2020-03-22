using Chatroom.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chatroom.Helpers
{
    public interface IDbHelper
    {
        ResponseHandler UpdateDatabasePost(string post, int userId);
        ResponseHandler UserRegister(User usuario);
        ResponseHandler UserValidate(User usuario);
        User GetUserById(int id);
    }
}

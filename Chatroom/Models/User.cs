using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Chatroom.Models
{
    public class User
    {
        private int id;
        private string email;
        private string passwrd;
        private string username;

        public int Id
        {
            get
            {
                return id;
            }

            set
            {
                id = value;
            }
        }

        public string Email
        {
            get
            {
                return email;
            }

            set
            {
                email = value;
            }
        }

        public string Passwrd
        {
            get
            {
                return passwrd;
            }

            set
            {
                passwrd = value;
            }
        }

        public string Username
        {
            get
            {
                return username;
            }

            set
            {
                username = value;
            }
        }
    }
}
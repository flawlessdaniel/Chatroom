using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;
using System.Web;
using Chatroom.Models;

namespace Chatroom.Helpers
{
    public class SqlHelper : IDbHelper
    {
        public SqlHelper()
        {

        }

        public ResponseHandler UpdateDatabasePost(string post, int userId)
        {
            try
            {
                using (TransactionScope transaction = new TransactionScope())
                {
                    using (DCFCHATROOMEntities db = new DCFCHATROOMEntities())
                    {
                        db.POSTS.Add(new POSTS()
                        {
                            IDUSERPRODUCER = userId,
                            MSG = post,
                            REGDATE = DateTime.Now.ToString("yyyyMMdd"),
                            REGTIME = DateTime.Now.ToString("HH:mm:ss")
                        });

                        db.SaveChanges();
                        transaction.Complete();
                        return new ResponseHandler(200, "OK");
                    }
                }
            }
            catch (Exception ex)
            {
                return new ResponseHandler(500, ex.Message);
            }
        }

        public ResponseHandler UserRegister(User usuario)
        {
            try
            {
                using (TransactionScope transaction = new TransactionScope())
                {
                    using (DCFCHATROOMEntities db = new DCFCHATROOMEntities())
                    {
                        db.USERS.Add(new USERS()
                        {
                            EMAIL = usuario.Email,
                            PASSWRD = ChatroomEncrypter.Encrypt(usuario.Passwrd),
                            USERNAME = usuario.Username,
                            REGDATE = DateTime.Now.ToString("yyyyMMdd"),
                            REGTIME = DateTime.Now.ToString("HH:mm:ss")
                        });

                        db.SaveChanges();
                        transaction.Complete();

                        return new ResponseHandler(200, "OK");
                    }
                }
            }
            catch (Exception ex)
            {
                return new ResponseHandler(500, ex.Message);
            }
        }

        public ResponseHandler UserValidate(User usuario)
        {
            try
            {
                using (DCFCHATROOMEntities db = new DCFCHATROOMEntities())
                {
                    string encryptedPassword = ChatroomEncrypter.Encrypt(usuario.Passwrd);
                    USERS user = db.USERS.Where(x => x.EMAIL == usuario.Email && (x.PASSWRD.Equals(encryptedPassword) || x.PASSWRD.Equals(usuario.Passwrd))).FirstOrDefault();
                    if (user != null)
                    {
                        return new ResponseHandler(200, user.ID.ToString());
                    }
                    else
                    {
                        return new ResponseHandler(301, "Usuario incorrecto");
                    }
                }
            }
            catch (Exception ex)
            {
                return new ResponseHandler(500, ex.Message);
            }
        }

        public User GetUserById(int id)
        {
            try
            {
                using (DCFCHATROOMEntities db = new DCFCHATROOMEntities())
                {
                    USERS us = db.USERS.Where(x => x.ID == id).FirstOrDefault();
                    if (us == null) return null;

                    return new User()
                    {
                        Id = us.ID,
                        Email = us.EMAIL,
                        Username = us.USERNAME
                    };
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}
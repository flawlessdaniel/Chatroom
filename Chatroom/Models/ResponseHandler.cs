using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Chatroom.Models
{
    public class ResponseHandler
    {
        private int code;
        private string msg;

        public int Code
        {
            get
            {
                return code;
            }

            set
            {
                code = value;
            }
        }

        public string Msg
        {
            get
            {
                return msg;
            }

            set
            {
                msg = value;
            }
        }

        public ResponseHandler(int code, string msg)
        {
            Code = code;
            Msg = msg;
        }
    }
}
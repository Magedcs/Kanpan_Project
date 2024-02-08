using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntroSE.Kanban.Backend.ServiceLayer
{
    public class Response
    {
        public  string ErrorMessage { get; set; }
        public bool ErrorOccured { get => ErrorMessage != null; }
        public Response() { }
        public Response(string msg)
        {
            this.ErrorMessage = msg;
        }
    
    public bool CheckforError()
        {
            if (ErrorMessage == null)
            {
                return false;
            }
            return true;
        }
    }
}


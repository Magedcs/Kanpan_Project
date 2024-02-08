using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Frontend.Model
{
    public class BoardModel : NotifiableModelObject
    {
        public BoardModel(BackendController controller, string id, string title,string userEmail) : base(controller)
        {
            _id = id;
            _titlee = title;
            Id = id;
            Titlee = title;
            this.userEmail= userEmail;
        }

        private string userEmail;
        private string _id;
        public string Id
        {
            get { return _id; }
            set
            {
                _id ="Boards ID: " + value;
                RaisePropertyChanged("Id");
            }
        }
        private string _titlee;
        public string Titlee
        {
            get { return _titlee; }

            set
            {
                _titlee ="Boards name: " + value; 
                RaisePropertyChanged("Titlee"); 
            }  
        }

       public BoardModel(BackendController controller, (string Id, string Title) board, UserModel user) : this(controller, board.Id, board.Title, user.Email) { }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;


namespace Frontend.Model
{
    public class BoardsListModel : NotifiableModelObject
    {
        private readonly UserModel user;
        public ObservableCollection<BoardModel> Boards { get; set; }
        

        public BoardsListModel(BackendController controller, ObservableCollection<BoardModel> boards): base(controller)
        {
            this.Boards = boards;
            Boards.CollectionChanged += HandleChange;

        }

        public BoardsListModel(BackendController controller, UserModel user) : base(controller)
        {
            this.user = user;
            Boards = new ObservableCollection<BoardModel>();
            List<int> curr = controller.GetBoardsids(user.Email);
            foreach (int id in curr)
            {
                Boards.Add(new BoardModel(controller, controller.GetBoard(user.Email, id), user));
            }    
            Boards.CollectionChanged += HandleChange;
        }



        private void HandleChange(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Remove)
            {

            }
        }
    }


}

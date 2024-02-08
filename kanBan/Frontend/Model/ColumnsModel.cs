using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using IntroSE.Kanban.Backend.ServiceLayer.ToReturn;

namespace Frontend.Model
{
    public class ColumnsModel : NotifiableModelObject
    {
        private readonly UserModel user;

        public ObservableCollection<TaskModel> Col1 { get; set; }
        public ObservableCollection<TaskModel> Col2 { get; set; }
        public ObservableCollection<TaskModel> Col3 { get; set; }


        public ColumnsModel(BackendController controller,UserModel user, int brdId) : base(controller)
        {
            this.user = user;
            Col1 = new ObservableCollection<TaskModel>();
            Col2 = new ObservableCollection<TaskModel>();
            Col3 = new ObservableCollection<TaskModel>();
            for (int i = 0; i < 3; i++)
            {
                IntroSE.Kanban.Backend.ServiceLayer.ToReturn.Task[] array = controller.GetColumn(user.Email, brdId, i);
                for (int j = 0; j < array.Length; j++)
                {
                    IntroSE.Kanban.Backend.ServiceLayer.ToReturn.Task currtask = array[j];
                    if (i == 0)
                    {
                        Col1.Add(new TaskModel(user.Controller, currtask.Id, currtask.Title, currtask.Description, currtask.assignee, currtask.CreationTime, currtask.DueDate));
                    }else if (i == 1)
                    {
                        Col2.Add(new TaskModel(user.Controller, currtask.Id, currtask.Title, currtask.Description, currtask.assignee, currtask.CreationTime, currtask.DueDate));
                    }
                    else
                    {
                        Col3.Add(new TaskModel(user.Controller, currtask.Id, currtask.Title, currtask.Description, currtask.assignee, currtask.CreationTime, currtask.DueDate));
                    }
                }
            }
        }

    }
}

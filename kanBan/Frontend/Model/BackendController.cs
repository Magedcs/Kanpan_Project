using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IntroSE.Kanban.Backend.ServiceLayer;
using System.Text.Json;
using System.Text.Json.Serialization;
using IntroSE.Kanban.Backend.BusinessLayer.Board;
using IntroSE.Kanban.Backend.ServiceLayer.ToReturn;

namespace Frontend.Model
{
     public class BackendController
    {
        public ServiceFactory Service { get;  set; }

        public BackendController(ServiceFactory service)
        {
            this.Service = service;
            Service.LoadData();
        }
        public BackendController()
        {
            this.Service = new ServiceFactory();
            Service.LoadData();
        }
        public UserModel Login(string username, string password)
        {
            string json = Service.Login(username, password);
            Response logged = JsonSerializer.Deserialize<Response>(json);
            if (logged.ErrorOccured)
            {
                throw new Exception(logged.ErrorMessage);
            }
            return new UserModel(this, username);
        }
        public UserModel Register(string username,string password)
        {
            string json = Service.Register(username, password);
            Response reg = JsonSerializer.Deserialize<Response>(json);
            if (reg.ErrorOccured)
            {
                throw new Exception(reg.ErrorMessage);
            }
            return new UserModel(this, username);
        }


        public List<int> GetBoardsids(string email)
        {
            IReadOnlyCollection<int> boardsids;
            string json = Service.GetUserBoards(email);
            Response<int[]> a = JsonSerializer.Deserialize<Response<int[]>>(json);
            boardsids = a.Value;
            return new List<int>(boardsids);
        }

        internal (string Id, string Title) GetBoard(string email, int boardid)
        {

            string json = Service.GetBoardName(boardid);
            Response<string> name = JsonSerializer.Deserialize<Response<string>>(json);
            return (boardid.ToString(), name.Value);
        }

        public void Logout(string email)
        {
            string json = Service.Logout(email);
            Response name = JsonSerializer.Deserialize<Response>(json);
            if (name.ErrorOccured)
            {
                throw new Exception(name.ErrorMessage);
            }

        }

        public IntroSE.Kanban.Backend.ServiceLayer.ToReturn.Task [] GetColumn(string email, int brdId, int Ordinal)
        {
            try
            {
                var a = this.GetBoard(email, brdId);
                string json = Service.GetColumn(email, a.Title, Ordinal);
                Response<IntroSE.Kanban.Backend.ServiceLayer.ToReturn.Task[]> b = JsonSerializer.Deserialize<Response<IntroSE.Kanban.Backend.ServiceLayer.ToReturn.Task[]>>(json);
                return b.Value;
            }catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
    }
}

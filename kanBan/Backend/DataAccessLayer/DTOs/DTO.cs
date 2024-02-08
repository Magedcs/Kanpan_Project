using System;
using System.Data.SQLite;
using System.IO;
using System.Collections.Generic;
namespace IntroSE.Kanban.Backend.DataAccessLayer.DTOs
{
    public abstract class DTO
    {
            protected ControllerDAL _controller;

            protected DTO(ControllerDAL controller)
            {
                _controller = controller;
            }

        }
}


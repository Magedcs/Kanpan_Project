using System;
using IntroSE.Kanban.Backend.BusinessLayer;
namespace IntroSE.Kanban.Backend.ServiceLayer.ToReturn
{
	public class Task
	{
		public int Id { get; set; }
		public DateTime CreationTime { get; set; }
		public string Title { get; set; }
		public string assignee { get; set; }
		public string Description { get; set; }		
		public DateTime DueDate { get; set; }

		public Task() { }
		public Task(BusinessLayer.Board.Task task)
		{
			this.assignee = task.Assignee;
			this.Title = task.Title;
			this.Description = task.Description;
			this.CreationTime = task.creation();
			this.DueDate = task.DueDate;
			this.Id = task.Taskid;
		}
	}
}

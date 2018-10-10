using ASP.NET_PersonControl.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using static ASP.NET_PersonControl.Controllers.Api.UsersController;

namespace ASP.NET_PersonControl.Controllers.Api
{
    public struct TasksAction
    {
        public int taskId { get; set; }
        public string token { get; set; }
        public string userId { get; set; }
        public string answer { get; set; }
    }

    public class TasksController : ApiController
    {
        private bool isTokenValid(string token)
        {
            db = new ApplicationDbContext();

            if (db.Tokens.FirstOrDefault(t => t.token == token) != null)
                return true;
            else
                return false;
        }

        private ApplicationDbContext db { get; set; }

        [HttpPost]
        [ResponseType(typeof(Dictionary<string, object>))]
        public IHttpActionResult GetTasks(TasksAction tasksAction)
        {
            Dictionary<string, object> result = new Dictionary<string, object>();
            if (!isTokenValid(tasksAction.token))
            {
                result.Add("code", HttpStatusCode.ExpectationFailed);
                result.Add("message", "Token is not valid.");
                result.Add("time", DateTime.Now.ToString("ddd, dd MMMM yyyy H:mm:ss tt"));

                return Ok(result);
            }

            if (tasksAction.userId == null)
            {
                result.Add("code", HttpStatusCode.ExpectationFailed);
                result.Add("message", "Users id is not correct.");
                result.Add("time", DateTime.Now.ToString("ddd, dd MMMM yyyy H:mm:ss tt"));

                return Ok(result);
            }

            db = new ApplicationDbContext();

            string id = tasksAction.userId;

            List<TasksForUser> tasklist = (from gr in db.TasksForUser.ToList() where gr.toUserId == tasksAction.userId select gr).ToList();

            foreach (TasksForUser taskForUser in tasklist)
            {
                if (taskForUser.toUserId != null)
                    taskForUser.userTo = db.Users.FirstOrDefault(user => user.Id == taskForUser.toUserId.ToString());
                if (taskForUser.fromUserId != null)
                    taskForUser.userFrom = db.Users.FirstOrDefault(user => user.Id == taskForUser.fromUserId.ToString());
            }

            // group taks 
            string curUserID = tasksAction.userId;
            List<Groups> groupsList = (from gr in db.Groups.ToList()
                                       from ug in db.UsersGroups.ToList()
                                       where gr.Id == ug.GroupId && ug.UserId == tasksAction.userId
                                       select gr).ToList();
            List<Groups> gWhereUserOwner = (from gr in db.Groups.ToList()
                                            where gr.Owner == tasksAction.userId
                                            select gr).ToList();
            //groupsList.AddRange(gWhereUserOwner);
            foreach (Groups g2 in gWhereUserOwner)
                if (groupsList.Contains(g2) == false)
                    groupsList.Add(g2);

            List<TasksForGroups> taskGroups = (from gr in groupsList
                                               from task in db.TasksForGroups.ToList()
                                               where task.toGroupId == gr.Id
                                               select task).ToList();

            groupsList.Clear();
            foreach (TasksForGroups groupTask in taskGroups)
            {
                if (groupTask.fromUserId != null)
                {
                    groupTask.userFrom = db.Users.FirstOrDefault(user => user.Id == groupTask.fromUserId.ToString());
                    groupsList.Add(db.Groups.FirstOrDefault(g => g.Id == groupTask.toGroupId));
                }
            }

            //project tasks
            List<Projects> projectsList = (from pj in db.Projects.ToList()
                                           from gl in groupsList
                                           from gp in db.ProjectsGroups.ToList()
                                           where pj.Id == gp.ProjId && gp.GroupId == gl.Id
                                           select pj).Distinct().ToList();

            List<TasksForProjects> taskProjects = (from pj in projectsList
                                                   from task in db.TasksForProjects.ToList()
                                                   where task.toProjectId == pj.Id
                                                   select task).ToList();
            foreach (TasksForProjects projTask in taskProjects)
            {
                if (projTask.fromUserId != null)
                    projTask.userFrom = db.Users.FirstOrDefault(user => user.Id == projTask.fromUserId.ToString());
            }

            Dictionary<String, Object> dictRes = new Dictionary<string, object>();
            dictRes.Add("tasksPerson", tasklist);

            dictRes.Add("tasksGroups", taskGroups);
            dictRes.Add("groups", groupsList);

            dictRes.Add("tasksProjects", taskProjects);
            dictRes.Add("projects", projectsList);

            result.Add("code", HttpStatusCode.Accepted);
            result.Add("data", dictRes);
            result.Add("time", DateTime.Now.ToString("ddd, dd MMMM yyyy H:mm:ss tt"));

            return Ok(result);
        }



        [HttpPost]
        [ResponseType(typeof(Dictionary<string, object>))]
        public IHttpActionResult UpdateTasksPersonal(TasksAction tasksAction)
        {
            Dictionary<string, object> result = new Dictionary<string, object>();
            if (!isTokenValid(tasksAction.token))
            {
                result.Add("code", HttpStatusCode.ExpectationFailed);
                result.Add("message", "Token is not valid.");
                result.Add("time", DateTime.Now.ToString("ddd, dd MMMM yyyy H:mm:ss tt"));

                return Ok(result);
            }

            TasksForUser tasksForUser = db.TasksForUser.FirstOrDefault(c => c.Id == tasksAction.taskId);
            if (tasksForUser != null) //add
            {
                //TasksForUser tasksForUser = db.TasksForUser.FirstOrDefault(c => c.Id == id);
                bool complete = true;
                tasksForUser.isComplite = complete;
                db.SaveChanges();

                result.Add("code", HttpStatusCode.Accepted);
                result.Add("message", "PTask was updated");
                result.Add("user", tasksForUser);
            }
            else
            {
                result.Add("code", HttpStatusCode.Found);
                result.Add("message", "PTask wasn't found in db.");
            }
            result.Add("time", DateTime.Now.ToString("ddd, dd MMMM yyyy H:mm:ss tt"));

            return Ok(result);
        }



        [HttpPost]
        [ResponseType(typeof(Dictionary<string, object>))]
        public IHttpActionResult UpdateTasksGroup(TasksAction tasksAction)
        {
            Dictionary<string, object> result = new Dictionary<string, object>();
            if (!isTokenValid(tasksAction.token))
            {
                result.Add("code", HttpStatusCode.ExpectationFailed);
                result.Add("message", "Token is not valid.");
                result.Add("time", DateTime.Now.ToString("ddd, dd MMMM yyyy H:mm:ss tt"));

                return Ok(result);
            }

            TasksForGroups tasksForGroup = db.TasksForGroups.FirstOrDefault(c => c.Id == tasksAction.taskId);
            if (tasksForGroup != null) //add
            {
                //TasksForUser tasksForUser = db.TasksForUser.FirstOrDefault(c => c.Id == id);
                bool complete = true;
                tasksForGroup.isComplite = complete;
                db.SaveChanges();

                result.Add("code", HttpStatusCode.Accepted);
                result.Add("message", "GTask was updated");
                result.Add("user", tasksForGroup);
            }
            else
            {
                result.Add("code", HttpStatusCode.Found);
                result.Add("message", "GTask wasn't found in db.");
            }
            result.Add("time", DateTime.Now.ToString("ddd, dd MMMM yyyy H:mm:ss tt"));

            return Ok(result);
        }


        [HttpPost]
        [ResponseType(typeof(Dictionary<string, object>))]
        public IHttpActionResult UpdateTasksProject(TasksAction tasksAction)
        {
            Dictionary<string, object> result = new Dictionary<string, object>();
            if (!isTokenValid(tasksAction.token))
            {
                result.Add("code", HttpStatusCode.ExpectationFailed);
                result.Add("message", "Token is not valid.");
                result.Add("time", DateTime.Now.ToString("ddd, dd MMMM yyyy H:mm:ss tt"));

                return Ok(result);
            }

            TasksForProjects tasksForProject = db.TasksForProjects.FirstOrDefault(c => c.Id == tasksAction.taskId);
            if (tasksForProject != null) //add
            {
                bool complete = true;
                tasksForProject.isComplite = complete;
                db.SaveChanges();

                result.Add("code", HttpStatusCode.Accepted);
                result.Add("message", "ProjTask was updated");
                result.Add("user", tasksForProject);
            }
            else
            {
                result.Add("code", HttpStatusCode.Found);
                result.Add("message", "ProjTask wasn't found in db.");
            }
            result.Add("time", DateTime.Now.ToString("ddd, dd MMMM yyyy H:mm:ss tt"));

            return Ok(result);
        }

    }
}
﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ASP.NET_PersonControl.Models
{
    public class TasksForUser
    {
        public int Id { get; set; }
        public String title { get; set; }
        public String toUserId { get; set; }
        public String fromUserId { get; set; }
        public String description { get; set; }
        public DateTime dateTimeBegin { get; set; }
        public DateTime dateTimeEnd { get; set; }
        public Boolean isComplite { get; set; }

        public ApplicationUser userTo { get; set; }
        public ApplicationUser userFrom { get; set; }
    }

    public class TasksForGroups
    {
        public int Id { get; set; }
        public String title { get; set; }
        public int toGroupId { get; set; }
        public String fromUserId { get; set; }
        public String description { get; set; }
        public DateTime dateTimeBegin { get; set; }
        public DateTime dateTimeEnd { get; set; }
        public Boolean isComplite { get; set; }

        public String groupName { get; set; }
        public ApplicationUser userFrom { get; set; }
    }

    public class TasksForProjects
    {
        public int Id { get; set; }
        public String title { get; set; }
        public int toProjectId { get; set; }
        public String fromUserId { get; set; }
        public String description { get; set; }
        public DateTime dateTimeBegin { get; set; }
        public DateTime dateTimeEnd { get; set; }
        public Boolean isComplite { get; set; }

        public String projectName { get; set; }
        public ApplicationUser userFrom { get; set; }
    }
}
﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ASP.NET_PersonControl.Models
{
    [Table("AspNetUserRoles")] // set like class for a table
    public class UserRoles
    {
        public string ID { get; set; }
        public string USERID { get; set; }
        public string ROLEID { get; set; }
    }
}
﻿using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace ASP.NET_PersonControl.Models
{
    public class EmployeesContext : DbContext
    {
        public DbSet<Employee> employeesDBContext { get; set; }
    }
}
﻿using ASP.NET_PersonControl.Models;
using Microsoft.AspNet.Identity.EntityFramework;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web.Http;
using System.Web.Http.Description;
using System.Web.Script.Serialization;

namespace ASP.NET_PersonControl.Controllers.Api
{
    //[Authorize]
    public class UsersController : ApiController
    {
        //from body dosen't work 
        //use struct like from body for post request's
        public struct Email {
            [Required]
            [MaxLength(140)]
            public string email { get; set; }
            public DateTime date { get; set; }
        }

        public struct User{
             [Required]
            public string email { get; set; }
            public string displayName { get; set; }
            public string address { get; set; }
            public string city { get; set; }
            public string country { get; set; }
            public string phone { get; set; }
            public bool twoFactorEnabled { get; set; }
            public bool emailConfirmed { get; set; }
            public bool phoneConfirmed { get; set; }
        }

        private ApplicationDbContext db { get; set; }

        // GET api/<controller>
        [AcceptVerbs("Post")]
        [ResponseType(typeof(Dictionary<string, object>))]
        public IHttpActionResult GetUsers()
        {
            db = new ApplicationDbContext();
            List<ApplicationUser> users = db.Users.ToList();

            if (users == null) return NotFound(); //Ok(new Dictionary<string, object>() { { "code", HttpStatusCode.NoContent } });

            var Roles = db.Roles.Include(r => r.Users).ToList(); // get all roles where we have user on position
            foreach (ApplicationUser user in users)
                user.RoleNames = (from r in Roles
                                  from u in r.Users
                                  where u.UserId == user.Id
                                  select r.Name).ToList();  // get roles, select role from Roles where Roles.userId == user.Id (in asp.net Roles have array-property "Users",
                                                            // 1 item of "Users" = Dictionary<UserId, RoleId>. It's like table AspNetUserRoles.)

            Dictionary<string, object> result = new Dictionary<string, object>();
            result.Add("code", HttpStatusCode.Accepted);
            result.Add("users", users);
            result.Add("time", DateTime.Now.ToString("ddd, dd MMMM yyyy H:mm:ss tt"));

            return Ok(result);
        }

        [HttpPost]
        [ResponseType(typeof(bool))]
        public IHttpActionResult isUserExist(Email email)
        {
            db = new ApplicationDbContext();
            bool isExist = db.Users.Where(e => e.Email == email.email).Count() >= 1;
            Dictionary<string, object> result = new Dictionary<string, object>();
            if (isExist)
            {
                result.Add("code", HttpStatusCode.Found);
                result.Add("message", "User exists in db.");
            }
            else
            {
                result.Add("code", HttpStatusCode.NotFound);
                result.Add("message", "User dose not exist in db.");
            }
            result.Add("time", DateTime.Now.ToString("ddd, dd MMMM yyyy H:mm:ss tt"));

            return Ok(result);
        }
        
        [HttpDelete]
        [ResponseType(typeof(Dictionary<string, object>))]
        public IHttpActionResult DeleteUser([FromBody]string id)
        {
            Dictionary<string, object> result = new Dictionary<string, object>();
            // TODO: Add delete logic here
            db = new ApplicationDbContext();
            var employee = db.Users.SingleOrDefault(c => c.Id == id);
            if (employee != null)
            {
                db.Users.Remove(employee);
                db.SaveChanges();
                result.Add("code", HttpStatusCode.Accepted);
            }
            else
            {
                result.Add("code", HttpStatusCode.NotFound);
                result.Add("message", "User not found");
            }
            result.Add("time", DateTime.Now.ToString("ddd, dd MMMM yyyy H:mm:ss tt"));

            return Ok(result);
        }

        [HttpPost]
        [ResponseType(typeof(Dictionary<string, object>))]
        public IHttpActionResult AddUser(User user)
        {
            Dictionary<string, object> result = new Dictionary<string, object>();

            if(user.email == null)
            {
                result.Add("code", HttpStatusCode.ExpectationFailed);
                result.Add("message", "Incorrect data.");
                return Ok(result);
            }

            db = new ApplicationDbContext();

            var employee = db.Users.SingleOrDefault(c => c.Email == user.email);
            if (employee == null) //add
            {
                ApplicationUser newUser = new ApplicationUser();
                newUser.Email = user.email;
                newUser.UserName = user.email;
                newUser.DisplayName = user.displayName;
                newUser.Address = user.address;
                newUser.City = user.city;
                newUser.Country = user.country;
                newUser.PhoneNumber = user.phone;
                newUser.PhoneNumberConfirmed = user.phoneConfirmed;
                newUser.EmailConfirmed = user.emailConfirmed;
                newUser.TwoFactorEnabled = user.twoFactorEnabled;
                //user.UserName = user.email;
                db.Users.Add(newUser);
                db.SaveChanges();
                result.Add("code", HttpStatusCode.Accepted);
                result.Add("message", "User was add");
            }
            else 
            {
                result.Add("code", HttpStatusCode.Found);
                result.Add("message", "User exists in db.");
            }
            result.Add("time", DateTime.Now.ToString("ddd, dd MMMM yyyy H:mm:ss tt"));

            return Ok(result);
        }

        [HttpGet]
        [AcceptVerbs("Get", "Post")]
        // GET api/<controller>/5
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<controller>
        public void Post([FromBody]string value)
        {
        }

        // PUT api/<controller>/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/<controller>/5
        public void Delete([FromBody]string id)
        {
        }
    }
}
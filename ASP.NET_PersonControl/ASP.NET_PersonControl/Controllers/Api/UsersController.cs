using ASP.NET_PersonControl.Models;
using Microsoft.AspNet.Identity;
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
using System.Web.Helpers;
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
        public struct Email
        {
            [Required]
            [MaxLength(140)]
            public string email { get; set; }
            public DateTime date { get; set; }
        }

        public struct User
        {
            public User(string email, string password, string displayName, string address, string city, string country, string phone, bool twoFactorEnabled, bool emailConfirmed, bool phoneConfirmed, string token) : this()
            {
                this.email = email;
                this.password = password;
                this.displayName = displayName;
                this.address = address;
                this.city = city;
                this.country = country;
                this.phone = phone;
                this.twoFactorEnabled = twoFactorEnabled;
                this.emailConfirmed = emailConfirmed;
                this.phoneConfirmed = phoneConfirmed;
                this.token = token;
            }

            [Required]
            public string email { get; set; }
            public string password { get; set; }
            public string displayName { get; set; }
            public string address { get; set; }
            public string city { get; set; }
            public string country { get; set; }
            public string phone { get; set; }
            public bool twoFactorEnabled { get; set; }
            public bool emailConfirmed { get; set; }
            public bool phoneConfirmed { get; set; }
            public string token { get; set; }
        }

        private ApplicationDbContext db { get; set; }

        // GET api/<controller>
        [AcceptVerbs("Post")]
        [ResponseType(typeof(Dictionary<string, object>))]
        public IHttpActionResult GetUsers(User user)
        {
            Dictionary<string, object> result = new Dictionary<string, object>();
            if (!isTokenValid(user.token))
            {
                result.Add("code", HttpStatusCode.ExpectationFailed);
                result.Add("message", "Token is not valid.");
                result.Add("time", DateTime.Now.ToString("ddd, dd MMMM yyyy H:mm:ss tt"));

                return Ok(result);
            }

            db = new ApplicationDbContext();
            List<ApplicationUser> users = db.Users.ToList();

            if (users == null) return NotFound(); //Ok(new Dictionary<string, object>() { { "code", HttpStatusCode.NoContent } });

            var Roles = db.Roles.Include(r => r.Users).ToList(); // get all roles where we have user on position
            foreach (ApplicationUser us in users)
                us.RoleNames = (from r in Roles
                                  from u in r.Users
                                  where u.UserId == us.Id
                                  select r.Name).ToList();  // get roles, select role from Roles where Roles.userId == user.Id (in asp.net Roles have array-property "Users",
                                                            // 1 item of "Users" = Dictionary<UserId, RoleId>. It's like table AspNetUserRoles.)

            
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
        public IHttpActionResult DeleteUser(User user, string deleteID)
        {
            Dictionary<string, object> result = new Dictionary<string, object>();
            if (!isTokenValid(user.token))
            {
                result.Add("code", HttpStatusCode.ExpectationFailed);
                result.Add("message", "Token is not valid.");
                result.Add("time", DateTime.Now.ToString("ddd, dd MMMM yyyy H:mm:ss tt"));

                return Ok(result);
            }

            // TODO: Add delete logic here
            db = new ApplicationDbContext();
            var employee = db.Users.SingleOrDefault(c => c.Id == deleteID);
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
        public IHttpActionResult SignUp(User user)
        {
            Dictionary<string, object> result = new Dictionary<string, object>();

            if (user.email == null || user.password == null || user.password.Length < 6)
            {
                result.Add("code", HttpStatusCode.ExpectationFailed);
                result.Add("message", "Incorrect data. Email, password, name & phone can't be empty.");
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
                result.Add("user", newUser);
            }
            else
            {
                result.Add("code", HttpStatusCode.Found);
                result.Add("message", "User exists in db.");
            }
            result.Add("time", DateTime.Now.ToString("ddd, dd MMMM yyyy H:mm:ss tt"));

            return Ok(result);
        }

        [HttpPost]
        [ResponseType(typeof(Dictionary<string, object>))]
        public IHttpActionResult SignIn(User user)
        {
            Dictionary<string, object> result = new Dictionary<string, object>();
            db = new ApplicationDbContext();

            //var passwordHash = userManager.PasswordHasher.HashPassword("mySecurePassword");
            if (user.password != null && user.email != null && db.Users.FirstOrDefault(u => u.Email == user.email) != null) { 
                if (Crypto.VerifyHashedPassword(db.Users.FirstOrDefault(u => u.Email == user.email).PasswordHash, user.password)){

                    ApplicationUser usertempl = db.Users.FirstOrDefault(u => u.Email == user.email);

                    string token;

                    if(db.Tokens.FirstOrDefault(t => t.userId == usertempl.Id) != null)
                        token = db.Tokens.FirstOrDefault(t => t.userId == usertempl.Id).token.ToString();
                    else
                    {
                        byte[] time = BitConverter.GetBytes(DateTime.UtcNow.ToBinary());
                        byte[] key = Guid.NewGuid().ToByteArray();
                        token = Convert.ToBase64String(time.Concat(key).ToArray());
                        db.Tokens.Add(new Token() { userId = usertempl.Id, token = token });
                        db.SaveChanges();
                    }

                    result.Add("code", HttpStatusCode.Accepted);
                    result.Add("message", "You can login");
                    result.Add("token", token);
                    result.Add("user", usertempl);
                    result.Add("time", DateTime.Now.ToString("ddd, dd MMMM yyyy H:mm:ss tt"));
                    return Ok(result);
                } else
                {
                    result.Add("code", HttpStatusCode.NotAcceptable);
                    result.Add("message", "Email or password is not correct.");
                    result.Add("time", DateTime.Now.ToString("ddd, dd MMMM yyyy H:mm:ss tt"));
                    return Ok(result);
                }
            } else {
                result.Add("code", HttpStatusCode.NotAcceptable);
                result.Add("message", "User not found.");
                result.Add("time", DateTime.Now.ToString("ddd, dd MMMM yyyy H:mm:ss tt"));
                return Ok(result);
            }

            //result.Add("code", HttpStatusCode.InternalServerError);
            //result.Add("message", "Server internal error.");
            //result.Add("time", DateTime.Now.ToString("ddd, dd MMMM yyyy H:mm:ss tt"));
            //return Ok(result);
        }

        [HttpPost]
        [ResponseType(typeof(Dictionary<string, object>))]
        public IHttpActionResult GoogleSignIn(User user)
        {
            Dictionary<string, object> result = new Dictionary<string, object>();

            if (user.email == null)
            {
                result.Add("code", HttpStatusCode.ExpectationFailed);
                result.Add("message", "Incorrect data. GoogleSignIn Email can't be empty.");
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
                result.Add("data", user);
                result.Add("message", "User was add by Google Account. You can login with Google.");
                
                string token;
                if (db.Tokens.FirstOrDefault(t => t.userId == newUser.Id) != null)
                    token = db.Tokens.FirstOrDefault(t => t.userId == newUser.Id).token.ToString();
                else
                {
                    byte[] time = BitConverter.GetBytes(DateTime.UtcNow.ToBinary());
                    byte[] key = Guid.NewGuid().ToByteArray();
                    token = Convert.ToBase64String(time.Concat(key).ToArray());
                    db.Tokens.Add(new Token() { userId = newUser.Id, token = token });
                    db.SaveChanges();
                }
                result.Add("token", token);
                result.Add("time", DateTime.Now.ToString("ddd, dd MMMM yyyy H:mm:ss tt"));
                return Ok(result);
            }else
            {
                result.Add("code", HttpStatusCode.Accepted);
                result.Add("message", "You can login with Google.");

                string token;
                if (db.Tokens.FirstOrDefault(t => t.userId == employee.Id) != null)
                    token = db.Tokens.FirstOrDefault(t => t.userId == employee.Id).token.ToString();
                else
                {
                    byte[] time = BitConverter.GetBytes(DateTime.UtcNow.ToBinary());
                    byte[] key = Guid.NewGuid().ToByteArray();
                    token = Convert.ToBase64String(time.Concat(key).ToArray());
                    db.Tokens.Add(new Token() { userId = employee.Id, token = token });
                    db.SaveChanges();
                }
                result.Add("token", token);
                result.Add("time", DateTime.Now.ToString("ddd, dd MMMM yyyy H:mm:ss tt"));
                return Ok(result);
            }
        }

        [HttpPost]
        [ResponseType(typeof(Dictionary<string, object>))]
        public IHttpActionResult ResetPassword(User user)
        {
            Dictionary<string, object> result = new Dictionary<string, object>();
            result.Add("time", DateTime.Now.ToString("ddd, dd MMMM yyyy H:mm:ss tt"));
            var userFounded = db.Users.FirstOrDefault(u => u.Email == user.email);
            //if (user == null || !(await UserManager.IsEmailConfirmedAsync(user.Id))) отправка писма только на подтвержденную почту
            if (userFounded == null)
            {
                // Не показывать, что пользователь не существует или не подтвержден
                result.Add("message", "Email is not valid.");
                result.Add("code", HttpStatusCode.NotFound);
                return Ok(result);
            }

            // Дополнительные сведения о включении подтверждения учетной записи и сброса пароля см. на странице https://go.microsoft.com/fwlink/?LinkID=320771.
            // Отправка сообщения электронной почты с этой ссылкой
            new AccountController().ForgotPasswordForApi(userFounded.Email);
            result.Add("message", "Reset link was send on email.");
            result.Add("code", HttpStatusCode.Accepted);
            return Ok(result);
        }

        private bool isTokenValid(string token) {
            db = new ApplicationDbContext();

            if (db.Tokens.FirstOrDefault(t => t.token == token) != null)
                return true;
            else
                return false;
        }

        private bool Check(String token)
        {
            //byte[] data = Convert.FromBase64String(token);
            //DateTime when = DateTime.FromBinary(BitConverter.ToInt64(data, 0));
            db = new ApplicationDbContext();
            if (db.Tokens.FirstOrDefault(t => t.token == token) != null)
                return true;
            else
                return false;
        }
    }
}
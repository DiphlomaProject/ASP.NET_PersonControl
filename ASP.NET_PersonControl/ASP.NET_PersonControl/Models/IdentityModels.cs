using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Drawing;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace ASP.NET_PersonControl.Models
{
    // В профиль пользователя можно добавить дополнительные данные, если указать больше свойств для класса ApplicationUser. Подробности см. на странице https://go.microsoft.com/fwlink/?LinkID=317594.
    public class ApplicationUser : IdentityUser
    {
        //Extended Properties
        //[Required(ErrorMessage = "Please enter Address.")]
        public string Address { get; set; }
        //[Required(ErrorMessage = "Please enter Country.")]
        public string Country { get; set; }
        //[Required(ErrorMessage = "Please enter City.")]
        public string City { get; set; }
        [StringLength(256)]
        //[Required(ErrorMessage = "Please enter your Name.")]
        public string DisplayName { get; set; }
        //[Required(ErrorMessage = "Please enter Role Empl")]
        public List<string> RoleNames;
        public byte[] img;

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Обратите внимание, что authenticationType должен совпадать с типом, определенным в CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Здесь добавьте утверждения пользователя
            return userIdentity;
        }
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<Customers> Customers { get; set; }
        public DbSet<UsersGroups> UsersGroups { get; set; }
        public DbSet<Groups> Groups { get; set; }
        public DbSet<ProjectsGroups> ProjectsGroups { get; set; }
        public DbSet<Projects> Projects { get; set; }
        public DbSet<Token> Tokens { get; set; }

        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }
    }
}
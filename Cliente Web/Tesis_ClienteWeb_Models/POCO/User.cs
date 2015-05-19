using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Threading.Tasks;
using System.Security.Claims;
using Microsoft.AspNet.Identity;

namespace Tesis_ClienteWeb_Models.POCO
{
    public class User : IdentityUser
    {
        #region Identity configuration
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<User> manager)
        {
            var userIdentity =
                await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);

            return userIdentity;
        }
        #endregion

        public string Name { get; set; }

        public string LastName { get; set; }

        public School School { get; set; }
                
        public List<Notification> Notifications { get; set; }

        public List<Event> Events { get; set; }

        public List<CASU> CASUs { get; set; }

        public List<SentNotification> ReceivedNotifications { get; set; }

        public User()
        {
            this.CASUs = new List<CASU>();
            this.Notifications = new List<Notification>(); //Lista de notificaciones creadas por este usuario
            this.Events = new List<Event>();
            this.ReceivedNotifications = new List<SentNotification>(); /* Lista de notificaciones recibidas
                                                                        * por este usuario*/
        }
    }
}

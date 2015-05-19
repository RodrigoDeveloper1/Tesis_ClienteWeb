using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tesis_ClienteWeb_Models.POCO
{
    public class Role : IdentityRole
    {
        #region Constructores
        public Role() : base() 
        { 
            Profiles = new HashSet<Profile>();
        }

        public Role(string name) : base(name)
        {

        }
        #endregion

        public string Description { get; set; }

        public ICollection<Profile> Profiles { get; set; }
    }
}

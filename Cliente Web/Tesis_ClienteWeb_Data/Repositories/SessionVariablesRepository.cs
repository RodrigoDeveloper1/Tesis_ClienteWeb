using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Tesis_ClienteWeb_Data.UserExceptions;

namespace Tesis_ClienteWeb_Data.Repositories
{
    public class SessionVariablesRepository
    {
        private bool _Administrador;
        private bool _Coordinador;
        private string _UserId;
        private string _Username;
        private string _RoleId;
        private string _RoleName;
        private int _SchoolId;
        private int _SchoolYearId;
        private DateTime _StartDate;
        private DateTime _DateOfCompletion;

        public bool ADMINISTRADOR { get { return _Administrador; } }
        public bool COORDINADOR { get { return _Coordinador; } }
        public string USERID { get { return _UserId; } }
        public string USERNAME { get { return _Username; } }
        public string ROLEID { get { return _RoleId; } }
        public string ROLENAME { get { return _RoleName; } }
        public int SCHOOLID { get { return _SchoolId; } }
        public int SCHOOLYEARID { get { return _SchoolYearId; } }
        public DateTime STARTDATE { get { return _StartDate; } }
        public DateTime DATEOFCOMPLETION { get { return _DateOfCompletion; } } 

        public SessionVariablesRepository()
        {
            if (HttpContext.Current == null)
            {
                throw new Exception();
            }
            else
            {
                if (!HttpContext.Current.Session.IsNewSession)
                {
                    this._Administrador = (bool)HttpContext.Current.Session["Administrador"];
                    this._Coordinador = (bool)HttpContext.Current.Session["Coordinador"];
                    this._UserId = HttpContext.Current.Session["UserId"].ToString();
                    this._Username = HttpContext.Current.Session["UserName"].ToString();
                    this._RoleId = HttpContext.Current.Session["RoleId"].ToString();
                    this._RoleName = HttpContext.Current.Session["RoleName"].ToString();
                    this._SchoolId = Convert.ToInt32(HttpContext.Current.Session["SchoolId"]);
                    this._SchoolYearId = Convert.ToInt32(HttpContext.Current.Session["SchoolYearId"]);
                    this._StartDate = Convert.ToDateTime(HttpContext.Current.Session["StartDate"]);
                    this._DateOfCompletion = Convert.ToDateTime(HttpContext.Current.Session["DateOfCompletion"]);
                }
                else
                    throw new SessionExpiredException();
            }            
        }
    }
}
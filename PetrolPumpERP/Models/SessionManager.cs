using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PetrolPumpERP.Models
{
    public class SessionManager
    {
        public static SessionManager _userBl = null;
        private SessionManager()
        {

        }

        public static SessionManager Instance
        {
            get
            {
                if (_userBl == null)
                {
                    _userBl = new SessionManager();
                }
                return _userBl;
            }
        }

        public UserModel ActiveUser
        {
            get
            {
                if (HttpContext.Current.Session["ActiveUser"] == null)
                {
                    return null;
                }
                return (UserModel)HttpContext.Current.Session["ActiveUser"];
            }
            set
            {
                HttpContext.Current.Session["ActiveUser"] = value;
            }
        }
    }
}
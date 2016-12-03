using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PetrolPumpERP.Models
{
    public class QueryStringManager
    {
        public static QueryStringManager _userBl = null;
        private QueryStringManager()
        {

        }

        public static QueryStringManager Instance
        {
            get
            {
                if (_userBl == null)
                {
                    _userBl = new QueryStringManager();
                }
                return _userBl;
            }
        }

        public string GetValueFromQueryString(string key)
        {
            return Convert.ToString(HttpContext.Current.Request.Params[key]);
        }
    }
}
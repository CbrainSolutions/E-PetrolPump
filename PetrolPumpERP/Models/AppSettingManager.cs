using PetrolPumpERP.Models.DataEntities;
using PetrolPumpERP.Models.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PetrolPumpERP.Models
{
    public class AppSettingManager
    {
        public static AppSettingManager _userBl = null;
        PetrolPumpERPEntities _db = new PetrolPumpERPEntities();
        OtherAccountModelBL otherac = OtherAccountModelBL.Instance;
        private AppSettingManager()
        {

        }

        public static AppSettingManager Instance
        {
            get
            {
                if (_userBl == null)
                {
                    _userBl = new AppSettingManager();
                }
                return _userBl;
            }
        }

        public tblLedger CashLedger
        {
            get
            {
                return _db.tblLedgers.Where(p => p.LedgerId == 1).FirstOrDefault();
            }
        }

        public OtherAccountModel SalesLedger
        {
            get
            {
                return otherac.GetOtherAccountDetails().OtherAccountList.Where(p => p.SubledgerId == 19).FirstOrDefault();
            }
        }

        
    }
}
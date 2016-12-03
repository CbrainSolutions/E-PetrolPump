using PetrolPumpERP.Models.DataEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PetrolPumpERP.Models.DataModels
{
    public class UserBL
    {
        public static UserBL _userBl = null;
        PetrolPumpERP.Models.DataEntities.PetrolPumpERPEntities _db = new PetrolPumpERPEntities();
        private UserBL()
        {

        }

        public static UserBL Instance
        {
            get {
                if (_userBl == null)
                {
                    _userBl = new UserBL();
                }
                return _userBl;
            }
        }


        public UserResponse AuthenticateUser(LoginViewModel model)
        {
            UserResponse response = new UserResponse() { Status=false};
            response.UserList = _db.tblUsers.Join(_db.tblUserTypes,
                                 user => user.UserTypeId,
                                 usertype => usertype.UserTypeId,
                                 (user, usertype) => new { entityuser = user, entityusertype = usertype }).Where(p =>
                                p.entityuser.Username == model.Email && p.entityuser.Password == model.Password).Select(p =>
                                new UserModel()
                                {
                                    IsDelete = p.entityuser.IsDelete,
                                    LedgerId = p.entityuser.LedgerId,
                                    Password = p.entityuser.Password,
                                    UserId = p.entityuser.UserId,
                                    Username = p.entityuser.Username,
                                    UserType = p.entityusertype.UserType,
                                    UserTypeId = p.entityusertype.UserTypeId
                                });
            if (response.UserList.Count()>0)
            {
                SessionManager.Instance.ActiveUser = response.UserList.FirstOrDefault();
                response.Status = true;
            }
            else
            {
                response.Message = "Invalid user name and password.";
            }
            return response;
        }

    }

    public class UserResponse:Error
    {
        public IQueryable<UserModel> UserList { get; set; }
    }

    public class Error
    {
        public bool Status { get; set; }

        public string Message { get; set; }

        public long? Id { get; set; }
    }
}
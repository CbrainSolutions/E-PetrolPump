using PetrolPumpERP.Models.DataEntities;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Web;

namespace PetrolPumpERP.Models.DataModels
{

    public class Employee
    {
        public int EmpCode { get; set; }
        public string EmpFirstName { get; set; }
        public string EmpMiddleName { get; set; }
        public string EmpLastName { get; set; }
        public string EmpAddress { get; set; }
        public string Password { get; set; }
        public int DeptId { get; set; }
        public DateTime? EmpDOB { get; set; }
        public DateTime? EmpDOJ { get; set; }
        public string UserType { get; set; }
        public int? DesignationId { get; set; }
        public bool IsDelete { get; set; }
        public string BankName { get; set; }
        public string BankACNo { get; set; }
        public string PFNo { get; set; }
        public string PanNo { get; set; }
        public decimal? BasicSalary { get; set; }
        public string EmpEmail { get; set; }

        public string AccountType { get; set; }
        public long? SubledgerId { get; set; }
        public int? AccountTypeId { get; set; }

        public Nullable<long> LedgerId { get; set; }

    }

    

    public class EmployeeResponse : Error
    {
        public IQueryable<Employee> EmployeeList { get; set; }
    }

    public class EmployeeBL
    {
        public static EmployeeBL _userBl = null;
        PetrolPumpERPEntities _db = new PetrolPumpERPEntities();
        private EmployeeBL()
        {

        }

        public static EmployeeBL Instance
        {
            get
            {
                if (_userBl == null)
                {
                    _userBl = new EmployeeBL();
                }
                return _userBl;
            }
        }

        public EmployeeResponse Update(Employee oEmployee)
        {
            //int EmployeeCode = 0;
            EmployeeResponse response = new EmployeeResponse() { Status = false };
            tblEmployee tbl = _db.tblEmployees.Where(p => p.EmpEmail.Trim().ToLower() == oEmployee.EmpEmail.Trim().ToLower()).FirstOrDefault();
            if (tbl == null)
            {
                try
                {
                    tbl = new tblEmployee()
                    {
                        EmpFirstName = oEmployee.EmpFirstName,
                        EmpMiddleName = oEmployee.EmpMiddleName,
                        EmpLastName = oEmployee.EmpLastName,
                        EmpAddress = oEmployee.EmpAddress,
                        Password = oEmployee.Password,
                        DeptId = 1,
                        EmpDOB = DateTime.Now.Date,
                        EmpDOJ = DateTime.Now.Date,
                        UserType = "User",
                        DesignationId = 1,
                        IsDelete = false,
                        BankName = "",
                        BankACNo = "",
                        PFNo = "",
                        PanNo = "",
                        BasicSalary = oEmployee.BasicSalary,
                        EmpEmail = oEmployee.EmpEmail,

                    };
                    _db.tblEmployees.Add(tbl);

                    var state = _db.SaveChanges();
                    response.Status = true;
                }
                catch (DbEntityValidationException ex)
                {
                    foreach (var entityValidationErrors in ex.EntityValidationErrors)
                    {
                        foreach (var validationError in entityValidationErrors.ValidationErrors)
                        {
                        }
                    }
                }
            }
            return response;
        }

        public EmployeeResponse GetAllEmployees()
        {
            EmployeeResponse response = new EmployeeResponse() { Status = false };
            response.EmployeeList = _db.tblEmployees.Select(p =>
                                new Employee()
                                {
                                    EmpCode = p.EmpCode,
                                    EmpFirstName = p.EmpFirstName,
                                    EmpMiddleName = p.EmpMiddleName,
                                    EmpLastName = p.EmpLastName,
                                    EmpAddress = p.EmpAddress,
                                    Password = p.Password,
                                    DeptId = p.DeptId,
                                    EmpDOB = p.EmpDOB,
                                    EmpDOJ = p.EmpDOJ,
                                    UserType = p.UserType,
                                    DesignationId = p.DesignationId,
                                    IsDelete = p.IsDelete,
                                    BankName = p.BankName,
                                    BankACNo = p.BankACNo,
                                    PFNo = p.PFNo,
                                    PanNo = p.PanNo,
                                    BasicSalary = p.BasicSalary,
                                    EmpEmail=p.EmpEmail
                                });
            if (response.EmployeeList.Count() > 0)
            {
                response.Status = true;
            }
            return response;
        }


        public EmployeeResponse Save(Employee oEmployee)
        {
            //int EmployeeCode = 0;
            EmployeeResponse response = new EmployeeResponse() { Status=false};
            tblEmployee tbl = _db.tblEmployees.Where(p => p.EmpEmail.Trim().ToLower() == oEmployee.EmpEmail.Trim().ToLower()).FirstOrDefault();
            if (tbl == null)
            {
                try
                {
                    tbl = new tblEmployee()
                    {
                        EmpFirstName = oEmployee.EmpFirstName,
                        EmpMiddleName = oEmployee.EmpMiddleName,
                        EmpLastName = oEmployee.EmpLastName,
                        EmpAddress = oEmployee.EmpAddress,
                        Password = oEmployee.Password,
                        DeptId = 1,
                        EmpDOB = DateTime.Now.Date,
                        EmpDOJ = DateTime.Now.Date,
                        UserType = "User",
                        DesignationId = 1,
                        IsDelete = false,
                        BankName = "",
                        BankACNo = "",
                        PFNo = "",
                        PanNo = "",
                        BasicSalary = oEmployee.BasicSalary,
                        EmpEmail = oEmployee.EmpEmail,

                    };
                    _db.tblEmployees.Add(tbl);

                    var state = _db.SaveChanges();
                    response.Status = true;
                }
                catch (DbEntityValidationException ex)
                {
                    foreach (var entityValidationErrors in ex.EntityValidationErrors)
                    {
                        foreach (var validationError in entityValidationErrors.ValidationErrors)
                        {
                        }
                    }
                }
            }
            return response;
        }
    }
}

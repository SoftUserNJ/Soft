using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using SoftaxeERP_API.Models;
using SoftaxeERP_API.VM;
using System.Data;

namespace SoftaxeERP_API.Services
{
    public interface IEmployee
    {
        // Employee Entry

        DataTable GetMaxEmpId();
        DataTable GetMainLocation();
        DataTable GetDepartment();
        DataTable GetLocation();
        DataTable GetShift();
        DataTable GetEmployeeList();

        DataTable GetEmployees();
        string AddUpdateEmployee(EmployeeVM emp);
        string EditEmployee(int empy_id);

        string DeleteEmployee(int empy_id);
        DataTable GetStatus();

        // Salary Setllement
        DataTable GetSalarySettlementNo();
        DataTable GetSalarySettlementLabels();

        DataTable GetEditSalrySetlment(int empy_id);
        string AddUpdateSalryStlmnt([FromBody] Tblemploysalarydt requestData);

        bool DelSalrySetlment(int empy_id, int SrNo);

        // Employee Family Detail

        string AddUpdateEmpFamily(List<EmpFamilyVM> family);

        DataTable GetEmpFamilyList();

        DataTable GetEditEmpFamily(int empy_id);

        bool DelEmpFamily(int empy_id);
    }

    public class Employee : IEmployee
    {
        private readonly ErpSoftaxeContext _context;
        private readonly IDataLogic _dataLogic;
        private readonly IAuth _auth;
        private readonly IWebHostEnvironment _hostingEnvironment;
        private readonly IFileUpload _fileUpload;
        readonly AuthVM auth = new();
        public Employee(ErpSoftaxeContext context, IDataLogic dataLogic, IAuth authData, IWebHostEnvironment hostingEnvironment, IFileUpload fileUpload)
        {
            _context = context;
            _dataLogic = dataLogic;
            _auth = authData;
            _hostingEnvironment = hostingEnvironment;
            _fileUpload = fileUpload;
            auth = _auth.GetUserData();
        }

        #region Employee Entry 

        // Employee Entry

        public DataTable GetMaxEmpId()
        {
            String qry = $@"Select  isnull(MAX(empy_id),0)+1 as empy_id from tblEmployeeSetup where comp_id = '"+auth.CmpId+"' and LocId = '"+auth.LocId+"'";
            return _dataLogic.LoadData(qry);
        }

        public DataTable GetDepartment()
        {
            String qry = $@"Select Id, Department from tblcompanydepartment where Comp_id = '"+auth.CmpId+"' and LocId = '"+auth.LocId+"'";
            return _dataLogic.LoadData(qry);
        }

        public DataTable GetEmployees()
        {
            String qry = $@"Select empy_id, name from tblEmployeeSetup where comp_id = '"+auth.CmpId+"' and LocId = '"+auth.LocId+"'";
            return _dataLogic.LoadData(qry);
        }


        public DataTable GetLocation()
        {
            String qry = $@"select CostcentreId, CostcentreName from TblCostCentre where locid='" +auth.LocId+ "' and cmpid = '"+auth.CmpId+"'";
            return _dataLogic.LoadData(qry);
        }

        public DataTable GetMainLocation()
        {
            String qry = $@"select LocName, LocId from Location where locid='"+auth.LocId+"' and cmp_id = '"+auth.CmpId+"'";
            return _dataLogic.LoadData(qry);
        }

        public DataTable GetShift()
        {
            String qry = $@"Select Id, Shift from TblEmployeeShift  where comp_id = '"+auth.CmpId+"' and LocId = '"+auth.LocId+"'";
            return _dataLogic.LoadData(qry);
        }

        public DataTable GetEmployeeList()
        {
            String qry = $@"SELECT Name,  empy_id, fname, email, Location, c.CostcentreName as LocatioName, d.Department, mob, 
                    '/Companies/" + auth.CmpName + "/EmployeeImages/' + ES.EmpPhoto as EmpPhoto from tblEmployeeSetup ES Inner Join tblcompanydepartment d on d.id = ES.deptId  and d.LocId = ES.LocId and d.Comp_id = ES.Comp_id  Inner Join TblEmployeeShift s on s.id = ES.shift and s.LocId = ES.LocId and s.Comp_id = ES.Comp_id left outer Join TblCostCentre c on c.CostcentreId = es.Location  and c.LocId = ES.LocId and c.CmpId = ES.Comp_id  where ES.comp_id = '" + auth.CmpId + "' And ES.LocId = '" + auth.LocId + "'";
            return _dataLogic.LoadData(qry);
        }

        public string AddUpdateEmployee(EmployeeVM emp)
        {
            DateTime dtNow = DateTime.Now;
            using var transaction = _context.Database.BeginTransaction();

            foreach (var property in typeof(EmployeeVM).GetProperties())
            {
                if (property.PropertyType == typeof(string))
                {
                    var value = (string)property.GetValue(emp);
                    if (value == "null")
                    {
                        property.SetValue(emp, null);
                    }
                }
            }

            try
            {
               // var L4Code = _context.Level4s
               //.Where(x => x.CompId == auth.CmpId && x.Tag == "HRSALARY" && x.Mappedcode == emp.Location && x.LocId == auth.LocId)
               // .Select(x => new { code = x.Level3 + x.Level41 })
               //    .FirstOrDefault()?.code;


                var query = _context.Level4s
                .Where(l4 => l4.CompId == auth.CmpId && l4.Mappedcode.StartsWith(auth.LocId) &&
                 l4.Tag.StartsWith("HR"))
                .Select(l4 => new
                {
                    code = l4.Level3 + l4.Level41
                }).ToList();


                if (query.Count == 0)
                {
                    return "L4Null";
                }

                string L5Code = $"{emp.EmpyId:D5}";

                TblEmployeeSetup employee = _context.TblEmployeeSetups.Where(x => x.CompId == auth.CmpId && x.EmpyId == emp.EmpyId && x.LocId == auth.LocId).FirstOrDefault();
                if (employee != null)
                {
                    _context.TblEmployeeSetups.Remove(employee);

                    foreach(var l4 in query)
                    {
                        var level5Entry = _context.Level5s.FirstOrDefault(x => (x.Level4 + x.Level51) == (l4.code + L5Code));
                        if (level5Entry != null)
                        {
                            _context.Level5s.Remove(level5Entry);
                        }
                    }
                   
                }

                if (emp.EmpyId == 0)
                {
                    emp.EmpyId = (_context.TblEmployeeSetups
                        .Max(x => (int?)x.EmpyId) ?? 0) + 1;
                }

                //bool email = _context.TblEmployeeSetups.Any(u => u.Email == emp.Email);

                //if (email == true)
                //{
                //    return "Email Already Exists";
                //}



                var fileName = "No-image.jpg";
                // var documentName = "No-image.jpg";

                if (emp.Picture != null)
                {
                    var extension = Path.GetExtension(emp.Picture.FileName);
                    fileName = emp.EmpyId + emp.Name + extension;
                }
                else
                {
                    if (employee != null)
                    {
                        if (employee.EmpPhoto != "No-image.jpg")
                        {
                            fileName = employee.EmpPhoto;
                        }
                    }
                    else
                    {
                        fileName = "No-image.jpg";
                    }
                }


                //if (emp.Document != null)
                //{
                //    var extension = Path.GetExtension(emp.Document.FileName);
                //    documentName = emp.EmpyId + emp.Name + extension;
                //}
                //else
                //{
                //    if (employee != null)
                //    {
                //        if (employee.Attachements != "No-image.jpg")
                //        {
                //            fileName = employee.Attachements;
                //        }
                //    }
                //    else
                //    {
                //        fileName = "No-image.jpg";
                //    }
                //}

                foreach (var l4 in query)
                {
                    _context.Level5s.Add(new Level5
                    {
                        Level4 = l4.code,
                        Level51 = L5Code,
                        Names = emp.Name,
                        LocId = auth.LocId,
                        CompId = auth.CmpId
                    });
                }

               


                _context.TblEmployeeSetups.Add(new TblEmployeeSetup
                {
                    EmpyId = emp.EmpyId,
                    CompId = auth.CmpId,
                    Name = emp.Name,
                    Fname = emp.Fname,
                    Fcnic = emp.Fcnic,
                    MotherName = emp.MotherName,
                    Mothercnic = emp.Mothercnic,
                    WifeName = emp.WifeName,
                    Wifecnic = emp.Wifecnic,
                    Address1 = emp.Address1,
                    Address2 = emp.Address2,
                    City = emp.City,
                    Ph1 = emp.Ph1,
                    Ph2 = emp.Ph2,
                    Mob = emp.Mob,
                    Email = emp.Email,
                    Nic = emp.Nic,
                    Ntn = emp.Ntn,
                    BirthDate = emp.BirthDate,
                    AppDate = emp.AppDate,
                    Gender = emp.Gender,
                    Marital = emp.Marital,
                    Jobstatus = emp.Jobstatus,
                    Remarks = emp.Remarks,
                    Tumbid = emp.Tumbid,
                    Acctno = emp.Acctno,
                    Location = emp.Location,
                    LocId = auth.LocId,
                    Shift = emp.Shift,
                    BloodGroup = emp.BloodGroup,
                    Eobino = emp.Eobino,
                    Ssno = emp.Ssno,
                    Srno = emp.EmpyId.ToString(),
                    Type = "Employee",
                    Active1 = emp.Active1,
                    DeptId = emp.DeptId,
                    DesgnId = emp.DesgnId,
                    EmpPhoto = fileName,
                    // Attachements = documentName,
                    Ot = emp.Ot

                });


                // file, comapnyName, FolderName, fileName
                _fileUpload.fileUpload(emp.Picture, auth.CmpName, "EmployeeImages", emp.EmpyId + emp.Name, _hostingEnvironment);
                // _fileUpload.fileUpload(emp.Document, auth.CmpName, "EmployeeDocuments", emp.EmpyId + emp.Name, _hostingEnvironment);


                if (!string.IsNullOrEmpty(emp.LeaveEntryString))
                {
                    var leaveEntries = JsonConvert.DeserializeObject<List<LeaveEntryVM>>(emp.LeaveEntryString);

                    DateTime dateTime = DateTime.Now;

                    List<Tblempleaf> leavesToRemove = _context.Tblempleaves
                    .Where(x => x.CompId == auth.CmpId && x.EmpyId == emp.EmpyId && x.LocId == auth.LocId)
                    .ToList();

                    foreach (var leave in leavesToRemove)
                    {
                        _context.Tblempleaves.Remove(leave);
                    }

                    foreach (var item in leaveEntries)
                    {
                        _context.Tblempleaves.Add(new Tblempleaf
                        {
                            EmpyId = emp.EmpyId,
                            LvId = item.leaveType,
                            NoOfLvs = item.LvAmnt,
                            CompId = auth.CmpId,
                            LocId = auth.LocId,
                            Trdate = dateTime

                        });
                    }
                }


                _context.SaveChanges();
                transaction.Commit();
                _dataLogic.LogEntry(emp.EmpyId, "Employee", $"{((employee == null) ? "Add" : "Edit")} Employee - {emp.Name} ", 0, dtNow, 0, 0, 0, dtNow);
                return "true";
            }
            catch (Exception)
            {
                transaction.Rollback();
                return "false";
                throw;
            }
        }

        public string EditEmployee(int empy_id)
        {
            string qry1 = @"SELECT Name, nullif(Fname, 'NULL') as Fname, nullif(Fcnic, 'NULL') as Fcnic, nullif(tumbid, 'NULL') as tumbid, nullif(Mothercnic, 'NULL') as Mothercnic, nullif(MotherName, 'NULL') as MotherName, nullif(Wifecnic, 'NULL') as Wifecnic, nullif(WifeName, 'NULL') as WifeName, nullif(address1, 'NULL') as address1, deptId, nullif(BloodGroup, 'NULL') as BloodGroup, city, ph1, ph2, birth_date, app_date, Email, mob, gender, ntn, nic, Location, Es.Shift, marital, Acctno, EOBINO, SSNO, remarks, active1, ot, empy_id, fname, email, Location, d.Department, mob, 
                    '/Companies/" + auth.CmpName + "/EmployeeImages/' + ES.EmpPhoto as EmpPhoto, '/Companies/" + auth.CmpName + "/EmployeeDocuments/' + ES.Attachements as document from tblEmployeeSetup ES Inner Join tblcompanydepartment d on d.id = ES.deptId  AND D.comp_id= '" + auth.CmpId + "'    and d.locid= '" + auth.LocId + "' Inner Join TblEmployeeShift s on s.id = ES.shift AND s.comp_id= '"+auth.CmpId+"' and s.locid= '"+auth.LocId+"'  where ES.comp_id = '" + auth.CmpId + "' AND  ES.empy_id = '" + empy_id + "' AND ES.LocId = '" + auth.LocId + "'";

            string qry2 = @"Select h.Name, el.lv_id, NoOfLvs from tblempleaves el
                        Inner Join TBLHRSetup h On h.HrSetupId = el.lv_id and el.Comp_id = h.Cmp_id and el.locId = h.LocId
                        where empy_id = '" + empy_id + "' And el.Comp_id = '" + auth.CmpId + "' And  el.locId = '" + auth.LocId + "' ";


            var dt1 = _dataLogic.LoadData(qry1);
            var dt2 = _dataLogic.LoadData(qry2);

            return JsonConvert.SerializeObject(new { EmpData = dt1, LeaveData = dt2 });


        }

        public string DeleteEmployee(int empy_id)
        {
            DateTime dtNow = DateTime.Now;
            using var transaction = _context.Database.BeginTransaction();
            try
            {

                bool isInUse = _context.Tblemploysalarydts.Any(t => t.EmpyId == empy_id) ||
                                _context.TblChildren.Any(t => t.EmpyId == empy_id) ||
                                _context.TblEobis.Any(t => t.EmpyId == empy_id) ||
                                _context.TblLvEnchasments.Any(t => t.EmpyId == empy_id) ||
                                _context.TblIncomeTaxes.Any(t => t.EmpyId == empy_id) ||
                                _context.TblStaffLoans.Any(t => t.EmpyId == empy_id) ||
                                _context.TblVehicleLoans.Any(t => t.EmpyId == empy_id) ||
                                _context.TblAdvanceSalaries.Any(t => t.EmpyId == empy_id) ||
                                _context.TblOverTimes.Any(t => t.EmpyId == empy_id) ||
                                _context.TblleavesEntries.Any(t => t.EmpyId == empy_id) ||
                                _context.Tblempleaves.Any(t => t.EmpyId == empy_id);

                if (isInUse)
                {
                    return "Already in Use";
                }

             var employee =   _context.TblEmployeeSetups
                    .Where(x => x.EmpyId == empy_id && x.CompId == auth.CmpId && x.LocId == auth.LocId)
                    .FirstOrDefault();
                if (employee == null)
                {
                    return "false";
                }
                _context.TblEmployeeSetups.Remove(employee);

                var query = _context.Level4s.Where(         
                    l4 => l4.CompId == auth.CmpId && l4.Mappedcode.StartsWith(auth.LocId) &&
                    l4.Tag.StartsWith("HR"))
                    .Select(l4 => new
                    {
                        code = l4.Level3 + l4.Level41
                    }).ToList();


                string L5Code = $"{employee.EmpyId:D5}";

                foreach (var l4 in query)
                {
                    var level5Entry = _context.Level5s.FirstOrDefault(x => (x.Level4 + x.Level51) == (l4.code + L5Code));
                    if (level5Entry != null)
                    {
                        _context.Level5s.Remove(level5Entry);
                    }
                }

                _context.SaveChanges();
                transaction.Commit();
                _dataLogic.LogEntry(empy_id, "Employee", $"Deleted Employee - {employee.Name}", 0, dtNow, 0, 0, 0, dtNow);
                return "true";
            }
            catch (Exception)
            {
                transaction.Rollback();
                return "false";
                throw;
            }
        }


        public DataTable GetStatus()
        {
            String qry = $@"Select Id, Description from tblEmployeeStatus  where locid = '"+auth.LocId+"' and comp_id ='"+auth.CmpId+"'";
            return _dataLogic.LoadData(qry);
        }


        #endregion

        #region Salary Settlement 

        // Salary Settlement

        public DataTable GetSalarySettlementNo()
        {
            String qry = $@"SELECT isnull(MAX(SrNo),0)+1 AS SrNo FROM tblEmploysalarydt where comp_id = '" + auth.CmpId + "' and LocId = '" + auth.LocId + "'";
            return _dataLogic.LoadData(qry);
        }


        public DataTable GetSalarySettlementLabels()
        {
            String qry = $@"Select p1, p2, p3, p4, p5, p6, p7 from tblSalarydtLables where cmp_id = '" + auth.CmpId + "' and LocId = '" + auth.LocId + "'";

            return _dataLogic.LoadData(qry);
        }

        public DataTable GetEditSalrySetlment(int empy_id)
        {
            String qry = $@"Select s.SrNo, E.empy_id, dept.Id as dept_Id, S.desg_Id, E.Name, dept.Department, desgn.Designation, emptype.EmployeeType, emptype.Id as TypeId, s.join_date, s.hire_date, s.trdate, s.Active, s.banksalary, s.CASHSALARY, s.grade, 
            s.Level2, s.Level3, s.Level4, s.Level4,s.bsalary, s.Level5, s.Level6, s.Level7, s.netsalary, st.SalaryType,  st.Id as SalaryTypeId, s.gsalary, s.reasons, r.Reason, s.through, s.remarks
            FROM tblEmployeeSetup E
            left Join tblemploysalarydt s On s.empy_id = E.empy_id and  s.Comp_id = E.Comp_id and s.LocId = E.LocId
            left JOIN tblcompanydepartment dept ON E.deptid = dept.ID and  dept.Comp_id = E.Comp_id and dept.LocId = E.LocId
            left JOIN tblcompanydesignation desgn ON S.desg_id = desgn.ID and desgn.Comp_id = E.Comp_id and desgn.LocId = E.LocId
            left JOIN TblEmployeeType emptype ON s.empy_type = emptype.Id and emptype.Comp_id = E.Comp_id and emptype.LocId = E.LocId
            left JOIN TblSalaryReason r On r.Id = s.reasons and r.Comp_id = E.Comp_id and r.LocId = E.LocId
            left JOIN TblSalaryType st On st.Id = s.through and st.Comp_id = E.Comp_id and st.LocId = E.LocId
            WHERE E.comp_id = '" +auth.CmpId+"' And E.empy_id = '"+empy_id+"' And E.LocId = '"+auth.LocId+"'";

            return _dataLogic.LoadData(qry);
        }

        public string AddUpdateSalryStlmnt([FromBody] Tblemploysalarydt requestData)
        {
            DateTime dtNow = DateTime.Now;
            using var transaction = _context.Database.BeginTransaction();

            try
            {

                if (requestData.Srno == 0)
                {
                    requestData.Srno = (_context.Tblemploysalarydts
                     .Where(x => x.CompId == auth.CmpId && x.LocId == auth.LocId && x.EmpyId == requestData.EmpyId)
                        .Max(x => (int?)x.Srno) ?? 0) + 1;
                }


                var setlement = _context.Tblemploysalarydts
                    .Where(x => x.EmpyId == requestData.EmpyId && x.CompId == auth.CmpId && x.LocId == auth.LocId && x.Srno == requestData.Srno)
                .ToList();

                if (setlement is not null)
                {
                    _context.Tblemploysalarydts.RemoveRange(setlement);
                }


                    _context.Tblemploysalarydts.Add(new Tblemploysalarydt
                    {
                        Srno = requestData.Srno,
                        EmpyId = requestData.EmpyId,
                        EmpyType = requestData.EmpyType,
                        DeptId = requestData.DeptId,
                        DesgId = requestData.DesgId,
                        Through = requestData.Through,
                        Reasons = requestData.Reasons,
                        Remarks = requestData.Remarks,
                        Banksalary = requestData.Banksalary,
                        Cashsalary = requestData.Cashsalary,
                        Netsalary = requestData.Netsalary,
                        Gsalary = requestData.Gsalary,
                        Grade = requestData.Grade,
                        Bsalary = requestData.Bsalary,
                        Level2 = requestData.Level2,
                        Level3 = requestData.Level3,
                        Level4 = requestData.Level4,
                        Level5 = requestData.Level5,
                        Level6 = requestData.Level6,
                        Level7 = requestData.Level7,
                        Active = requestData.Active,
                        HireDate = requestData.HireDate,
                        JoinDate = requestData.JoinDate,
                        Trdate = dtNow,
                        CompId = auth.CmpId,
                        LocId = auth.LocId



                    });


                _context.SaveChanges();
                transaction.Commit();
                _dataLogic.LogEntry(requestData.EmpyId, "Salary Settlement", $"Add/Edit Salary of Employee Id {requestData.EmpyId} & Salary VchNo: {requestData.Srno} ", 0, dtNow, 0, 0, 0, dtNow);
                return "true";
            }
            catch (Exception)
            {
                transaction.Rollback();
                return "false";
                throw;

            }

        }

        public bool DelSalrySetlment(int empy_id, int SrNo)
        {
            DateTime dtNow = DateTime.Now;
            using var transaction = _context.Database.BeginTransaction();
            try
            {
                var salary = _context.Tblemploysalarydts.Where(x => x.EmpyId == empy_id && x.CompId == auth.CmpId && x.LocId == auth.LocId && x.Srno == SrNo).ToList();

                if (salary is null)
                {
                    return false;
                }

                _context.Tblemploysalarydts.RemoveRange(salary);

                _context.SaveChanges();
                transaction.Commit();
                _dataLogic.LogEntry(empy_id, "Salary Setllement", $"Deleted Salary - Employee Id: {empy_id} - VchNo = {SrNo}", 0, dtNow, 0, 0, 0, dtNow);
                return true;
            }
            catch (Exception)
            {
                transaction.Rollback();
                return false;
                throw;
            }
        }


        #endregion

        #region Employee Family

        // Employee Family Infomation

        public string AddUpdateEmpFamily(List<EmpFamilyVM> family)
        {
            DateTime dtNow = DateTime.Now;
            EmpFamilyVM child = family.First();
            using var transaction = _context.Database.BeginTransaction();

            try
            {
                var existingName = _context.TblChildren
               .FirstOrDefault(x => x.Name.Trim().Replace(" ", "") == child.Name.Trim().Replace(" ", "")
                && x.CmpId == auth.CmpId && x.LocId == auth.LocId && x.EmpyId == child.EmpyId) ;

                if (existingName != null)
                {
                    return "false";
                }

                var children =  _context.TblChildren
                    .Where(x => x.EmpyId == child.EmpyId && x.CmpId == auth.CmpId && x.LocId == auth.LocId)
                .ToList();


                if (children is not null)
                {
                    _context.TblChildren.RemoveRange(children);
                }


                foreach (var item in family)
                {


                    _context.TblChildren.Add(new TblChild
                    {

                        CmpId = auth.CmpId,
                        EmpyId = item.EmpyId,
                        Name = item.Name,
                        Gender = item.Gender,
                        Cnic = item.Cnic,
                        SrNo = item.SrNo,
                        LocId = auth.LocId

                    });
                }

                _context.SaveChanges();
                transaction.Commit();
                _dataLogic.LogEntry(child.EmpyId, "Employee Family", $"{((children == null) ? "Add" : "Add/Edit")} Employee Family ", 0, dtNow, 0, 0, 0, dtNow);
                return "true";
            }
            catch (Exception)
            {
                transaction.Rollback();
                return "false";
                throw;

            }

        }

        public DataTable GetEmpFamilyList()
        {
            String qry = $@"Select c.Id, emp.name as EmpName, c.empy_id, c.Name, c.Gender, c.CNIC, dept.Department from tblchildren c
                            Inner Join tblEmployeeSetup emp On emp.empy_id = C.empy_id
                            Inner Join tblcompanydepartment dept On emp.deptId = dept.ID
                            where c.cmp_id = '" + auth.CmpId + "'";

            return _dataLogic.LoadData(qry);
        }

        public DataTable GetEditEmpFamily(int empy_id)
        {
            String qry = $@"Select emp.Name as EmpName, c.SrNo, c.Id, c.Name, c.empy_id, c.Gender, CNIC FROM tblchildren c
                            Inner Join tblEmployeeSetup emp On emp.empy_id = c.empy_id and c.cmp_id = emp.comp_id and c.LocId = emp.LocId
                            Where c.cmp_id = '" + auth.CmpId + "' and c.empy_id = '" + empy_id + "' and c.LocId = '"+auth.LocId+"'";

            return _dataLogic.LoadData(qry);
        }


        public bool DelEmpFamily(int empy_id)
        {
            DateTime dtNow = DateTime.Now;
            using var transaction = _context.Database.BeginTransaction();
            try
            {
                _context.TblChildren.Where(x => x.EmpyId == empy_id && x.CmpId == auth.CmpId && x.LocId == auth.LocId).ExecuteDelete();

                _context.SaveChanges();
                transaction.Commit();
                _dataLogic.LogEntry(empy_id, "Employee Family", $"Deleted Employee Family - Employee Id: {empy_id}", 0, dtNow, 0, 0, 0, dtNow);
                return true;
            }
            catch (Exception)
            {
                transaction.Rollback();
                return false;
                throw;
            }
        }

        #endregion



    }
}

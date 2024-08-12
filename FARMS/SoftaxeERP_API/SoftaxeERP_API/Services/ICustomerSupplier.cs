using SoftaxeERP_API.Models;
using SoftaxeERP_API.VM;
using System.Data;

namespace SoftaxeERP_API.Services
{
    public interface ICustomerSupplier
    {
        // MAIN AREA
        DataTable GetMainArea(string tag);
        bool AddUpdateMainArea(int id, string name, string tag, DateTime dtNow);
        string DeleteMainArea(int id, string tag, DateTime dtNow);

        // SUB AREA
        DataTable GetSubArea(int mainAreaId, string tag);
        bool AddUpdateSubArea(int mainAreaId, int id, string name, string tag, DateTime dtNow);
        string DeleteSubArea(int mainAreaId, int id, string tag, DateTime dtNow);

        // CUSTOMER SUPPLIER
        object GetCustomerSupplier(string tag, bool status);
        string AddUpdateCustomerSupplier(CustomerSupplierVM customerSupplierVM);
        string DeleteCustomerSupplier(string code, string tag, DateTime dtNow);
        bool CustomerTaxUpdate(List<CustomerTaxVM> vM);

        DataTable GetLedgerList(string tag);
        DataTable GetAging(string tag);
        bool AddUpdateAging(int d1, int d2, int d3, int d4, int d5, int d6, int d7, string tag, DateTime dtNow);
        DataTable GetCostCodes();
    }

    public class CustomerSupplier : ICustomerSupplier
    {
        private readonly ErpSoftaxeContext _context;
        private readonly IDataLogic _dataLogic;
        private readonly IAuth _auth;

        readonly AuthVM auth = new();
        public CustomerSupplier(ErpSoftaxeContext context, IDataLogic dataLogic, IAuth authData)
        {
            _context = context;
            _dataLogic = dataLogic;
            _auth = authData;

            auth = _auth.GetUserData();
        }

        #region MAIN AREA

        public DataTable GetMainArea(string tag)
        {

            string qry = @"SELECT GROUPID AS id, GROUPNAME AS name FROM TBLGROUP WHERE COMP_ID = " + auth.CmpId + " AND TAG = '" + tag + "' ORDER BY GROUPNAME";

            return _dataLogic.LoadData(qry);
        }

        public bool AddUpdateMainArea(int id, string name, string tag, DateTime dtNow)
        {
            using var transaction = _context.Database.BeginTransaction();

            try
            {
                TblGroup group = _context.TblGroups.Where(x => x.Groupid == id && x.CompId == auth.CmpId).FirstOrDefault();

                if (group == null)
                {
                    var max = (_context.TblGroups.Where(x => x.CompId == auth.CmpId).Max(x => (int?)x.Groupid) ?? 0) + 1;

                    _context.TblGroups.Add(new TblGroup { Groupid = max, Groupname = name, Tag = tag, CompId = auth.CmpId });
                }
                else
                {
                    group.Groupname = name;
                    _context.TblGroups.Update(group);
                }

                _context.SaveChanges();
                transaction.Commit();
                _dataLogic.LogEntry(0, ((tag.ToLower() == "supplier") ? "Purchase" : "Sale"), $"{((group == null) ? "Add" : "Edit")} Main Area - {name} ", 0, dtNow, 0, 0, 0, dtNow);
                return true;
            }
            catch (Exception)
            {
                transaction.Rollback();
                return false;
            }
        }

        public string DeleteMainArea(int id, string tag, DateTime dtNow)
        {
            using var transaction = _context.Database.BeginTransaction();

            try
            {
                var subgroup = _context.Tblsubgroups.Where(x => x.Groupid == id && x.CompId == auth.CmpId).FirstOrDefault();
                if (subgroup != null)
                {
                    return "Can't Delete Main Area";
                }

                TblGroup group = _context.TblGroups.Where(x => x.Groupid == id && x.Tag == tag && x.CompId == auth.CmpId).FirstOrDefault();
                _context.TblGroups.Remove(group);

                _context.SaveChanges();
                transaction.Commit();
                _dataLogic.LogEntry(0, ((tag.ToLower() == "supplier") ? "Purchase" : "Sale"), $"Delete Main Area - {group.Groupname} ", 0, dtNow, 0, 0, 0, dtNow);
                return "true";
            }
            catch (Exception)
            {
                transaction.Rollback();
                return "false";
            }

        }

        #endregion

        #region SUB AREA

        public DataTable GetSubArea(int mainAreaId, string tag)
        {
            string qry = @"SELECT GROUPSUBID AS id, GROUPNAME AS name FROM TBLSUBGROUP WHERE COMP_ID = " + auth.CmpId + " AND GROUPID = " + mainAreaId + " ORDER BY GROUPNAME ";

            return _dataLogic.LoadData(qry);
        }

        public bool AddUpdateSubArea(int mainAreaId, int id, string name, string tag, DateTime dtNow)
        {
            using var transaction = _context.Database.BeginTransaction();
            try
            {
                Tblsubgroup subGroup = _context.Tblsubgroups.Where(x => x.Groupid == mainAreaId && x.Groupsubid == id && x.CompId == auth.CmpId).FirstOrDefault();

                if (subGroup == null)
                {
                    var max = (_context.Tblsubgroups.Where(x => x.CompId.Equals(auth.CmpId)).Max(x => (int?)x.Groupsubid) ?? 0) + 1;

                    _context.Tblsubgroups.Add(new Tblsubgroup { Groupsubid = max, Groupname = name, CompId = auth.CmpId, Groupid = mainAreaId });
                }
                else
                {
                    subGroup.Groupname = name;
                    subGroup.Groupid = mainAreaId;
                    _context.Tblsubgroups.Update(subGroup);
                }

                _context.SaveChanges();
                transaction.Commit();
                _dataLogic.LogEntry(0, ((tag.ToLower() == "supplier") ? "Purchase" : "Sale"), $"{((subGroup == null) ? "Add" : "Edit")} Sub Area - {name} ", 0, dtNow, 0, 0, 0, dtNow);
                return true;
            }
            catch (Exception)
            {
                transaction.Rollback();
                return false;
            }
        }

        public string DeleteSubArea(int mainAreaId, int id, string tag, DateTime dtNow)
        {
            using var transaction = _context.Database.BeginTransaction();
            try
            {
                var level5 = _context.Level5s.Where(x => x.Groupid == mainAreaId && x.Groupsubid == id && x.CompId == auth.CmpId).FirstOrDefault();

                if (level5 != null)
                {
                    return "Can't Delete Sub Area";
                }

                Tblsubgroup group = _context.Tblsubgroups.Where(x => x.Groupid == mainAreaId && x.Groupsubid == id && x.CompId == auth.CmpId).FirstOrDefault();
                _context.Tblsubgroups.Remove(group);

                _context.SaveChanges();
                transaction.Commit();
                _dataLogic.LogEntry(0, ((tag.ToLower() == "supplier") ? "Purchase" : "Sale"), $"Delete Sub Area - {group.Groupname} ", 0, dtNow, 0, 0, 0, dtNow);
                return "true";
            }
            catch (Exception)
            {
                transaction.Rollback();
                return "false";
            }
        }

        #endregion

        #region CUSTOMER SUPPLIER 

        public object GetCustomerSupplier(string tag, bool status)
        {
            string filterBy = "";
            if (tag.ToLower() == "supplier")
            {
                tag = "C";
            }
            else if (tag.ToLower() == "customer")
            {
                tag = "D";
            }

            if(status == true)
            {
                filterBy = "OR ISNULL(BOTH,0) = 1";
            }

            string qry = $@"SELECT L5.LEVEL4 + LEVEL5 AS code, ISNULL(L5.LEVEL5,'') AS id,ISNULL(L5.NAMES,'') AS name, ISNULL(L5.ADDRESS,'') AS address, ISNULL(L5.CITY,'') AS city,ISNULL(L5.CONTACT,'') AS contact,
			ISNULL(L5.PHONE,'') AS phone, ISNULL(L5.EMAIL,'') AS email, ISNULL(L5.GROUPID,'') AS mainAreaId, ISNULL(G.GROUPNAME,'') AS mainArea, ISNULL(L5.GROUPSUBID,'') AS subAreaId,
			ISNULL(SG.GROUPNAME,'') AS subArea, ISNULL(L5.POSTALCODE,'') AS postalCode, ISNULL(L5.Commission,0) AS commission, ISNULL(L5.CrLimit,0) AS creditLimit, ISNULL(BOTH, 0) as inActive,
            ISNULL(Nic, '') Nic , isnull(STRN,'') Strn, ISNULL(Ntn, '') Ntn,ISNULL(L5.ALLOWSALETAX,'') AS saleTax, ISNULL(L5.ALLOWWHTAX, '') AS whTax , isnull(RateDiff,0) ratediff , isnull(AccNo,'')  AccNo
			FROM LEVEL5 L5
			INNER JOIN LEVEL4 L4 ON L4.LEVEL3+L4.LEVEL4 = L5.LEVEL4 AND L4.COMP_ID = L5.COMP_ID
			LEFT OUTER JOIN TBLGROUP G ON G.GROUPID = L5.GROUPID AND L5.COMP_ID = G.COMP_ID
			LEFT OUTER JOIN TBLSUBGROUP SG ON SG.GROUPSUBID = L5.GROUPSUBID AND L5.COMP_ID = SG.COMP_ID
			WHERE (L4.TAG1 = '{tag}' {filterBy}) AND L5.COMP_ID = {auth.CmpId} {auth.LocationControl} ORDER BY L5.NAMES";

            string l4 = $@"SELECT LEVEL3+LEVEL4 AS code, names as name FROM LEVEL4 L4 WHERE COMP_ID = {auth.CmpId} AND TAG1 = '{tag}' {auth.LocationControl.Replace("L5", "L4")} ORDER BY L4.NAMES";

            return new {
                party = _dataLogic.LoadData(qry),
                l4 = _dataLogic.LoadData(l4)
            };
        }

        public string AddUpdateCustomerSupplier(CustomerSupplierVM vM)
        {
            using var transaction = _context.Database.BeginTransaction();

            try
            {
                Level5 level = _context.Level5s.Where(x => x.Level4 + x.Level51 == vM.Code && x.CompId == auth.CmpId).FirstOrDefault();
                
                if (level == null)
                {
                    Level5 l5Name = _context.Level5s.Where(x => x.Names.ToLower().Trim() == vM.Name.ToLower().Trim() && x.CompId == auth.CmpId).FirstOrDefault();

                    if (l5Name != null)
                    {
                        return "This account is already added.";
                    }

                    string max = Convert.ToString(Convert.ToInt32(_context.Level5s.Where(x => x.CompId == auth.CmpId && x.Level4 == vM.L4Code).Max(x => (string)x.Level51) ?? "0") + 1);

                    if (max.Length == 1)
                    { max = "0000" + max; }
                    else if (max.Length == 2)
                    { max = "000" + max; }
                    else if (max.Length == 3)
                    { max = "00" + max; }
                    else if (max.Length == 4)
                    { max = "0" + max; }

                    _context.Level5s.Add(new Level5
                    {
                        Level4 = vM.L4Code,
                        Level51 = max,
                        Names = vM.Name,
                        Groupid = vM.MainAreaId,
                        Groupsubid = vM.SubAreaId,
                        Address = vM.Address,
                        City = vM.City,
                        Contact = vM.Contact,
                        Phone = vM.Phone,
                        Email = vM.Email,
                        PostalCode = vM.PostalCode,
                        Nic = vM.CNIC,
                        Ntn = vM.Ntn,
                        Strn= vM.Strn,
                        LocId = auth.LocId!,
                        CompId = auth.CmpId,
                        Commission = vM.Commission,
                        CrLimit = vM.CreditLimit,
                        Both = vM.InActive,
                        AllowSaleTax = vM.SaleTax,
                        AllowWhtax = vM.WHTax,
                        RateDiff = vM.RateDiff,
                        AccNo = vM.AccNo,
                        Mappedcode = (auth.LocationWise != "Location Wise") ? auth.LocId : ""
                    });
                }
                else
                {
                    level.Names = vM.Name;
                    level.Groupid = vM.MainAreaId;
                    level.Groupsubid = vM.SubAreaId;
                    level.Address = vM.Address;
                    level.City = vM.City;
                    level.Contact = vM.Contact;
                    level.Phone = vM.Phone;
                    level.Email = vM.Email;
                    level.PostalCode = vM.PostalCode;
                    level.Nic = vM.CNIC;
                    level.Ntn = vM.Ntn;
                    level.Strn = vM.Strn;
                    level.Commission = vM.Commission;
                    level.CrLimit = vM.CreditLimit;
                    level.Both = vM.InActive;
                    level.AllowSaleTax = vM.SaleTax;
                    level.AllowWhtax = vM.WHTax;
                    level.RateDiff = vM.RateDiff;
                    level.AccNo= vM.AccNo;
                    _context.Level5s.Update(level);
                }

                _context.SaveChanges();
                transaction.Commit();
                _dataLogic.LogEntry(0, $"{((vM.Tag.ToLower() == "d") ? "Sale" : "Purchase")}", $"{((level == null) ? "Add" : "Edit")} {((vM.Tag.ToLower() == "d") ? "Customer" : "Supplier")} - {vM.Name} ", 0, vM.DtNow, 0, 0, 0, vM.DtNow);
                return "true";
            }
            catch (Exception)
            {
                transaction.Rollback();
                return "false";
            }
        }

        public string DeleteCustomerSupplier(string code, string tag, DateTime dtNow)
        {
            using var transaction = _context.Database.BeginTransaction();
            try
            {
                Level5 level = _context.Level5s.Where(x =>  x.Level4 + x.Level51 == code && x.CompId == auth.CmpId).FirstOrDefault();
                _context.Level5s.Remove(level);

                _context.SaveChanges();
                transaction.Commit();
                _dataLogic.LogEntry(0, $"{((tag.ToLower() == "d") ? "Sale" : "Purchase")}", $"Delete {((tag.ToLower() == "d") ? "Customer" : "Supplier")} - {level.Names} ", 0, dtNow, 0, 0, 0, dtNow);
                return "true";
            }
            catch (Exception)
            {
                transaction.Rollback();
                return "false";
            }
        }

        public bool CustomerTaxUpdate(List<CustomerTaxVM> vM)
        {
            using var transaction = _context.Database.BeginTransaction();
            try
            {
                foreach (var item in vM)
                {
                    Level5 cus = _context.Level5s.Where(x => x.Level4 + x.Level51 == item.Code && x.CompId == auth.CmpId).FirstOrDefault();
                    cus.AllowSaleTax = item.SaleTax;
                    cus.AllowWhtax = item.WHTax;
                    _context.Level5s.Update(cus);
                    _context.SaveChanges();
                }

                transaction.Commit();
                _dataLogic.LogEntry(0, "Sale", $"Update Customer Tax / WhTax", 0, vM.First().DtNow, 0, 0, 0, vM.First().DtNow);
                return true;
            }
            catch (Exception)
            {
                transaction.Commit();
                return false;
                throw;
            }
        }

        #endregion

        #region LEDGER

        public DataTable GetLedgerList(string tag)
        {
            string qry = $@"SELECT ISNULL(L4.TAG1,'') AS TAG,ISNULL(L5.CITY,'') AS CITY, L5.LEVEL4+L5.LEVEL5 AS CODE, L5.NAMES
            FROM LEVEL5 L5  
            INNER JOIN LEVEL4 L4 ON L4.LEVEL3 + L4.LEVEL4=L5.LEVEL4 AND L4.COMP_ID = L5.COMP_ID
			WHERE ISNULL(L4.TAG1,'') = '{tag}' AND L5.COMP_ID = {auth.CmpId} {auth.LocationControl} 
            ORDER BY L5.NAMES";

            return _dataLogic.LoadData(qry);
        }

        #endregion

        #region CUSTOMER AGING

        public DataTable GetAging(string tag)
        {
            string qry = @"SELECT * FROM AgingDr WHERE TAG = '" + tag + "' ";

            return _dataLogic.LoadData(qry);
        }

        public bool AddUpdateAging(int d1, int d2, int d3, int d4, int d5, int d6, int d7, string tag, DateTime dtNow)
        {
            using var transaction = _context.Database.BeginTransaction();
            try
            {
                var aging = _context.AgingDrs.FirstOrDefault(x => x.Tag == tag);

                if (aging == null)
                {
                    _context.AgingDrs.Add(new AgingDr
                    {
                        D1 = d1,
                        D2 = d2,
                        D3 = d3,
                        D4 = d4,
                        D5 = d5,
                        D6 = d6,
                        D7 = d7,
                        Tag = tag,
                    });
                }
                else
                {
                    aging.D1 = d1;
                    aging.D2 = d2;
                    aging.D3 = d3;
                    aging.D4 = d4;
                    aging.D5 = d5;
                    aging.D6 = d6;
                    aging.D7 = d7;
                    aging.Tag = tag;

                    _context.AgingDrs.Update(aging);
                }

                _context.SaveChanges();
                transaction.Commit();
                _dataLogic.LogEntry(0, ((tag.ToLower() == "customer") ? "Sale" : "Purchase"), $"{((aging == null) ? "Add" : "Edit")} Aging d1{d1}, d2{d2}, d3{d3}, d4{d4}, d5{d5}, d{d6}, d7{d7}", 0, dtNow, 0, 0, 0,dtNow);
                return true;
            }
            catch (Exception)
            {
                transaction.Rollback();
                return false;
            }
        }

        #endregion

        public DataTable GetCostCodes()
        {
            string qry = $@"SELECT L5.LEVEL4 + LEVEL5 AS code, ISNULL(L5.LEVEL5,'') AS id,ISNULL(L5.NAMES,'') AS name
            FROM LEVEL5 L5
            INNER JOIN TBLGROUP G ON L5.LEVEL4 + LEVEL5 = G.CONCODE AND L5.COMP_ID = G.COMP_ID
            WHERE L5.COMP_ID = {auth.CmpId} {auth.LocationControl} ORDER BY L5.NAMES";

            return _dataLogic.LoadData(qry);
        }
    }
}

using SoftaxeERP_API.Models;
using SoftaxeERP_API.VM;
using System.Data;

namespace SoftaxeERP_API.Services
{
    public interface IWarehouse
    {
        DataTable GetWarehouseList();

        // WAREHOUSE
        DataTable GetWarehouse();
        bool AddUpdateWarehouse(int id, string name, string alias, DateTime dtNow);
        string DeleteWarehouse(int id, DateTime dtNow);

        // RACK
        DataTable GetRack(int wId);
        bool AddUpdateRack(int wId, int id, string name, string alias, DateTime dtNow);
        string DeleteRack(int wId, int id, DateTime dtNow);

        // SHELS
        DataTable GetShelf(int wId, int rId);
        bool AddUpdateShelf(int wId, int rId, int id, string name, string alias, DateTime dtNow);
        string DeleteShelf(int wId, int rId, int id, DateTime dtNow);
    }

    public class Warehouse : IWarehouse
    {
        private readonly ErpSoftaxeContext _context;
        private readonly IDataLogic _dataLogic;
        private readonly IAuth _auth;

        readonly AuthVM auth = new();
        public Warehouse(ErpSoftaxeContext context, IDataLogic dataLogic, IAuth authData)
        {
            _context = context;
            _dataLogic = dataLogic;
            _auth = authData;

            auth = _auth.GetUserData();
        }

        public DataTable GetWarehouseList()
        {
            string qry = $@"SELECT ISNULL(GD.GODOWNID,'') AS WID, ISNULL(GD.GODOWNNAME,'') AS WNAME,ISNULL(GD.ALIAS,'') AS WALIAS, 
            ISNULL(R.RACKNO,'') AS RID,		ISNULL(R.RACKNAME,'') AS RNAME,ISNULL(R.ALIAS,'') AS RALIAS, 
            ISNULL(S.SHELFNO,'') AS SID, ISNULL(S.SHELFNAME,'') AS SNAME,ISNULL(S.ALIAS,'') AS SALIAS, ISNULL(S.SKU,'') AS SKU
            FROM TBLGODOWNS GD
            LEFT OUTER JOIN TBLRACKS R ON R.GODOWNID = GD.GODOWNID AND R.COMP_ID = GD.COMP_ID AND R.LOCID = GD.LOCID
            LEFT OUTER JOIN TBLSHELFS S ON S.RACKNO = R.RACKNO AND S.GODOWNID = GD.GODOWNID AND S.COMP_ID =  GD.COMP_ID AND S.LOCID = GD.LOCID
            WHERE GD.COMP_ID = {auth.CmpId} AND GD.LOCID = '{auth.LocId}'
            ORDER BY GD.GODOWNNAME, R.RACKNAME, S.SHELFNAME";

            return _dataLogic.LoadData(qry);
        }

        #region WAREHOUSE

        public DataTable GetWarehouse()
        {
            string qry = $@"SELECT GODOWNID AS id, GODOWNNAME AS name, alias FROM TBLGODOWNS WHERE COMP_ID = {auth.CmpId} AND LOCID = '{auth.LocId}' ORDER BY GODOWNNAME ";

            return _dataLogic.LoadData(qry);
        }

        public bool AddUpdateWarehouse(int id, string name, string alias, DateTime dtNow)
        {
            using var transaction = _context.Database.BeginTransaction();

            try
            {
                Tblgodown warehouse = _context.Tblgodowns.Where(x => x.Godownid == id && x.CompId == auth.CmpId).FirstOrDefault();

                if (warehouse == null)
                {
                    int max = Convert.ToInt32(_context.Tblgodowns.Where(x => x.CompId == auth.CmpId).Max(x => (int?)x.Godownid) ?? 0) + 1;

                    _context.Tblgodowns.Add(new Tblgodown { 
                        Godownid = max, 
                        Godownname = name, 
                        Alias = alias, 
                        CompId = auth.CmpId,
                        Locid = auth.LocId
                    });
                }
                else
                {
                    warehouse.Godownname = name;
                    warehouse.Alias = alias;
                    _context.Tblgodowns.Update(warehouse);
                }

                _context.SaveChanges();
                transaction.Commit();
                _dataLogic.LogEntry(0, "Warehouse", $"{((warehouse == null) ? "Add" : "Edit")} WareHouse - {name} - {alias} ", 0, dtNow, 0, 0, 0,dtNow);
                return true;
            }
            catch (Exception)
            {
                transaction.Rollback();
                return false;
                throw;
            }
        }

        public string DeleteWarehouse(int id, DateTime dtNow)
        {
            using var transaction = _context.Database.BeginTransaction();

            try
            {
                List<Tblrack> racks = _context.Tblracks.Where(x => x.Godownid == id && x.CompId == auth.CmpId).ToList();

                if (racks.Count > 0)
                {
                    return "Can't Delete Warehouse";
                }

                Tblgodown warehouse = _context.Tblgodowns.Where(x => x.Godownid == id && x.CompId == auth.CmpId).FirstOrDefault();
                _context.Tblgodowns.Remove(warehouse);

                _context.SaveChanges();
                transaction.Commit();
                _dataLogic.LogEntry(0, "Warehouse", $"Delete WareHouse - {warehouse.Godownname} - {warehouse.Alias} ", 0, dtNow, 0, 0, 0,dtNow);
                return "true";
            }
            catch (Exception)
            {
                transaction.Rollback();
                return "false";
                throw;
            }
        }

        #endregion

        #region RACK
       
        public DataTable GetRack(int wId)
        {
            string qry = $@"SELECT RACKNO AS id, RACKNAME AS name, alias FROM TBLRACKS WHERE GODOWNID = {wId} AND COMP_ID = {auth.CmpId} AND LOCID = '{auth.LocId}' ORDER BY RACKNAME ";

            return _dataLogic.LoadData(qry);
        }

        public bool AddUpdateRack(int wId, int id, string name, string alias, DateTime dtNow)
        {
            using var transaction = _context.Database.BeginTransaction();

            try
            {
                Tblrack rack = _context.Tblracks.Where(x => x.Godownid == wId && x.Rackno == id && x.CompId == auth.CmpId).FirstOrDefault();

                if (rack == null)
                {
                    int max = Convert.ToInt32(_context.Tblracks.Where(x => x.CompId == auth.CmpId).Max(x => (int?)x.Rackno) ?? 0) + 1;

                    _context.Tblracks.Add(new Tblrack { 
                        Godownid = wId, 
                        Rackno = max, 
                        Rackname = name, 
                        Alias = alias, 
                        CompId = auth.CmpId,
                        Locid = auth.LocId
                    });
                }
                else
                {
                    rack.Rackname = name;
                    rack.Alias = alias;
                    _context.Tblracks.Update(rack);
                }

                _context.SaveChanges();
                transaction.Commit();
                _dataLogic.LogEntry(0, "Warehouse", $"{((rack == null) ? "Add" : "Edit")} Rack - {name} - {alias} ", 0, dtNow, 0, 0, 0,dtNow);
                return true;
            }
            catch (Exception)
            {
                transaction.Rollback();
                return false;
                throw;
            }
        }

        public string DeleteRack(int wId, int id, DateTime dtNow)
        {
            using var transaction = _context.Database.BeginTransaction();

            try
            {
                List<Tblshelf> shelf = _context.Tblshelfs.Where(x => x.Godownid == wId && x.Rackno == id && x.CompId == auth.CmpId).ToList();

                if (shelf.Count > 0)
                {
                    return "Can't Delete Rack";
                }

                Tblrack rack = _context.Tblracks.Where(x => x.Godownid == wId && x.Rackno == id && x.CompId == auth.CmpId).FirstOrDefault();
                _context.Tblracks.Remove(rack);

                _context.SaveChanges();
                transaction.Commit();
                _dataLogic.LogEntry(0, "Warehouse", $"Delete Rack - {rack.Rackname} - {rack.Alias} ", 0, dtNow, 0, 0, 0,dtNow);
                return "true";
            }
            catch (Exception)
            {
                transaction.Rollback();
                return "false";
                throw;
            }
        }

        #endregion

        #region SHELF

        public DataTable GetShelf(int wId, int rId)
        {
            string qry = $@"SELECT SHELFNO AS id, SHELFNAME AS name, alias, sku FROM TBLSHELFS WHERE GODOWNID = {wId} AND RACKNO = {rId} AND COMP_ID = {auth.CmpId} AND LOCID = '{auth.LocId}' ORDER BY SHELFNAME ";

            return _dataLogic.LoadData(qry);
        }

        public bool AddUpdateShelf(int wId, int rId, int id, string name, string alias,  DateTime dtNow)
        {
            using var transaction = _context.Database.BeginTransaction();

            try
            {
                var sku = (from W in _context.Tblgodowns
                          join R in _context.Tblracks on W.Godownid equals R.Godownid where R.CompId == auth.CmpId
                          where W.Godownid == wId && R.Rackno == rId && W.CompId == auth.CmpId 
                          select new
                          {
                              sku = W.Alias + "-" + R.Alias
                          }).FirstOrDefault();

                Tblshelf shelf = _context.Tblshelfs.Where(x => x.Godownid == wId && x.Rackno == rId && x.Shelfno == id && x.CompId == auth.CmpId).FirstOrDefault();

                if (shelf == null)
                {
                    int max = Convert.ToInt32(_context.Tblshelfs.Where(x => x.CompId == auth.CmpId).Max(x => (int?)x.Shelfno) ?? 0) + 1;

                    _context.Tblshelfs.Add(new Tblshelf { 
                        Godownid = wId, 
                        Rackno = rId, 
                        Shelfno = max, 
                        Shelfname = name, 
                        Alias = alias, 
                        Sku = sku.sku + "-" + alias, 
                        CompId = auth.CmpId,
                        Locid = auth.LocId
                    });
                }
                else
                {
                    shelf.Shelfname = name;
                    shelf.Alias = alias;
                    shelf.Sku = sku.sku + "-" + alias;
                    _context.Tblshelfs.Update(shelf);
                }

                _context.SaveChanges();
                transaction.Commit();
                _dataLogic.LogEntry(0, "Warehouse", $"{((shelf == null) ? "Add" : "Edit")} Shelf - {name} - {alias} ", 0, dtNow, 0, 0, 0,dtNow);
                return true;
            }
            catch (Exception)
            {
                transaction.Rollback();
                return false;
                throw;
            }
        }

        public string DeleteShelf(int wId, int rId, int id, DateTime dtNow)
        {
            using var transaction = _context.Database.BeginTransaction();

            try
            {
                List<TblTransVch> voucher = _context.TblTransVches.Where(x => x.GodownId == wId && x.RackId == rId && x.ShelfId == id && x.CmpId == auth.CmpId).ToList();
                if (voucher.Count > 0)
                {
                    return "Can't Delete Shelf";
                }

                Tblshelf shelf = _context.Tblshelfs.Where(x => x.Godownid == wId && x.Rackno == rId && x.Shelfno == id && x.CompId == auth.CmpId).FirstOrDefault();
                _context.Tblshelfs.Remove(shelf);

                _context.SaveChanges();
                transaction.Commit();
                _dataLogic.LogEntry(0, "Warehouse", $"Delete Shelf - {shelf.Shelfname} - {shelf.Alias} ", 0, dtNow, 0, 0, 0,dtNow);
                return "true";
            }
            catch (Exception)
            {
                transaction.Rollback();
                return "false";
                throw;
            }
        }

        #endregion
    }
}

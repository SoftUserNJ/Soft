using SoftaxeERP_API.Models;
using SoftaxeERP_API.VM;
using System.Data;

namespace SoftaxeERP_API.Services
{
    public interface IChartOfAccount
    {
        DataTable GetCOA();

        // LEVEL 1
        DataTable GetLevel1();
        string SaveUpdateLevel1(string code, string name, DateTime dtNow);
        string DeleteLevel1(string code, DateTime dtNow);

        // LEVEL 2
        DataTable GetLevel2(string code);
        string SaveUpdateLevel2(string preLvl, string code, string name, DateTime dtNow);
        string DeleteLevel2(string code, DateTime dtNow);
        
        // LEVEL 3
        DataTable GetLevel3(string code);
        string SaveUpdateLevel3(string preLvl, string code, string name, DateTime dtNow);
        string DeleteLevel3(string code, DateTime dtNow);

        // LEVEL 4
        DataTable GetLevel4(string code);
        string SaveUpdateLevel4(string preLvl, string code, string name, string tag, string tag1, string saleCode, string mapCode, DateTime dtNow);
        string DeleteLevel4(string code, DateTime dtNow);

        // LEVEL 5
        DataTable GetLevel5(string code);
        string SaveUpdateLevel5(string preLvl, string code, string name, string mapCode, DateTime dtNow);
        string DeleteLevel5(string code, DateTime dtNow);
    }

    public class COA : IChartOfAccount
    {
        private readonly ErpSoftaxeContext _context;
        private readonly IDataLogic _dataLogic;
        private readonly IAuth _auth;

        readonly AuthVM auth = new();
        public COA(ErpSoftaxeContext context, IDataLogic dataLogic, IAuth authData)
        {
            _context = context;
            _dataLogic = dataLogic;
            _auth = authData;

            auth = _auth.GetUserData();
        }

        public DataTable GetCOA()
        {
            string qry = $@"SELECT ISNULL(L1.LEVEL1,'') AS L1CODE,ISNULL(L1.NAMES,'') AS L1NAMES, ISNULL(L2.LEVEL2,'') AS L2CODE, ISNULL(L2.NAMES,'') AS L2NAMES,
            ISNULL(L3.LEVEL3,'') AS L3CODE,ISNULL(L3.NAMES,'') AS L3NAMES,ISNULL(L4.LEVEL4,'') AS L4CODE,ISNULL(L4.NAMES,'') AS L4NAMES,
            ISNULL(L5.LEVEL5,'') AS L5CODE,ISNULL(L5.NAMES,'') AS L5NAMES, ISNULL(L5.NOTCHANGE,0) AS NOTCHANGE
            FROM LEVEL1 L1
            LEFT OUTER JOIN LEVEL2 L2 ON L2.LEVEL1 = L1.LEVEL1 AND L2.COMP_ID = L1.COMP_ID
            LEFT OUTER JOIN LEVEL3 L3 ON L3.LEVEL2 = L2.LEVEL1 + L2.LEVEL2 AND L3.COMP_ID = L2.COMP_ID 
            LEFT OUTER JOIN LEVEL4 L4 ON L4.LEVEL3 = L3.LEVEL2 + L3.LEVEL3 AND L4.COMP_ID = L3.COMP_ID {auth.LocationControl.Replace("L5", "L4")}
            LEFT OUTER JOIN LEVEL5 L5 ON L5.LEVEL4 = L4.Level3 + L4.LEVEL4 AND L5.COMP_ID = L4.COMP_ID {auth.LocationControl} AND ISNULL(L4.TAG1,'') NOT IN ('J','S','D','C')
            WHERE L1.COMP_ID = {auth.CmpId}  ORDER BY L1.LEVEL1, L2.LEVEL2, L3.LEVEL3, L4.LEVEL4, L5.LEVEL5";

            return _dataLogic.LoadData(qry);
        }

        #region LEVEL 1

        public DataTable GetLevel1()
        {
            string qry = @"SELECT LEVEL1 AS code, NAMES AS name, ISNULL(NotChange,0) AS notChange FROM LEVEL1 WHERE COMP_ID = " + auth.CmpId + " ";

            return _dataLogic.LoadData(qry);
        }

        public string SaveUpdateLevel1(string code, string name, DateTime dtNow)
        {
            using var transaction = _context.Database.BeginTransaction();

            try
            {
                Level1 level = _context.Level1s.Where(x => x.Level11 == code && x.CompId == auth.CmpId).FirstOrDefault();

                if (level == null)
                {
                    int max = Convert.ToInt32(_context.Level1s.Where(x => x.CompId == auth.CmpId).Max(x => (string)x.Level11) ?? "0") + 1;

                    _context.Level1s.Add(new Level1 { Level11 = Convert.ToString(max), Names = name, CompId = auth.CmpId, LocId = auth.LocId! });
                }
                else
                {
                    level.Names = name;
                    _context.Level1s.Update(level);
                }

                _context.SaveChanges();
                transaction.Commit();

                _dataLogic.LogEntry(0, "COA", $"{((level == null) ? "Add" : "Edit")} LEVEL 1 - {name} ", 0, dtNow, 0, 0, 0,dtNow);
                return "true";
            }
            catch (Exception)
            {
                transaction.Rollback();
                return "false";
                throw;
            }
        }

        public string DeleteLevel1(string code, DateTime dtNow)
        {
            using var transaction = _context.Database.BeginTransaction();

            try
            {
                List<Level2> level2 = _context.Level2s.Where(x => x.Level1 == code && x.CompId == auth.CmpId).ToList();

                if (level2.Count > 0)
                {
                    return "Can't delete Level 1";
                }

                Level1 level = _context.Level1s.Where(x => x.Level11 == code && x.CompId == auth.CmpId).FirstOrDefault();
                _context.Level1s.Remove(level);

                _context.SaveChanges();
                transaction.Commit();
                _dataLogic.LogEntry(0, "COA", $"Delete LEVEL 1 - {level.Names} ", 0, dtNow, 0, 0, 0,dtNow);
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

        #region LEVEL 2

        public DataTable GetLevel2(string code)
        {
            string qry = @"SELECT LEVEL1 + LEVEL2 AS code, NAMES AS name, ISNULL(NotChange,0) AS notChange FROM LEVEL2 WHERE LEVEL1 = " + code + " AND COMP_ID = " + auth.CmpId + "";

            return _dataLogic.LoadData(qry);
        }

        public string SaveUpdateLevel2(string preLvl, string code, string name, DateTime dtNow)
        {
            using var transaction = _context.Database.BeginTransaction();

            try
            {
                Level2 level = _context.Level2s.Where(x => x.Level1 + x.Level21 == code && x.CompId == auth.CmpId).FirstOrDefault();

                if (level == null)
                {
                    string max = Convert.ToString(Convert.ToInt32(_context.Level2s.Where(x => x.CompId == auth.CmpId && x.Level1 == preLvl).Max(x => (string)x.Level21) ?? "0") + 1);

                    if (max.Length == 1){max = "0" + max;}

                    _context.Level2s.Add(new Level2 { Level1 = preLvl!, Level21 = max, Names = name, CompId = auth.CmpId, LocId = auth.LocId! });
                }
                else
                {
                    level.Names = name;
                    _context.Level2s.Update(level);
                }

                _context.SaveChanges();
                transaction.Commit();
                _dataLogic.LogEntry(0, "COA", $"{((level == null) ? "Add" : "Edit")} LEVEL 2 - {name} ", 0, dtNow, 0, 0, 0,dtNow);
                return "true";
            }
            catch (Exception)
            {
                transaction.Rollback();
                return "false";
                throw;
            }
        }

        public string DeleteLevel2(string code, DateTime dtNow)
        {
            using var transaction = _context.Database.BeginTransaction();

            try
            {
                List<Level3> level3 = _context.Level3s.Where(x => x.Level2 == code && x.LocId == auth.LocId && x.CompId == auth.CmpId).ToList();

                if (level3.Count > 0)
                {
                    return "Can't delete Level 2";
                }

                Level2 level = _context.Level2s.Where(x => x.Level1 + x.Level21 == code && x.LocId == auth.LocId && x.CompId == auth.CmpId).FirstOrDefault();
                _context.Level2s.Remove(level);

                _context.SaveChanges();
                transaction.Commit();
                _dataLogic.LogEntry(0, "COA", $"Delete LEVEL 2 - {level.Names} ", 0, dtNow, 0, 0, 0,dtNow);
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

        #region LEVEL 3

        public DataTable GetLevel3(string code)
        {
            string qry = @"SELECT LEVEL2 + LEVEL3 AS code, NAMES AS name, ISNULL(NotChange,0) AS notChange FROM LEVEL3 WHERE LEVEL2 = " + code + " AND COMP_ID = " + auth.CmpId + " ";

            return _dataLogic.LoadData(qry);
        }

        public string SaveUpdateLevel3(string preLvl, string code, string name, DateTime dtNow)
        {
            using var transaction = _context.Database.BeginTransaction();

            try
            {
                Level3 level = _context.Level3s.Where(x => x.Level2 + x.Level31 == code && x.CompId == auth.CmpId).FirstOrDefault();

                if (level == null)
                {
                    string max = Convert.ToString(Convert.ToInt32(_context.Level3s.Where(x => x.CompId == auth.CmpId && x.Level2 == preLvl).Max(x => (string)x.Level31) ?? "0") + 1);

                    if (max.Length == 1){ max = "00" + max; } else if (max.Length == 2){ max = "0" + max; }

                    _context.Level3s.Add(new Level3 { Level2 = preLvl!, Level31 = max, Names = name, CompId = auth.CmpId, LocId = auth.LocId! });
                }
                else
                {
                    level.Names = name;
                    _context.Level3s.Update(level);
                }

                _context.SaveChanges();
                transaction.Commit();
                _dataLogic.LogEntry(0, "COA", $"{((level == null) ? "Add" : "Edit")} LEVEL 3 - {name} ", 0, dtNow, 0, 0, 0,dtNow);
                return "true";
            }
            catch (Exception)
            {
                transaction.Rollback();
                return "false";
                throw;
            }
        }

        public string DeleteLevel3(string code, DateTime dtNow)
        {
            using var transaction = _context.Database.BeginTransaction();

            try
            {
                List<Level4> level4 = _context.Level4s.Where(x => x.Level3 == code && x.CompId == auth.CmpId).ToList();

                if (level4.Count > 0)
                {
                    return "Can't delete Level 3";
                }

                Level3 level = _context.Level3s.Where(x => x.Level2 + x.Level31 == code && x.CompId == auth.CmpId).FirstOrDefault();
                _context.Level3s.Remove(level);

                _context.SaveChanges();
                transaction.Commit();
                _dataLogic.LogEntry(0, "COA", $"Delete LEVEL 3 - {level.Names} ", 0, dtNow, 0, 0, 0,dtNow);
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

        #region LEVEL 4

        public DataTable GetLevel4(string code)
        {
            string qry = $@"SELECT LEVEL3 + LEVEL4 AS code, NAMES AS name, tag, tag1, consCode saleCode, ISNULL(NotChange,0) AS notChange, ISNULL(MappedCode,'') MAPCODE FROM LEVEL4 L4 WHERE LEVEL3 = '{code}' AND COMP_ID = {auth.CmpId} {auth.LocationControl.Replace("L5", "L4")}";

            return _dataLogic.LoadData(qry);
        }

        public string SaveUpdateLevel4(string preLvl, string code, string name, string tag, string tag1, string saleCode, string mapCode, DateTime dtNow)
        {
            using var transaction = _context.Database.BeginTransaction();

            try
            {
                Level4 level = _context.Level4s.Where(x => x.Level3 + x.Level41 == code && x.CompId == auth.CmpId).FirstOrDefault();

                if (level == null)
                {
                    if (auth.DistributionPos != "ERP")
                    {
                        if (!string.IsNullOrEmpty(tag) && tag == "J" || tag == "S")
                        {
                            if(_context.Level4s.Where(x => x.Tag == tag && x.CompId == auth.CmpId && x.LocId == auth.LocId).FirstOrDefault() != null)
                            {
                                return "This account is already added";
                            }
                        }
                    }
                    string max = Convert.ToString(Convert.ToInt32(_context.Level4s.Where(x => x.CompId == auth.CmpId && x.Level3 == preLvl).Max(x => (string)x.Level41) ?? "0") + 1);

                    if (max.Length == 1){ max = "00" + max; } else if (max.Length == 2){ max = "0" + max; }

                    _context.Level4s.Add(new Level4 { 
                        Level3 = preLvl, 
                        Level41 = max, 
                        Names = name, 
                        Tag = tag, 
                        Tag1 = tag1, 
                        ConsCode = saleCode,
                        Mappedcode = (auth.LocationWise != "Location Wise") ? auth.LocId : mapCode, 
                        CompId = auth.CmpId, 
                        LocId = auth.LocId
                    });
                }
                else
                {
                    level.Names = name;
                    level.Tag = tag;
                    level.Tag1 = tag1;
                    level.ConsCode = saleCode;
                    level.Mappedcode = mapCode;
                    _context.Level4s.Update(level);
                }

                _context.SaveChanges();
                transaction.Commit();
                _dataLogic.LogEntry(0, "COA", $"{((level == null) ? "Add" : "Edit")} LEVEL 4 - {name} ", 0, dtNow, 0, 0, 0,dtNow);
                return "true";
            }
            catch (Exception)
            {
                transaction.Rollback();
                return "false";
                throw;
            }
        }

        public string DeleteLevel4(string code, DateTime dtNow)
        {
            using var transaction = _context.Database.BeginTransaction();

            try
            {
                List<Level5> level5 = _context.Level5s.Where(x => x.Level4 == code && x.CompId == auth.CmpId).ToList();

                if (level5.Count > 0)
                {
                    return "Can't delete Level 4";
                }

                Level4 level =  _context.Level4s.Where(x => x.Level3 + x.Level41 == code && x.CompId == auth.CmpId).FirstOrDefault();
                _context.Level4s.Remove(level);

                _context.SaveChanges();
                transaction.Commit();
                _dataLogic.LogEntry(0, "COA", $"Delete LEVEL 4 - {level.Names} ", 0, dtNow, 0, 0, 0,dtNow);
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

        #region LEVEL 5

        public DataTable GetLevel5(string code)
        {
            string qry = $@"SELECT L5.LEVEL4 + L5.LEVEL5 AS code, L5.NAMES AS name , ISNULL(L5.NotChange,0) AS notChange, ISNULL(L5.MappedCode,'') MAPCODE
            FROM LEVEL5 L5
            LEFT OUTER JOIN LEVEL4 L4 ON L5.LEVEL4 = L4.LEVEL3 + L4.LEVEL4  AND L5.COMP_ID = L4.COMP_ID {auth.LocationControl.Replace("L5", "L4")}
            WHERE ISNULL(L4.TAG1,'') NOT IN ('J','S','D','C') AND L5.LEVEL4 = '{code}' AND L5.COMP_ID = {auth.CmpId} {auth.LocationControl}";

            return _dataLogic.LoadData(qry);
        }

        public string SaveUpdateLevel5(string preLvl, string code, string name, string mapCode,DateTime dtNow)
        {
            using var transaction = _context.Database.BeginTransaction();

            try
            {
                Level5 level = _context.Level5s.Where(x => x.Level4 + x.Level51 == code && x.CompId == auth.CmpId).FirstOrDefault();

                if (level == null)
                {
                    string max = Convert.ToString(Convert.ToInt32(_context.Level5s.Where(x => x.CompId == auth.CmpId && x.Level4 == preLvl).Max(x => (string)x.Level51) ?? "0") + 1);

                    if (max.Length == 1)
                    { max = "0000" + max; }
                    else if (max.Length == 2)
                    { max = "000" + max; }
                    else if (max.Length == 3)
                    { max = "00" + max; }
                    else if (max.Length == 4)
                    { max = "0" + max; }

                    _context.Level5s.Add(new Level5 { 
                        Level4 = preLvl, 
                        Level51 = max, 
                        Names = name, 
                        Mappedcode = (auth.LocationWise != "Location Wise") ? auth.LocId : mapCode, 
                        CompId = auth.CmpId, 
                        LocId = auth.LocId 
                    });
                }
                else
                {
                    level.Names = name;
                    level.Mappedcode = mapCode;
                    _context.Level5s.Update(level);
                }

                _context.SaveChanges();
                transaction.Commit();
                _dataLogic.LogEntry(0, "COA", $"{((level == null) ? "Add" : "Edit")} LEVEL 5 - {name} ", 0, dtNow, 0, 0, 0,dtNow);
                return "true";
            }
            catch (Exception)
            {
                transaction.Rollback();
                return "false";
                throw;
            }
        }

        public string DeleteLevel5(string code, DateTime dtNow)
        {
            using var transaction = _context.Database.BeginTransaction();

            try
            {
                List<TblTransVch> tblTrans = _context.TblTransVches.Where(x => x.Dmcode + x.Code == code && x.CmpId == auth.CmpId).ToList();

                if (tblTrans.Count != 0)
                {
                    return "Can't delete Level 5";
                }

                Level5 level = _context.Level5s.Where(x => x.Level4 + x.Level51 == code && x.CompId == auth.CmpId).FirstOrDefault();
                _context.Level5s.Remove(level);

                _context.SaveChanges();
                transaction.Commit();
                _dataLogic.LogEntry(0, "COA", $"Delete LEVEL 5 - {level.Names} ", 0, dtNow, 0, 0, 0,dtNow);
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

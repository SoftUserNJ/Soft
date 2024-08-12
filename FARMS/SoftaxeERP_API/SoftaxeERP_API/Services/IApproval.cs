using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using SoftaxeERP_API.Models;
using SoftaxeERP_API.VM;
using System.Data;
using System.Linq;

namespace SoftaxeERP_API.Services
{
    public interface IApproval
    {
        DataTable GetAllowVchType(string tag);
        object TopMultiData();
        DataTable GetVchApproval(DateTime fromDate, DateTime toDate, string status, string tag, string locId, string userId);
        DataTable GetVchDetail(string vchType, string vchNo, string locId, DateTime vchDate);
        bool SaveVouchersApproval(List<ApprovalVM> vM);
        object GetVchData(string locId, string userId);
        DataTable GetVchTypes();
        bool GetVchTypesDataUpdate(int id, string name, string description, string verifyName, bool verifyRequired, string approvalName, bool approvalRequired, string auditName, string lastText);

        string UpdateGPIVoucherStatus(List<GPIApprovalVM> status);
        DataTable GetGPIVch(DateTime fromDate, DateTime toDate, bool unapproved, bool approved, bool rejected, bool all);
        DataTable GetGPIVchDetail(int vchNo, string vchType);
        DataTable GetPoVch(DateTime fromDate, DateTime toDate);

        string UpdatePOVoucherStatus(List<POApprovalVM> status);

        string UpdateMaizeRateStatus(List<MaizeRateApprovalVM> status);
        string UpdatePartyTermCondition(List<PartyTermConditionsApprovalVM> status);


        DataTable GetDOApprovalList(DateTime fromDate, DateTime toDate, string vchType);

        string UpdateDoStatus(List<DOApprovalVM> status);
    }

    public class Approval : IApproval
    {
        private readonly ErpSoftaxeContext _context;
        private readonly IDataLogic _dataLogic;
        private readonly IAuth _auth;

        readonly AuthVM auth = new();
        public Approval(ErpSoftaxeContext context, IDataLogic dataLogic, IAuth authData)
        {
            _context = context;
            _dataLogic = dataLogic;
            _auth = authData;

            auth = _auth.GetUserData();
        }

        public DataTable GetAllowVchType(string tag)
        {
            string filterBy = "";

            if (tag.ToLower() == "approve")
            {
                filterBy = "ISNULL(TU.CANAPPROVE,0) <> 0 AND ISNULL(TU.CANUNAPPROVE,0) <> 0 AND";
            }
            else if (tag.ToLower() == "verify")
            {
                filterBy = "ISNULL(TU.CANVERIFY,0) <> 0 AND ISNULL(TU.CANUNVERIFY,0) <> 0 AND";
            }
            else if (tag.ToLower() == "audit")
            {
                filterBy = "ISNULL(TU.CANAUDIT,0) <> 0 AND ISNULL(TU.CANUNAUDIT,0) <> 0 AND";
            }

            string qry = @"SELECT T.VCHTYPE 
            FROM TBLVCHTYPES T
            LEFT OUTER JOIN TBLUSERVCHTYPES TU ON TU.UID  = " + auth.UserId + " AND  T.VCHTYPE = TU.VCHTYPE WHERE " + filterBy + " T.CMPID = " + auth.CmpId + " ORDER BY T.VCHTYPE";

            return _dataLogic.LoadData(qry);
        }

        public object TopMultiData()
        {
            var finYear = _context.Tblfinyears.Where(x => x.Finid == auth.FinId && x.CompId == auth.CmpId).Select(y => new { y.Fromdate, y.Todate }).FirstOrDefault();
            var location = _context.Locations.Where(x => x.LocId == auth.LocId && x.CmpId == auth.CmpId).Select(y => new { y.LocId, y.LocName }).ToList();

            return new
            {
                finYear,
                location
            };
        }

        public DataTable GetVchApproval(DateTime fromDate, DateTime toDate, string status, string tag, string locId, string userId)
        {
            string filterBy = "";
            string colum = "";

            if (tag.ToLower() == "approve")
            {
                if (status.ToLower() == "unapprove")
                {
                    filterBy = "AND ISNULL(APROVE,0) = 0 AND ISNULL(UT.CANAPPROVE,0) <> 0 ";
                }
                else if (status.ToLower() == "approve")
                {
                    filterBy = "AND ISNULL(APROVE,0) <> 0 AND ISNULL(VERIFY,0) = 0 AND ISNULL(UT.CANUNAPPROVE,0) <> 0";
                }

                colum = "ISNULL(APROVE,0)";
            }
            else if (tag.ToLower() == "verify")
            {
                if (status.ToLower() == "unapprove")
                {
                    filterBy = "AND ISNULL(APROVE,0) <> 0 AND ISNULL(VERIFY,0) = 0 AND ISNULL(UT.CANVERIFY,0) <> 0 ";
                }
                else if (status.ToLower() == "approve")
                {
                    filterBy = "AND ISNULL(APROVE,0) <> 0 AND ISNULL(VERIFY,0) <> 0 AND ISNULL(AUDITBY,0) = 0  AND ISNULL(UT.CANUNVERIFY,0) <> 0";
                }

                colum = "ISNULL(VERIFY,0)";
            }
            else if (tag.ToLower() == "audit")
            {
                if (status.ToLower() == "unapprove")
                {
                    filterBy = "AND ISNULL(APROVE,0) <> 0 AND ISNULL(VERIFY,0) <> 0 AND ISNULL(AUDITBY,0) = 0  AND ISNULL(UT.CANAUDIT,0) <> 0";
                }
                else if (status.ToLower() == "approve")
                {
                    filterBy = "AND ISNULL(APROVE,0) <> 0 AND ISNULL(VERIFY,0) <> 0 AND ISNULL(AUDITBY,0) <> 0  AND ISNULL(UT.CANUNAUDIT,0) <> 0";
                }

                colum = "ISNULL(AUDITBY,0)";
            }

            if (locId == null || locId == "null")
            {
                locId = "%";
            }

            if (userId == null || userId == "null")
            {
                userId = "%";
            }

            string qry = $@"SELECT M.VCHTYPE, M.VCHNO, M.LOCID, M.FINID, CONVERT(VARCHAR(10),M.VCHDATEM,103) AS VCHDATE, U.USERNAME, {colum} AS VOUCHER
            FROM TRANSMAIN M 
            INNER JOIN TBLTRANSVCH V ON V.VCHTYPE = M.VCHTYPE AND V.VCHNO = M.VCHNO AND LEFT(V.LOCID,2) = M.LOCID AND V.CMP_ID = M.CMP_ID AND V.FINID = M.FINID
            INNER JOIN USERS U ON U.CMP_ID = M.CMP_ID AND U.ID = V.UID
            LEFT OUTER JOIN TBLUSERVCHTYPES UT ON UT.UID = {auth.UserId} AND M.VCHTYPE = UT.VCHTYPE
            WHERE CONVERT(VARCHAR(11),M.VCHDATEM,111) BETWEEN '{fromDate.ToString("yyyy/MM/dd")}' AND '{toDate.ToString("yyyy/MM/dd")}' AND COALESCE (V.DEBIT, V.CREDIT, 0) > 0
            AND M.LOCID LIKE '{locId}' AND M.CMP_ID = {auth.CmpId} AND V.UID LIKE '{userId}' AND M.VCHTYPE NOT IN ('SP-COST', 'SP-RAW', 'SR-COST') AND  M.FINID = {auth.FinId} {filterBy} 
            GROUP BY M.VCHTYPE, M.VCHNO, M.LOCID, M.FINID, M.VCHDATEM,U.USERNAME, {colum}
            ORDER BY M.VCHTYPE, M.VCHNO, M.VCHDATEM, U.USERNAME";

            return _dataLogic.LoadData(qry);
        }

        public DataTable GetVchDetail(string vchType, string vchNo, string locId, DateTime vchDate)
        {
            string qry = $@"SELECT T.VCHNO, T.VCHTYPE, DMCODE+CODE CODE, NAMES NAME, ISNULL(DEBIT,0) DEBIT, ISNULL(CREDIT,0) CREDIT,DESCRP, ISNULL(T.QTY,0) QTY, ISNULL(T.RATE,0) RATE, LTRIM(STR(J.JOBNO)) +'-'+ C.COSTCENTRENAME AS JOBNAME
            FROM TBLTRANSVCH T 
            INNER JOIN LEVEL5 L5 ON T.DMCODE+T.CODE=L5.LEVEL4+L5.LEVEL5 AND T.CMP_ID = L5.COMP_ID
            LEFT OUTER JOIN TBLJOBNO J ON T.JOBNO = J.ID AND T.CMP_ID = J.CMP_ID
            LEFT OUTER JOIN TBLCOSTCENTRE C ON J.COSTCENTREID = C.COSTCENTREID AND T.CMP_ID = C.CMPID 
            WHERE VCHTYPE = '{vchType}' AND CONVERT(VARCHAR(11),T.VCHDATE,111) BETWEEN '{vchDate.ToString("yyyy/MM/dd")}' AND '{vchDate.ToString("yyyy/MM/dd")}' AND LEFT(T.LOCID,2) = '{locId}' AND VCHNO = {vchNo} AND T.CMP_ID = {auth.CmpId} AND  FINID = {auth.FinId} ";

            if (vchType == "PI")
            {
                qry += $@"UNION ALL 
                SELECT  V.VCHNO, VCHTYPE, V.DMCODE + V.CODE AS CODE, L5.NAMES NAME, 0 DEBIT, 0 CREDIT, DESCRP, V.QTY, V.BRATE RATE, LTRIM(STR(J.JOBNO)) + '-' + C.COSTCENTRENAME + '-' + L.LOCNAME JOBNAME
                FROM TBLTRANSVCH V
                LEFT OUTER JOIN LEVEL5 L5 ON L5.LEVEL4 + L5.LEVEL5 = V.DMCODE + V.CODE AND L5.COMP_ID = V.CMP_ID 
                LEFT OUTER JOIN LEVEL4 L4 ON L4.LEVEL3 + L4.LEVEL4 = L5.LEVEL4 AND L4.COMP_ID = V.CMP_ID 
                LEFT OUTER JOIN TBLGROUP G ON G.GROUPID = L5.GROUPID AND G.COMP_ID = V.CMP_ID
                LEFT OUTER JOIN TBLJOBNO J ON J.ID = V.TVCHNO AND J.CMP_ID = V.CMP_ID
                LEFT OUTER JOIN TBLCOSTCENTRE C ON J.COSTCENTREID = C.COSTCENTREID AND J.CMP_ID = C.CMPID
                LEFT OUTER JOIN LOCATION L ON J.LOCID = L.LOCID AND V.CMP_ID = L.CMP_ID
                WHERE L4.TAG = 'S' AND V.VCHTYPE = 'SP-RAW' AND
                CONVERT(VARCHAR(11), V.VCHDATE, 111) BETWEEN '{vchDate.ToString("yyyy/MM/dd")}' AND '{vchDate.ToString("yyyy/MM/dd")}' 
                AND VCHNO BETWEEN {vchNo} AND {vchNo} 
                AND ISNULL(V.TVCHNO, 0) <> 0 AND V.CMP_ID = {auth.CmpId} AND (V.LOCIDn = '{auth.LocId}') AND V.FINID = {auth.FinId}";
            }

            qry += " ORDER BY DEBIT DESC";

            return _dataLogic.LoadData(qry);
        }

        public bool SaveVouchersApproval(List<ApprovalVM> vM)
        {
            if (vM.Count == 0)
            {
                return false;
            }

            ApprovalVM fr = vM.FirstOrDefault();
            string myLog = "";
            string type = "";
            bool isVch = false;

            using var transaction = _context.Database.BeginTransaction();
            try
            {
                foreach (var item in vM)
                {
                    var main = _context.TransMains.Where(x => x.CmpId == auth.CmpId && x.FinId == item.FinId &&
                    new[] { item.VchType, (item.VchType == "SP") ? "SP-COST" : (item.VchType == "SR") ? "SR-COST" : (item.VchType == "PI") ? "SP-RAW" : (item.VchType == "STK-TRF") ? "STK-RCV" : "" }.Contains(x.VchType) &&
                    x.LocId == item.LocId && x.VchNo == item.VchNo);
                    if (main != null)
                    {
                        if (item.Tag == "verify")
                        {
                            main.ExecuteUpdate(setters => setters
                            .SetProperty(b => b.Verify, item.Voucher)
                            .SetProperty(b => b.VerifyBy, (item.Voucher == 0) ? 0 : auth.UserId));
                            type = (item.Voucher == 0) ? "UnVerify" : "Verify";
                        }
                        else if (item.Tag == "audit")
                        {
                            main.ExecuteUpdate(setters => setters
                            .SetProperty(b => b.AuditBy, item.Voucher)
                            .SetProperty(b => b.AuditByN, (item.Voucher == 0) ? 0 : auth.UserId));
                            type = (item.Voucher == 0) ? "UnAudit" : "Audit";
                        }
                        else if (item.Tag == "approve")
                        {
                            main.ExecuteUpdate(setters => setters
                            .SetProperty(b => b.Aprove, (item.Voucher == 1) ? true : false)
                            .SetProperty(b => b.AppBy, (item.Voucher == 0) ? 0 : auth.UserId));
                            type = (item.Voucher == 0) ? "UnApprove" : "Approve";

                            if (item.Voucher == 0)
                            {
                                _dataLogic.LogEntry(item.VchNo, item.VchType, "UnApprove-" + item.Reason, 0, fr.DtNow, 0, 0, 0, fr.DtNow);
                                isVch = true;
                            }
                        }
                    }

                    myLog += item.VchType + " " + item.VchNo + ", ";

                    if (!isVch)
                    {
                        if (vM.Count == 1)
                        {
                            _dataLogic.LogEntry(item.VchNo, item.VchType, type, 0, fr.DtNow, 0, 0, 0, fr.DtNow);
                        }
                    }
                }

                if (!isVch)
                {
                    if (vM.Count > 1)
                    {
                        _dataLogic.LogEntry(0, type, myLog, 0, fr.DtNow, 0, 0, 0, fr.DtNow);
                    }
                }

                _context.SaveChanges();
                transaction.Commit();

                return true;
            }
            catch (Exception)
            {
                transaction.Rollback();
                return false;
            }
        }

        public object GetVchData(string locId, string userId)
        {
            if (locId == null || locId == "null")
            {
                locId = "%";
            }

            if (userId == null || userId == "null")
            {
                userId = "%";
            }

            string approval = $@"SELECT DISTINCT M.VCHTYPE, M.VCHNO, M.LOCID, CONVERT(VARCHAR(10),VCHDATEM,103) AS VCHDATE, ISNULL(APROVE,0) AS VOUCHER, U.USERNAME
            FROM TRANSMAIN M
            INNER JOIN TBLTRANSVCH V ON V.VCHTYPE = M.VCHTYPE AND V.VCHNO = M.VCHNO AND LEFT(V.LOCID,2) = M.LOCID AND V.CMP_ID = M.CMP_ID AND V.FINID = M.FINID
            INNER JOIN USERS U ON U.CMP_ID = M.CMP_ID AND U.ID = V.UID
            WHERE M.CMP_ID = {auth.CmpId} AND M.FINID = {auth.FinId} AND M.VCHTYPE NOT IN ('SP-COST', 'SP-RAW', 'SR-COST') 
            AND M.LOCID LIKE '{locId}' AND ISNULL(APROVE,0) = 0 AND V.UID LIKE '{userId}'";

            string verify = $@"SELECT DISTINCT M.VCHTYPE, M.VCHNO, M.LOCID, CONVERT(VARCHAR(10),VCHDATEM,103) AS VCHDATE, ISNULL(VERIFY,0) AS VOUCHER, U.USERNAME
            FROM TRANSMAIN M
            INNER JOIN TBLTRANSVCH V ON V.VCHTYPE = M.VCHTYPE AND V.VCHNO = M.VCHNO AND LEFT(V.LOCID,2) = M.LOCID AND V.CMP_ID = M.CMP_ID AND V.FINID = M.FINID
            INNER JOIN USERS U ON U.CMP_ID = M.CMP_ID AND U.ID = V.UID
            WHERE M.CMP_ID = {auth.CmpId} AND M.FINID = {auth.FinId} AND M.VCHTYPE NOT IN ('SP-COST', 'SP-RAW', 'SR-COST') 
            AND M.LOCID LIKE '{locId}' AND ISNULL(VERIFY,0) = 0 AND ISNULL(APROVE,0) <> 0 AND V.UID LIKE '{userId}'";

            string audit = $@"SELECT DISTINCT M.VCHTYPE, M.VCHNO, M.LOCID, CONVERT(VARCHAR(10),VCHDATEM,103) AS VCHDATE, ISNULL(AUDITBY,0) AS VOUCHER, U.USERNAME
            FROM TRANSMAIN M
            INNER JOIN TBLTRANSVCH V ON V.VCHTYPE = M.VCHTYPE AND V.VCHNO = M.VCHNO AND LEFT(V.LOCID,2) = M.LOCID AND V.CMP_ID = M.CMP_ID AND V.FINID = M.FINID
            INNER JOIN USERS U ON U.CMP_ID = M.CMP_ID AND U.ID = V.UID
            WHERE M.CMP_ID = {auth.CmpId} AND M.FINID = {auth.FinId} AND M.VCHTYPE NOT IN ('SP-COST', 'SP-RAW', 'SR-COST') 
            AND M.LOCID LIKE '{locId}' AND ISNULL(AUDITBY,0) = 0 AND ISNULL(VERIFY,0) <> 0 AND ISNULL(APROVE,0) <> 0  AND V.UID LIKE '{userId}'";

            var result = new
            {
                verify = _dataLogic.LoadData(verify),
                approval = _dataLogic.LoadData(approval),
                audit = _dataLogic.LoadData(audit),
            };

            return JsonConvert.SerializeObject(result);
        }

        #region VOUCHER TYPE UPDATE

        public DataTable GetVchTypes()
        {
            string qty = $"SELECT * FROM TBLVCHTYPES WHERE CMPID = {auth.CmpId} ORDER BY VCHTYPE";
            return _dataLogic.LoadData(qty);
        }

        public bool GetVchTypesDataUpdate(int id, string name, string description, string verifyName, bool verifyRequired, string approvalName, bool approvalRequired, string auditName, string lastText)
        {
            using var transaction = _context.Database.BeginTransaction();
            try
            {
                var vch = _context.Tblvchtypes.Where(x => x.Id == id && x.Vchtype == name && x.CmpId == auth.CmpId).FirstOrDefault();
                if (vch != null)
                {
                    vch.Description = description ?? "";
                    vch.VerifyName = verifyName ?? "";
                    vch.VerifyRequired = verifyRequired;
                    vch.ApprovalName = approvalName ?? "";
                    vch.ApprovalRequired = approvalRequired;
                    vch.AuditName = auditName ?? "";
                    vch.Lasttext = lastText ?? "";
                    _context.Tblvchtypes.Update(vch);
                }

                _context.SaveChanges();
                transaction.Commit();
                //_dataLogic.LogEntry(0, type, myLog, 0, fr.DtNow, 0, 0, 0, fr.DtNow);
                return true;
            }
            catch (Exception)
            {
                transaction.Rollback();
                return false;
            }
        }

        #endregion

        #region Purchase Order Approval

        public DataTable GetPoVch(DateTime fromDate, DateTime toDate)
        {
            string qty = $@"Select Distinct M.PoNo as VchNo, M.VchType,  L5.Names as PartyName, M.PODate as VchDate, D.Remarks as Description, ISNULL(D.Verify,0) Verify, ISNULL(D.Aprove,0) Aprove, ISNULL(AuditByN,0) AuditByN, ISNULL(Rcvd,0) Rcvd  
            from TblPurchaseContractMain M Left Outer Join TblPurchaseContractDetail D On D.PoNo = M.PONo 
            Left Outer Join Level5 L5 On L5.Level4+L5.Level5 = D.PCode+PSubCode and L5.Comp_id = M.CmpId
            where M.cmpId = {auth.CmpId} and M.LocId like '{auth.LocId}' and M.PODate between '{fromDate.ToString("yyyy/MM/dd")}' and '{toDate.ToString("yyyy/MM/dd")}'";
            return _dataLogic.LoadData(qty);
        }

        public string UpdatePOVoucherStatus(List<POApprovalVM> status)
        {
            POApprovalVM approval = status.First();
            using var transaction = _context.Database.BeginTransaction();

            try
            {


                foreach (var item in status)
                {
                    var Vch = _context.TblPurchaseContractDetails.Where(e => e.PoNo == item.VchNo && e.CmpId == auth.CmpId && e.LocId == auth.LocId).ToList();

                    foreach (var PO in Vch)
                    {

                        PO.Verify = item.IsVerified;
                        PO.Aprove = item.IsApproved;
                        PO.AuditByN = item.IsAudited;
                        PO.Rcvd = item.IsPending;
                        PO.AppBy = auth.UserId;
                    }

                }


                _context.SaveChanges();
                transaction.Commit();
                return "true";
            }
            catch (Exception)
            {
                transaction.Rollback();
                return "false";
            }
        }


        #endregion

        #region GatePas Inward Approval

        public DataTable GetGPIVch(DateTime fromDate, DateTime toDate, bool unapproved, bool approved, bool rejected, bool all)
        {

            string Filter = "";
            if (approved)
            {
                Filter = "AND  ISNULL(GPApprove,0) =1 ";
            }
            else if (unapproved)
            {
                Filter = "AND  ISNULL(GPApprove,0) =0 ";
            }
            else if (rejected)
            {
                Filter = "AND  ISNULL(reject,0) =1 ";
            }






            string qty = "SELECT Distinct t.VCHNO, Isnull(Gross,0) Gross,Isnull(Tare,0) Tare,CONVERT(varchar(11),T.VchDate,103) VchDate,  LB.ArrivalNo, CONVERT(VARCHAR(10),Lb.ResDate,103) AS ResultDate, t.DESCRP, TimeIn,TimeOut,T.VehicleNo," +
                " t.UID, Isnull(tm.Verify,0) Verify, Isnull(tm.VerifyBY,0) VerifyBY, " +
                " Isnull(tm.GPApprove,0) Aprove, Isnull(tm.GPApproveBy,0) AppBy, Isnull(tm.AuditBy,0) AuditBy, Isnull(tm.AuditByN,0) AuditByN, " +
                " Isnull(t.Reject,0) Reject, Isnull(t.RejectedBy,0) RejectedBy,Isnull(WB,'')" +
                " WB,Isnull(Gpno,0) Gpno, PL5.Names Party, PL5.LEVEL4+PL5.LEVEL5 PCODE,Isnull(Freight,0) Freight, Isnull(FreightType,'') FreightType," +
                " IsNull(FirstWeight,0) FirstWeight,IsNull(SecWeight,0) SecWeight, isnull((Gross-Tare),0) Net FROM TblTRANSVCH T" +
                " left Join Transmain TM On tm.vchtype=t.vchtype and tm.vchno=t.vchno AND " +
                " t.FINID = TM.FinId AND t.LOCID= TM.LocId and t.Cmp_Id = TM.Cmp_Id " +
                " Left Outer Join Level5 PL5 On Pl5.Level4+Pl5.Level5= T.MCode  AND t.LOCID= PL5.LocID and t.Cmp_Id = PL5.comp_id" +
                " LEFT OUTER JOIN TblLabResults LB ON LB.ArrivalNo=T.VchNo AND LB.locid=T.LocID AND LB.finid=T.FinID AND LB.Comp_id=T.Cmp_Id " +
                " Where T.Cmp_Id = '" + auth.CmpId + "' and T.LocId = '" + auth.LocId + "' and isnull(T.secweight,0)=0 and T.VchType = 'RP-RAW' and T.VchDate between '" + fromDate.ToString("yyyy/MM/dd") + "' and '" + toDate.ToString("yyyy/MM/dd") + "' and T.Sno=1   AND  (CASE WHEN ISNULL(T.EXPWT,0)=0 THEN ISNULL(LB.ArrivalNo,0) ELSE 1 END ) >0" + Filter;

            return _dataLogic.LoadData(qty);
        }


        public DataTable GetGPIVchDetail(int vchNo, string vchType)
        {
            string qry = @$"SELECT T.VCHNO, (I.LEVEL4+I.LEVEL5) CODE, I.NAMES ITEMNAME,
                            ISNULL(T.SQTY, 0) QTY, ISNULL(T.SBAGS, 0) BAGS,
                            ISNULL(SUM(T.SQTY),0) AS TOTALSQTY, ISNULL(SUM(T.SBAGS),0) AS TOTALBAGS
                            FROM TBLTRANSVCH T
                            INNER JOIN LEVEL5 I  ON T.DMCODE+T.CODE = I.LEVEL4+I.LEVEL5 AND T.CMP_ID = I.COMP_ID
                            WHERE T.VCHNO = {vchNo} AND VCHTYPE = 'RP-RAW' AND T.CMP_ID = {auth.CmpId} AND T.LOCID = '{auth.LocId}'
                            GROUP BY T.VCHNO, I.LEVEL4, I.LEVEL5, I.NAMES, T.SQTY, T.SBAGS;";

            return _dataLogic.LoadData(qry);
        }


        public string UpdateGPIVoucherStatus(List<GPIApprovalVM> status)
        {
            using var transaction = _context.Database.BeginTransaction();

            try
            {
                var vchNumbers = status.Select(s => s.VchNo).ToList();

                var gpis = _context.TransMains
                    .Where(e => vchNumbers.Contains(e.VchNo) && e.VchType == "RP-RAW" &&
                                e.CmpId == auth.CmpId && e.LocId == auth.LocId &&
                                e.FinId == auth.FinId)
                    .ToList();

                var tblTransVches = _context.TblTransVches
                    .Where(e => vchNumbers.Contains((int)e.VchNo) && e.VchType == "RP-RAW" &&
                                e.CmpId == auth.CmpId && e.LocId == auth.LocId &&
                                e.FinId == auth.FinId)
                    .ToList();

                foreach (var item in status)
                {
                    var gpi = gpis.FirstOrDefault(g => g.VchNo == item.VchNo);
                    if (gpi != null)
                    {
                        gpi.Gpapprove = item.IsApproved;
                        gpi.GpapproveBy = auth.UserId;
                    }

                    var tblTransVch = tblTransVches.FirstOrDefault(t => t.VchNo == item.VchNo);
                    if (tblTransVch != null)
                    {
                        tblTransVch.Reject = item.IsRejected;
                        tblTransVch.RejectedBy = auth.UserId;
                    }
                }

                _context.SaveChanges();
                transaction.Commit();
                return "true";
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                return "false";
            }
        }



        #endregion


        #region Maize-Rate-Approval

        public string UpdateMaizeRateStatus(List<MaizeRateApprovalVM> status)
        {
            MaizeRateApprovalVM approval = status.First();
            using var transaction = _context.Database.BeginTransaction();

            try
            {


                foreach (var item in status)
                {
                    var Vch = _context.Tblratediffs.Where(e => e.Vchno == item.VchNo && e.CmpId == auth.CmpId && e.LocId == auth.LocId && e.FinId == auth.FinId).ToList();

                    foreach (var PO in Vch)
                    {
                        PO.Approve = item.IsApproved;
                    }

                }


                _context.SaveChanges();
                transaction.Commit();
                return "true";
            }
            catch (Exception)
            {
                transaction.Rollback();
                return "false";
            }
        }

        #endregion


        #region Party Terms And Conditon Approval

        public string UpdatePartyTermCondition(List<PartyTermConditionsApprovalVM> status)
        {
            using var transaction = _context.Database.BeginTransaction();
            try
            {


                foreach (var item in status)
                {
                    var Vch = _context.TblPartyTcs.Where(e => e.Id == item.VchNo && e.CmpId == auth.CmpId && e.LocId == auth.LocId).ToList();

                    foreach (var PO in Vch)
                    {
                        PO.Aprove = item.IsApproved;
                        PO.AppBy = auth.UserId;
                    }

                }


                _context.SaveChanges();
                transaction.Commit();
                return "true";
            }
            catch (Exception)
            {
                transaction.Rollback();
                return "false";
            }
        }

        #endregion



        #region DO Approval 

        public DataTable GetDOApprovalList(DateTime fromDate, DateTime toDate, string vchType)
        {
            string qry = $@"SELECT  T.VCHNO VchNO, CONVERT(VARCHAR(10),T.VCHDATE,103)  VchDate, {(vchType == "SR" ? "ISNULL(T.CREDIT,0)" : "ISNULL(T.DEBIT,0)")}  AMOUNT,
			(SELECT SUM(QTY) FROM TBLTRANSVCH WHERE VCHTYPE = T.VCHTYPE AND VCHNO = T.VCHNO AND CMP_ID = T.CMP_ID AND LOCID = T.LOCID AND FINID = T.FINID AND TUCKS = 8) QTY
            , L5.NAMES CUSTOMER, isnull(M.GPApprove,0) as Approve
			FROM TBLTRANSVCH T
            INNER JOIN TRANSMAIN M ON T.VCHTYPE = M.VCHTYPE AND T.CMP_ID = M.CMP_ID AND T.VCHNO = M.VCHNO AND T.LOCID  = M.LOCID
			INNER JOIN LEVEL5 L5 ON T.DMCODE + T.CODE = L5.LEVEL4+L5.LEVEL5 AND T.CMP_ID  = L5.COMP_ID
			WHERE T.VCHTYPE  = '{vchType}' AND T.TUCKS = 9 AND T.CMP_ID = {auth.CmpId} AND T.FINID = {auth.FinId} AND T.LOCID = '{auth.LocId}' 
            AND CONVERT(VARCHAR,T.VCHDATE,111)  BETWEEN '{fromDate.ToString("yyyy/MM/dd")}' AND '{toDate.ToString("yyyy/MM/dd")}'
            ORDER BY T.VCHNO DESC";

            return _dataLogic.LoadData(qry);
        }


        public string UpdateDoStatus(List<DOApprovalVM> status)
        {
            DOApprovalVM approval = status.First();
            using var transaction = _context.Database.BeginTransaction();

            try
            {

                foreach (var item in status)
                {
                    var Vch = _context.TransMains.Where(e => e.VchNo == item.VchNo && e.VchType.StartsWith("SP") && e.CmpId == auth.CmpId && e.LocId == auth.LocId && e.FinId == auth.FinId).ToList();

                    foreach (var DO in Vch)
                    {


                        DO.Gpapprove = item.IsApproved;
                        DO.GpapproveBy = auth.UserId;

                    }
                }

                _context.SaveChanges();
                transaction.Commit();
                return "true";
            }
            catch (Exception)
            {
                transaction.Rollback();
                return "false";
            }
        }

        #endregion
    }


}

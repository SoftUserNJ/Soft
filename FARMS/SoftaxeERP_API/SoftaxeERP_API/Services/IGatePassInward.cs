using DevExpress.Charts.Native;
using DevExpress.XtraRichEdit.Import.Rtf;
using Microsoft.CodeAnalysis.FlowAnalysis;
using Microsoft.EntityFrameworkCore;
using SoftaxeERP_API.Models;
using SoftaxeERP_API.VM;
using System.Data;
using System.Security.AccessControl;

namespace SoftaxeERP_API.Services
{
    public interface IGatePassInward
    {
        DataTable GetGatePassInwardEntryVchNo(string vchType);
        object SaveGatePassInwardEntry(List<PurchaseGatePassInwardEntryVM> gp);
        DataTable GetGatePassInwardList(DateTime fromDate, DateTime toDate, string vchType, string grnNo, string tag);
        DataTable GetEditGatePassInward(int vchNo, string vchType);
        DataTable CheckVechicleGPInward(string vehicalNo, int vchno, string vchtType, DateTime vchDate);
        bool DelGatePassInward(int vchNo, string vchType);
    }

    public class GatePassInward : IGatePassInward
    {

        private readonly ErpSoftaxeContext _context;
        private readonly IDataLogic _dataLogic;
        private readonly IAuth _auth;

        readonly AuthVM auth;
        public GatePassInward(ErpSoftaxeContext context, IDataLogic dataLogic, IAuth authData)
        {
            _context = context;
            _dataLogic = dataLogic;
            _auth = authData;

            auth = _auth.GetUserData();
        }

        public DataTable GetGatePassInwardEntryVchNo(string vchType)
        {
            String qry = $@"SELECT ISNULL(MAX(VCHNO),0)+1 VCHNO 
                            FROM TBLTRANSVCH WHERE CMP_ID = {auth.CmpId} AND FINID = {auth.FinId}  AND VCHTYPE = '{vchType}'";
            return _dataLogic.LoadData(qry);
        }

        public object SaveGatePassInwardEntry(List<PurchaseGatePassInwardEntryVM> gp)
        {
            PurchaseGatePassInwardEntryVM fr = gp.FirstOrDefault();
            using var transaction = _context.Database.BeginTransaction();

            try
            {
                if (fr.vchNo == 0 || fr.vchNo == null)
                {
                    fr.vchNo = (_context.TransMains
                        .Where(x => x.VchType == fr.vchType && x.CmpId == auth.CmpId && x.FinId == auth.FinId && x.LocId == auth.LocId)
                        .Max(x => (int?)x.VchNo) ?? 0) + 1;

                }
                var rackno = _context.Tblshelfs
                    .Where(x => x.CompId == auth.CmpId &&
                                x.Locid == auth.LocId &&
                                x.Godownid == fr.godown)
                    .Select(x => x.Rackno)
                    .FirstOrDefault();

                var shelfid = _context.Tblshelfs
                    .Where(x => x.CompId == auth.CmpId &&
                                x.Locid == auth.LocId &&
                                x.Godownid == fr.godown)
                    .Select(x => x.Shelfno)
                    .FirstOrDefault();
                int firstWeightby = 0;
                var mains = _context.TransMains.Where(x => x.CmpId == auth.CmpId && x.FinId == auth.FinId && x.VchType == fr.vchType && x.LocId == auth.LocId && x.VchNo == fr.vchNo).FirstOrDefault();
                var vches = _context.TblTransVches.Where(x => x.CmpId == auth.CmpId && x.FinId == auth.FinId && x.VchType == fr.vchType && x.LocId == auth.LocId && x.VchNo == fr.vchNo && x.Sno==1).FirstOrDefault();

                int? firstwtby = mains?.Firstwtby ?? 0;
                int? firstweight = vches?.FirstWeight ?? 0; // Assuming Firstwtby is an int and using 0 as default


                
                DateTime? dateIn = vches?.DateIn;
                TimeSpan? timeIn = vches?.TimeIn;
                if (mains !=null)
                {
                    _context.Entry(mains).State = EntityState.Detached;
                }

                if (vches != null)
                {
                    _context.Entry(vches).State = EntityState.Detached;
                }
               

                _context.TransMains
                    .Where(x => x.VchType == fr.vchType && x.VchNo == fr.vchNo
                    && x.FinId == auth.FinId && x.CmpId == auth.CmpId
                    && x.LocId == auth.LocId).
                    ExecuteDelete();


                _context.TransMains.Add(new TransMain
                {

                    VchType = fr.vchType,
                    VchNo = fr.vchNo,
                    VchDateM = Convert.ToDateTime(fr.grnDate),

                    FinId = auth.FinId,
                    LocId = auth.LocId,
                    CmpId = auth.CmpId,
                    Firstwtby = firstwtby,
                    Aprove = false,
                    AppBy = 0,
                });

                _context.TblTransVches
                    .Where(x => x.VchType == fr.vchType && x.CmpId == auth.CmpId && x.FinId == auth.FinId && x.LocId == auth.LocId && x.VchNo == fr.vchNo)
                    .ExecuteDelete();

                int sno = 0;
                foreach (var item in gp)
                {
                    sno = sno + 1;
                    Double Rate = Convert.ToDouble(item.rate);
                    if (item.pono==0)
                    {
                        var Dailyrate = _context.Tblratediffs
                            .Where(x => x.CmpId == auth.CmpId &&
                                        x.LocId == auth.LocId &&
                                        x.ItemCode == item.itemSub &&
                                        x.FromDate <= Convert.ToDateTime(fr.grnDate) &&
                                        Convert.ToDateTime(fr.grnDate) <= x.ToDate)
                            .FirstOrDefault();

                        if (Dailyrate!=null)
                         {
                            Rate = Convert.ToDouble(Dailyrate.Rate);
                        }

                    }
                    _context.TblTransVches.Add(new TblTransVch
                    {
                        VchType = fr.vchType,
                        VchNo = fr.vchNo,               
                        Sno = sno,
                        VchDate = Convert.ToDateTime(fr.grnDate),
                        EntryDate = Convert.ToDateTime(fr.entrydate),
                        Descrp = item.remarks,
                        Location = fr.location,
                        FirstWeight = firstweight,
                        SecWeight = 0,
                        DateIn = dateIn,
                        TimeIn = timeIn,
                        Pono = item.pono,
                        //Gpno = Convert.ToInt32(fr.gpno),
                        VehicleNo = fr.vehicleNo,
                        BilltyNo = fr.biltyNo,
                        Freight = Convert.ToInt32(fr.freight),
                        FreightType = fr.freightDD,
                        //SecWeight check
                        Remarks = fr.remarks,
                        SubParty = fr.subParty,
                        Mcode = fr.partySub,
                        Dmcode = item.itemSub.Substring(0, 9),
                        Code = item.itemSub.Substring(9, 5),
                        Sqty = item.net,
                        GodownId = item.godown,
                        RackId = rackno,
                        ShelfId = shelfid,
                        Uom = item.uom.ToString(),
                        Sbags = item.bags,
                        BagsType = item.bagsType,
                        MiniWt = fr.chkMinWeight,
                        Wtype = fr.minWeight,
                        //bagsWt
                        Gross = item.gross,
                        Tare = item.tare,
                        ExpWt = item.expWt,
                        SubName = item.retStat,
                        //standarWt

                        Rate = Convert.ToDouble(item.rate),

                        FinId = auth.FinId,
                        LocId = auth.LocId,
                        CmpId = auth.CmpId,
                        Tucks = 8,
                        DriverName = item.driverName,
                        DriverContact = item.contact,
                        DriverCnic = item.tCNIC,
                        Vaprove = 0,
                        Uid = Convert.ToString(auth.UserId),

                    }); ;
                }

                _context.SaveChanges();
                transaction.Commit();
                return new {
                    status = true,
                    vchNo = fr.vchNo
                    };
            }
            catch (Exception)
            {
                transaction.Rollback();
                  return new {
                    status = false,
                    vchNo = fr.vchNo
                    };
                throw;
            }
        }


        public DataTable GetGatePassInwardList(DateTime fromDate, DateTime toDate, string vchType, string grnNo, string tag)
        {
            string and = "";
            if (!string.IsNullOrEmpty(grnNo))
            {
                if (grnNo != "0")
                {
                    and = $" AND VCHNO = '{grnNo}' ";
                }
            }

            string qry = $@"SELECT DISTINCT TV.VCHTYPE, TV.VCHNO, CONVERT(VARCHAR(10),VCHDATE,103) VCHDATE, ISNULL(VEHICLENO,'') VEHICLENO,
            ISNULL(BILLTYNO,'')BILTYNO, P.NAMES PARTY
            FROM TBLTRANSVCH TV
            JOIN TRANSMAIN M ON TV.VCHTYPE = M.VCHTYPE AND TV.VCHNO = M.VCHNO AND TV.CMP_ID = M.CMP_ID AND TV.LOCID = M.LocId
            INNER JOIN LEVEL5 P ON TV.MCODE = P.LEVEL4+P.LEVEL5 AND P.COMP_ID = TV.CMP_ID
            WHERE TV.VCHTYPE = '{vchType}' AND TV.CMP_ID = '{auth.CmpId}' AND TV.LOCID = '{auth.LocId}' {and}
            AND CAST(VCHDATE AS DATE) BETWEEN '{fromDate.ToString("yyyy/MM/dd")}' AND '{toDate.ToString("yyyy/MM/dd")}'
            AND ISNULL(SECWEIGHT,'0') = '0' 
            AND ISNULL(M.GPAPPROVE, 0) <> 1
            ORDER BY VCHNO";

            return _dataLogic.LoadData(qry);
        }

        public DataTable GetEditGatePassInward(int vchNo, string vchType)
        {
            String qry = $@"SELECT   ROW_NUMBER() OVER (ORDER BY TV.vchno) AS SNO,  TV.VCHTYPE, TV.VCHNO, CONVERT(VARCHAR(10),TV.VCHDATE , 103) VCHDATE, TV.DESCRP, TV.LOCATION, VEHICLENO, BILLTYNO,
            FREIGHT, FREIGHTTYPE, REMARKS, SUBPARTY, MCODE PARTYSUB, DMCODE+CODE ITEMSUBCODE, 
            ISNULL(SQTY,0) SQTY , ISNULL(QTY,0) QTY, TV.GODOWNID, TV.UOM, ISNULL(BAGS,0) BAGS, ISNULL(SBAGS,0) SBAGS ,BAGSTYPE, GROSS, TARE, EXPWT, SUBNAME RETSTAT, TV.RATE,
            G.GODOWNNAME, U.UOM UOMNAME, I.NAMES ITEMNAME, I.SDWT , TV.DRIVERNAME , TV.DRIVERCONTACT ,TV.DRIVERCNIC , WTYPE ,MINIWT,
            ISNULL(TM.GpApprove,0) APROVE, ISNULL(TM.GpapproveBy,0) APPBY, ISNULL(TM.VERIFY,0) VERIFY, ISNULL(TM.VERIFYBY,0) VERIFYBY,
            ISNULL(TM.AUDITBY,0) AUDITBY, ISNULL(TM.AUDITBYN,0) AUDITBYN,  ISNULL(TM.PRINTED,0) PRINTED, TV.PONO
            FROM TBLTRANSVCH TV
            LEFT JOIN TRANSMAIN TM ON TV.VCHTYPE = TM.VCHTYPE AND TV.VCHNO = TM.VCHNO AND TM.CMP_ID = TV.CMP_ID AND TV.LOCID = TM.LOCID
            INNER JOIN TBLGODOWNS G ON TV.GODOWNID = G.GODOWNID AND G.COMP_ID = TV.CMP_ID
            INNER JOIN TBLUOM U ON TV.UOM = U.ID AND U.COMP_ID = TV.CMP_ID
            INNER JOIN LEVEL5 I ON TV.DMCODE+TV.CODE = I.LEVEL4+I.LEVEL5 AND I.COMP_ID = TV.CMP_ID
            WHERE TV.VCHTYPE = '{vchType}' AND TV.VCHNO = '{vchNo}' AND TV.CMP_ID = {auth.CmpId} AND TV.LOCID = '{auth.LocId}'";

            return _dataLogic.LoadData(qry);
        }

        public DataTable CheckVechicleGPInward(string vehicalNo, int vchno, string vchtType, DateTime vchDate)
        {
            String qry = $@"SELECT * FROM TBLTRANSVCH WHERE VEHICLENO = '{vehicalNo}' 
                            AND CMP_ID = '{auth.CmpId}' AND  LOCID = '{auth.LocId}'  AND (ISNULL(FIRSTWEIGHT,'0') = '0' OR ISNULL(SECWEIGHT,'0') = '0')    
                            AND ID NOT IN ( SELECT ID FROM TBLTRANSVCH  WHERE  VCHTYPE = '{vchtType}' AND VCHNO = '{vchno}' AND VCHDATE =  '{vchDate.ToString("yyyy/MM/dd")}'     AND   CMP_ID = '{auth.CmpId}'  AND  LOCID = '{auth.LocId}' ) ";
            return _dataLogic.LoadData(qry);
        }


        public bool DelGatePassInward(int vchNo, string vchType)
        {
            using var transaction = _context.Database.BeginTransaction();
            try
            {
                _context.TransMains.Where(x => x.VchType == vchType && x.VchNo == vchNo && x.CmpId == auth.CmpId && x.LocId == auth.LocId && x.FinId == auth.FinId).ExecuteDelete();
                _context.TblTransVches.Where(x => x.VchType == vchType && x.VchNo == vchNo && x.CmpId == auth.CmpId && x.LocId == auth.LocId && x.FinId == auth.FinId).ExecuteDelete();

                _context.SaveChanges();
                transaction.Commit();
                return true;
            }
            catch (Exception)
            {
                transaction.Rollback();
                return false;
                throw;
            }
        }

    }
}

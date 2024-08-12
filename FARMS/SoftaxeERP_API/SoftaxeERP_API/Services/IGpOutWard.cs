using Microsoft.EntityFrameworkCore;
using SoftaxeERP_API.Models;
using SoftaxeERP_API.VM;
using System.Data;

namespace SoftaxeERP_API.Services
{
    public interface IGpOutWard
    {
        DataTable GetGatePassList();
        DataTable GetMaxGpNo();
        DataTable GetPendingOrders();
        DataTable GetIOrderDetail(int doNo);
        bool UpdateDO(List<DOVM> vM);
        DataTable GetEditGatePassOutward(int GPNO);
        bool ClearGPNO(ClearGpNoVM request);

        bool DeleteGPNO(ClearGpNoVM request);
    }
    public class GpOutWard : IGpOutWard
    {
        private readonly ErpSoftaxeContext _context;
        private readonly IDataLogic _dataLogic;
        private readonly IAuth _auth;

        readonly AuthVM auth = new();
        public GpOutWard(ErpSoftaxeContext context, IDataLogic dataLogic, IAuth authData)
        {
            _context = context;
            _dataLogic = dataLogic;
            _auth = authData;

            auth = _auth.GetUserData();
        }

        public DataTable GetGatePassList()
        {
            string qry = $@"SELECT GPNO, CONVERT(VARCHAR(11), CHQDATE, 103) GPDATE, VEHICLENO, DRIVERNAME, Sum(QTY) AS QTY, FREIGHT
                        FROM TBLTRANSVCH 
                        WHERE CMP_ID = '{auth.CmpId}' AND LOCID = '{auth.LocId}' AND FINID = '{auth.FinId}' AND VCHTYPE = 'SP' AND ISNULL(GPNO,0) <> 0 AND Tucks = 9
                        Group by GPNO, ChqDate, VEHICLENO, DRIVERNAME, FREIGHT";
            return _dataLogic.LoadData(qry);
        }

        public DataTable GetMaxGpNo()
        {
            string qry = $@"SELECT ISNULL(MAX(GPNO),0) + 1 AS VCHNO FROM TBLTRANSVCH WHERE VCHTYPE = 'SP' AND CMP_ID = {auth.CmpId} AND LOCID = '{auth.LocId}' AND FINID = {auth.FinId}";

            return _dataLogic.LoadData(qry);
        }

        public DataTable GetPendingOrders()
        {
            string qry = $@"SELECT DISTINCT ISNULL(DONO, V.VCHNO) DONO, CONVERT(VARCHAR(11), VCHDATE, 103) DODATE, L5.NAMES PARTY, ISNULL(V.QTY,0) QTY, ISNULL(SQTY,0) DELQTY
            FROM TBLTRANSVCH V 
			JOIN TRANSMAIN M ON V.VCHNO = M.VCHNO AND V.VCHTYPE = M.VCHTYPE AND V.CMP_ID = M.CMP_ID 
            JOIN LEVEL5 L5 ON L5.LEVEL4 + L5.LEVEL5 = V.DMCODE + CODE AND L5.COMP_ID = V.CMP_ID
            WHERE V.VCHTYPE = 'SP' AND ISNULL(V.SENT, 0) = 1 AND ISNULL(M.GPAPPROVE, 0) = 1 AND TUCKS = 9 AND ISNULL(GPNO, 0) = 0 AND V.CMP_ID = {auth.CmpId} AND V.LOCID = '{auth.LocId}'";

            return _dataLogic.LoadData(qry);
        }

        public DataTable GetIOrderDetail(int doNo)
        {
            string qry = $@"SELECT ISNULL(DONO, V.VCHNO) DONO, CONVERT(VARCHAR(11), VCHDATE, 103) DODATE, L51.NAMES AS PARTY, SP.SUBPARTY, L5.Level4+L5.Level5 as ProductCode, L5.NAMES PRODUCT, ISNULL(V.QTY,0) QTY, ISNULL(SQTY,0) DELQTY
            FROM TBLTRANSVCH V 
            JOIN LEVEL5 L5 ON L5.LEVEL4 + L5.LEVEL5 = V.DMCODE + CODE AND L5.COMP_ID = V.CMP_ID
            JOIN LEVEL5 L51 ON L51.LEVEL4 + L51.LEVEL5 = V.MCODE AND L51.COMP_ID = V.CMP_ID
			JOIN TBLSUBPARTY SP ON V.SUBPARTYID = SP.ID AND SP.CMPID = V.CMP_ID
            WHERE V.VCHTYPE = 'SP' AND DONO = {doNo} AND TUCKS = 8 AND V.CMP_ID = {auth.CmpId} AND V.LOCID = '{auth.LocId}'";

            return _dataLogic.LoadData(qry);
        }

        public DataTable GetEditGatePassOutward(int GPNO)
        {
            string qry = $@"SELECT DISTINCT ISNULL(DONO, V.VCHNO) DONO, CONVERT(VARCHAR(11), VCHDATE, 103) DODATE, L5.NAMES PARTY, ISNULL(V.QTY,0) QTY, ISNULL(SQTY,0) DELQTY, GPNO, ChqDate GPDate, Freight, VehicleNo, DriverName, DriverContact, BilltyNo
                FROM TBLTRANSVCH V 
                JOIN TRANSMAIN M ON V.VCHNO = M.VCHNO AND V.VCHTYPE = M.VCHTYPE AND V.CMP_ID = M.CMP_ID 
                JOIN LEVEL5 L5 ON L5.LEVEL4 + L5.LEVEL5 = V.DMCODE + CODE AND L5.COMP_ID = V.CMP_ID
                WHERE V.VCHTYPE = 'SP' AND ISNULL(V.SENT, 0) = 1 AND ISNULL(M.GPAPPROVE, 0) = 1 AND TUCKS = 9 AND V.CMP_ID = '{auth.CmpId}' AND V.LOCID = '{auth.LocId}' and GPNO = '{GPNO}'";

            return _dataLogic.LoadData(qry);
        }





        public bool UpdateDO(List<DOVM> vM)
        {
            var transaction = _context.Database.BeginTransaction();

            try
            {
                foreach (var fr in vM)
                {
                    var vch = _context.TblTransVches
                        .Where(x => x.VchNo == fr.DONO && x.VchType.StartsWith("SP") && x.CmpId == auth.CmpId && x.LocId == auth.LocId && x.FinId == auth.FinId && x.Tucks == 8 && fr.ProductCode == x.Dmcode+x.Code)
                        .ToList();

                    foreach (var item in vch)
                    {
                        item.Gpno = fr.GpNo;
                        item.ChqDate = fr.GpDate;
                        item.VehicleNo = fr.VehicleNo;
                        item.DriverName = fr.DriverName;
                        item.DriverContact = fr.Phone;
                        item.BilltyNo = fr.BiltyNo;
                        item.Sqty = (double?)fr.DELQTY;
                    }

                    var Frightvch = _context.TblTransVches
                        .Where(x => x.VchNo == fr.DONO && x.VchType.StartsWith("SP") && x.CmpId == auth.CmpId && x.LocId == auth.LocId && x.FinId == auth.FinId && x.Tucks == 9)
                        .ToList();
                    foreach (var item in Frightvch)
                    {
                        item.Gpno = fr.GpNo;
                        item.Freight = fr.Freight;
                        item.Sqty = fr.sumDelQty;
                        item.ChqDate = fr.GpDate;
                        item.DriverContact = fr.Phone;
                        item.BilltyNo = fr.BiltyNo;
                        item.VehicleNo = fr.VehicleNo;
                        item.DriverName = fr.DriverName;

                    }

                }

                transaction.Commit();
                _context.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                transaction.Rollback();
                return false;
                throw;
            }
        }


        public bool ClearGPNO(ClearGpNoVM request)
        {
            var transaction = _context.Database.BeginTransaction();

            try
            {

                    var vch = _context.TblTransVches
                        .Where(x => x.VchNo == request.DONO && x.VchType.StartsWith("SP") && x.CmpId == auth.CmpId && x.LocId == auth.LocId && x.FinId == auth.FinId)
                        .ToList();

                    foreach (var item in vch)
                    {
                        item.Gpno = 0;
                        item.Sqty = 0;
                          
                    }

                 
                transaction.Commit();
                _context.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                transaction.Rollback();
                return false;
                throw;
            }
        }


        public bool DeleteGPNO(ClearGpNoVM request)
        {
            var transaction = _context.Database.BeginTransaction();

            try
            {

                var vch = _context.TblTransVches
                    .Where(x => x.Gpno == request.GPNO && x.VchType.StartsWith("SP") && x.CmpId == auth.CmpId && x.LocId == auth.LocId && x.FinId == auth.FinId)
                    .ToList();

                foreach (var item in vch)
                {
                    item.Gpno = 0;
                    item.Sqty = 0;
                    item.Freight = 0;

                }


                transaction.Commit();
                _context.SaveChanges();
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
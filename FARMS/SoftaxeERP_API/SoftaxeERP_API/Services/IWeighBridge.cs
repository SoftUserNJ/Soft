using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using SoftaxeERP_API.Models;
using SoftaxeERP_API.VM;
using System.Data;

namespace SoftaxeERP_API.Services
{
    public interface IWeighBridge
    {

        DataTable GetFirstWeight(string VchType);
        DataTable GetSecondWeight(string VchType);

        string GetFirstWeightDetail(int VchNo, int ArrivalNo, string VchType, string Status);
        string GetSecondWeightDetail(int VchNo, int ArrivalNo, string VchType);

        Object SaveWeighment(WeighBridgeVM weigh);

        // GatePass Outward
        DataTable GetAllowedWtDiff();
        DataTable GetFirstWeightOutward();
        DataTable GetFirstWeightOutwardDetail(int VchNo);
        DataTable GetSecondWeightOutward();
        DataTable GetSecondWeightOutwardDetail(int VchNo);
        bool SaveOutWardWeighment(WeighBridgeVM weigh);

        // Outward Status

        DataTable GetOutwardStatus();
        DataTable GetOutwardbyDate(DateTime fromDate, DateTime toDate);

        // WB Settings

        DataTable GetWBSettings();
    }

    public class WeighBridge : IWeighBridge
    {
        private readonly ErpSoftaxeContext _context;
        private readonly IDataLogic _dataLogic;
        private readonly IAuth _auth;

        readonly AuthVM auth = new();
        public WeighBridge(ErpSoftaxeContext context, IDataLogic dataLogic, IAuth authData)
        {
            _context = context;
            _dataLogic = dataLogic;
            _auth = authData;

            auth = _auth.GetUserData();
        }

        #region GATE PASS INWARD

        public DataTable GetFirstWeight(string VchType)
        {
            string qry = @"Select T.sno,  T.VchNo ArrivalNo, T.VchNo, T.VchType, T.VehicleNo,FirstWeight, FORMAT(VchDate, 'dd-MM-yyyy') AS VchDate  from tblTransVch T
                        INNER JOIN TransMain TM ON TM.VchType = T.VchType and TM.VchNo = T.VchNo  and TM.Cmp_Id = T.Cmp_Id and T.LocID = TM.LocId and T.FinID = TM.FinId
                        where T.VchType = '" + VchType + "'  AND T.Cmp_id = '" + auth.CmpId + "' and T.locid = '" + auth.LocId + "'  and isnull(TM.GPApprove, 0) = 1 and T.FinID = '" + auth.FinId + "' and T.sno = 1 and isnull(T.FirstWeight,0) = 0 and isnull(T.SecWeight,0) = 0 and isnull(T.Reject,0) = 0";

            return _dataLogic.LoadData(qry);
        }

        public DataTable GetSecondWeight(string VchType)
        {
            string qry = @"Select T.sno,  T.VchNo ArrivalNo, T.VchNo, T.VchType, T.VehicleNo,FirstWeight, FORMAT(VchDate, 'dd-MM-yyyy') AS VchDate  from tblTransVch T
                        INNER JOIN TransMain TM ON TM.VchType = T.VchType and TM.VchNo = T.VchNo  and TM.Cmp_Id = T.Cmp_Id and T.LocID = TM.LocId and T.FinID = TM.FinId
                        where T.VchType = '" + VchType + "'  AND T.Cmp_id = '" + auth.CmpId + "' and T.locid = '" + auth.LocId + "'  and isnull(TM.GPApprove, 0) = 1 and T.FinID = '" + auth.FinId + "' and T.sno = 1 and isnull(T.FirstWeight,0) > 0 and isnull(T.SecWeight,0) = 0 and isnull(T.Reject,0) = 0";
            return _dataLogic.LoadData(qry);
        }

        public string GetFirstWeightDetail(int VchNo, int ArrivalNo, string VchType, string Status)
        {
            string filter = " and isnull(T.FirstWeight,0) > 0 and  isnull(T.SecWeight,0) = 0 ";
            if (Status == "true")
            {
                filter = " and isnull(T.FirstWeight,0) = 0 and  isnull(T.SecWeight,0) = 0 ";

            }


            string qry = @"Select sno, T.VchNo, VehicleNo, T.PONO, i.Names as ItemName, p.Names as PartyName, T.Bags, T.Rate, T.Qty,T.BagsType, T.Freight, T.FreightType, 
                        T.BilltyNo, isnull(T.ExpWt,0) as ExpWt, T.ArrivalNo, T.Gross, isnull(FirstWeight,0) as FirstWeight, isnull(SecWeight,0) as SecWeight, WType, MiniWt, ExpWt, T.Tare, G.GodownId, G.GodownName, T.Cmb1, T.Cmb2, T.Cmb3,
                        FORMAT(VchDate, 'yyyy-MM-dd') AS VchDate , ISNULL( T.SBags,0)  SBAGS  , ISNULL( T.SQTY,0)  SQTY  from tblTransVch T
                        LEFT OUTER JOIN TransMain TM ON TM.VchType = T.VchType and TM.VchNo = T.VchNo and T.Cmp_id = TM.Cmp_id and T.LocId = TM.LocId and T.FinID = TM.FinId
                        LEFT OUTER JOIN Level5 i on i.Level4+i.Level5 = T.DMCode+Code and T.Cmp_id = i.comp_id  
                        LEFT OUTER JOIN Level5 p on p.Level4+p.Level5 = T.Mcode and T.Cmp_id = p.comp_id 
                        LEFT OUTER JOIN TBLGODOWNS G On G.GODOWNID = T.GodownId and T.Cmp_id = G.comp_id and T.LocId = G.LocId
                        where T.VchType = '" + VchType + "' AND T.Cmp_id = '" + auth.CmpId + "' and T.locid = '" + auth.LocId + "' and T.VchNo = '" + VchNo + "' " + filter + " and isnull(TM.GPApprove, 0) = 1 and T.Finid = '" + auth.FinId + "' and isnull(T.Reject,0) = 0";

            string qry1 = @"Select Id, LabTestName, Percentage, Bags, Isnull(PartyDed,0) PartyDed , isnull(StockDed,0) StockDed , isnull(PartyDedKg,0) PartyDedKg  ,isnull(StockDedKg,0)  StockDedKg  from tblLabResults where Comp_id = '" + auth.CmpId + "' and locid = '" + auth.LocId + "' and ArrivalNo = '" + ArrivalNo + "'";
            var dt1 = _dataLogic.LoadData(qry1);
            var dt2 = _dataLogic.LoadData(qry);

            return JsonConvert.SerializeObject(new { LabDed = dt1, FirstWeight = dt2 });


        }

        public string GetSecondWeightDetail(int VchNo, int ArrivalNo, string VchType)
        {
            string qry = @"Select sno, T.VchNo, VehicleNo, T.PONO, i.Names as ItemName, p.Names as PartyName, T.Bags, T.Rate, T.Qty,T.BagsType, T.Freight, T.FreightType, 
                        T.BilltyNo, isnull(T.ExpWt,0) as ExpWt, T.ArrivalNo, T.Gross, isnull(FirstWeight,0) as FirstWeight, isnull(SecWeight,0) as SecWeight, WType, MiniWt, ExpWt, T.Tare, G.GodownName, T.Cmb1, T.Cmb2, T.Cmb3,
                        FORMAT(VchDate, 'yyyy-MM-dd') AS VchDate , ISNULL( T.SBags,0)  SBAGS  , ISNULL( T.SQTY,0)  SQTY  from tblTransVch T
                        LEFT OUTER JOIN TransMain TM ON TM.VchType = T.VchType and TM.VchNo = T.VchNo and T.Cmp_id = TM.Cmp_id and T.LocId = TM.LocId and T.FinID = TM.FinId
                        LEFT OUTER JOIN Level5 i on i.Level4+i.Level5 = T.DMCode+Code and T.Cmp_id = i.comp_id   
                        LEFT OUTER JOIN Level5 p on p.Level4+p.Level5 = T.Mcode and T.Cmp_id = p.comp_id 
                        LEFT OUTER JOIN TBLGODOWNS G On G.GODOWNID = T.GodownId and T.Cmp_id = G.comp_id and T.LocId = G.LocId
                        where T.VchType = '" + VchType + "' AND T.Cmp_id = '" + auth.CmpId + "' and T.locid = '" + auth.LocId + "' and T.VchNo = '" + VchNo + "' and isnull(T.FirstWeight,0) > 0 and  isnull(T.SecWeight,0) = 0 and isnull(TM.GPApprove, 0) = 1  and T.Finid = '" + auth.FinId + "' and isnull(T.Reject,0) = 0";

            string qry1 = @"Select Id, LabTestName, Percentage, Bags,  Isnull(PartyDed,0) PartyDed , isnull(StockDed,0) StockDed , isnull(PartyDedKg,0) PartyDedKg  ,isnull(StockDedKg,0)  StockDedKg from tblLabResults where Comp_id = '" + auth.CmpId + "' and locid = '" + auth.LocId + "' and ArrivalNo = '" + ArrivalNo + "'";
            var dt1 = _dataLogic.LoadData(qry1);
            var dt2 = _dataLogic.LoadData(qry);

            return JsonConvert.SerializeObject(new { LabDed = dt1, SecWeight = dt2 });
        }


        public Object SaveWeighment(WeighBridgeVM weigh)

        {
            using var transaction = _context.Database.BeginTransaction();
            try
            {
                DateTime currentDate = DateTime.Now;
                var report = "";
         
                var vches = _context.TblTransVches.Where(x => x.CmpId == auth.CmpId && x.FinId == auth.FinId && x.VchType == weigh.Vchtype && x.LocId == auth.LocId && x.VchNo == weigh.VchNo && x.Tucks==8 && x.Sno!=51).ToList();
                var mains = _context.TransMains.Where(x => x.CmpId == auth.CmpId && x.FinId == auth.FinId && x.VchType == weigh.Vchtype && x.LocId == auth.LocId && x.VchNo == weigh.VchNo).FirstOrDefault();
                var Gpno = (_context.TblTransVches.Where(x => x.VchType == weigh.Vchtype && x.CmpId == auth.CmpId && x.LocId == auth.LocId).Max(x => (int?)x.Gpno) ?? 0) + 1;
                var firstRow = vches.FirstOrDefault();

                if (vches != null && vches.Any())
                {

                    if (weigh.Type == "FirstWeight")
                    {

                        mains.Firstwtby = auth.UserId;
                        mains.VchDateM = weigh.vchDate;
                        _context.TransMains.Update(mains);
                       
                    }
                    else
                    {
                        mains.Secwtby = auth.UserId;
                        mains.VchDateM = weigh.vchDate;
                        _context.TransMains.Update(mains);

                    }


                    foreach (var vch in vches)
                    {
                        if (weigh.Type == "FirstWeight")
                        {
                           
                            vch.FirstWeight = weigh.FirstWeight;
                            vch.TimeIn = weigh.TimeIn;
                            vch.DateIn = weigh.vchDate;
                            vch.Fwm = weigh.manualWeight;
                            vch.VchDate = weigh.vchDate;
                            _context.TblTransVches.Update(vch);
                           
                            report = "FirstWeight";


                        }
                        else
                        {
                            vch.Gpno = Gpno;
                            vch.SecWeight = weigh.SecondWeight;
                            vch.TimeOut = weigh.TimeOut;
                            vch.DateOut = weigh.vchDate;
                            vch.VchDate = weigh.vchDate;
                     
                            vch.Bags = vch.ExpWt > 0 ? vch.Sbags : weigh.Bags;
                            vch.Qty = Convert.ToDecimal(vch.ExpWt > 0 ? vch.Sqty : weigh.Qty);
                            vch.PayableWt = vch.ExpWt > 0 ? vch.Sqty : weigh.StockWeight;
                            vch.PayableWt1 = vch.ExpWt > 0 ? vch.Sqty : weigh.PayableWeight;
                            vch.Cmb1 = weigh.Cmb1;
                            vch.Cmb2 = weigh.Cmb2;
                            vch.Cmb3 = weigh.Cmb3;
                            vch.Bags1 = weigh.Bags1;
                            vch.Bags2 = weigh.Bags2;
                            vch.Bags3 = weigh.Bags3;
                            vch.Ded1 = Math.Round( weigh.Ded1,3);
                            vch.Ded2 = Math.Round(weigh.Ded2, 3);
                            vch.Ded3 = Math.Round(weigh.Ded3, 3);
                            vch.Rt1 = weigh.Godownid;
                            vch.LabDed = weigh.LabDedStock;
                            vch.LabDedP = weigh.LabDedParty;
                            vch.Labdeds = weigh.LabDedStock;
                            vch.BagsDed = weigh.BagsDed;
                            vch.Swm = weigh.manualWeight;

                            report = "SecondWeight";
                        }
                    }



              
                    foreach (var lb in weigh.lab)
                    {
                        var labResult = _context.TblLabResults.FirstOrDefault(l => l.Id == lb.Id);
                        if (labResult != null)
                        {
                            labResult.PartyDedKg = lb.PartyDedKg;
                            labResult.StockDedKg = lb.StockDedKg;
                            _context.TblLabResults.Update(labResult);
                        }

                    }

                  


                }

                _context.SaveChanges();
                if (weigh.Type != "FirstWeight")
                {
                    _dataLogic.MakePurchasePayable(firstRow.VchType, firstRow.VchNo, true);
                }
                    
                transaction.Commit();
                return new
                {
                    report = report,
                    VchNo = weigh.VchNo,
                    FromDate = vches[0].VchDate,
                    Vehicle = vches[0].VehicleNo,
                    GpNo = vches[0].Gpno
                };
            }
            catch (Exception)
            {
                transaction.Rollback();
                return false;
            }
        }


        #endregion

        #region GATE PASS OUTWARD


        // Gate Pass Outward

        public DataTable GetAllowedWtDiff()
        {
            string qry = @"Select OutLimitOn from Company where cmp_id = '"+auth.CmpId+"'";

            return _dataLogic.LoadData(qry);
        }

        public DataTable GetFirstWeightOutward()
        {
            string qry = @"Select T.sno,  T.VchNo ArrivalNo, T.VchNo, T.VchType, T.VehicleNo,FirstWeight, FORMAT(VchDate, 'dd-MM-yyyy') AS VchDate  from tblTransVch T
                        INNER JOIN TransMain TM ON TM.VchType = T.VchType and TM.VchNo = T.VchNo  and TM.Cmp_Id = T.Cmp_Id and T.LocID = TM.LocId and T.FinID = TM.FinId
                        where T.VchType = 'SP' AND T.Cmp_id = '" + auth.CmpId + "' and T.locid = '" + auth.LocId + "'  and isnull(TM.GPApprove, 0) = 1 and T.FinID = '" + auth.FinId + "' " +
                        "and T.sno = 1 and isnull(T.FirstWeight,0) = 0 and isnull(T.SecWeight,0) = 0 and isnull(T.Reject,0) = 0";

            return _dataLogic.LoadData(qry);
        }

        public DataTable GetFirstWeightOutwardDetail(int VchNo)
        {
            string qry = @"Select sno, T.VchNo, VehicleNo, GPNO, T.PONO, i.Names as ItemName, L4.Names AS MainItem, L41.Names as PartyMain, p.Names as PartyName, T.Bags, T.Rate, T.Qty,T.BagsType, T.Freight, T.FreightType, 
                T.BilltyNo, isnull(T.ExpWt,0) as ExpWt, T.ArrivalNo, T.Gross, isnull(FirstWeight,0) as FirstWeight, isnull(SecWeight,0) as SecWeight, WType, MiniWt, ExpWt, T.Tare, G.GodownId, G.GodownName, T.Cmb1, T.Cmb2, T.Cmb3,
                FORMAT(VchDate, 'yyyy-MM-dd') AS VchDate , ISNULL( T.SBags,0)  SBAGS  , ISNULL( T.SQTY,0)  SQTY  from tblTransVch T
                LEFT OUTER JOIN TransMain TM ON TM.VchType = T.VchType and TM.VchNo = T.VchNo and T.Cmp_id = TM.Cmp_id and T.LocId = TM.LocId and T.FinID = TM.FinId
                LEFT OUTER JOIN Level5 i on i.Level4+i.Level5 = T.DMCode+Code and T.Cmp_id = i.comp_id  
                LEFT OUTER JOIN Level5 p on p.Level4+p.Level5 = T.Mcode and T.Cmp_id = p.comp_id 
                Left Join Level4 L4 ON L4.Level3+L4.Level4 = T.DMCODE and T.Cmp_id = L4.comp_id 
                Left Join Level4 L41 ON L41.Level3+L41.Level4 = SUBSTRING(T.Mcode, 1, 9) and T.Cmp_id = L4.comp_id
                LEFT OUTER JOIN TBLGODOWNS G On G.GODOWNID = T.GodownId and T.Cmp_id = G.comp_id and T.LocId = G.LocId
                where T.VchType = 'SP' AND T.Cmp_id = '" + auth.CmpId + "' and T.locid = '" + auth.LocId + "' and T.VchNo = '"+VchNo+"' and isnull(T.FirstWeight,0) = 0 and  isnull(T.SecWeight,0) = 0 and isnull(TM.GPApprove, 0) = 1 and T.Finid = '"+auth.FinId+"' and isnull(T.Reject,0) = 0";


            return _dataLogic.LoadData(qry);
        }

        public DataTable GetSecondWeightOutward()
        {
            string qry = @"Select T.sno,  T.VchNo ArrivalNo, T.VchNo, T.VchType, T.VehicleNo,FirstWeight, FORMAT(VchDate, 'dd-MM-yyyy') AS VchDate  from tblTransVch T
                        INNER JOIN TransMain TM ON TM.VchType = T.VchType and TM.VchNo = T.VchNo  and TM.Cmp_Id = T.Cmp_Id and T.LocID = TM.LocId and T.FinID = TM.FinId
                        where T.VchType = 'SP'  AND T.Cmp_id = '" + auth.CmpId + "' and T.locid = '" + auth.LocId + "'  and isnull(TM.GPApprove, 0) = 1 and T.FinID = '" + auth.FinId + "' and T.sno = 1 and isnull(T.FirstWeight,0) > 0 and isnull(T.SecWeight,0) = 0 and isnull(T.Reject,0) = 0";

            return _dataLogic.LoadData(qry);

        }

        public DataTable GetSecondWeightOutwardDetail(int VchNo)
        {
            string qry = @"Select sno, T.VchNo, VehicleNo, GPNO, T.PONO, i.Names as ItemName, L4.Names AS MainItem, L41.Names as PartyMain, p.Names as PartyName, T.Bags, T.Rate, T.Qty,T.BagsType, T.Freight, T.FreightType, 
                T.BilltyNo, isnull(T.ExpWt,0) as ExpWt, T.ArrivalNo, T.Gross, isnull(FirstWeight,0) as FirstWeight, isnull(SecWeight,0) as SecWeight, WType, MiniWt, ExpWt, T.Tare, G.GodownId, G.GodownName, T.Cmb1, T.Cmb2, T.Cmb3,
                FORMAT(VchDate, 'yyyy-MM-dd') AS VchDate , ISNULL( T.SBags,0)  SBAGS  , ISNULL( T.SQTY,0)  SQTY  from tblTransVch T
                LEFT OUTER JOIN TransMain TM ON TM.VchType = T.VchType and TM.VchNo = T.VchNo and T.Cmp_id = TM.Cmp_id and T.LocId = TM.LocId and T.FinID = TM.FinId
                LEFT OUTER JOIN Level5 i on i.Level4+i.Level5 = T.DMCode+Code and T.Cmp_id = i.comp_id  
                LEFT OUTER JOIN Level5 p on p.Level4+p.Level5 = T.Mcode and T.Cmp_id = p.comp_id 
                Left Join Level4 L4 ON L4.Level3+L4.Level4 = T.DMCODE and T.Cmp_id = L4.comp_id 
                Left Join Level4 L41 ON L41.Level3+L41.Level4 = SUBSTRING(T.Mcode, 1, 9) and T.Cmp_id = L4.comp_id
                LEFT OUTER JOIN TBLGODOWNS G On G.GODOWNID = T.GodownId and T.Cmp_id = G.comp_id and T.LocId = G.LocId
                where T.VchType = 'SP' AND T.Cmp_id = '" + auth.CmpId + "' and T.locid = '" + auth.LocId + "' and T.VchNo = '" + VchNo + "' and isnull(T.FirstWeight,0) > 0 and  isnull(T.SecWeight,0) = 0 and isnull(TM.GPApprove, 0) = 1 and T.Finid = '" + auth.FinId + "' and isnull(T.Reject,0) = 0";

            return _dataLogic.LoadData(qry);
        }

        public bool SaveOutWardWeighment(WeighBridgeVM weigh)
        {
            using var transaction = _context.Database.BeginTransaction();
            try
            {
                DateTime currentDate = DateTime.Now;

                var vches = _context.TblTransVches.Where(x => x.CmpId == auth.CmpId && x.FinId == auth.FinId && x.VchType ==  "SP" && x.LocId == auth.LocId && x.VchNo == weigh.VchNo).ToList();
                var mains = _context.TransMains.Where(x => x.CmpId == auth.CmpId && x.FinId == auth.FinId && x.VchType == "SP" && x.LocId == auth.LocId && x.VchNo == weigh.VchNo).FirstOrDefault();

                if (vches != null && vches.Any())
                {
                    if (weigh.Type == "FirstWeight")
                    {

                        mains.Firstwtby = auth.UserId;
                        _context.TransMains.Update(mains);

                    }
                    else
                    {
                        mains.Secwtby = auth.UserId;
                        _context.TransMains.Update(mains);

                    }
                    foreach (var vch in vches)
                    {
                        if (weigh.Type == "FirstWeight")
                        {

                            vch.FirstWeight = weigh.FirstWeight;
                            vch.TimeIn = weigh.TimeIn;
                            vch.DateIn = currentDate;
                            vch.Fwm = true;
                            _context.TblTransVches.Update(vch);

                        }
                        else
                        {
                            vch.SecWeight = weigh.SecondWeight;
                            vch.TimeOut = weigh.TimeOut;
                            vch.DateOut = currentDate;
                            //vch.VchDate = weigh.vchDate;

                        }
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

        #endregion

        // Sale Status - Outward Status

        public DataTable GetOutwardStatus()
        {
            string qry = @"Select VchNo, FORMAT(VchDate, 'yyyy-MM-dd') AS VchDate, ArrivalNo, GPNO, VehicleNo, Qty, TimeIn, TimeOut, FirstWeight, 
                    SecWeight, l.Names as Party, I.Names as Item, T.Freight
                    from tbltransVch T
                    Left Join Level5 l ON l.Level4+l.Level5 = T.Mcode and T.Cmp_Id = l.Comp_Id
                    Left Join Level5 I ON I.Level4+I.Level5 = T.DMCode+Code and T.Cmp_Id = I.Comp_Id
                    where T.VchType = 'SP' and T.Cmp_Id = '" + auth.CmpId + "' and T.LociD = '" + auth.LocId + "' AND T.FinID = '" + auth.FinId + "'";

            return _dataLogic.LoadData(qry);
        }


        public DataTable GetOutwardbyDate(DateTime fromDate, DateTime toDate)
        {
            //string qry = @"Select VchNo, FORMAT(VchDate, 'yyyy-MM-dd') AS VchDate, ArrivalNo, GPNO, VehicleNo, Qty, TimeIn, TimeOut, FirstWeight,
            //        SecWeight, l.Names as Party, I.Names as Item, T.Freight
            //        from tbltransVch T
            //        Left Join Level5 l ON l.Level4+l.Level5 = T.Mcode and T.Cmp_Id = l.Comp_Id
            //        Left Join Level5 I ON I.Level4+I.Level5 = T.DMCode+Code and T.Cmp_Id = I.Comp_Id
            //        where T.VchType = 'SP' and T.Cmp_Id = '" + auth.CmpId + "' and T.LociD = '" + auth.LocId + "' AND T.FinID = '" + auth.FinId + "' And T.VchDate Between '" + fromDate.ToString("yyyy/MM/dd") + "' and '" + toDate.ToString("yyyy/MM/dd") + "'";

            string qry = @$"EXEC [dbo].[WBFinishedGoodsStatus] 
                            @FromDate = '{fromDate.ToString("yyyy/MM/dd")}', 
                            @ToDate = '{toDate.ToString("yyyy/MM/dd")}', 
                            @LocId = '{auth.LocId}', 
                            @finid = '{auth.FinId}', 
                            @cmpid = {auth.CmpId};";

            return _dataLogic.LoadData(qry);
        }

        public DataTable GetWBSettings()
        {
            string qry = @"Select * from TblWBSettings where Cmpid = '" + auth.CmpId + "' and LocId = '" + auth.LocId + "'";

            return _dataLogic.LoadData(qry);
        }
    }
}

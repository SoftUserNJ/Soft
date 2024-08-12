using Microsoft.EntityFrameworkCore;
using SoftaxeERP_API.Models;
using SoftaxeERP_API.VM;
using System.Data;
using System.Drawing.Drawing2D;

namespace SoftaxeERP_API.Services
{
    public interface ILab
    {
        DataTable GetTestTypes();

        DataTable GetArrivalList();


        DataTable GetLabNo();

        DataTable GetFirstLabList();

        bool SaveLabResult(List<LabVM> Lab);

        bool SaveLabFirstSample(TblLabResult Lab);
        DataTable GetLabsResultList(DateTime fromDate, DateTime toDate);

        DataTable GetEditLab(int LabTestNo);

        bool DelLab(int LabTestNo);

        DataTable GetLabTestTypeList();

        bool AddUpdateLabTestType(int LabTestNo, string LabTestName, string TestUom);

        string DeleteLabTestType(int LabTestNo);
    }

    public class Lab : ILab
    {
        private readonly ErpSoftaxeContext _context;
        private readonly IDataLogic _dataLogic;
        private readonly IAuth _auth;

        readonly AuthVM auth = new();

        public Lab(ErpSoftaxeContext context, IDataLogic dataLogic, IAuth authData)
        {
            _context = context;
            _dataLogic = dataLogic;
            _auth = authData;

            auth = _auth.GetUserData();
        }


        public DataTable GetTestTypes()
        {
            string qry = @"SELECT LabTestNo, LabTestName  FROM TblLabTestTypes WHERE comp_id = '" + auth.CmpId + "' and  LocId = '" + auth.LocId + "'";

            return _dataLogic.LoadData(qry);
        }


        public DataTable GetArrivalList()
        {
                    string qry = @"
                         Select  distinct VchNo, T.Vchtype , T.Vehicleno , P.Names as PartyName,  Dmcode, Code, l5.NAMES , sum(Sqty) qty,sum(T.Sbags) Bags from tblTransVch T
                        Inner Join Level5 l5 On l5.Level4+Level5 = T.Dmcode+Code AND l5.comp_id = T.Cmp_Id 
                        Inner Join Level5 P On P.Level4+p.Level5 = T.Mcode AND P.comp_id = T.Cmp_Id 
                        Left outer join  TblLabResults L On L.ArrivalNo = T.VchNo AND L.comp_id = T.Cmp_Id 
                         where T.VchType = 'RP-RAW' and T.Cmp_Id = '" + auth.CmpId + "' and  T.LocId= '" + auth.LocId + "' AND T.Finid = '" + auth.FinId + "' And T.Sno = 1 and T.ExpWt = 0 AND ISNULL(SECWEIGHT,0)=0  and isnull(L.ArrivalNo,0) <> T.VchNo Group by VchNo, T.Vchtype, T.Vehicleno, Dmcode, Code, l5.NAMES,  P.Names";

            return _dataLogic.LoadData(qry);
        }

        public DataTable GetLabNo()
        {
            string qry = @"Select isnull(Max(LabTestNo),0)+1 as LabNo from TblLabResults where Comp_id = '" + auth.CmpId + "' and LocId = '" + auth.LocId + "'";

            return _dataLogic.LoadData(qry);
        }


        public DataTable GetFirstLabList()
        {
            string qry = @" select L.VchType, LabTestNo as LabNo, Format(ResDate, 'yyyy-MM-dd') as LabDate,
                            ArrivalNo, VehicleNo as VehicleNo from tbllabresults L
                            Left Join TransMain M ON M.VchNo = L.ArrivalNo  and M.Cmp_Id = L.Comp_id and L.LocId = M.LocId
                             where L.Comp_id = '" + auth.CmpId+ "' and M.VchType = 'RP-RAW' AND L.LocId = '" + auth.LocId+"' and isnull(L.Sno,0) = 1 and isnull(M.GpApprove,0) = 0";

            return _dataLogic.LoadData(qry);
        }



        public bool SaveLabResult(List<LabVM> Lab)
        {
            LabVM lb = Lab.First();
            using var transaction = _context.Database.BeginTransaction();

            try
            {
                //if (lb.LabTestNo == 0)
                //{
                //    lb.LabTestNo = (_context.TblLabResults
                //        .Where(x => x.VchType == "LB" && x.CompId == auth.CmpId && x.Locid == auth.LocId)
                //        .Max(x => (int?)x.LabTestNo) ?? 0) + 1;
                //}

                _context.TblLabResults
                    .Where(x => x.VchType == "LB" && x.CompId == auth.CmpId && x.Locid == auth.LocId && x.LabTestNo == lb.LabTestNo)
                    .ExecuteDelete();


                foreach (var item in Lab)
                {
                    _context.TblLabResults.Add(new TblLabResult
                    {
                        Sno = item.Sno,
                        VchType = "LB",
                        LabTestNo = lb.LabTestNo,
                        ResDate = lb.ResDate,
                        CompId = auth.CmpId,
                        Locid = auth.LocId,
                        Bags = item.Bags,
                        BagsIn = item.BagsIn,
                        ArrivalNo = lb.ArrivalNo,
                        VehicleNo = lb.VehicleNo,
                        Test1 = lb.Test1,
                        Test2 = lb.Test2,
                        Test3 = lb.Test3,
                        Remarks = lb.Remarks,
                        VisAcc = lb.VisAcc,
                        VisRej = lb.VisRej,
                        LabTestName = item.LabTestName,
                        Percentage = item.Percentage,
                        Uom = item.Uom,
                        PartyDed = item.PartyDed,
                        StockDed = item.PartyDed,
                        PartyDedKg = item.PartyDedKg,
                        StockDedKg = item.StockDedKg,
                        Finid = auth.FinId,
                        Uid1 = auth.UserId.ToString(),
                        Uid = lb.Uid


                    });


                }

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


        public bool SaveLabFirstSample(TblLabResult Lab)
        {
            using var transaction = _context.Database.BeginTransaction();

            try
            {
                if (Lab.LabTestNo == 0)
                {
                    Lab.LabTestNo = (_context.TblLabResults
                        .Where(x => x.VchType == "LB" && x.CompId == auth.CmpId && x.Locid == auth.LocId)
                        .Max(x => (int?)x.LabTestNo) ?? 0) + 1;
                }

                _context.TblLabResults
                    .Where(x => x.VchType == "LB" && x.CompId == auth.CmpId && x.Locid == auth.LocId && x.LabTestNo == Lab.LabTestNo)
                    .ExecuteDelete();



                _context.TblLabResults.Add(new TblLabResult
                {
                    VchType = "LB",
                    Sno=1,
                    LabTestNo = Lab.LabTestNo,
                    ResDate = Lab.ResDate,
                    CompId = auth.CmpId,
                    Locid = auth.LocId,
                    BagsIn = Lab.BagsIn,
                    ArrivalNo = Lab.ArrivalNo,
                    VehicleNo = Lab.VehicleNo,
                    Test1 = Lab.Test1,
                    Test2 = Lab.Test2,
                    Test3 = Lab.Test3,
                    Remarks = Lab.Remarks,
                    VisAcc = Lab.VisAcc,
                    VisRej = Lab.VisRej,
                    Finid = auth.FinId,
                    Uid = auth.UserId.ToString()

                });

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

        //public DataTable GetLabsList(DateTime fromDate, DateTime toDate)
        //{
        //    String qry = $@"SELECT DISTINCT LabTestNo, VehicleNo, CONVERT(VARCHAR(10),ResDate,103) AS ResultDate 
        //                    FROM TblLabResults  WHERE Comp_id = '{auth.CmpId}' AND LocId = '{auth.LocId}' And ResDate BETWEEN '{fromDate.ToString("yyyy-MM-dd")}'
        //                    AND '{toDate.ToString("yyyy-MM-dd")}' AND VCHTYPE = 'LB'";
        //    return _dataLogic.LoadData(qry);
        //}


        public DataTable GetLabsResultList(DateTime fromDate, DateTime toDate)
        {
            String qry = $@" Select Distinct P.Names as Party, LabTestNo, L.ArrivalNo, L.VehicleNo, L5.Names as Item, CONVERT(VARCHAR(10),ResDate,103) AS ResultDate from tblLabResults L
                            Left Outer Join TblTransVch T ON T.VchNo = L.ArrivalNo AND  T.VCHTYPE='RP-RAW' AND T.FINID=L.FINID AND T.LOCID=L.LOCID AND T.Cmp_id=L.Comp_id
                            Inner Join Level5 l5 On l5.Level4+Level5 = T.Dmcode+Code AND l5.comp_id = T.Cmp_Id 
                            Inner Join Level5 P On P.Level4+P.Level5 = T.Mcode AND P.comp_id = T.Cmp_Id 
                            WHERE L.Comp_id = '{auth.CmpId}' AND L.LocId = '{auth.LocId}' And ResDate BETWEEN '{fromDate.ToString("yyyy-MM-dd")}' AND '{toDate.ToString("yyyy-MM-dd")}' AND L.VCHTYPE = 'LB'
                            AND T.Sno = 1 order by LabTestNo";
            return _dataLogic.LoadData(qry);
        }

        public DataTable GetEditLab(int LabTestNo)
        {
            String qry = $@"Select VchType, ArrivalNo, ResDate, LabTestName, UID, Sno, Bags, BagsIn, LabTestNo, PartyDed, StockDed,
                            PartyDedKg, StockDedKg, VehicleNo, Percentage, UOM,sno, SampleNo, SampleDecAs, Remarks, Test1, Test2, Test3, VisAcc, VisRej  
                            from TblLabResults where Comp_id = '" + auth.CmpId + "' and locid = '" + auth.LocId + "' and LabTestNo = '" + LabTestNo + "'";
            return _dataLogic.LoadData(qry);
        }


        public bool DelLab(int LabTestNo)
        {
            using var transaction = _context.Database.BeginTransaction();
            try
            {
                _context.TblLabResults.Where(x => x.VchType == "LB" && x.LabTestNo == LabTestNo && x.CompId == auth.CmpId && x.Locid == auth.LocId).ExecuteDelete();

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



        #region Lab Test Type Entry

        public DataTable GetLabTestTypeList()
        {
            String qry = @"SELECT LabTestNo, LabTestName, TestUOM  FROM TblLabTestTypes where Comp_Id = '" + auth.CmpId + "' and LocId = '" + auth.LocId + "'";
            return _dataLogic.LoadData(qry);
        }

        public bool AddUpdateLabTestType(int LabTestNo, string LabTestName, string TestUom)
        {
            using var transaction = _context.Database.BeginTransaction();
            try
            {
                var lb = _context.TblLabTestTypes.Where(x => x.LabTestNo == LabTestNo && x.CompId == auth.CmpId && x.LocId == auth.LocId).FirstOrDefault();

                if (lb == null)
                {
                    LabTestNo = (_context.TblLabTestTypes
                   .Where(x => x.CompId == auth.CmpId && x.LocId == auth.LocId)
                      .Max(x => (int?)x.LabTestNo) ?? 0) + 1;


                    _context.TblLabTestTypes.Add(new TblLabTestType
                    {
                        LabTestNo = LabTestNo,
                        LabTestName = LabTestName,
                        TestUom = TestUom,
                        CompId = auth.CmpId,
                        LocId = auth.LocId
                    });
                }
                else
                {
                    lb.LabTestName = LabTestName;
                    lb.TestUom = TestUom;
                    _context.TblLabTestTypes.Update(lb);
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
        public string DeleteLabTestType(int LabTestNo)
        {
            using var transaction = _context.Database.BeginTransaction();

            bool idCheck =
                        _context.TblLabResults.Any(x => x.LabTestNo.Equals(LabTestNo));

            if (idCheck)
            {
                return "Already In Use";
            }

            try
            {
                var sd = _context.TblLabTestTypes.Where(x => x.LabTestNo == LabTestNo).FirstOrDefault();
                _context.TblLabTestTypes.Remove(sd);
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
    }
}


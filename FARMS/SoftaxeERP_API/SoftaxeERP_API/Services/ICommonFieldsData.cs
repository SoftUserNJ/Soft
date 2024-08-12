using DevExpress.XtraEditors.Filtering;
using DevExpress.XtraRichEdit.Import.Html;
using DevExpress.XtraRichEdit.Model;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using SoftaxeERP_API.Models;
using SoftaxeERP_API.VM;
using System.ComponentModel.Design;
using System.Data;
using System.Security.Cryptography;

namespace SoftaxeERP_API.Services
{
    public interface ICommonFieldsData
    {

        DataTable GetMaxNo(string vchNoColumnName, string vchType, string tableName);
        DataTable GetDirectL4L5CodeNamesByL4Tag(string l4Tag);

        #region LEVEL4
        DataTable GetLevel4();
        DataTable GetLevel4CodeNameByTag(string tag);
        #endregion

        #region LEVEL5
        DataTable GetLevel5CodeNameByL4Code(string code);
        #endregion

        #region UOM
        DataTable GetUOM();
        #endregion

        #region CropYear
        DataTable CropYear();
        #endregion

        #region Location
        DataTable Location();
        DataTable LocationWithLoc();

        #endregion

        #region Vch Types

        DataTable GetTypes();

        #endregion

        #region Godowns
        DataTable Godowns();

        #endregion

        #region Sub Party
        DataTable GetSubParty(string code);

        #endregion

        #region PO Details By Party And Items

        DataTable GetPoDetailsByPartyAndItems(string party, string item ,DateTime TransDate , int Vchno , int Pono);

        DataTable GetPurchaseBagsType();

        #endregion

        #region Product Location
        DataTable GetProductLocation();
        #endregion

        DataTable GetCostCenter();
    }

    public class CommonFieldsData : ICommonFieldsData
    {

        private readonly ErpSoftaxeContext _context;
        private readonly IDataLogic _dataLogic;
        private readonly IAuth _auth;

        readonly AuthVM auth = new();
        public CommonFieldsData(ErpSoftaxeContext context, IDataLogic dataLogic, IAuth authData)
        {
            _context = context;
            _dataLogic = dataLogic;
            _auth = authData;

            auth = _auth.GetUserData();
        }

        // GetMaxNo(string vchNoColumnName, string vchType, string tableName)
        public DataTable GetMaxNo(string vchNoColumnName, string vchType, string tableName)
        {
            String qry = $@"SELECT ISNULL(MAX({vchNoColumnName}),0)+1 AS VCHNO FROM {tableName} WHERE VCHTYPE = '{vchType}' AND CMPID = '{auth.CmpId}'";
            return _dataLogic.LoadData(qry);
        }

        public DataTable GetDirectL4L5CodeNamesByL4Tag(string l4Tag)
        {
            String qry = $@"SELECT L5.LEVEL4+L5.LEVEL5 AS L5CODE, L5.NAMES AS L5NAMES, L4.LEVEL3+L4.LEVEL4 AS L4CODE, L4.NAMES AS L4NAMES, * 
                            FROM LEVEL5 L5
                            INNER JOIN LEVEL4 L4 ON L5.LEVEL4 = L4.LEVEL3+L4.LEVEL4 AND L4.COMP_ID = L5.COMP_ID
                            WHERE L5.COMP_ID = '{auth.CmpId}' AND L4.TAG1 = '{l4Tag}' Order By L5.Level4, L5.Level5, L5.Names";
            return _dataLogic.LoadData(qry);
        }


        public DataTable GetSubParty(string code)
        {
            String qry = $@"SELECT * , (DMCODE+CODE) SubPartyCode FROM TBLSUBPARTY WHERE CMPID  = '{auth.CmpId}' AND  DMCODE+CODE = '{code}'";
            return _dataLogic.LoadData(qry);
        }

        #region LEVEL4

        public DataTable GetLevel4()
        {
            String qry = $@"SELECT LEVEL3+LEVEL4 AS CODE, NAMES AS NAME, * FROM LEVEL4 
                            WHERE COMP_ID = '{auth.CmpId}' AND LOCID = '{auth.LocId}'";
            return _dataLogic.LoadData(qry);
        }

        public DataTable GetLevel4CodeNameByTag(string tag)
        {
            String qry = $@"SELECT LEVEL3+LEVEL4 AS CODE, NAMES AS NAME FROM LEVEL4 
                            WHERE TAG1 = '{tag}' AND COMP_ID = '{auth.CmpId}' Order By Level4";
            return _dataLogic.LoadData(qry);
        }


        #endregion

        #region LEVEL5

        public DataTable GetLevel5CodeNameByL4Code(string code)
        {
            String qry = $@"SELECT L5.LEVEL4+L5.LEVEL5 AS CODE, L5.NAMES AS NAME, L5.*  FROM LEVEL5 L5
                            WHERE L5.COMP_ID = '{auth.CmpId}' AND L5.LEVEL4 = '{code}' ORDER BY L5.NAMES";
            return _dataLogic.LoadData(qry);
        }


        #endregion

        #region UOM

        public DataTable GetUOM()
        {
            String qry = $@"SELECT ID, UOM, DIVUOM FROM TBLUOM WHERE COMP_ID = '{auth.CmpId}'";
            return _dataLogic.LoadData(qry);
        }

        #endregion

        #region CropYear
        public DataTable CropYear()
        {
            String qry = $@"SELECT * FROM TBLCROPYEAR";
            return _dataLogic.LoadData(qry);
        }
        #endregion

        #region Location

        public DataTable Location()
        {
            String qry = $@"SELECT * FROM LOCATION WHERE CMP_ID = '{auth.CmpId}'";
            return _dataLogic.LoadData(qry);
        }

        public DataTable LocationWithLoc()
        {
            String qry = $@"SELECT * FROM LOCATION WHERE CMP_ID = '{auth.CmpId}' AND LOCID <> 'HO'";
            return _dataLogic.LoadData(qry);
        }

        #endregion

        #region Vch Types

        public DataTable GetTypes()
        {
            String qry = $@"SELECT * FROM TBLVCHTYPES";
            return _dataLogic.LoadData(qry);
        }

        #endregion

        #region Godowns

        public DataTable Godowns()
        {
            String qry = $@"SELECT * FROM TBLGODOWNS WHERE COMP_ID = '{auth.CmpId}' AND LOCID = '{auth.LocId}'";
            return _dataLogic.LoadData(qry);
        }

        #endregion

        #region PO Details By Party And Items

        public DataTable GetPoDetailsByPartyAndItems(string party, string item , DateTime TransDate , int Vchno , int Pono)
        {
            //string qry = @$"select  Isnull(P.PONO,0) PoNo , sum(ISNULL(P.Qty,0)) - isnull((select SUM(ISNULL(T.PAYABLEWT1,0)) from tbltransvch T where T.VCHTYPE='RP-RAW'  and  T.Mcode=P.PCode+P.PSubCode AND T.DMCode+T.Code=P.ICODE+P.ISUBCODE  and t.pono=p.pono  and tucks=8    
            //                AND CONVERT(varchar(11),VchDate,111) +'-'+ LTRIM(STR(VCHNO))  <> 0) ,0)  BalQty, CONVERT(varchar(11),PODATE,103) PODATE  , CONVERT(varchar(11),PoCompDate,103)  ExpiryDate ,l5p.Names Party , l5.Names Product ,sum(P.Qty) PoQty , p.FreightType  
            //                from TblPurchaseContractDetail P
            //                inner join Level5 L5P on L5P.Level4+L5P.Level5 = p.PCode+p.PSubCode and l5p.comp_id = '{auth.CmpId}' 
            //                inner join Level5 L5 on L5.Level4+L5.Level5 = p.ICode+p.ISubCode and l5.comp_id = '{auth.CmpId}' 
            //                where p.PCode+p.PSubCode  = '{party}' and p.ICode+p.ISubCode ='{item}'
            //                And ISNULL(P.APROVE,0)>0 And ISNULL(P.CANCEL,0)=0   and p.cmpid = '{auth.CmpId}' 
            //                Group by  p.PoNo,p.PCode,p.PSubCode,p.ICode,p.ISubCode   ,PODATE,PoCompDate , l5p.Names  , l5.Names   ,p.FreightType,p.NoOfVehicles
            //                HAVING
            //                (  CASE WHEN    ISNULL(P.NoOfVehicles, 0) > 0  THEN P.NoOfVehicles - ISNULL((SELECT COUNT(T.PAYABLEWT1) from  tbltransvch T where T.VCHTYPE='RP-RAW' and  T.Mcode=P.PCode+P.PSubCode AND T.DMCode+T.Code=P.ICODE+P.ISUBCODE  and t.pono=p.pono  and tucks=8 AND CONVERT(varchar(11),VchDate,111) +'-'+ LTRIM(STR(VCHNO))  <> 0),0)
            //                ELSE
            //                sum(ISNULL(P.Qty,0)) - isnull((select SUM(ISNULL(T.PAYABLEWT1,0)) from tbltransvch T where T.VCHTYPE='RP-RAW' and  T.Mcode=P.PCode+P.PSubCode AND T.DMCode+T.Code=P.ICODE+P.ISUBCODE  and t.pono=p.pono  and tucks=8 AND CONVERT(varchar(11),VchDate,111) +'-'+ LTRIM(STR(VCHNO))  <> 0 ) ,0)
            //                END ) >0   
            //                AND   sum(ISNULL(P.Qty,0)) - isnull((select SUM(ISNULL(T.PAYABLEWT1,0))  from tbltransvch T where T.VCHTYPE='RP-RAW' 
            //                and  T.Mcode=P.PCode+P.PSubCode AND T.DMCode+T.Code=P.ICODE+P.ISUBCODE  and t.pono=p.pono  and tucks=8 
            //                AND CONVERT(varchar(11),VchDate,111) +'-'+ LTRIM(STR(VCHNO))  <> 0 ) ,0)  >0
            //                ORDER BY P.PODATE , P.PONO";


            string pofilter = "" ;
            if (Pono>0)
            {
                pofilter = $"AND P.PONO = '{Pono}'";
            }


         
         



           
            string qry = @$"select Isnull(p.rate,0) Rate,  isnull(Bcode + BsubCode, '') BrokerCode ,  isnull(ItemUOM,'') ItemUOM ,  isnull(BrokerComm,0) BrokerComm , isnull(BrokerCommUom,'') BrokerCommUom , isnull(P.SaleTax,0) SaleTax  , isnull(IncomeTax, 0) IncomeTax,      Isnull(P.PONO,0) PoNo , sum(ISNULL(P.Qty,0)) - isnull((select SUM( (CASE WHEN ISNULL(PAYABLEWT1,0)=0 THEN      ISNULL(T.SQTY,0) ELSE   ISNULL(PAYABLEWT1,0) END )   ) from tbltransvch T where T.VCHTYPE='RP-RAW'  and  T.Mcode=P.PCODE+P.PSUBCODE AND T.DMCode+T.Code=P.ICODE+P.ISUBCODE  and t.pono=p.pono  and t.id<>'{Vchno}'    ) ,0)  BalQty
                            , CONVERT(varchar(11),PODATE,103) PODATE  , CONVERT(varchar(11),PoCompDate,103)  ExpiryDate ,l5p.Names Party , l5.Names Product ,sum(P.Qty) PoQty 
                            , p.FreightType  
                            from TblPurchaseContractDetail P
                            inner join Level5 L5P on L5P.Level4+L5P.Level5 = p.PCode+p.PSubCode and l5p.comp_id = '{auth.CmpId}' 
                            inner join Level5 L5 on L5.Level4+L5.Level5 = p.ICode+p.ISubCode and l5.comp_id = '{auth.CmpId}' 
                            where p.PCode+p.PSubCode  = '{party}' and p.ICode+p.ISubCode ='{item}' 
                            And ISNULL(P.APROVE,0)>0 And ISNULL(P.CANCEL,0)=0   and p.cmpid = '{auth.CmpId}' AND   CONVERT(VARCHAR(11),PoCompDate, 111)  >= CONVERT(VARCHAR(11), '{TransDate.ToString("yyyy/MM/dd")}', 111)    {pofilter}
                            Group by  p.PoNo,p.PCode,p.PSubCode,p.ICode,p.ISubCode   ,PODATE,PoCompDate , l5p.Names  , l5.Names   ,p.FreightType,p.NoOfVehicles , Isnull(p.rate,0) ,  isnull(Bcode + BsubCode, '') , isnull(ItemUOM,'')  ,  isnull(BrokerComm,0)  , isnull(BrokerCommUom,'')  , isnull(P.SaleTax,0)   , isnull(IncomeTax, 0) 
                            HAVING
                            (  CASE WHEN    ISNULL(P.NoOfVehicles, 0) > 0  THEN P.NoOfVehicles - ISNULL((SELECT COUNT(T.PAYABLEWT1) from  tbltransvch T where T.VCHTYPE='RP-RAW' and  T.Mcode=P.PCode+P.PSubCode AND T.DMCode+T.Code=P.ICODE+P.ISUBCODE  and t.pono=p.pono  and tucks=8 ),0)
                            ELSE
                            sum(ISNULL(P.Qty,0)) - isnull((select SUM( (CASE WHEN ISNULL(PAYABLEWT1,0)=0 THEN      ISNULL(T.SQTY,0) ELSE   ISNULL(PAYABLEWT1,0) END ) ) from tbltransvch T where T.VCHTYPE='RP-RAW' and  T.Mcode=P.PCode+P.PSubCode AND T.DMCode+T.Code=P.ICODE+P.ISUBCODE  and t.pono=p.pono  and tucks=8 ) ,0)
                            END ) >0   
                            AND   sum(ISNULL(P.Qty,0)) - isnull((select SUM((CASE WHEN ISNULL(PAYABLEWT1,0)=0 THEN      ISNULL(T.SQTY,0) ELSE   ISNULL(PAYABLEWT1,0) END ) )  from tbltransvch T where T.VCHTYPE='RP-RAW' 
                            and  T.Mcode=P.PCode+P.PSubCode AND T.DMCode+T.Code=P.ICODE+P.ISUBCODE  and t.pono=p.pono  and tucks=8  and t.id<>'{Vchno}' 
                            ) ,0)  >0
                            ORDER BY P.PODATE , P.PONO";


            return _dataLogic.LoadData(qry);
        }

        public DataTable GetPurchaseBagsType()
        {
            string qry = @"SELECT L5.Level4 + L5.Level5 AS Code, L5.Names  ,ISNULL(Rate,0) Rate
                            FROM Level5 L5 Left Outer Join Level4 L4  On L5.Level4=L4.Level3+L4.Level4 
                            WHERE L4.Tag1 = 'B'";
            return _dataLogic.LoadData(qry);
        }

        #endregion

        #region Product Location
        public DataTable GetProductLocation()
        {
            String qry = $@"SELECT * FROM TBLSHELFS WHERE COMP_ID = {auth.CmpId}";
            return _dataLogic.LoadData(qry);
        }
        #endregion


        public DataTable GetCostCenter()
        {
            String qry = $@"Select CostcentreId, CostcentreName from TblCostCentre where LocId = '"+auth.LocId+"' and CmpId = '"+auth.CmpId+"'";
            return _dataLogic.LoadData(qry);
        }
    }
}

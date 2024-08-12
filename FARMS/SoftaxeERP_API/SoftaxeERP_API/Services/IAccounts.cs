using SoftaxeERP_API.Models;
using SoftaxeERP_API.VM;
using System.Data;

namespace SoftaxeERP_API.Services
{
    public interface IAccounts
    {
        DataTable GetVchFec(DateTime vchDate, DateTime fromDate, DateTime toDate, string tag);

    }
    public class Accounts : IAccounts
    {
        private readonly ErpSoftaxeContext _context;
        private readonly IDataLogic _dataLogic;
        private readonly IAuth _auth;

        readonly AuthVM auth = new();
        public Accounts(ErpSoftaxeContext context, IDataLogic dataLogic, IAuth authData)
        {
            _context = context;
            _dataLogic = dataLogic;
            _auth = authData;

            auth = _auth.GetUserData();
        }

        public DataTable GetVchFec(DateTime vchDate, DateTime fromDate, DateTime toDate, string tag)
        {

            if (!string.IsNullOrWhiteSpace(tag))
            {
                tag = $"AND CAST(VCHDATE AS DATE) BETWEEN '{fromDate.ToString("yyyy/MM/dd")}' AND '{toDate.ToString("yyyy/MM/dd")}'";
            }

            string qry = $@"SELECT VCHNO, GPNO, DESCRP, FREIGHT, CONVERT(VARCHAR(10),VCHDATE,103) VCHDATE,
                            DMCODE MAINACCOUNT, (DMCODE+CODE) SUBACCOUNT 
                            FROM TBLTRANSVCHFAC WHERE CMP_ID = {auth.CmpId} AND LOCID = '{auth.LocId}' {tag}";

            return _dataLogic.LoadData(qry);
        }

    }

}

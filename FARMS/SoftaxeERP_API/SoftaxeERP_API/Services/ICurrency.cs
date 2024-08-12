using DevExpress.CodeParser;
using DevExpress.PivotGrid.QueryMode;
using SoftaxeERP_API.Models;
using SoftaxeERP_API.VM;
using System.Security.AccessControl;
using System.Data;

namespace SoftaxeERP_API.Services
{
    public interface ICurrency
    {
        DataTable GetCurrency();
    }

    public class Currency : ICurrency
    {


        private readonly ErpSoftaxeContext _context;
        private readonly IDataLogic _dataLogic;
        private readonly IAuth _auth;

        readonly AuthVM auth = new();
        public Currency(ErpSoftaxeContext context, IDataLogic dataLogic, IAuth authData)
        {
            _context = context;
            _dataLogic = dataLogic;
            _auth = authData;

            auth = _auth.GetUserData();
        }


        public DataTable GetCurrency()
        {
            String qry = $@"SELECT CURRENCYNAME,ID FROM TBLCURRENCY WHERE CMPID = '{auth.CmpId}'";
            return _dataLogic.LoadData(qry);
        }
    }
}

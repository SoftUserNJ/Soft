using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SoftaxeERP_API.Services;
using SoftaxeERP_API.VM;

namespace SoftaxeERP_API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class InventoryController : ControllerBase
    {
        private readonly IWarehouse _warehouse;
        private readonly IProduct _product;
        private readonly IStock _stock;

        public InventoryController(IWarehouse warehouse, IProduct product, IStock stock)
        {
            _warehouse = warehouse;
            _product = product;
            _stock = stock;
        }


        #region Material Consumption

        [HttpGet("GetMaterialConsumptionBalance")]
        public IActionResult GetMaterialConsumptionBalance(DateTime date, string itemCode)
        {
            var data = _stock.GetMaterialConsumptionBalance(date, itemCode);
            string result = JsonConvert.SerializeObject(data);
            return Ok(result);
        }
        
        [HttpGet("GetMaterialConsumption")]
        public IActionResult GetMaterialConsumption(DateTime fromDate, DateTime toDate)
        {
            var data = _stock.GetMaterialConsumption(fromDate, toDate);
            string result = JsonConvert.SerializeObject(data);
            return Ok(result);
        }

        [HttpGet("GetEditMaterialConsumption")]
        public IActionResult GetEditMaterialConsumption(int vchNo)
        {
            var data = _stock.GetEditMaterialConsumption(vchNo);
            string result = JsonConvert.SerializeObject(data);
            return Ok(result);
        }

        [HttpPost("SaveMaterialConsumption")]
        public IActionResult SaveMaterialConsumption(List<MaterialConsumptionVM> mc)
        {
            bool data = _stock.SaveMaterialConsumption(mc);
            return Ok(data);
        }

        [HttpDelete("DelMaterialConsumption")]
        public IActionResult DelMaterialConsumption(int vchNo)
        {
            bool data = _stock.DelMaterialConsumption(vchNo);
            return Ok(data);
        }


        #endregion

        #region WAREHOUSES

        [HttpGet("GetWarehouseList")]
        public IActionResult GetWarehouseList()
        {
            var data = _warehouse.GetWarehouseList();
            string result = JsonConvert.SerializeObject(data);

            return Ok(result);
        }

        [HttpGet("Warehouse")]
        public IActionResult GetWarehouse()
        {
            var data = _warehouse.GetWarehouse();
            string result = JsonConvert.SerializeObject(data);

            return Ok(result);
        }

        [HttpPost("AddUpdateWarehouse")]
        public IActionResult AddUpdateWarehouse(int id, string name, string alias, DateTime dtNow)
        {
            bool result = _warehouse.AddUpdateWarehouse(id, name, alias, dtNow);

            return Ok(result);
        }

        [HttpDelete("DeleteWarehouse")]
        public IActionResult DeleteWarehouse(int id, DateTime dtNow)
        {
            var result = _warehouse.DeleteWarehouse(id, dtNow);

            return Ok(result);
        }


        [HttpGet("Rack")]
        public IActionResult GetRack(int wId)
        {
            var data = _warehouse.GetRack(wId);
            string result = JsonConvert.SerializeObject(data);

            return Ok(result);
        }

        [HttpPost("AddUpdateRack")]
        public IActionResult AddUpdateRack(int wId, int id, string name, string alias, DateTime dtNow)
        {
            bool result = _warehouse.AddUpdateRack(wId, id, name, alias, dtNow);

            return Ok(result);
        }

        [HttpDelete("DeleteRack")]
        public IActionResult DeleteRack(int wId, int id, DateTime dtNow)
        {
            var result = _warehouse.DeleteRack(wId, id, dtNow);

            return Ok(result);
        }


        [HttpGet("Shelf")]
        public IActionResult GetShelf(int wId, int rId)
        {
            var data = _warehouse.GetShelf(wId, rId);
            string result = JsonConvert.SerializeObject(data);

            return Ok(result);
        }

        [HttpPost("AddUpdateShelf")]
        public IActionResult AddUpdateShelf(int wId, int rId, int id, string name, string alias, DateTime dtNow)
        {
            bool result = _warehouse.AddUpdateShelf(wId, rId, id, name, alias, dtNow);

            return Ok(result);
        }

        [HttpDelete("DeleteShelf")]
        public IActionResult DeleteShelf(int wId, int rId, int id, DateTime dtNow)
        {
            var result = _warehouse.DeleteShelf(wId, rId, id, dtNow);

            return Ok(result);
        }

        #endregion

        #region CATEGORY

        [HttpGet("GetCategory")]
        public IActionResult GetCategory()
        {
            var data = _product.GetCategory();
            string result = JsonConvert.SerializeObject(data);

            return Ok(result);
        }

        [HttpPost("AddUpdateCategory")]
        public IActionResult AddUpdateCategory([FromForm] int id,[FromForm] string name,[FromForm] int expiryDays, [FromForm] string expCode, [FromForm] bool isCommission, [FromForm] IFormFile picture,[FromForm] DateTime dtNow)
        {
            bool result = _product.AddUpdateCategory(id, name, expiryDays, expCode, isCommission, picture, dtNow);
            return Ok(result);
        }

        [HttpDelete("DeleteCategory")]
        public IActionResult DeleteCategory(int id, DateTime dtNow)
        {
            string result = _product.DeleteCategory(id, dtNow);
            return Ok(result);
        }

        #endregion

        #region BRAND

        [HttpGet("GetBrand")]
        public IActionResult GetBrand(int categoryId)
        {
            var data = _product.GetBrand(categoryId);
            string result = JsonConvert.SerializeObject(data);

            return Ok(result);
        }

        [HttpPost("AddUpdateBrand")]
        public IActionResult AddUpdateBrand(int categoryId, int id, string name, DateTime dtNow)
        {
            bool result = _product.AddUpdateBrand(categoryId, id, name, dtNow);
            return Ok(result);
        }

        [HttpDelete("DeleteBrand")]
        public IActionResult DeleteBrand(int categoryId, int id, DateTime dtNow)
        {
            string result = _product.DeleteBrand(categoryId, id, dtNow);
            return Ok(result);
        }

        #endregion

        #region UOM

        [HttpGet("GetUom")]
        public IActionResult GetUom()
        {
            var data = _product.GetUOM();
            string result = JsonConvert.SerializeObject(data);

            return Ok(result);
        }

        [HttpPost("AddUpdateUom")]
        public IActionResult AddUpdateUom(int id, string name, DateTime dtNow)
        {
            bool result = _product.AddUpdateUOM(id, name, dtNow);
            return Ok(result);
        }

        [HttpDelete("DeleteUom")]
        public IActionResult DeleteUom(int id, DateTime dtNow)
        
            {
            string result = _product.DeleteUOM(id, dtNow);
            return Ok(result);
        }

        #endregion

        #region MADE IN

        [HttpGet("GetMadeIn")]
        public IActionResult GetMadeIn()
        {
            var data = _product.GetMadeIn();
            string result = JsonConvert.SerializeObject(data);

            return Ok(result);
        }

        [HttpPost("AddUpdateMadeIn")]
        public IActionResult AddUpdateMadeIn(int id, string name, DateTime dtNow)
        {
            bool result = _product.AddUpdateMadeIn(id, name, dtNow);
            return Ok(result);
        }

        [HttpDelete("DeleteMadeIn")]
        public IActionResult DeleteMadeIn(int id, DateTime dtNow)
        {
            string result = _product.DeleteMadeIn(id, dtNow);
            return Ok(result);
        }

        #endregion

        #region PRODUCT

        [HttpGet("GetLocation")]
        public IActionResult GetLocation()
        {
            var data = _product.GetLocation();
            string result = JsonConvert.SerializeObject(data);

            return Ok(result);
        }

        [HttpGet("GetProducts")]
        public IActionResult GetProducts(int categoryId)
        {
            var data = _product.GetProducts(categoryId);
            string result = JsonConvert.SerializeObject(data);

            return Ok(result);
        }

        [HttpGet("GetMainAccount")]
        public IActionResult GetMainAccount()
        {
            var data = _product.GetMainAccount();
            string result = JsonConvert.SerializeObject(data);

            return Ok(result);
        }

        [HttpPost("AddUpdateProduct")]
        public IActionResult AddUpdateProduct([FromForm] ProductVM product)
        {
            string result = _product.AddUpdateProduct(product);
            return Ok(result);
        }

        [HttpGet("EditProduct")]
        public IActionResult EditProduct(string code)
        {
            var data = _product.EditProduct(code);

            string result = JsonConvert.SerializeObject(data);
            return Ok(result);
        }

        [HttpDelete("DeleteProduct")]
        public IActionResult DeleteProduct(string stockCode, string saleCode, string productName, DateTime dtNow)
        {
            var result = _product.DeleteProduct(stockCode, saleCode, productName, dtNow);
            return Ok(result);
        }

        [HttpGet("GenBarCode")]
        public IActionResult GenBarCode(string saleCode, string code)
        {
            var data = _product.GenBarCode(saleCode, code);

            string result = JsonConvert.SerializeObject(data);
            return Ok(result);
        }

        #endregion

        #region CHANGE CATEGORY

        [HttpPost("UpdateCategory")]
        public IActionResult UpdateCategory(int fCategory, int fBrand, int tCategory, DateTime dtNow)
        {
            var result = _product.UpdateCategory(fCategory, fBrand, tCategory, dtNow);
            return Ok(result);
        }

        #endregion

        #region PRODUCT RATE UPDATE

        [HttpGet("GetProductRateUpdate")]
        public IActionResult GetProductRateUpdate()
        {
            var data = _product.GetProductRateUpdate();
            string result = JsonConvert.SerializeObject(data);
            return Ok(result);
        }

        [HttpPost("SaveProductRateUpdate")]
        public IActionResult SaveProductRateUpdate([FromBody] List<ProductRateUpdateVM> vM)
        {
            var data = _product.SaveProductRateUpdate(vM);
            return Ok(data);
        }

        #endregion

        #region STOCK OPENING

        [HttpGet("GetSOList")]
        public IActionResult GetSOList()
        {
            var data = _stock.GetSOList();
            string result = JsonConvert.SerializeObject(data);

            return Ok(result);
        }

        [HttpGet("GetSearchProductList")]
        public IActionResult GetSearchProductList(string name)
        {
            var data = _stock.GetSearchProductList(name);
            string result = JsonConvert.SerializeObject(data);

            return Ok(result);
        }

        [HttpGet("GetProductDetail")]
        public IActionResult GetProductDetail(string code)
        {
            var data = _stock.GetProductDetail(code);
            string result = JsonConvert.SerializeObject(data);

            return Ok(result);
        }

        [HttpPost("SaveStockOpening")]
        public IActionResult SaveStockOpening([FromBody] StockOpVM vm)
        {
            var data = _stock.SaveStockOpening(vm);
            return Ok(data);
        }

        [HttpGet("EditSOProduct")]
        public IActionResult EditSOProduct(string code, int id, int uomId)
        {
            var data = _stock.EditSOProduct(code, id, uomId);
            string result = JsonConvert.SerializeObject(data);

            return Ok(result);
        }

        [HttpDelete("DeleteOPStock")]
        public IActionResult DeleteOPStock(int id, string code, DateTime expDate, string location, DateTime dtNow)
        {
            var data = _stock.DeleteOPStock(id, code, expDate, location, dtNow);
            return Ok(data);
        }

        #endregion

        [HttpGet("GetExpenseList")]
        public IActionResult GetExpenseList()
        {
            var data = _product.ExpenseList();
            string result = JsonConvert.SerializeObject(data);

            return Ok(result);
        }

        [HttpGet("GetProductList")]
        public IActionResult GetProductList(bool isStock)
        {
            var data = _stock.GetProductList(isStock);
            string result = JsonConvert.SerializeObject(data);

            return Ok(result);
        }

        [HttpGet("GetProductUom")]
        public IActionResult GetProductUom(string code)
        {
            var data = _stock.GetUom(code);
            string result = JsonConvert.SerializeObject(data);

            return Ok(result);
        }

        [HttpGet("GetProductUoms")]
        public IActionResult GetProductUom()
        {
            var data = _stock.GetUom();
            string result = JsonConvert.SerializeObject(data);

            return Ok(result);
        }

        #region STOCK DEBIT NOTE

        [HttpGet("GetDebitNoteList")]
        public IActionResult GetDebitNoteList(DateTime fromDate, DateTime toDate)
        {
            var data = _stock.GetDebitCreditNoteList(fromDate, toDate, "STK-DR", "Debit");
            string result = JsonConvert.SerializeObject(data);

            return Ok(result);
        }

        [HttpGet("GetDNMax")]
        public IActionResult GetDNMax()
        {
            var data = _stock.GetMax("STK-DR");
            string result = JsonConvert.SerializeObject(data);

            return Ok(result);
        }

        [HttpPost("SaveUpdateDebitNote")]
        public async Task<IActionResult> SaveUpdateDebitNote(List<StockDebitCreditVM> vm)
        {
            vm.ForEach(x => x.VchType = "STK-DR");
            var data = await _stock.SaveUpdateStockDebitCredit(vm);
            return Ok(data);
        }

        [HttpGet("EditDebitNote")]
        public IActionResult EditDebitNote(int vchNo)
        {
            var data = _stock.EditDebitCreditNote(vchNo, "STK-DR");
            string result = JsonConvert.SerializeObject(data);

            return Ok(result);
        }

        [HttpDelete("DeleteDebitNote")]
        public IActionResult DeleteDebitNote(int vchNo, DateTime dtNow)
        {
            var data = _stock.DeleteDebitCreditNote(vchNo, "STK-DR", dtNow);
            return Ok(data);
        }

        #endregion

        #region STOCK CREDIT NOTE

        [HttpGet("GetCreditNoteList")]
        public IActionResult GetCreditNoteList(DateTime fromDate, DateTime toDate)
        {
            var data = _stock.GetDebitCreditNoteList(fromDate, toDate, "STK-CR", "Credit");
            string result = JsonConvert.SerializeObject(data);

            return Ok(result);
        }

        [HttpGet("GetCNMax")]
        public IActionResult GetCNMax()
        {
            var data = _stock.GetMax("STK-CR");
            string result = JsonConvert.SerializeObject(data);

            return Ok(result);
        }

        [HttpGet("GetProductLastExp")]
        public IActionResult GetProductLastExp(string code)
        {
            var data = _stock.GetLastExp(code);
            string result = JsonConvert.SerializeObject(data);

            return Ok(result);
        }

        [HttpPost("SaveUpdateCreditNote")]
        public async Task<IActionResult> SaveUpdateCreditNote(List<StockDebitCreditVM> vm)
        {
            vm.ForEach(x => x.VchType = "STK-CR");
            var data = await _stock.SaveUpdateStockDebitCredit(vm);
            return Ok(data);
        }

        [HttpGet("EditCreditNote")]
        public IActionResult EditCreditNote(int vchNo)
        {
            var data = _stock.EditDebitCreditNote(vchNo, "STK-CR");
            string result = JsonConvert.SerializeObject(data);

            return Ok(result);
        }

        [HttpDelete("DeleteCreditNote")]
        public IActionResult DeleteCreditNote(int vchNo, DateTime dtNow)
        {
            var data = _stock.DeleteDebitCreditNote(vchNo, "STK-CR", dtNow);
            return Ok(data);
        }

        #endregion

        #region STOCK TRANSFER 

        [HttpGet("GetTransferList")]
        public IActionResult GetTransferList(DateTime fromDate, DateTime toDate)
        {
            var data = _stock.GetTransferList(fromDate, toDate);
            string result = JsonConvert.SerializeObject(data);

            return Ok(result);
        }

        [HttpGet("GetSTMax")]
        public IActionResult GetSTMax()
        {
            var data = _stock.GetMax("STK-TRF");
            string result = JsonConvert.SerializeObject(data);

            return Ok(result);
        }

        [HttpGet("GetSTFromLocation")]
        public IActionResult GetSTFromLocation(string code, int vchNo, string vchType)
        {
            var data = _stock.GetSTFromLocation(code, vchNo, vchType);
            string result = JsonConvert.SerializeObject(data);

            return Ok(result);
        }

        [HttpGet("GetSTToLocation")]
        public IActionResult GetSTToLocation(int id, string tag)
        {
            var data = _stock.GetSTToLocation(id, tag);
            string result = JsonConvert.SerializeObject(data);

            return Ok(result);
        }

        [HttpPost("SaveUpdateTransfer")]
        public async Task<IActionResult> SaveUpdateTransfer(List<TransferVM> transfers)
        {
            var data = await _stock.SaveUpdateTransfer(transfers);
            return Ok(data);
        }

        [HttpGet("EditTransfer")]
        public IActionResult EditTransfer(int vchNo)
        {
            var data = _stock.EditTransfer(vchNo);
            string result = JsonConvert.SerializeObject(data);

            return Ok(result);
        }

        [HttpDelete("DeleteTransfer")]
        public IActionResult DeleteTransfer(int vchNo, DateTime dtNow)
        {
            var data = _stock.DeleteTransfer(vchNo, dtNow);
            return Ok(data);
        }

        #endregion

        #region STOCK STATUS

        [HttpGet("GetStockStatus")]
        public IActionResult GetStockStatus()
        {
            var data = _stock.GetStockStatus();
            string result = JsonConvert.SerializeObject(data);

            return Ok(result);
        }

        [HttpGet("GetStockDetail")]
        public IActionResult GetStockDetail(string code)
        {
            var data = _stock.GetStockDetail(code);
            string result = JsonConvert.SerializeObject(data);

            return Ok(result);
        }

        #endregion

        #region PRODUCT LEDGER

        [HttpGet("GetProductLedger")]
        public IActionResult GetProductLedger(string name, int category)
		{
            var data = _product.GetProductLedger(name, category);
            string result = JsonConvert.SerializeObject(data);

            return Ok(result);
		}

        [HttpGet("GetProductLocation")]
        public IActionResult GetProductLocation(string code)
		{
            var data = _product.StockLocation(code);
            string result = JsonConvert.SerializeObject(data);

            return Ok(result);
		}

        #endregion

        #region PURCHASE RATE COMPARISON

        [HttpGet("GetPurchaseRateComparison")]
        public IActionResult GetPurchaseRateComparison(string name)
        {
            var data = _product.GetPurchaseRateComparison(name);
            string result = JsonConvert.SerializeObject(data);

            return Ok(result);
        }

        #endregion

    }
}

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
    public class AdminController : ControllerBase
    {

        private readonly IAdmin _cmp;

        public AdminController(IAdmin company)
        {
            _cmp = company;
        }

        [HttpGet("GetCompanyById")]
        public IActionResult GetCompanyById(int groupId)
        {
            var data = _cmp.GetCompanyById(groupId);
            string result = JsonConvert.SerializeObject(data);
            return Ok(result);
        }

        [HttpGet("GetLocationById")]
        public IActionResult GetLocationById(int companyId)
        {
            var data = _cmp.GetLocationById(companyId);
            string result = JsonConvert.SerializeObject(data);
            return Ok(result);
        }

        #region COMPANY GROUP


        [HttpGet("GetCompanyGroup")]
        public IActionResult GetCompanyGroup()
        {
            var data = _cmp.GetCompanyGroup();
            string result = JsonConvert.SerializeObject(data);
            return Ok(result);
        }

        [HttpPost("AddCompanyGroup")]
        public IActionResult AddCompanyGroup(int grpId, string groupName, string address, string city, string ntn, string contact, string email, bool isMulti)
        {
            var data = _cmp.AddCompanyGroup(grpId, groupName, address, city, ntn, contact, email, isMulti);
            return Ok(data);
        }

        [HttpDelete("DeleteCompanyGroup")]
        public IActionResult DeleteCompanyGroup(int id)
        {
            string data = _cmp.DeleteCompanyGroup(id);
            return Ok(data);
        }

        #endregion

        #region COMPANY

        [HttpGet("GetCompanyList")]
        public IActionResult GetCompanyList()
        {
            var data = _cmp.GetCompanyList();
            string result = JsonConvert.SerializeObject(data);

            return Ok(result);
        }

        [HttpGet("GetCompanyDetail")]
        public IActionResult GetCompanyDetail(int id)
        {
            var data = _cmp.GetCompanyDetail(id);
            string result = JsonConvert.SerializeObject(data);

            return Ok(result);
        }

        [HttpPost("AddUpdateCompany")]
        public IActionResult AddUpdateCompany([FromForm] CompanyViewModel company)
        {
            var data = _cmp.AddUpdateCompany(company);
            return Ok(data);
        }

        [HttpGet("GetNumber")]
        public IActionResult GetNumber()
        {
            object result = _cmp.GetNumber();
            return Ok(result);
        }

        [HttpDelete("DeleteCompany")]
        public IActionResult DeleteCompany(int groupId, int companyId)
        {
            var data = _cmp.DeleteCompany(groupId, companyId);
            return Ok(data);
        }
        #endregion

        #region LOCATION

        [HttpGet("GetLocation")]
        public IActionResult GetLocation()
        {
            var data = _cmp.GetLocation();
            string result = JsonConvert.SerializeObject(data);

            return Ok(result);
        }

        [HttpPost("AddUpdateLocation")]
        public IActionResult AddUpdateLocation(int companyId, string locId, string name, string city, string address, string contact, string email, string cmpName)
        {
            var data = _cmp.AddUpdateLocation(companyId, locId, name, city, address, contact, email, cmpName);
            return Ok(data);
        }


        [HttpDelete("DeleteLocation")]
        public IActionResult DeleteLocation(string locId, int companyId)
        {
            var data = _cmp.DeleteLocation(locId, companyId);
            return Ok(data);
        }


        #endregion

        #region Shift

        [HttpGet("GetShift")]
        public IActionResult GetShift()
        {
            var data = _cmp.GetShift();
            string result = JsonConvert.SerializeObject(data);

            return Ok(result);
        }

        [HttpPost("AddUpdateShift")]
        public IActionResult AddUpdateShift(int id, string name, string fromTime, string toTime)
        {
            var data = _cmp.AddUpdateShift(id, name, fromTime, toTime);
            if (data == true)
            {
                return Ok("Data Saved Successfully");
            }
            else
            {
                return BadRequest("Error Saving the Data");
            }
        }


        [HttpDelete("DeleteShift")]
        public IActionResult DeleteShift(int? id)
        {
            var data = _cmp.DeleteShift(id);
            if (data == true)
            {
                return Ok("Deleted Successfully");
            }
            else
            {
                return BadRequest("Error Deleting the Data");
            }
        }

        #endregion

        #region Points

        [HttpGet("GetPoints")]
        public IActionResult GetPoints()
        {
            var data = _cmp.GetPoints();
            string result = JsonConvert.SerializeObject(data);

            return Ok(result);
        }

        [HttpPost("AddUpdatePoints")]
        public IActionResult AddUpdatePoints(int id, string name)
        {
            var data = _cmp.AddUpdatePoints(id, name);
            if (data == true)
            {
                return Ok("Data Saved Successfully");
            }
            else
            {
                return BadRequest("Error Saving the Data");
            }
        }

        [HttpDelete("DeletePoints")]
        public IActionResult DeletePoints(int? id)
        {
            var data = _cmp.DeletePoints(id);
            if (data == true)
            {
                return Ok("Deleted Successfully");
            }
            else
            {
                return BadRequest("Error Deleting the Data");
            }
        }

        #endregion

        #region EXCEL UPLOAD

        [HttpPost("SaveProduct")]
        public IActionResult SaveProduct(string productExcel)
        {
            var data = _cmp.SaveProduct(productExcel);
            return Ok(data);
        }

        [HttpPost("SaveCustomerSupplier")]
        public IActionResult SaveCustomerSupplier(string customerSupplier)
        {
            var data = _cmp.SaveCustomerSupplier(customerSupplier);
            return Ok(data);
        }

        #endregion
    }
}

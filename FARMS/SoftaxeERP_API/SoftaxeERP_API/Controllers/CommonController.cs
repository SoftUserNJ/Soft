using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SoftaxeERP_API.Services;

namespace SoftaxeERP_API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class CommonController : ControllerBase
    {
        private readonly ICommonFieldsData _cmd;
        public CommonController(ICommonFieldsData commonFieldsData)
        {
            _cmd = commonFieldsData;
        }

        [HttpGet("GetLevel4")]
        public IActionResult GetLevel4()
        {
            var data = _cmd.GetLevel4();
            string result = JsonConvert.SerializeObject(data);
            return Ok(result);
        }


        [HttpGet("GetDirectL4L5CodeNamesByL4Tag")]
        public IActionResult GetDirectL4L5CodeNamesByL4Tag(string l4Tag)
        {
            var data = _cmd.GetDirectL4L5CodeNamesByL4Tag(l4Tag);
            string result = JsonConvert.SerializeObject(data);
            return Ok(result);
        }


        [HttpGet("GetLevel5CodeNameByL4Code")]
        public IActionResult GetLevel5CodeNameByL4Code(string code)
        {
            var data = _cmd.GetLevel5CodeNameByL4Code(code);
            string result = JsonConvert.SerializeObject(data);
            return Ok(result);
        }

        [HttpGet("GetLevel4CodeNameByTag")]
        public IActionResult GetLevel4CodeNameByTag(string tag)
        {
            var data = _cmd.GetLevel4CodeNameByTag(tag);
            string result = JsonConvert.SerializeObject(data);
            return Ok(result);
        }

        [HttpGet("GetLocation")]
        public IActionResult GetLocation()
        {
            var data = _cmd.Location();
            string result = JsonConvert.SerializeObject(data);
            return Ok(result);
        }
        
        [HttpGet("LocationWithLoc")]
        public IActionResult LocationWithLoc()
        {
            var data = _cmd.LocationWithLoc();
            string result = JsonConvert.SerializeObject(data);
            return Ok(result);
        }

        [HttpGet("GetTypes")]
        public IActionResult GetTypes()
        {
            var data = _cmd.GetTypes();
            string result = JsonConvert.SerializeObject(data);
            return Ok(result);
        }

        [HttpGet("GetCropYear")]
        public IActionResult GetCropYear()
        {
            var data = _cmd.CropYear();
            string result = JsonConvert.SerializeObject(data);
            return Ok(result);
        }


        [HttpGet("GetUom")]
        public IActionResult GetUom()
        {
            var data = _cmd.GetUOM();
            string result = JsonConvert.SerializeObject(data);

            return Ok(result);
        }


        [HttpGet("GetGodowns")]
        public IActionResult GetGodowns()
        {
            var g = _cmd.Godowns();
            string result = JsonConvert.SerializeObject(g);
            return Ok(result);
        }


        [HttpGet("GetCostCenter")]
        public IActionResult GetCostCenter()
        {
            var g = _cmd.GetCostCenter();
            string result = JsonConvert.SerializeObject(g);
            return Ok(result);
        }

        [HttpGet("GetPoDetailsByPartyAndItems")]
        public IActionResult GetPoDetailsByPartyAndItems(string party, string item , DateTime TransDate , int Vchno , int Pono)
        {
            var data = _cmd.GetPoDetailsByPartyAndItems(party, item , TransDate , Vchno , Pono);
            string result = JsonConvert.SerializeObject(data);
            return Ok(result);
        }
        
        [HttpGet("GetPurchaseBagsType")]
        public IActionResult GetPurchaseBagsType()
        {
            var data = _cmd.GetPurchaseBagsType();
            string result = JsonConvert.SerializeObject(data);
            return Ok(result);
        }


        [HttpGet("GetSubParty")]
        public IActionResult GetSubParty(string code)
        {
            var data = _cmd.GetSubParty(code);
            string result = JsonConvert.SerializeObject(data);

            return Ok(result);
        }

        [HttpGet("GetProductLocation")]
        public IActionResult GetProductLocation()
        {
            var data = _cmd.GetProductLocation();
            string result = JsonConvert.SerializeObject(data);

            return Ok(result);
        }

    }
}

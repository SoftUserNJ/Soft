using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SoftaxeERP_API.Services;
using SoftaxeERP_API.VM;
using System.Security.AccessControl;

namespace SoftaxeERP_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WeighBridgeController : ControllerBase
    {
        private readonly IWeighBridge _weighBridge;
        public WeighBridgeController(IWeighBridge weighBridge)
        {
            _weighBridge = weighBridge;

        }

        [HttpGet("GetFirstWeight")]
        public IActionResult GetFirstWeight(string VchType)
        {
            var data = _weighBridge.GetFirstWeight(VchType);
            string result = JsonConvert.SerializeObject(data);
            return Ok(result);
        }


        [HttpGet("GetSecondWeight")]
        public IActionResult GetSecondWeight(string VchType)
        {
            var data = _weighBridge.GetSecondWeight(VchType);
            string result = JsonConvert.SerializeObject(data);
            return Ok(result);
        }

        [HttpGet("GetFirstWeightDetail")]
        public IActionResult GetFirstWeightDetail(int VchNo, int ArrivalNo, string VchType, string Status)
        {
            var data = _weighBridge.GetFirstWeightDetail(VchNo, ArrivalNo, VchType, Status);
            return Ok(data);
        }


        [HttpGet("GetSecondWeightDetail")]
        public IActionResult GetSecondWeightDetail(int VchNo, int ArrivalNo, string VchType)
        {
            var data = _weighBridge.GetSecondWeightDetail(VchNo, ArrivalNo, VchType);
            return Ok(data);
        }


        [HttpPost("SaveWeighment")]
        public IActionResult SaveWeighment(WeighBridgeVM weigh)
        {
            var data = _weighBridge.SaveWeighment(weigh);
            return Ok(data);
        }

        // Gate Pass Outward

        [HttpGet("GetFirstWeightOutward")]

        public IActionResult GetFirstWeightOutward()
        {
            var data = _weighBridge.GetFirstWeightOutward();
            string result = JsonConvert.SerializeObject(data);
            return Ok(result);
        }

        [HttpGet("GetFirstWeightOutwardDetail")]
        public IActionResult GetFirstWeightOutwardDetail(int VchNo)
        {
            var data = _weighBridge.GetFirstWeightOutwardDetail(VchNo);
            string result = JsonConvert.SerializeObject(data);
            return Ok(result);
        }

        [HttpGet("GetSecondWeightOutward")]
        public IActionResult GetSecondWeightOutward()
        {
            var data = _weighBridge.GetSecondWeightOutward();
            string result = JsonConvert.SerializeObject(data);
            return Ok(result);
        }

        [HttpGet("GetSecondWeightOutwardDetail")]
        public IActionResult GetSecondWeightOutwardDetail(int VchNo)
        {
            var data = _weighBridge.GetSecondWeightOutwardDetail(VchNo);
            string result = JsonConvert.SerializeObject(data);
            return Ok(result);
        }

        [HttpPost("SaveOutWardWeighment")]
        public IActionResult SaveOutWardWeighment(WeighBridgeVM weigh)
        {
            var data = _weighBridge.SaveOutWardWeighment(weigh);
            return Ok(data);
        }


        // Outward Status

        [HttpGet("GetAllowedWtDiff")]
        public IActionResult GetAllowedWtDiff()
        {
            var data = _weighBridge.GetAllowedWtDiff();
            string result = JsonConvert.SerializeObject(data);
            return Ok(result);
        }

        [HttpGet("GetOutwardStatus")]
        public IActionResult GetOutwardStatus()
        {
            var data = _weighBridge.GetOutwardStatus();
            string result = JsonConvert.SerializeObject(data);
            return Ok(result);
        }

        [HttpGet("GetOutwardbyDate")]
        public IActionResult GetOutwardbyDate(DateTime fromDate, DateTime toDate)
        {
            var data = _weighBridge.GetOutwardbyDate(fromDate, toDate);
            string result = JsonConvert.SerializeObject(data);
            return Ok(result);
        }

        [HttpGet("GetWBSettings")]
        public IActionResult GetWBSettings()
        {
            var data = _weighBridge.GetWBSettings();
            string result = JsonConvert.SerializeObject(data);
            return Ok(result);
        }

    }
}

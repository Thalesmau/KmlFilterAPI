using Microsoft.AspNetCore.Mvc;
using KmlFilterAPI.Services;

namespace KmlFilterAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class KmlController : ControllerBase
    {
        private readonly KmlHelper _kmlService;

        public KmlController(KmlHelper kmlService)
        {
            _kmlService = kmlService;
        }

        [HttpGet("filter")]
        public IActionResult GetFilteredPlacemarks([FromQuery] string cliente, [FromQuery] string situacao, [FromQuery] string bairro, [FromQuery] string referencia, [FromQuery] string ruaCruzamento)
        {
            var placemarks = _kmlService.GetFilteredPlacemarks(cliente, situacao, bairro, referencia, ruaCruzamento);
            return Ok(placemarks);
        }

        [HttpGet("unique-values/{field}")]
        public IActionResult GetUniqueValues(string field)
        {
            var values = _kmlService.GetUniqueValues(field);
            return Ok(values);
        }

        [HttpPost("export")]
        public IActionResult ExportFilteredKml([FromBody] FilterRequest filterRequest)
        {
            var placemarks = _kmlService.GetFilteredPlacemarks(filterRequest.Cliente, filterRequest.Situacao, filterRequest.Bairro, filterRequest.Referencia, filterRequest.RuaCruzamento);
            _kmlService.ExportFilteredKml(placemarks, "filtered.kml");
            return Ok();
        }
    }

}

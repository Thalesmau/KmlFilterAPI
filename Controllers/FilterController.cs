using Microsoft.AspNetCore.Mvc;
using KmlFilterAPI.Services;
using KmlFilterAPI.Models;
using KmlFilterAPI.Utils;

namespace KmlFilterAPI.Controllers;

[ApiController]
[Route("api/placemarks")]
public class KmlController : ControllerBase
{
    private readonly IKmlService _kmlService;

    public KmlController(IKmlService kmlService)
    {
        _kmlService = kmlService;
    }

    /// <summary>
    /// Filtrar e exportar dados em formato KML.
    /// </summary>
    [HttpPost("export")]
    public ActionResult ExportKml([FromBody] List<FiltersModel> filters)
    {
        if (filters == null || !filters.Any())
        {
            return BadRequest("A lista de dados para exportação não pode estar vazia.");
        }

        try
        {
            var elements = filters.Select(x => new FiltersModel
            {
                Cliente = x.Cliente ?? "",
                Situacao = x.Situacao ?? "",
                Bairro = x.Bairro ?? "",
                Referencia = x.Referencia ?? "",
                RuaCruzamento = x.RuaCruzamento ?? ""
            }).ToList();

            var kmlContent = KmlHelper.ExportToKml(filters);
            
            var fileName = $"filtered_{DateTime.UtcNow:yyyyMMddHHmmss}.kml";

            return File(System.Text.Encoding.UTF8.GetBytes(kmlContent), "application/vnd.google-earth.kml+xml", fileName);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Erro ao exportar o arquivo KML: {ex.Message}");
        }
    }

    /// <summary>
    /// Listar os elementos filtrados no formato JSON.
    /// </summary>
    [HttpGet("placemarks")]
    public ActionResult GetFilteredPlacemarks([FromQuery] FiltersModel filters)
    {
        if (!IsValidFilters(filters))
        {
            return BadRequest("Os filtros fornecidos são inválidos. Verifique e tente novamente.");
        }

        try
        {
            var placemarks = _kmlService.GetFilteredPlacemarks(filters);
            return Ok(placemarks);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Erro ao listar os elementos: {ex.Message}");
        }
    }

    /// <summary>
    /// Obter valores únicos disponíveis para filtragem.
    /// </summary>
    [HttpGet("filters")]
    public ActionResult GetUniqueValues()
    {
        try
        {
            var clientes = _kmlService.GetUniqueValues("CLIENTE");
            var situacoes = _kmlService.GetUniqueValues("SITUAÇÃO");
            var bairros = _kmlService.GetUniqueValues("BAIRRO");

            return Ok(new
            {
                Clientes = clientes,
                Situacoes = situacoes,
                Bairros = bairros
            });
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Erro ao obter os filtros disponíveis: {ex.Message}");
        }
    }

    /// <summary>
    /// Valida os filtros fornecidos pelo cliente.
    /// </summary>
    private static bool IsValidFilters(FiltersModel filters)
    {
        if (!string.IsNullOrEmpty(filters.Referencia) && filters.Referencia.Length < 3)
            return false;

        if (!string.IsNullOrEmpty(filters.RuaCruzamento) && filters.RuaCruzamento.Length < 3)
            return false;

        return true;
    }

}

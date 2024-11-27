using KmlFilterAPI.Services;
using KmlFilterAPI.Models;
using KmlFilterApi.Utils;

namespace KmlFilterApi.Services;

public class KmlService : IKmlService
{
    private readonly string _kmlPath = "DIRECIONADORES1.kml";

    public async Task<List<PlacemarkModel>> GetFilteredElementsAsync(FiltersModel filters)
    {
        var elements = KmlHelper.ParseKmlFile(_kmlPath);

        return elements.Where(e =>
            (string.IsNullOrEmpty(filters.Cliente) || e.Cliente == filters.Cliente) &&
            (string.IsNullOrEmpty(filters.Situacao) || e.Situacao == filters.Situacao) &&
            (string.IsNullOrEmpty(filters.Bairro) || e.Bairro == filters.Bairro) &&
            (string.IsNullOrEmpty(filters.Referencia) || e.Referencia.Contains(filters.Referencia)) &&
            (string.IsNullOrEmpty(filters.RuaCruzamento) || e.RuaCruzamento.Contains(filters.RuaCruzamento))
        ).ToList();
    }

    public async Task<List<string>> GetUniqueValuesAsync(string fieldName)
    {
        var elements = KmlHelper.ParseKmlFile(_kmlPath);

        return fieldName switch
        {
            "CLIENTE" => elements.Select(e => e.Cliente).Distinct().ToList(),
            "SITUACAO" => elements.Select(e => e.Situacao).Distinct().ToList(),
            "BAIRRO" => elements.Select(e => e.Bairro).Distinct().ToList(),
            _ => throw new ArgumentException("Campo inválido para filtragem."),
        };
    }

    public async Task<string> ExportKmlAsync(FiltersModel filters)
    {
        var filteredElements = await GetFilteredElementsAsync(filters);
        return KmlHelper.ExportToKml(filteredElements);
    }
}
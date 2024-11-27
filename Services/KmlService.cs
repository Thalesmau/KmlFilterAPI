using KmlFilterAPI.Services;
using KmlFilterAPI.Models;
using KmlFilterAPI.Utils;

namespace KmlFilterApi.Services;

public class KmlService : IKmlService
{
    private readonly string _kmlPath = "DIRECIONADORES1.kml";

    public List<FiltersModel> GetFilteredPlacemarks(FiltersModel filters)
    {
        var elements = KmlHelper.ParseKmlFile(_kmlPath);

        var filteredElements = elements.Where(e =>
            (string.IsNullOrEmpty(filters.Cliente) || e.Cliente == filters.Cliente) &&
            (string.IsNullOrEmpty(filters.Situacao) || e.Situacao == filters.Situacao) &&
            (string.IsNullOrEmpty(filters.Bairro) || e.Bairro == filters.Bairro) &&
                (string.IsNullOrEmpty(filters.Referencia) ||
            (filters.Referencia.Length >= 3 &&
             !string.IsNullOrEmpty(e.Referencia) &&
             e.Referencia.Contains(filters.Referencia, StringComparison.OrdinalIgnoreCase))) &&

            (string.IsNullOrEmpty(filters.RuaCruzamento) ||
                (filters.RuaCruzamento.Length >= 3 &&
                 !string.IsNullOrEmpty(e.RuaCruzamento) &&
                 e.RuaCruzamento.Contains(filters.RuaCruzamento, StringComparison.OrdinalIgnoreCase)))
        ).ToList();

        if (!filteredElements.Any())
        {
            throw new InvalidOperationException("Nenhum dado encontrado para os filtros fornecidos.");
        }

        return filteredElements;
    }

    public List<string> GetUniqueValues(string fieldName)
    {
        var elements = KmlHelper.ParseKmlFile(_kmlPath);

        return fieldName switch
        {
            "CLIENTE" => elements.Select(e => e.Cliente ?? "").Distinct().ToList(),
            "SITUAÇÃO" => elements.Select(e => e.Situacao?? "").Distinct().ToList(),
            "BAIRRO" => elements.Select(e => e.Bairro ?? "").Distinct().ToList(),
            _ => throw new ArgumentException("Campo inválido para filtragem."),
        };
    }
}
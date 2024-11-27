using KmlFilterAPI.Models;

namespace KmlFilterAPI.Services;

public interface IKmlService 
{
    Task<List<PlacemarkModel>> GetFilteredPlacemarksAsync(FiltersModel filters);
    Task<List<string>> GetUniqueValuesAsync(string field);
    Task<string> ExportKmlAsync(FiltersModel placemarks);
}

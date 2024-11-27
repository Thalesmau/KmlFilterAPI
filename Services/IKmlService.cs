using KmlFilterAPI.Models;

namespace KmlFilterAPI.Services;

public interface IKmlService 
{
    List<FiltersModel> GetFilteredPlacemarks(FiltersModel filters);
    List<string> GetUniqueValues(string field);
}

using KmlFilterAPI.Models;
using SharpKml.Engine;

namespace KmlFilterAPI.Repositories
{
    public class KmlRepository
    {
        private readonly KmlFile _kmlFile;

        public KmlRepository(string filePath)
        {
            using var stream = File.OpenRead(filePath);
            _kmlFile = KmlFile.Load(stream);
        }

        public IEnumerable<PlacemarkModel> GetPlacemarks()
        {
            var placemarks = new List<PlacemarkModel>();
            var root = _kmlFile.Root as SharpKml.Dom.Document;

            foreach (var placemark in root.Features.OfType<SharpKml.Dom.Placemark>())
            {
                var data = placemark.ExtendedData.Data;
                placemarks.Add(new PlacemarkModel
                {
                    Cliente = data.FirstOrDefault(d => d.Name == "CLIENTE")?.Value,
                    Situacao = data.FirstOrDefault(d => d.Name == "SITUAÇÃO")?.Value,
                    Bairro = data.FirstOrDefault(d => d.Name == "BAIRRO")?.Value,
                    Referencia = placemark.Name,
                    RuaCruzamento = placemark.Description?.Text
                });
            }
            return placemarks;
        }
    }

}

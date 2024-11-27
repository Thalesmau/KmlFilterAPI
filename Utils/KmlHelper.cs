using KmlFilterAPI.Models;
using System.Xml.Linq;

namespace KmlFilterAPI.Utils;

public static class KmlHelper
{
    public static List<FiltersModel> ParseKmlFile(string filePath)
    {
        var elements = new List<FiltersModel>();

        try
        {
            var doc = XDocument.Load(filePath);
            var placemarks = doc.Descendants().Where(e => e.Name.LocalName == "Placemark");

            foreach (var placemark in placemarks)
            {
                var extendedData = placemark.Descendants().FirstOrDefault(e => e.Name.LocalName == "ExtendedData");

                if (extendedData != null)
                {
                    var element = new FiltersModel
                    {
                        Cliente = GetDataValue(extendedData, "CLIENTE"),
                        Situacao = GetDataValue(extendedData, "SITUAÇÃO"),
                        Bairro = GetDataValue(extendedData, "BAIRRO"),
                        Referencia = GetDataValue(extendedData, "REFERENCIA"),
                        RuaCruzamento = GetDataValue(extendedData, "RUA/CRUZAMENTO")
                    };

                    elements.Add(element);
                }
            }
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException($"Erro ao ler o arquivo KML: {ex.Message}");
        }

        return elements;
    }

    private static string GetDataValue(XElement extendedData, string key)
    {
        var dataElement = extendedData.Descendants().FirstOrDefault(e => e.Name.LocalName == "Data" && e.Attribute("name")?.Value == key);

        if (dataElement != null)
        {
            var valueElement = dataElement.Descendants().FirstOrDefault(e => e.Name.LocalName == "value");
            return valueElement?.Value ?? string.Empty;
        }

        return string.Empty;
    }

    public static string ExportToKml(List<FiltersModel> elements)
    {
        XNamespace kmlNamespace = "http://www.opengis.net/kml/2.2";

        var kml = new XDocument(
        new XElement(kmlNamespace + "kml",
            new XElement(kmlNamespace + "Document",
                elements.Select(e =>
                    new XElement(kmlNamespace + "Placemark",
                        new XElement(kmlNamespace + "ExtendedData",
                            new XElement(kmlNamespace + "Data",
                                new XAttribute("name", "CLIENTE"),
                                new XElement(kmlNamespace + "value", e.Cliente ?? "")
                            ),
                            new XElement(kmlNamespace + "Data",
                                new XAttribute("name", "SITUAÇÃO"),
                                new XElement(kmlNamespace + "value", e.Situacao ?? "")
                            ),
                            new XElement(kmlNamespace + "Data",
                                new XAttribute("name", "BAIRRO"),
                                new XElement(kmlNamespace + "value", e.Bairro ?? "")
                            ),
                            new XElement(kmlNamespace + "Data",
                                new XAttribute("name", "REFERENCIA"),
                                new XElement(kmlNamespace + "value", e.Referencia ?? "")
                            ),
                            new XElement(kmlNamespace + "Data",
                                new XAttribute("name", "RUA/CRUZAMENTO"),
                                new XElement(kmlNamespace + "value", e.RuaCruzamento ?? "")
                            )
                        )
                    )
                )
            )
        )
    );

        return kml.ToString();
    }
}

using System;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Xml.Serialization;
using System.IO;

public class WeatherDataJson
{
    public Main Main { get; set; }
    public string Name { get; set; }
}

public class Main
{
    public double Temp { get; set; }
    public int Humidity { get; set; }
}

// XML classes
[XmlRoot("current")]
public class WeatherDataXml
{
    [XmlElement("temperature")]
    public Temperature Temperature { get; set; }

    [XmlElement("city")]
    public City City { get; set; }
}

public class Temperature
{
    [XmlAttribute("value")]
    public double Value { get; set; }
}

public class City
{
    [XmlAttribute("name")]
    public string Name { get; set; }
}

class Program
{
    private static readonly string apiKey = "0bc62565d4580b1d603e899ae56d6075";  // Replace with your API key
    private static readonly string baseUrl = "https://api.openweathermap.org/data/2.5/weather";

    static async Task Main(string[] args)
    {
        Console.Write("Enter city name: ");
        string city = Console.ReadLine();

        Console.WriteLine("Choose data format (1 = JSON, 2 = XML): ");
        string format = Console.ReadLine();

        try
        {
            if (format == "1")
                await FetchWeatherDataJsonAsync(city);
            else if (format == "2")
                await FetchWeatherDataXmlAsync(city);
            else
                Console.WriteLine("Invalid format choice.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }

        // Pause to keep the console window open
        Console.WriteLine("\nPress any key to exit...");
        Console.ReadKey();
    }

    // Fetch Weather Data in JSON format
    static async Task FetchWeatherDataJsonAsync(string city)
    {
        using (HttpClient client = new HttpClient())
        {
            string url = $"{baseUrl}?q={city}&appid={apiKey}&units=metric";
            HttpResponseMessage response = await client.GetAsync(url);

            if (response.IsSuccessStatusCode)
            {
                string jsonResponse = await response.Content.ReadAsStringAsync();
                WeatherDataJson weather = JsonConvert.DeserializeObject<WeatherDataJson>(jsonResponse);

                Console.WriteLine($"\nWeather in {weather.Name}:");
                Console.WriteLine($"Temperature: {weather.Main.Temp}°C");
                Console.WriteLine($"Humidity: {weather.Main.Humidity}%");
            }
            else
            {
                Console.WriteLine($"Failed to fetch data. Status code: {response.StatusCode}");
            }
        }
    }

    // Fetch Weather Data in XML format
    static async Task FetchWeatherDataXmlAsync(string city)
    {
        using (HttpClient client = new HttpClient())
        {
            string url = $"{baseUrl}?q={city}&mode=xml&appid={apiKey}";
            HttpResponseMessage response = await client.GetAsync(url);

            if (response.IsSuccessStatusCode)
            {
                string xmlResponse = await response.Content.ReadAsStringAsync();
                var serializer = new XmlSerializer(typeof(WeatherDataXml));
                using (TextReader reader = new StringReader(xmlResponse))
                {
                    WeatherDataXml weather = (WeatherDataXml)serializer.Deserialize(reader);

                    Console.WriteLine($"\nWeather in {weather.City.Name}:");
                    Console.WriteLine($"Temperature: {weather.Temperature.Value}°C");
                }
            }
            else
            {
                Console.WriteLine($"Failed to fetch data. Status code: {response.StatusCode}");
            }
        }
    }
}

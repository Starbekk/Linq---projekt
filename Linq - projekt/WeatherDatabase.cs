using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;
using System.Security.Cryptography;
using System.IO;
using CsvHelper.Configuration;
using CsvHelper;

namespace Linq;

public static class WeatherDatabase
{
    public static List<Weather> LoadWeather(string path)
    {
        var configuration = new CsvConfiguration(CultureInfo.InvariantCulture)
        {
            HasHeaderRecord = true,
            BadDataFound = null,
            MissingFieldFound = null,
            HeaderValidated = null,
            TrimOptions = TrimOptions.Trim
        };
        using var Reader = new StreamReader(path);
        using var csv = new CsvReader(Reader, configuration);
        csv.Context.TypeConverterOptionsCache.GetOptions<DateTime>().Formats = new[] { "yyyy-MM-dd HH:mm:ss.fff", "yyyy-MM-dd HH:mm:ss" };
        var result = new List<Weather>();
        csv.Read();
        csv.ReadHeader();
        while (csv.Read())
        {
            var line = new Weather
            {
                Date = csv.GetField<DateTime>("date"),
                Tavg = csv.GetField<double?>("tavg"),
                Tmin = csv.GetField<double?>("tmin"),
                Tmax = csv.GetField<double?>("tmax"),
                Prcp = csv.GetField<double?>("prcp"),
                Snow = csv.GetField<double?>("snow"),
                Wdir = csv.GetField<double?>("wdir"),
                Wspd = csv.GetField<double?>("wspd"),
                Wpgt = csv.GetField<double?>("wgpt"),
                Pres = csv.GetField<double?>("pres"),
                Tsun = csv.GetField<double?>("tsun"),

            };
            result.Add(line);
        }
        return result;
    }


}

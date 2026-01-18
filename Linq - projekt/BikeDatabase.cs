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

public static class BikeDataBase
{
    public static List<Bikes> LoadBikes(string path)
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
        var bikes = new List<Bikes>();
        csv.Read();
        csv.ReadHeader();
        while (csv.Read())
        {
            var line = new Bikes
            {
                RideId = csv.GetField("ride_id"),
                RideableType = csv.GetField("rideable_type"),
                StartedAt = csv.GetField<DateTime>("started_at"),
                EndedAt = csv.GetField<DateTime>("ended_at"),
                StartStationName = csv.GetField("start_station_name"),
                EndStationName = csv.GetField("end_station_name"),
                EndStationId = csv.GetField("end_station_id"),
                StartLat = csv.GetField<double?>("start_lat"),
                StartLng = csv.GetField<double?>("start_lng"),
                MemberCasual = csv.GetField("member_casual"),

            };
            bikes.Add(line);
        }
        return bikes;
    }


}

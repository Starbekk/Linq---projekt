namespace Linq;
internal class Program
{
    static void Main(string[] args)
    {
        var bikes = BikeDataBase.LoadBikes(@"C:\Users\User\Desktop\C#\Linq - projekt\Linq - projekt\JC-202507-citibike-tripdata.csv");
        var weather = WeatherDatabase.LoadWeather(@"C:\Users\User\Desktop\C#\Linq - projekt\Linq - projekt\WeatherNYC.csv");


        Console.WriteLine("Liczba wypożyczeń rowerów w zależności od temperatury:");
        Console.WriteLine("Data           Temperatura               Liczba wypożyczonych rowerów");

        var bikeByDate = bikes.GroupBy(b => b.StartedAt.Date).Select(g => new
        {
            Date = g.Key,
            Count = g.Count()
        });

        var weatherByDate = weather.Where(w=>w.Tavg.HasValue).Select(w => new
        {
            Date = w.Date.Date,
            Temperature = w.Tavg.Value
        });

        var BikesByTemp = bikeByDate.Join(weatherByDate, b => b.Date, w => w.Date, (b, w) => new
        {
            Date = b.Date,
            Temperature = w.Temperature,
            Ride = b.Count
        });

        foreach (var b in BikesByTemp.OrderBy(x => x.Ride))
        {
            Console.WriteLine($"{b.Date:yyyy-MM-dd}     {b.Temperature} stopni Celsjusza         {b.Ride}");
        }


        Console.WriteLine();
        Console.WriteLine();
        Console.WriteLine("Najwyższa temperatura w lipcu:");
        var MaxTemp = weather.Where(w => w.Tavg.HasValue).Max(w => w.Tavg.Value);

        Console.WriteLine($"{MaxTemp} stopni Celsjusza");


        Console.WriteLine();
        Console.WriteLine();
        Console.WriteLine("Rodzaj najczęściej wypożyczanego roweru:");

        var MostPopBike = bikes.GroupBy(g => g.RideableType).Select(w => new
        {
            Type = w.Key,
            Count = w.Count()
        }).OrderByDescending(x => x.Count).First();
        Console.WriteLine(MostPopBike.Type);

        Console.WriteLine();
        Console.WriteLine();
        Console.WriteLine("Liczba wypożyczanych rowerów w deszczowe dni:");
        Console.WriteLine("Data           Temperatura                Deszcz      Liczba wypożyczonych rowerów");

        var RainBikes = bikeByDate.Join(weather.Where(w => w.Prcp > 0 && w.Tavg.HasValue),
            b => b.Date,
            w => w.Date.Date,
            (b, w) => new
            {
                Date = b.Date,
                Temperature = w.Tavg.Value,
                Rain = w.Prcp.Value,
                Ride = b.Count
            });
        foreach(var r in RainBikes)
        {

            Console.WriteLine($"{r.Date:yyyy-MM-dd}     {r.Temperature} stopni Celsjusza      {r.Rain}mm        {r.Ride}");
        }



        Console.WriteLine();
        Console.WriteLine();
        Console.WriteLine("Najdłuższy przejazd:");
        var LongestRide = bikes.Select(w => new
        {
            Bike = w,
            DurationMinutes = (w.EndedAt - w.StartedAt).TotalMinutes
        }).OrderByDescending (x => x.DurationMinutes).First();
        Console.WriteLine($"Czas: {LongestRide.DurationMinutes} minut");
        Console.WriteLine($"Użytkownik: {LongestRide.Bike.MemberCasual}");
        Console.WriteLine($"Typ roweru: {LongestRide.Bike.RideableType}");



        Console.WriteLine();
        Console.WriteLine();
        Console.WriteLine("Najczęściej wybierana stacja końcowa:");
        var PopularStation = bikes.Where(b => !string.IsNullOrWhiteSpace(b.EndStationName)).GroupBy(b => b.EndStationName).Select(g => new
        {
            Station = g.Key,
            Count = g.Count()
        }).OrderByDescending(x => x.Count).First();
        Console.WriteLine($"Liczba: {PopularStation.Count}, Stacja: {PopularStation.Station}");



        Console.WriteLine();
        Console.WriteLine();
        Console.WriteLine("Najczęstszy typ użytkownika:");
        var UserType = bikes.GroupBy(b => b.MemberCasual).Select(g => new
        {
            UserType = g.Key,
            Count = g.Count()
        }).OrderByDescending(x=>x.Count);
        foreach(var b in UserType)
        {
            Console.WriteLine($"Typ użytkownika: {b.UserType}, liczba wypożyczeń: {b.Count}");
        }

        Console.WriteLine();
        Console.WriteLine();
        Console.WriteLine("Średni czas przejazdu na typ użytkownika:");
        var AvgRide = bikes.Where(b => b.EndedAt > b.StartedAt).GroupBy(b => b.MemberCasual).Select(g => new
        {
            UserType = g.Key,
            Avg = g.Average(b => (b.EndedAt - b.StartedAt).TotalMinutes)
        });
        foreach(var b in AvgRide)
        {
            Console.WriteLine($"Typ użytkownika: {b.UserType}, średni czas przejazdu: {b.Avg}");
        }
    }
}
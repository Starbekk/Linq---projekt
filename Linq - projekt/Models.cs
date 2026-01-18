using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Linq;
public class Bikes
{
    public string? RideId { get; set; }
    public string? RideableType { get; set; }
    public DateTime StartedAt {get; set; }
    public DateTime EndedAt { get; set; }
    public string? StartStationName { get; set; }
    public string? EndStationName { get; set; }
    public string? EndStationId { get; set; }
    public double? StartLat { get; set; }
    public double? StartLng { get; set; }
    public string? MemberCasual {  get; set; }
    public double Duration => (EndedAt - StartedAt).TotalMinutes;
}

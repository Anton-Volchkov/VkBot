using System;
using Newtonsoft.Json;

namespace CurrencyConverter.Models;

public class Currency
{
    [JsonProperty("Cur_ID")] public long Id { get; set; }

    [JsonProperty("Date")] public DateTimeOffset Date { get; set; }

    [JsonProperty("Cur_Abbreviation")] public string Abbreviation { get; set; }

    [JsonProperty("Cur_Scale")] public long Scale { get; set; }

    [JsonProperty("Cur_Name")] public string Name { get; set; }

    [JsonProperty("Cur_OfficialRate")] public double OfficialRate { get; set; }
}
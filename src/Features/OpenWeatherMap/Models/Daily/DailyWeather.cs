﻿using Newtonsoft.Json;

namespace OpenWeatherMap.Models.Daily;

public class DailyWeather
{
    [JsonProperty("lat")] public double Lat { get; set; }

    [JsonProperty("lon")] public double Lon { get; set; }

    [JsonProperty("timezone")] public string Timezone { get; set; }

    [JsonProperty("timezone_offset")] public long TimezoneOffset { get; set; }

    [JsonProperty("daily")] public Daily[] Daily { get; set; }
}

public class Daily
{
    [JsonProperty("dt")] public long Dt { get; set; }

    [JsonProperty("sunrise")] public long Sunrise { get; set; }

    [JsonProperty("sunset")] public long Sunset { get; set; }

    [JsonProperty("temp")] public Temp Temp { get; set; }

    [JsonProperty("feels_like")] public FeelsLike FeelsLike { get; set; }

    [JsonProperty("pressure")] public long Pressure { get; set; }

    [JsonProperty("humidity")] public long Humidity { get; set; }

    [JsonProperty("dew_point")] public double DewPoint { get; set; }

    [JsonProperty("wind_speed")] public double WindSpeed { get; set; }

    [JsonProperty("wind_deg")] public long WindDeg { get; set; }

    [JsonProperty("weather")] public Weather[] Weather { get; set; }

    [JsonProperty("clouds")] public long Clouds { get; set; }

    [JsonProperty("rain", NullValueHandling = NullValueHandling.Ignore)]
    public double? Rain { get; set; }

    [JsonProperty("uvi")] public double Uvi { get; set; }
}

public class FeelsLike
{
    [JsonProperty("day")] public double Day { get; set; }

    [JsonProperty("night")] public double Night { get; set; }

    [JsonProperty("eve")] public double Eve { get; set; }

    [JsonProperty("morn")] public double Morn { get; set; }
}

public class Temp
{
    [JsonProperty("day")] public double Day { get; set; }

    [JsonProperty("min")] public double Min { get; set; }

    [JsonProperty("max")] public double Max { get; set; }

    [JsonProperty("night")] public double Night { get; set; }

    [JsonProperty("eve")] public double Eve { get; set; }

    [JsonProperty("morn")] public double Morn { get; set; }
}

public class Weather
{
    [JsonProperty("id")] public long Id { get; set; }

    [JsonProperty("main")] public string Main { get; set; }

    [JsonProperty("description")] public string Description { get; set; }

    [JsonProperty("icon")] public string Icon { get; set; }
}
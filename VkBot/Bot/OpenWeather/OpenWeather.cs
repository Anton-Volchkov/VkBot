﻿using Newtonsoft.Json;

namespace VkBot.Bot.OpenWeather
{
    public class OpenWeather
    {
        public coord coord;

        public weather[] weather;

        [JsonProperty("base")]
        public string Base;

        public main main;

        public double visibility;

        public wind wind;

        public clouds clouds;

        public double dt;

        public sys sys;

        public int id;

        public string name;

        public double cod;
    }
}
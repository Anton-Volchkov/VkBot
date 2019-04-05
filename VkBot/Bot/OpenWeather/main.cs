namespace VkBot.Bot.OpenWeather
{
    public class main
    {
        public double temp { get; set; }

        private double _pressure;

        public double pressure
        {
            get => _pressure;
            set => _pressure = value / 1.3332239;
        }

        public double humidity;

        private double _temp_min;

        public double temp_min
        {
            get => _temp_min;
            set => temp = value;
        }

        private double _temp_max;

        public double temp_max
        {
            get => _temp_max;
            set => temp = value;
        }
    }
}
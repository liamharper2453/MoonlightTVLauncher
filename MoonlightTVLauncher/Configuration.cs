using Microsoft.Extensions.Configuration;

namespace MoonlightTVLauncher
{
    internal class Configuration
    {
        internal Configuration()
        {
            var builder = new ConfigurationBuilder()
            .AddJsonFile(File.Exists("appsettings - DEV.json") ? "appsettings - DEV.json" : 
            "appsettings.json", optional: false, reloadOnChange: true);

            var configurationRoot = builder.Build();

            ConfigurationBinder.Bind(configurationRoot, this);

            if (string.IsNullOrEmpty(this.TvClientKey))
            {
                Console.WriteLine("You don't have a TV Client key set. Please run GetWebOSClientKey.py to get one.");
                Console.Read();
                Environment.Exit(0);
            }

            if (string.IsNullOrEmpty(this.TvIpAddress))
            {
                Console.WriteLine("Your TV IP address is missing. Please set it and try again.");
                Console.Read();
                Environment.Exit(0);
            }

            if (string.IsNullOrEmpty(this.TvMoonlightGameIndex))
            {
                Console.WriteLine("Your Moonlight game index is missing. Please set it and try again.");
                Console.Read();
                Environment.Exit(0);
            }

            if (this.OriginalResolutionX == default(int) ||
               this.OriginalResolutionY == default(int) ||
               this.OriginalResolutionHz == default(int) ||
               this.StreamResolutionX == default(int) ||
               this.StreamResolutionY == default(int) ||
               this.StreamResolutionHz == default(int))
            {
                Console.WriteLine("Your resolution parameters have not been set correctly. Please set them and try again.");
                Console.Read();
                Environment.Exit(0);
            }
        }

        public string? TvMacAddress { get; set; }
        public string? TvIpAddress { get; set; }
        public string? TvClientKey { get; set; }
        public string? TvMoonlightGameIndex { get; set; }
        public string? PcApplicationBeingLaunched { get; set; }
        public string? PcApplicationUsedForStreaming { get; set; }
        public int OriginalResolutionX { get; set; }
        public int OriginalResolutionY { get; set; }
        public int OriginalResolutionHz { get; set; }
        public int StreamResolutionX { get; set; }
        public int StreamResolutionY { get; set; }
        public int StreamResolutionHz { get; set; }
    }
}

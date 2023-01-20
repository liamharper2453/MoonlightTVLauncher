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

            var configurationReady = true;
            if (string.IsNullOrEmpty(TvClientKey))
            {
                configurationReady = false;
                Console.WriteLine("You don't have a TV Client key set. Please run GetWebOSClientKey.py to get one.");
            }

            if (string.IsNullOrEmpty(TvIpAddress))
            {
                configurationReady = false;
                Console.WriteLine("Your TV IP address is missing. Please set it and try again.");
            }

            if (string.IsNullOrEmpty(TvMoonlightGameIndex))
            {
                configurationReady = false;
                Console.WriteLine("Your Moonlight game index is missing. Please set it and try again.");
            }

            if (string.IsNullOrEmpty(PcApplicationBeingLaunched))
            {
                configurationReady = false;
                Console.WriteLine("You haven't specified the PC application to be launched. Please set it and try again.");
            }

            if (string.IsNullOrEmpty(PcApplicationUsedForStreaming))
            {
                configurationReady = false;
                Console.WriteLine("You haven't specified the PC application used for streaming. Please set it and try again.");
            }

            if (string.IsNullOrEmpty(TvClientKey))
            {
                configurationReady = false;
                Console.WriteLine("You don't have a TV Client key set. Please run GetWebOSClientKey.py to get one.");
            }

            if (OriginalResolutionX == null ||
               OriginalResolutionY == null ||
               OriginalResolutionHz == null ||
               StreamResolutionX == null ||
               StreamResolutionY == null ||
               StreamResolutionHz == null)
            {
                configurationReady = false;
                Console.WriteLine("Your resolution parameters have not been set. Please set them and try again.");
            }

            if (!configurationReady)
            {
                Console.WriteLine("\nThe application has now stopped. Please close and try again.");
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
        public int? OriginalResolutionX { get; set; }
        public int? OriginalResolutionY { get; set; }
        public int? OriginalResolutionHz { get; set; }
        public int? StreamResolutionX { get; set; }
        public int? StreamResolutionY { get; set; }
        public int? StreamResolutionHz { get; set; }
    }
}

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

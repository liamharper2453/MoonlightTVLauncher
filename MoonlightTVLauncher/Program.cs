using Microsoft.Win32;
using MoonlightTVLauncher;
using System.Diagnostics;
using XInputDotNetPure;

var configuration = new Configuration();
var periodicTimer = new PeriodicTimer(TimeSpan.FromSeconds(1));
var moonlightTvStreamActive = false;
var waitingToWakeFromSleep = false;

//Create event to monitor for waking up of PC
SystemEvents.PowerModeChanged += OnPowerChange;

await WaitForConnection();

async Task WaitForConnection()
{
    while (await periodicTimer.WaitForNextTickAsync())
    {
        var connected = GamePad.GetState(PlayerIndex.One).IsConnected;

        if (waitingToWakeFromSleep)
        {
            continue;
        }

        if (connected)
        {
            if (moonlightTvStreamActive)
            {
                //Continually hide cursor in top-left
                DesktopUtilities.SetCursorPos(-1, 9999);
                continue;
            }

            var startStreamTask = Task.Run(() => StartStream());

            if (!startStreamTask.Wait(TimeSpan.FromSeconds(120)))
            {
                Console.WriteLine("Could not start stream. Please check your settings parameters and try again.");
                EndSession();
                Environment.Exit(0);
            }
            else
            {
                moonlightTvStreamActive = true;
            }
        }
        else if (moonlightTvStreamActive)
        {
            EndSession();
        }
    }
}

void RunCommand(string arguments)
{
    var process = new Process();
    process.StartInfo.FileName = "cmd.exe";
    process.StartInfo.Arguments = arguments;
    process.StartInfo.CreateNoWindow = true;
    process.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
    process.Start();
}

void EndSession()
{
    RunCommand("/c ares-launch -d tv --close com.limelight.webos");
    RunCommand("/c explorer.exe");
    RunCommand(@"/c Dependencies/ChangeScreenResolution.exe /w=" + configuration.OriginalResolutionX + "/h=" + configuration.OriginalResolutionY + "/f=" + configuration.OriginalResolutionHz  + "/ d=0");
    RunCommand("/c taskkill /F /IM" + configuration.PcApplicationUsedForStreaming);
    RunCommand("/c taskkill /IM " + configuration.PcApplicationBeingLaunched);
    moonlightTvStreamActive = false;
    //Wait for explorer to start up again before switching back to main desktop
    Thread.Sleep(5000);
    RunCommand("/c Dependencies/nircmdc sendkeypress rwin+ctrl+left");
}

bool StartStream()
{
    while (Process.GetProcessesByName("nvstreamer").Length == 0)
    {
        WakeOnLan.Wake(configuration.TvMacAddress);
        RunCommand("/c Dependencies/nircmdc sendkeypress rwin+ctrl+right");
        //Wait for desktop to switch BEFORE killing explorer
        Thread.Sleep(1000);
        RunCommand("/c taskkill /F /IM explorer.exe");
        RunCommand(@"/c Dependencies/ChangeScreenResolution.exe /w=" + configuration.StreamResolutionX + "/h=" + configuration.StreamResolutionY + "/f=" + configuration.StreamResolutionHz + "/ d=0");
        RunCommand("/c ares-launch -d tv com.limelight.webos");
        //Give TV some time to open the MoonlightTV app before attempting TV input
        Thread.Sleep(3000);
        RunCommand("/c py Scripts/MoonlightTVAutoConnect.py" + " " + configuration.TvIpAddress + " " + configuration.TvClientKey + " " + configuration.TvMoonlightGameIndex);
    }

    if (configuration.TvClientKey == "EMPTY")
    {
        configuration = new Configuration();
    }

    return true;
}

void OnPowerChange(object s, PowerModeChangedEventArgs e)
{
    switch (e.Mode)
    {
        case PowerModes.Resume:
            waitingToWakeFromSleep = true;
            //Wait for PC to wake up and re-establish network connectivity
            Thread.Sleep(10000);
            waitingToWakeFromSleep = false;
            break;
    }
}
using Microsoft.Win32;
using MoonlightTVLauncher;
using System.Diagnostics;
using XInputDotNetPure;

var configuration = new Configuration();
var periodicTimer = new PeriodicTimer(TimeSpan.FromSeconds(1));
var moonlightTvStreamActive = false;
var waitingToWakeFromSleep = false;

Console.WriteLine("Hello! The application has started successfully. I am now waiting for a controller.\n");
Console.WriteLine("Once the controller is detected, I will try to connect to the TV with IP address " + configuration.TvIpAddress + " to start the Moonlight session.\n");
Console.WriteLine("The executable I am going to look for on this PC to know the stream has started is " + configuration.PcApplicationUsedForStreaming + ".\n");
Console.WriteLine("The application I aim to launch when the stream starts is " + configuration.PcApplicationBeingLaunched + ".\n");
Console.WriteLine("When the stream starts I will set the resolution of this PC to " + configuration.StreamResolutionX + "x" + configuration.StreamResolutionY + "@" + configuration.StreamResolutionHz + ".\n");
Console.WriteLine("When the stream ends I will return the resolution of this PC to " + configuration.OriginalResolutionX + "x" + configuration.OriginalResolutionY + "@" + configuration.OriginalResolutionHz + ".\n");
Console.WriteLine("-----------------------------------------------------------------------------------");
//Create event to monitor for waking up of PC
SystemEvents.PowerModeChanged += OnPowerChange;

try
{
    await WaitForConnection();
}
catch (Exception ex)
{
    WriteTimestampedLogToConsole("I have encountered an error: " + ex.Message.ToString());
    WriteTimestampedLogToConsole("The application has now stopped. Please close and try again.");
    Console.Read();
    Environment.Exit(0);
}

async Task WaitForConnection()
{
    while (await periodicTimer.WaitForNextTickAsync())
    {
        var connected = IsControllerConnected();

        if (waitingToWakeFromSleep)
        {
            continue;
        }

        if (connected)
        {
            if (moonlightTvStreamActive)
            {
                //Continually hide cursor in bottom left of screen. Many applications like to put it in the middle of your screen when they open and they keep it there!
                InteropUtilities.SetCursorPos(-1, 9999);
                continue;
            }

            WriteTimestampedLogToConsole("Controller detected. Starting stream and trying to open " + configuration.PcApplicationBeingLaunched);

            var startStreamTask = Task.Run(() => StartStream());

            if (!startStreamTask.Wait(TimeSpan.FromSeconds(120)))
            {
                WriteTimestampedLogToConsole("Could not start stream. Please check your settings parameters and your network and try again.");
                EndSession();
                Console.Read();
                Environment.Exit(0);
            }
            else
            {
                moonlightTvStreamActive = true;
            }
        }
        else if (moonlightTvStreamActive)
        {
            WriteTimestampedLogToConsole("Controller disconnected. Ending stream.");
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
    process.StartInfo.UseShellExecute = false;
    process.Start();
}

void EndSession()
{
    //com.limelight.webos = Moonlight TV
    RunCommand("/c ares-launch -d tv --close com.limelight.webos");
    RunCommand("/c explorer.exe");
    RunCommand("/c \"" + Environment.CurrentDirectory + @"\Dependencies\ChangeScreenResolution.exe" + "\"" + " /w=" + configuration.OriginalResolutionX + " /h=" + configuration.OriginalResolutionY + " /f=" + configuration.OriginalResolutionHz + " /d=0");
    RunCommand("/c taskkill /F /IM " + configuration.PcApplicationUsedForStreaming);
    RunCommand("/c taskkill /IM " + configuration.PcApplicationBeingLaunched);
    moonlightTvStreamActive = false;
    //Wait for explorer to start up again before switching back to main desktop
    Thread.Sleep(5000);
    RunCommand("/c \"" + Environment.CurrentDirectory + @"\Dependencies\nircmdc" + "\"" + " sendkeypress rwin+ctrl+left");
}

bool StartStream()
{
    while (Process.GetProcessesByName(configuration.PcApplicationUsedForStreaming?.Replace(".exe", "")).Length == 0)
    {
        if (!string.IsNullOrEmpty(configuration.TvMacAddress))
        {
            WakeOnLan.Wake(configuration.TvMacAddress);
        }

        RunCommand("/c \"" + Environment.CurrentDirectory + @"\Dependencies\nircmdc" + "\"" + " sendkeypress rwin+ctrl+right");
        //Wait for desktop to switch BEFORE killing explorer
        Thread.Sleep(1000);
        //Why do we kill explorer.exe you ask? Because it does not like when resolutions are changed
        //For me, it always crashes the process and when the process automatically restarts it results in a taskbar overlaying my application that I just opened
        RunCommand("/c taskkill /F /IM explorer.exe");
        RunCommand("/c \"" + Environment.CurrentDirectory + @"\Dependencies\ChangeScreenResolution.exe" + "\"" + " /w=" + configuration.StreamResolutionX + " /h=" + configuration.StreamResolutionY + " /f=" + configuration.StreamResolutionHz + " /d=0");
        //com.limelight.webos = Moonlight TV
        RunCommand("/c ares-launch -d tv com.limelight.webos");
        //Give TV some time to open the MoonlightTV app before attempting TV input
        Thread.Sleep(3000);
        RunCommand("/c py \"" + Environment.CurrentDirectory + @"\Scripts\MoonlightTVAutoConnect.py" + "\"" + " " + configuration.TvIpAddress + " " + configuration.TvClientKey + " " + configuration.TvMoonlightGameIndex);
    }

    WriteTimestampedLogToConsole("Streaming application started (" + configuration.PcApplicationUsedForStreaming + "). Output should be appearing on your TV now.");

    return true;
}

void OnPowerChange(object s, PowerModeChangedEventArgs e)
{
    switch (e.Mode)
    {
        case PowerModes.Resume:
            if (IsControllerConnected()) {
                waitingToWakeFromSleep = true;
                WriteTimestampedLogToConsole("PC has been woken up by controller. Getting ready to start stream.");
                //Wait for PC to wake up and re-establish network connectivity
                Thread.Sleep(10000);
                waitingToWakeFromSleep = false;
            }
            break;
    }
}

bool IsControllerConnected()
{
    return GamePad.GetState(PlayerIndex.One).IsConnected;
}

void WriteTimestampedLogToConsole(string text)
{
    Console.WriteLine("[" + DateTime.Now.ToString() + "] " + text);
}
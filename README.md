# MoonlightTVLauncher

Moonlight TV Launcher allows you to start and stop a Moonlight stream on your TV with only a game controller.

> This is not at all affiliated with Moonlight or Moonlight TV, this is just a fun side project!

[![Github All Releases](https://img.shields.io/github/downloads/liamharper2453/MoonlightTVLauncher/total.svg)]()

# Features

## Controller on

 - Turn on your PC and TV (read FAQ below).
 - Control a webOS TV and automatically open the Moonlight TV app.
 - Navigate the Moonlight TV app to select a game/launcher.
 - Start the stream!

https://user-images.githubusercontent.com/25060863/213593229-30248388-b0da-4cb3-8f11-8d23d1722d32.mp4


## Controller off

 - The Moonlight TV app will be closed and you will be returned back to where the TV was before.
 - The stream will end.
 - The host PC will return back to the state it was prior to the stream starting.

https://user-images.githubusercontent.com/25060863/213593024-3e326b66-adeb-47ab-83d7-7f5bc70c74cf.mp4

# How to use

## What do I need to use this? 
 - A rooted webOS TV with the Moonlight TV app installed.
 - A game controller that can connect to your PC.
 - A Gamestream host on your PC. For example, NVIDIA Gamestream.
 - Python 3
 - [pywebostv](https://pypi.org/project/pywebostv/)
 - [ares-cli](https://www.npmjs.com/package/@webosose/ares-cli)
 - Windows

## How do I run it?
You need to first populate the appsettings.json file with values relevant to your setup.

Here is an example of what your appsettings.json should look like when you are done:

![devenv_LBFy89KKMj](https://user-images.githubusercontent.com/25060863/213511867-8c1f41b1-ec65-43ab-97fb-93d69a5c62d9.png)

For the TvMoonlightGameIndex parameter, you need to enter the position of the game/launcher you want to launch. So using the screenshot below if I was wanting to launch Black Mesa for example, the value I would use would be '3'.

![moonlight](https://user-images.githubusercontent.com/830358/141690137-529d3b94-b56a-4f24-a3c5-00a56eb30952.png)

For the TvClientKey parameter, you will need to run the included script GetWebOSClientKey.py in the command line. As part of running this, you will be prompted to accept a request on your TV. Once you click accept on your TV you will then be given a client key that you can put into the appsettings.json file.

![WindowsTerminal_vd5P0yL2pE](https://user-images.githubusercontent.com/25060863/213506005-7bd970d8-b268-465b-85c4-445162539e14.png)

You also probably want to make a second desktop. To do this press Win+CTRL+D (and then press Win+CTRL+Left to come back).	The reason for this is the application will try and use a different desktop to your main one (as you probably don't want to see what is in the background etc if you are using a game launcher).

After all of that is done, just run the executable. 

What it will do is run in the background and wait for a controller to be turned on. When this happens, it will then activate and start your Moonlight stream. If you turn your controller off, it will close everything and return your PC and TV to the state it was prior to the stream.

# FAQ's
 
## **Why are you doing this? Can't you just use your TV remote?**

So my main motivations for this are:

 - It's fun!
 - My TV is in my living-room and my PC is upstairs, and I don't have a spare PC to connect to my TV directly.
 - My TV remote is awful and the batteries only last around a day or two. So I thought to myself, wouldn't it be cool if I didn't have to use the remote at all?

 
## **What about if my TV is off?**

Using Wake-on-Lan, this project can turn your TV on! (assuming it is on stand-by and not *completely* off)

If you want this to happen, populate the TvMacAddress setting in appsettings.json. On your TV you should ensure Quick Start+ is enabled.

For me it can take some time for the Wake-on-Lan to start the TV but it should eventually come on.

## **What about if my PC is off?**

If your PC is in hibernate/sleep, what you can do is go into Device Manager and set your Xbox Wireless Adapter to be able to wake your PC.

![mmc_rcCqr10yOl](https://user-images.githubusercontent.com/25060863/213506222-58df13db-7979-4e3b-8f84-fa0b7470bf77.gif)

If you combine this with Wake-on-Lan for your TV it means just by turning your game controller on you can start your PC, TV and your Moonlight stream!

# Credits
 - [ares-cli](https://github.com/webosose/ares-cli) for providing the wrapper for the webOS api's
 - [ChangeScreenResolution](https://tools.taubenkorb.at/change-screen-resolution/) for creating the tool to be able to programmatically change resolutions
 - [Moonlight TV](https://github.com/mariotaku/moonlight-tv) for creating the GameStream client
 - [NirCmd](https://www.nirsoft.net/utils/nircmd2.html) for creating the tool to programmatically send key presses
 - [PyWebOSTV](https://github.com/supersaiyanmode/PyWebOSTV) for creating the tool to pass input remotely to the TV

from pywebostv.discovery import *
from pywebostv.connection import *
from pywebostv.controls import *
import sys

def Authenticate(tvIpAddress, tvClientKey, tvMoonlightGameIndex, client):
    if tvClientKey == "EMPTY":
        store = {}
    else:
         store = { "client_key": tvClientKey }

    for status in client.register(store):
        if status == WebOSClient.PROMPTED:
            print("Please accept the connection request on your TV")
        elif status == WebOSClient.REGISTERED:
            print("Connected to TV")
            if tvClientKey == "EMPTY":
                tvClientKey = store.values[0]
                with open("appsettings.json") as x:
                    configuration = json.load(x)
                    configuration["TvClientKey"] = tvClientKey
                    json.dump(configuration, x)

def Main():
    tvIpAddress = sys.argv[1]
    tvClientKey = sys.argv[2]
    tvMoonlightGameIndex = sys.argv[3]

    client = WebOSClient(tvIpAddress)
    client.connect()

    Authenticate(tvIpAddress, tvClientKey, tvMoonlightGameIndex, client)

    inp = InputControl(client)
    inp.connect_input()

    for _ in range(int(tvMoonlightGameIndex)):
        inp.right()

    inp.ok()
    inp.disconnect_input()

Main()


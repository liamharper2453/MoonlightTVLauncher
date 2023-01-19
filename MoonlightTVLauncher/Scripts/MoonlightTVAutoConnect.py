from pywebostv.discovery import *
from pywebostv.connection import *
from pywebostv.controls import *
import sys

def Authenticate(tvClientKey, client):
    store = { "client_key": tvClientKey }

    for status in client.register(store):
        if status == WebOSClient.PROMPTED:
            print("Did you run GetWebOSClientKey.py?")
        elif status == WebOSClient.REGISTERED:
            print("Connected to TV")

def Main():
    tvIpAddress = sys.argv[1]
    tvClientKey = sys.argv[2]
    tvMoonlightGameIndex = sys.argv[3]

    client = WebOSClient(tvIpAddress)
    client.connect()

    Authenticate(tvClientKey, client)

    inp = InputControl(client)
    inp.connect_input()

    for _ in range(int(tvMoonlightGameIndex)):
        inp.right()

    inp.ok()
    inp.disconnect_input()

Main()


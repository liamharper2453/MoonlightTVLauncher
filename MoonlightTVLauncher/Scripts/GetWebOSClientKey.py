from pywebostv.discovery import *
from pywebostv.connection import *
from pywebostv.controls import *
import sys

def GetClientKey(client):
    store = { }

    for status in client.register(store):
        if status == WebOSClient.PROMPTED:
            print("Please accept the connection request on your TV")
  
    print("Your client key is: ", store)

def Main():
    tvIpAddress = sys.argv[1]

    client = WebOSClient(tvIpAddress)
    client.connect()

    GetClientKey(client)

Main()


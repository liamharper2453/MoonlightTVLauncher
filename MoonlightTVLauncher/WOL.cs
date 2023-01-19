using System.Net;
using System.Net.Sockets;
using System.Globalization;

internal static class WOL
{
    internal static void Wake(string macaddress)
    {
        var udpClient = new UdpClient();

        //Enable UDP broadcasting for UdpClient
        udpClient.EnableBroadcast = true;

        var dataGram = new byte[1024];

        //6 magic bytes
        for (int i = 0; i < 6; i++)
        {
            dataGram[i] = 255;
        }

        //Convert MAC-address to bytes
        byte[] address_bytes = new byte[6];
        for (int i = 0; i < 6; i++)
        {
            address_bytes[i] = byte.Parse(macaddress.Substring(3 * i, 2), NumberStyles.HexNumber);
        }

        //Repeat MAC address 16 times in the datagram
        var macaddress_block = dataGram.AsSpan(6, 16 * 6);
        for (int i = 0; i < 16; i++)
        {
            address_bytes.CopyTo(macaddress_block.Slice(6 * i));
        }

        //Send datagram using UDP and port 9
        udpClient.Send(dataGram, dataGram.Length, new IPEndPoint(IPAddress.Broadcast, 9));
        udpClient.Close();
    }
}
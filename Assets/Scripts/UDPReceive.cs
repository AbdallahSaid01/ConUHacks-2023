using UnityEngine;
using System;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading;

public class UDPReceive : MonoBehaviour
{

    Thread receiveThread;
    UdpClient client; 
    public int port = 5052;
    public bool startRecieving = true;
    public bool printToConsole = false;
    public string data;
    public string[] gestureMovementArr;


    public void Start()
    {

        receiveThread = new Thread(
            new ThreadStart(ReceiveData));
        receiveThread.IsBackground = true;
        receiveThread.Start();
    }


    // receive thread
    private void ReceiveData()
    {

        client = new UdpClient(port);
        while (startRecieving)
        {

            try
            {
                IPEndPoint anyIP = new IPEndPoint(IPAddress.Any, 0);
                byte[] dataByte = client.Receive(ref anyIP);
                data = Encoding.UTF8.GetString(dataByte);

                //gestures can be: pointer, open, close, ok
                //movement can be: clockwise, counter clockwise, move, stop
                gestureMovementArr = data.Split(',');

                if (printToConsole) { print(gestureMovementArr[0] + " " + gestureMovementArr[1]); }
            }
            catch (Exception err)
            {
                print(err.ToString());
            }
        }
    }

}

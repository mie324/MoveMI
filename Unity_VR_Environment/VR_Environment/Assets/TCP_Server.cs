using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net;
using System.Net.Sockets;
using System.Linq;

public class TCP_Server : MonoBehaviour {

    public string IP_address = "127.0.0.1";
    public int port = 1000;
    public Socket client_socket;

    public void receive()
    {
        client_socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        client_socket.Connect(IP_address, port);

        byte[] received_bytes = new byte[1024];
        int raw = client_socket.Receive(received_bytes);
        string message_received = System.Text.Encoding.ASCII.GetString(received_bytes, 0, raw);

        if(client_socket.Connected)
        {
            Debug.Log(message_received);
        } else
        {
            Debug.Log("Not connected");
        }
        client_socket.Close();
    }

	// Use this for initialization
	void Start () {
        receive();
	}
	
	// Update is called once per frame
	void Update () {
        receive();
	}
}

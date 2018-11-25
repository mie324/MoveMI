using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net;
using System.Net.Sockets;
using System.Linq;
using System;

public class Socket_Receive : MonoBehaviour {

	private static string IP_address = "127.0.0.1";
	private static int port = 2048;
	UdpClient client;
	IPEndPoint IP_end_point;

	public void UDP_setup() {
		client = new UdpClient(port);
		client.Client.ReceiveBufferSize = 256; // Set buffer size to be as small as possible to minimize buffer clog (received data is up to 100 bytes approximately so set buffer size to 256 considering factor of safety of approximately 2.5
		IP_end_point = new IPEndPoint(IPAddress.Parse(IP_address), 0);
	}

	public void UDP_receive() {
		byte[] received_bytes = client.Receive(ref IP_end_point);
		string message_received = System.Text.Encoding.ASCII.GetString(received_bytes);
		string gesture = message_received.Substring(0, 1);
		//Debug.Log(gesture);
		Debug.Log(message_received);
	}

	// Use this for initialization
	void Start () {
		UDP_setup();	
	}
	
	// Update is called once per frame
	void Update () {
		UDP_receive();
	}
}

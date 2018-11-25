using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net;
using System.Net.Sockets;
using System.Linq;
using System;

public class Hand_Manipulate : MonoBehaviour {

    private static string IP_address = "127.0.0.1";
    private static int port = 2048;
    UdpClient client;
    IPEndPoint IP_end_point;

    public GameObject hand;

    private bool moving;
    private float start_time;
    private float[] angles = new float[5];

    // dim0: 0 = thumb, 1 = pointer, 2 = middle, 3 = ring, 4 = pinky
    // dim1: 0 = default, 1 = high, 2 = low
    // dim2: 0 = outermost object, 1 = second object, 2 = third object, 3 = innermost object
    float[,,] finger_values = new float[5, 3, 4]
    {
        {
            {254f, 237.6f, 238.6f, 238.6f}, {263f, 275.5f, 279.4f, 238.6f}, {245.6f, 226.2f, 194.7f, 238.6f}
        },
        {
            {345f, 323.5f, 316.7f, 316.7f}, {3.7f, 0.6f, 4.8f, 316.7f}, {269.4f, 270.3f, 252.7f, 316.7f}
        },
        {
            {351.1f, 323.9f, 316.3f, 316.3f}, {3.4f, 4.3f, 357.2f, 316.3f}, {289.7f, 279.4f, 235.2f, 316.3f}
        },
        {
            {350.1f, 326.7f, 317.1f, 317.1f}, {15.4f, 4.0f, 5.1f, 317.1f}, {297.9f, 270.6f, 246.5f, 317.1f}
        },
        {
            {179.9f, 345.5f, 326.6f, 321.2f}, {179.8f, 351.6f, 341.6f, 333.3f}, {166.1f, 317.6f, 239.1f, 274.9f}
        }
    };

    int[] curr_finger_positions = new int[5] { 0, 0, 0, 0, 0 };
    int[] new_finger_positions = new int[5];

    public void UDP_receive()
    {

        byte[] received_bytes = client.Receive(ref IP_end_point);
        string message_received = System.Text.Encoding.ASCII.GetString(received_bytes);
	string gesture = message_received.Substring(0, 1);

        Debug.Log(gesture);
		/*new_finger_positions[0] = 0;
		new_finger_positions[1] = 2;
		new_finger_positions[2] = 1;
		new_finger_positions[3] = 1;
		new_finger_positions[4] = 1;
		for(int i = 0; i < 5; i++) fingerMove(i, new_finger_positions[i]);*/
	if(gesture == "0") {
		new_finger_positions[0] = 2;
		new_finger_positions[1] = 2;
		new_finger_positions[2] = 2;
		new_finger_positions[3] = 2;
		new_finger_positions[4] = 2;
		for(int i = 0; i < 5; i++) fingerMove(i, new_finger_positions[i]);
	} else if(gesture == "1") {
		new_finger_positions[0] = 1;
		new_finger_positions[1] = 1;
		new_finger_positions[2] = 1;
		new_finger_positions[3] = 1;
		new_finger_positions[4] = 1;
		for(int i = 0; i < 5; i++) fingerMove(i, new_finger_positions[i]);
	} else if(gesture == "2") {
		new_finger_positions[0] = 0;
		new_finger_positions[1] = 0;
		new_finger_positions[2] = 0;
		new_finger_positions[3] = 0;
		new_finger_positions[4] = 0;
		for(int i = 0; i < 5; i++) fingerMove(i, new_finger_positions[i]);
	} else if(gesture == "3") {
		new_finger_positions[0] = 2;
		new_finger_positions[1] = 1;
		new_finger_positions[2] = 1;
		new_finger_positions[3] = 2;
		new_finger_positions[4] = 2;
		for(int i = 0; i < 5; i++) fingerMove(i, new_finger_positions[i]);
	}
    }

    void fingerMove(int finger, int action)
    {
        /* finger:
         * 0 - thumb
         * 1 - pointer
         * 2 - middle
         * 3 - ring
         * 4 - pinky
         */
        /* action:
         * 0 - return to default
         * 1 - raise
         * 2 - lower
         */

        if (!moving)
        {
            GameObject curr_finger = hand.transform.GetChild(finger + 2).gameObject;
            if (curr_finger_positions[finger] != action)
            {
                curr_finger_positions[finger] = action;
                moving = true;
                start_time = Time.time;
                for (int i = 0; i < 4; i++)
                {
			angles[i] = finger_values[finger, action, i];
			if (i < 3) curr_finger = curr_finger.transform.GetChild(0).gameObject;
                }
            }
        } else
        {
            if (Time.time - start_time <= 1)
            {
                GameObject curr_finger = hand.transform.GetChild(finger + 2).gameObject;
                for (int i = 0; i < 4; i++)
                {
			        curr_finger.transform.localEulerAngles = Vector3.Lerp(curr_finger.transform.localEulerAngles, new Vector3(curr_finger.transform.localEulerAngles.x, curr_finger.transform.localEulerAngles.y, angles[i]), Time.deltaTime);
			        if (i < 3) curr_finger = curr_finger.transform.GetChild(0).gameObject;
                }
            }
            else
            {
                moving = false;
		for(int i = 0; i < 5; i++) curr_finger_positions[i] = new_finger_positions[i];
            }
        }

    }

	// Use this for initialization
	void Start () {
        moving = false;
        /*client = new UdpClient(port);
        IP_end_point = new IPEndPoint(IPAddress.Parse(IP_address), 0);
        UDP_receive();*/

		for(int finger = 0; finger < 5; finger++) {

			GameObject curr_finger = hand.transform.GetChild(finger + 2).gameObject;
			for (int i = 0; i < 4; i++)
			{
				curr_finger.transform.localEulerAngles = new Vector3(curr_finger.transform.localEulerAngles.x, curr_finger.transform.localEulerAngles.y, finger_values[finger, 1, i]);
				if (i < 3) curr_finger = curr_finger.transform.GetChild(0).gameObject;
			}
		}
    }
	
	// Update is called once per frame
	void Update () {
		//UDP_receive();
	}
}

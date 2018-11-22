using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net;
using System.Net.Sockets;
using System.Linq;

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
            {27.229f, -16.368f, 0.9990001f, 0}, {31.7f, 21.5f, 0.9990001f, 0}, {18.8f, -27.8f, -42.96f, 0}
        },
        {
            {61.113f, -21.468f, -6.869f, 0}, {79.8f, 5.2f, 7.2f, 0}, {-14.5f, -74.7f, -70.8f, 0}
        },
        {
            {151.906f, -27.252f, -7.543f, 0}, {162, -5, 3.5f, 0}, {90.5f, -71.7f, -88.7f, 0}
        },
        {
            {-169.447f, -23.414f, -9.631001f, 0}, {-156.76f, -9.6f, 2.4f, 0}, {-221.7f, -79.5f, -80.2f, 0}
        },
        {
            {1, 1, 1, 1}, {29.5f, 172.6f, -3.98f, 6.72f}, {9.1f, 138.6f, -106.4f, -51.7f}
        }
    };

    int[] curr_finger_positions = new int[5] { 0, 0, 0, 0, 0 };

    public void UDP_receive()
    {
        byte[] received_bytes = client.Receive(ref IP_end_point);
        string message_received = System.Text.Encoding.ASCII.GetString(received_bytes);

        Debug.Log(message_received);
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
                    angles[i] = finger_values[finger, action, i] - curr_finger.transform.localRotation.z;
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
                    curr_finger.transform.Rotate(0, 0, angles[i] * Time.deltaTime);
                    if (i < 3) curr_finger = curr_finger.transform.GetChild(0).gameObject;
                }
            }
            else
            {
                moving = false;
            }
        }

    }

	// Use this for initialization
	void Start () {
        moving = false;
        client = new UdpClient(port);
        IP_end_point = new IPEndPoint(IPAddress.Parse(IP_address), 0);
        UDP_receive();
    }
	
	// Update is called once per frame
	void Update () {
        UDP_receive();
        /*fingerMove(1, 1);
        GameObject curr_finger = hand.transform.GetChild(0 + 2).gameObject;
        Debug.Log(curr_finger.name);
        Debug.Log(curr_finger.transform.eulerAngles.z);*/
    }
}

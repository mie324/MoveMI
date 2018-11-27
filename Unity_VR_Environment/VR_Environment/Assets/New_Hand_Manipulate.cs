using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Net;
using System.Net.Sockets;
using System.Linq;

public class New_Hand_Manipulate : MonoBehaviour {

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
		//message_received = message_received.Substring(2);
		//float[] acceleration = new float[3]{, , }
		//Debug.Log(gesture);


		/*
		0 - fist clench - lightly
		1 - hand wide open - hard
		2 - relaxed - relaxed
		3 - scissors - hard
		4 - rock symbol - medium
		5 - thumbs up - hard
		*/
		
		switch( gesture ){
			case "0":
				rotationF[0] = 2;
				rotationF[1] = 2;
				rotationF[2] = 2;
				rotationF[3] = 2;
				rotationF[4] = 2;
				break;
			case "1":
				rotationF[0] = 1;
				rotationF[1] = 1;
				rotationF[2] = 1;
				rotationF[3] = 1;
				rotationF[4] = 1;
				break;
			case "2":
				rotationF[0] = 0;
				rotationF[1] = 0;
				rotationF[2] = 0;
				rotationF[3] = 0;
				rotationF[4] = 0;
				break;
			case "3":
				rotationF[0] = 2;
				rotationF[1] = 1;
				rotationF[2] = 1;
				rotationF[3] = 2;
				rotationF[4] = 2;
				break;
			case "4":
				rotationF[0] = 1;
				rotationF[1] = 1;
				rotationF[2] = 2;
				rotationF[3] = 2;
				rotationF[4] = 1;
				break;
			case "5":
				rotationF[0] = 1;
				rotationF[1] = 2;
				rotationF[2] = 2;
				rotationF[3] = 2;
				rotationF[4] = 2;
				break;
			default:
				rotationF[0] = 0;
				rotationF[1] = 0;
				rotationF[2] = 0;
				rotationF[3] = 0;
				rotationF[4] = 0;
				break;
		}
		
	}

	public readonly Vector3[,,] angleCases = new Vector3[3, 5, 3]
		// Relaxed positions for all fingers	[0]
		{ { { new Vector3(	9.5f	,	156.9f	,	27.2f	), new Vector3(		0f		,	0f		,	343.6f	), new Vector3(		0f		,	0f		,	1.0f	) },
			{ new Vector3(	285.1f	,	106.6f	,	61.1f	), new Vector3(		0f		,	0f		,	338.5f	), new Vector3(		0f		,	0f		,	353.1f	) },
			{ new Vector3(	279.6f	,	18.8f	,	151.9f	), new Vector3(		0f		,	0f		,	332.7f	), new Vector3(		0f		,	0f		,	352.5f	) },
			{ new Vector3(	290.8f	,	338.5f	,	190.6f	), new Vector3(		0f		,	0f		,	336.6f	), new Vector3(		0f		,	0f		,	350.4f	) },
			{ new Vector3(	359.0f	,	357.9f	,	166.5f	), new Vector3(		0f		,	0f		,	341.1f	), new Vector3(		0f		,   0f		,   354.6f  ) } },
		
		// Extended positions for all fingers	[1]
		  { { new Vector3(	355.1f	,   174.1f  ,   40.1f   ), new Vector3(    355.5f	,   359.5f  ,   5.8f	), new Vector3(    355.7f	,   359.1f	,   11.5f	) },
			{ new Vector3(	279.2f  ,   8.6f    ,   185.6f  ), new Vector3(    5.3f		,   1.4f    ,   9.6f    ), new Vector3(    342.0f	,   355.8f	,   3.4f	) },
			{ new Vector3(	279.6f  ,   22.8f   ,   168.3f  ), new Vector3(    357.0f	,   359.6f  ,   8.5f    ), new Vector3(    352.5f	,   0f		,   0f		) },
			{ new Vector3(	273.4f  ,   311.6f  ,   238.7f  ), new Vector3(    0f		,   0f      ,   9.1f    ), new Vector3(    1.3f		,   1.2f	,   355.6f  ) },
			{ new Vector3(	342.7f  ,   12.8f   ,   183.4f  ), new Vector3(    357.4f	,   359.5f  ,   359.0f  ), new Vector3(    11.2f	,   354.7f	,   11.1f   ) } },
		
		// Contracted positions for all fingers	[2]
		  { { new Vector3(	357.4f  ,   160.8f  ,   355.2f  ), new Vector3(    348.1f	,   18.3f   ,   331.4f  ), new Vector3(    354.9f	,   9.0f    ,   329.9f  ) },
			{ new Vector3(	280.9f  ,   76.7f   ,   37.5f   ), new Vector3(    1.7f		,   1.6f    ,   294.1f  ), new Vector3(    359.1f	,   2.0f    ,   324.9f  ) },
			{ new Vector3(	286.2f  ,   352.6f  ,   124.6f  ), new Vector3(    16.2f	,   356.2f  ,   294.0f  ), new Vector3(    350.5f	,   4.7f    ,   317.4f  ) },
			{ new Vector3(	287.2f  ,   39.3f   ,   90.7f   ), new Vector3(    18.8f	,   316.9f  ,   289.4f  ), new Vector3(    337.9f	,   345.7f  ,   337.0f  ) },
			{ new Vector3(	354.6f  ,   5.2f    ,   124.1f  ), new Vector3(    16.3f	,   354.0f  ,   294.6f  ), new Vector3(    334.8f	,   10.7f   ,   320.8f  ) } } };


	public Vector3[,] currAngles = new Vector3[5,3]
		{ { new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f) },		//thumb:	0 metacarpal | 1 proximal | 2 distal 
		  { new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f) },		//index:	0 proximal | 1 middle | 2 distal
		  { new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f) },		//middle:	0 proximal | 1 middle | 2 distal
		  { new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f) },		//ring:		0 proximal | 1 middle | 2 distal
		  { new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f), new Vector3(0f, 0f, 0f) } };		//pinky:	0 proximal | 1 middle | 2 distal

	public GameObject hand;
	public float rotationSpeed = 5f;

	//public int gesture = 0;
	public int[] rotationF = new int[5] { 0, 0, 0, 0, 0 };  //stores the future individual finger rotations based on the gesture number recieved 
	
	void cycleFingers() {
		GameObject thumb  = hand.transform.GetChild(0 +2).gameObject;
		GameObject index  = hand.transform.GetChild(1 +2).gameObject;
		GameObject middle = hand.transform.GetChild(2 +2).gameObject;
		GameObject ring   = hand.transform.GetChild(3 +2).gameObject;
		GameObject pinky  = hand.transform.GetChild(4 +2).gameObject.transform.GetChild(0).gameObject;

		GameObject[] fingers = new GameObject[5] { thumb, index, middle, ring, pinky };
		GameObject jointF;

		float delta = 0;
		float angleUpdate = Time.deltaTime * rotationSpeed;

		for (int i = 0; i < 5; i++) {
			jointF = fingers[i].gameObject;

			for (int j = 0; j < 3; j++) {
				currAngles[i,j] = new Vector3(  Mathf.LerpAngle(currAngles[i,j].x, angleCases[rotationF[i],i,j].x, Time.deltaTime * rotationSpeed),
												Mathf.LerpAngle(currAngles[i,j].y, angleCases[rotationF[i],i,j].y, Time.deltaTime * rotationSpeed),
												Mathf.LerpAngle(currAngles[i,j].z, angleCases[rotationF[i],i,j].z, Time.deltaTime * rotationSpeed)  );

				jointF.transform.localEulerAngles = currAngles[i,j];

				jointF = jointF.transform.GetChild(0).gameObject;
			}
		}

		if (Input.GetKeyDown(KeyCode.A)) {
			rotationF[4] = 0;
		} else if (Input.GetKeyDown(KeyCode.S)) {
			rotationF[3] = 0;
		} else if (Input.GetKeyDown(KeyCode.D)) {
			rotationF[2] = 0;
		} else if (Input.GetKeyDown(KeyCode.F)) {
			rotationF[1] = 0;
		} else if (Input.GetKeyDown(KeyCode.G)) {
			rotationF[0] = 0;
		} else if (Input.GetKeyDown(KeyCode.Q)) {
			rotationF[4] = 1;
		} else if (Input.GetKeyDown(KeyCode.W)) {
			rotationF[3] = 1;
		} else if (Input.GetKeyDown(KeyCode.E)) {
			rotationF[2] = 1;
		} else if (Input.GetKeyDown(KeyCode.R)) {
			rotationF[1] = 1;
		} else if (Input.GetKeyDown(KeyCode.T)) {
			rotationF[0] = 1;
		} else if (Input.GetKeyDown(KeyCode.Z)) {
			rotationF[4] = 2;
		} else if (Input.GetKeyDown(KeyCode.X)) {
			rotationF[3] = 2;
		} else if (Input.GetKeyDown(KeyCode.C)) {
			rotationF[2] = 2;
		} else if (Input.GetKeyDown(KeyCode.V)) {
			rotationF[1] = 2;
		} else if (Input.GetKeyDown(KeyCode.B)) {
			rotationF[0] = 2;
		}
	}
    
	// Use this for initialization
	void Start () {
		int startingPosition = 0;	// 0 relaxed | 1 extended | 2 contracted

		GameObject thumb  = hand.transform.GetChild(0 + 2).gameObject;
		GameObject index  = hand.transform.GetChild(1 + 2).gameObject;
		GameObject middle = hand.transform.GetChild(2 + 2).gameObject;
		GameObject ring   = hand.transform.GetChild(3 + 2).gameObject;
		GameObject pinky  = hand.transform.GetChild(4 + 2).gameObject.transform.GetChild(0).gameObject;

		GameObject[] fingers = new GameObject[5] { thumb, index, middle, ring, pinky };
		GameObject jointF;

		for (int finger_i = 0; finger_i < fingers.Length; finger_i++) {
			jointF = fingers[finger_i].gameObject;
			for (int joint_j = 0; joint_j < 3; joint_j++) {

				currAngles[finger_i, joint_j] = new Vector3( Mathf.LerpAngle(currAngles[finger_i, joint_j].x, angleCases[startingPosition, finger_i, joint_j].x, Time.deltaTime * rotationSpeed),
															 Mathf.LerpAngle(currAngles[finger_i, joint_j].y, angleCases[startingPosition, finger_i, joint_j].y, Time.deltaTime * rotationSpeed),
															 Mathf.LerpAngle(currAngles[finger_i, joint_j].z, angleCases[startingPosition, finger_i, joint_j].z, Time.deltaTime * rotationSpeed) );

				jointF.transform.localEulerAngles = currAngles[finger_i, joint_j];

				jointF = jointF.transform.GetChild(0).gameObject;
			}
		}

		UDP_setup();
	}
	
	// Update is called once per frame
	void Update () {
		UDP_receive();
		cycleFingers();
	}
}

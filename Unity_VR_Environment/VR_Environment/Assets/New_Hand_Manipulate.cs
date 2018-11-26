using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class New_Hand_Manipulate : MonoBehaviour {

	public readonly float[,,,] angleCases = new float[3, 5, 3, 3]
		// Relaxed positions for all fingers	[0]
		{ { { {	9.5f	,	156.9f	,	27.2f	}, {	0f		,	0f		,	343.6f	}, {	0f		,	0f		,	1.0f	} },
			{ {	285.1f	,	106.6f	,	61.1f	}, {	0f		,	0f		,	338.5f	}, {	0f		,	0f		,	353.1f	} },
			{ {	279.6f	,	18.8f	,	151.9f	}, {	0f		,	0f		,	332.7f	}, {	0f		,	0f		,	352.5f	} },
			{ { 290.8f	,	338.5f	,	190.6f	}, {	0f		,	0f		,	336.6f	}, {	0f		,	0f		,	350.4f	} },
			{ {	359.0f	,	357.9f	,	166.5f	}, {	0f		,	0f		,	341.1f	}, {	0f		,   0f		,   354.6f  } } },
		
		// Extended positions for all fingers	[1]
		  { { {	355.1f	,   174.1f  ,   40.1f   }, {    355.5f	,   359.5f  ,   5.8f	}, {    355.7f  ,   359.1f	,   11.5f	} },
			{ { 279.2f  ,   8.6f    ,   185.6f  }, {    5.3f    ,   1.4f    ,   9.6f    }, {    342.0f	,   355.8f	,   3.4f	} },
			{ { 279.6f  ,   22.8f   ,   168.3f  }, {    357.0f  ,   359.6f  ,   8.5f    }, {    352.5f	,   0f		,   0f		} },
			{ { 273.4f  ,   311.6f  ,   238.7f  }, {    0f      ,   0f      ,   9.1f    }, {    1.3f	,   1.2f	,   355.6f  } },
			{ { 342.7f  ,   12.8f   ,   183.4f  }, {    357.4f  ,   359.5f  ,   359.0f  }, {    11.2f	,   354.7f	,   11.1f   } } },
		
		// Contracted positions for all fingers	[2]
		  { { { 357.4f  ,   160.8f  ,   355.2f  }, {    348.1f  ,   18.3f   ,   331.4f  }, {    354.9f  ,   9.0f    ,   329.9f  } },
			{ { 280.9f  ,   76.7f   ,   37.5f   }, {    1.7f    ,   1.6f    ,   294.1f  }, {    359.1f  ,   2.0f    ,   324.9f  } },
			{ { 286.2f  ,   352.6f  ,   124.6f  }, {    16.2f   ,   356.2f  ,   294.0f  }, {    350.5f  ,   4.7f    ,   317.4f  } },
			{ { 287.2f  ,   39.3f   ,   90.7f   }, {    18.8f   ,   316.9f  ,   289.4f  }, {    337.9f  ,   345.7f  ,   337.0f  } },
			{ { 354.6f  ,   5.2f    ,   124.1f  }, {    16.3f   ,   354.0f  ,   294.6f  }, {    334.8f  ,   10.7f   ,   320.8f  } } } };


	public float[,,] currAngles = new float[5,3,3]
		{ { {0f, 0f, 0f}, {0f, 0f, 0f}, {0f, 0f, 0f} },		//thumb:	0 metacarpal | 1 proximal | 2 distal 
		  { {0f, 0f, 0f}, {0f, 0f, 0f}, {0f, 0f, 0f} },		//index:	0 proximal | 1 middle | 2 distal
		  { {0f, 0f, 0f}, {0f, 0f, 0f}, {0f, 0f, 0f} },		//middle:	0 proximal | 1 middle | 2 distal
		  { {0f, 0f, 0f}, {0f, 0f, 0f}, {0f, 0f, 0f} },		//ring:		0 proximal | 1 middle | 2 distal
		  { {0f, 0f, 0f}, {0f, 0f, 0f}, {0f, 0f, 0f} } };   //pinky:	0 proximal | 1 middle | 2 distal

	public GameObject hand;
	public float rotationSpeed = 60f;

	public int counter = 100;

	public int gesture = 0;
	public int[] rotationF = new int[5] { 0, 0, 0, 0, 0 };  //stores the future individual finger rotations based on the gesture number recieved 
	

	public float globalTime = 0;

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

		for (int finger_i = 0; finger_i < fingers.Length; finger_i++){
			jointF = fingers[finger_i].gameObject;
			for (int joint_i = 0; joint_i < 3; joint_i++) {  // thumb: 0 metacarpal | 1 proximal | 2 distal  // fingers : 0 proximal | 1 middle | 2 distal


				///* //Slerp Quaternions, comment out for other method
				Quaternion current = Quaternion.Euler(currAngles[finger_i, joint_i, 0], currAngles[finger_i, joint_i, 1], currAngles[finger_i, joint_i, 2]);
				Quaternion destination = Quaternion.Euler(angleCases[rotationF[finger_i], finger_i, joint_i, 0], angleCases[rotationF[finger_i], finger_i, joint_i, 1], angleCases[rotationF[finger_i], finger_i, joint_i, 2]);
				//*/

				///* //Quaternion Common Method
				for (int i = 0; i < 3; i++) {

					delta = currAngles[finger_i, joint_i, i] - angleCases[rotationF[finger_i], finger_i, joint_i, i];

					Debug.Log(currAngles[0, 0, i] + " | " + angleCases[rotationF[0], 0, 0, i] + " || " + i);

					int direction = Math.Sign(delta);

					currAngles[finger_i, joint_i, i] = currAngles[finger_i, joint_i, i] - direction * Math.Min(angleUpdate, Math.Abs(delta));
				}
				//*/

				//Debug.Log(angleUpdate);

				/* //Quaternion Euler method
				Quaternion temp = Quaternion.Euler(currAngles[finger_i,joint_i,0], currAngles[finger_i,joint_i,1], currAngles[finger_i,joint_i,2]);
				//*/

				///* //Quaternion Slerp method
				jointF.transform.localRotation = Quaternion.Slerp(current, destination, -1f);	// change to "1f" if you want animations to be instant
				//*/
				

				/* //Quaternion Euler method
				Debug.Log(temp);
				jointF.transform.localRotation = temp;
				//*/

				jointF = jointF.transform.GetChild(0).gameObject;		// Set joint pointer to next joint down in the finger/thumb
			}
		}

		globalTime = globalTime + Time.deltaTime;

		if (counter > 0) {
			counter--;
		} else {
			counter = 100;

			if(rotationF[0] == 0) {
				rotationF[0] = 1;
				rotationF[1] = 1;
				rotationF[2] = 1;
				rotationF[3] = 1;
				rotationF[4] = 1;
			} else if(rotationF[0] == 1) {
				rotationF[0] = 2;
				rotationF[1] = 2;
				rotationF[2] = 2;
				rotationF[3] = 2;
				rotationF[4] = 2;
			} else {
				rotationF[0] = 0;
				rotationF[1] = 0;
				rotationF[2] = 0;
				rotationF[3] = 0;
				rotationF[4] = 0;
			}

			//rotationF[0] = (rotationF[0] + 1) % 3;
			//rotationF[1] = (rotationF[1] + 1) % 3;
			//rotationF[2] = (rotationF[2] + 1) % 3;
			//rotationF[3] = (rotationF[3] + 1) % 3;
			//rotationF[4] = (rotationF[4] + 1) % 3;

			//Debug.Log(rotationF[0]);

			/*
			rotationF[4] = (rotationF[4] + (int)Math.Floor((double)(rotationF[0] + rotationF[1] + rotationF[2] + rotationF[3]) / 8)) % 3;
			rotationF[3] = (rotationF[3] + (int)Math.Floor((double)(rotationF[0] + rotationF[1] + rotationF[2]) / 6)) % 3;
			rotationF[2] = (rotationF[2] + (int)Math.Floor((double)(rotationF[0] + rotationF[1]) / 4)) % 3;
			rotationF[1] = (rotationF[1] + (int)Math.Floor((double)rotationF[0] / 2)) % 3;
			rotationF[0] = (rotationF[0] + 1) % 3;
			*/
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
			for (int joint_i = 0; joint_i < 3; joint_i++) {

				for (int i = 0; i < 3; i++) {
					currAngles[finger_i, joint_i, i] = angleCases[startingPosition, finger_i, joint_i, i];
				}

				Quaternion temp = Quaternion.Euler(currAngles[finger_i, joint_i, 0], currAngles[finger_i, joint_i, 1], currAngles[finger_i, joint_i, 2]);

				jointF.transform.localRotation = temp;

				jointF = jointF.transform.GetChild(0).gameObject;
			}
		}
	}
	
	// Update is called once per frame
	void Update () {
		cycleFingers();
	}
}
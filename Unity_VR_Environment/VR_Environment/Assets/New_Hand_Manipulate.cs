using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class New_Hand_Manipulate : MonoBehaviour {

	public float[,,] fjAngles = new float[5,3,3]
		{ { {0f, 0f, 0f}, {0f, 0f, 0f}, {0f, 0f, 0f} },
		  { {0f, 0f, 0f}, {0f, 0f, 0f}, {0f, 0f, 0f} },
		  { {0f, 0f, 0f}, {0f, 0f, 0f}, {0f, 0f, 0f} },
		  { {0f, 0f, 0f}, {0f, 0f, 0f}, {0f, 0f, 0f} },
		  { {0f, 0f, 0f}, {0f, 0f, 0f}, {0f, 0f, 0f} } };

	public GameObject hand;
	public float rotationSpeed = 100f;

	void cycleFingers() {
		GameObject thumb  = hand.transform.GetChild(0 +2).gameObject;
		GameObject index  = hand.transform.GetChild(1 +2).gameObject;
		GameObject middle = hand.transform.GetChild(2 +2).gameObject;
		GameObject ring   = hand.transform.GetChild(3 +2).gameObject;
		GameObject pinky  = hand.transform.GetChild(4 +2).gameObject;

		GameObject[] fingers = new GameObject[5] { thumb, index, middle, ring, pinky };
		GameObject jointF;

		for(int finger_i = 0; finger_i < fingers.Length; finger_i++){
			jointF = fingers[finger_i].gameObject;
			for (int joint_i = 0; joint_i < 3; joint_i++) {  // thumb: 0 metacarpal | 1 proximal | 2 distal  // fingers : 0 proximal | 1 middle | 2 distal

				float angleUpdate = -1f;//Time.deltaTime * rotationSpeed;

				for(int i = 0; i < 3; i++) {
					fjAngles[finger_i, joint_i, i] = fjAngles[finger_i, joint_i, i] + angleUpdate;
				}

				Debug.Log(angleUpdate);

				Quaternion temp2 = Quaternion.Euler(fjAngles[finger_i,joint_i,0], fjAngles[finger_i,joint_i,1], fjAngles[finger_i,joint_i,2]);

				Debug.Log(temp2);
				jointF.transform.localRotation = temp2;

				jointF = jointF.transform.GetChild(0).gameObject;		// Set joint pointer to next joint down in the finger/thumb
			}
		}
	}
    
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		//cycleFingers();
	}
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Show_Euler_Angles : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        Debug.Log(this.gameObject.transform.localEulerAngles);
	}
}

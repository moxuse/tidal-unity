using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move : MonoBehaviour {
	public Vector3 move = new Vector3(0, 0, 0);

	// Use this for initialization
	void Start () {
			
	}
	
	// Update is called once per frame
	void Update () {
		var pos = this.transform.position;
//		Debug.Log(pos);
		pos.x += move.x;
		pos.y += move.y;
		pos.z += move.z;
		this.transform.position = pos;
	}
}

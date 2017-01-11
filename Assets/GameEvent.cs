using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameEvent : MonoBehaviour {

	OSCController osc_controller;

	private Dictionary<string, GameObject> things;

	void Start () {
		things = new Dictionary<string, GameObject> ();
		string thingsPath = "Assets/Resources/Things";
		//string shaderPath = "Assets/Resources/Shader";
		DirectoryInfo dir = new DirectoryInfo (thingsPath);
		FileInfo[] info = dir.GetFiles ("*.prefab");

		foreach (FileInfo f in info) {

			string key = f.Name.Replace(".prefab", string.Empty);
			GameObject value = Resources.Load("Things/" + key, typeof(GameObject))as GameObject;

			if (value != null) {
				things.Add (key, value);
				Debug.Log (key, value);
			} else {
				Debug.Log ("couldnt load object: " + key);
			}
		}

		osc_controller = GameObject.Find("OSCController").GetComponent<OSCController>();

		osc_controller.onMsg += (msg) => {

			//print (msg.Data[1].ToString());
			var thing_str = msg.Data [1].ToString ();

			var pos = new Vector3((float)msg.Data[2], (float)msg.Data[3], (float)msg.Data[4]);
			var dur = (float)msg.Data[5];

			GameObject thing = things[thing_str];

			AppendItem(thing, pos, dur);
		};
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void AppendItem (GameObject thing, Vector3 pos, float dur) {
		var clone = Instantiate(thing, pos, Quaternion.identity);	
		Destroy(clone, dur * 1.0f);
	}
}

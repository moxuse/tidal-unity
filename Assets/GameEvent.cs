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
			var twist = (float)msg.Data[6];
			var rigid = (int)msg.Data[7];
			var randCam = (float)msg.Data[8];

			GameObject thing = things[thing_str];

			if (0 < twist) {
				ApplyShader(thing, "Custom/Twist", twist);
			}

			if (0 < randCam) {
				this.RandCamera(randCam);
			}

			AppendItem(thing, pos, dur, rigid);
		};
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void AppendItem (GameObject thing, Vector3 pos, float dur, int rigid) {
		var clone = Instantiate (thing, pos, Quaternion.identity);
		if (0 < rigid) {
			clone.AddComponent<Rigidbody> (); // Add the rigidbody.
			clone.AddComponent<MeshCollider> (); // Add the rigidbody.
			MeshCollider collider = clone.GetComponent<MeshCollider> ();
			Rigidbody rigidbody = clone.GetComponent<Rigidbody> ();
			collider.convex = true;
			rigidbody.useGravity = true;
		}
		Destroy(clone, dur * 1.0f);
	}

	void RandCamera (float speed) {
		SpringCamera cam = GameObject.Find("SpringCamera").GetComponent<SpringCamera> ();
		cam.NextPosition(speed);
	}

	void ApplyShader (GameObject thing, string shaderName, float val) {
		//Debug.Log("setShaderToGameobject started: " + thing.name + " / " + shaderName);
		var shader = Shader.Find(shaderName);

		//foreach (Transform t in thing.GetComponentsInChildren<Transform>()) {

		foreach (var renderer in thing.GetComponents<Renderer>()) {
		
			//foreach (var material in renderer.material) {
			Material material = new Material(shader);
			renderer.material = material;
			//renderer.material.shader = shader;

			renderer.sharedMaterial.SetFloat ("_Freq", val);

			//}

		}

		//}
	}
}

using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameEvent : MonoBehaviour {

	OSCController osc_controller;

	private Dictionary<string, GameObject> things;
	private Dictionary<string, Material> skyboxes;
	private GameObject plane;
	private GameObject dirLight;

	private float offsetScale;
	
	private string currentSkyboxName;

	void Start () {
		offsetScale = 1.0f;
		plane = GameObject.Find ("Plane");
		dirLight = GameObject.Find ("Directional Light");
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
				//Debug.Log (key, value);
			} else {
				Debug.Log ("couldnt load object: " + key);
			}
		}

		skyboxes = new Dictionary<string, Material> ();
		string skyboxesPath = "Assets/Resources/Skyboxes";
		DirectoryInfo s_dir = new DirectoryInfo (skyboxesPath);
		FileInfo[] s_info = s_dir.GetFiles ("*.mat");

		foreach (FileInfo f in s_info) {
			string key = f.Name.Replace(".mat", string.Empty);
			Material value = Resources.Load("Skyboxes/" + key) as Material;
//			Debug.Log (value);
			if (value != null) {
				skyboxes.Add (key, value);
			} else {
				Debug.Log ("couldnt load skybox: " + key);
			}
		}
		currentSkyboxName = "studio";

		osc_controller = GameObject.Find("OSCController").GetComponent<OSCController>();

		osc_controller.onMsg += (msg) => {
			// print (msg.Data[0].ToString());

			var thing_str = msg.Data[0].ToString();
			var pos = new Vector3((float)msg.Data[1], (float)msg.Data[2], (float)msg.Data[3]);
			var move = new Vector3((float)msg.Data[4], (float)msg.Data[5], (float)msg.Data[6]);
			var scale = (float)msg.Data[7];
			var dur = (float)msg.Data[8];
			var twist = (float)msg.Data[9];
			var rigid = (int)msg.Data[10];
			var randCam = (float)msg.Data[11];
			var vortexRadX = (float)msg.Data[12];
			var vortexRadY = (float)msg.Data[13];
			var vortexAngle = (float)msg.Data[14];
			var ripple = (float)msg.Data[15];
//			var skyboxName = (string)msg.Data[13];
			var dLight = (string)msg.Data[16];

			GameObject thing = things[thing_str];
			// if (0 < twist) {
				//Debug.Log("apply");
			if (thing) {
				ApplyShader(thing, "Custom/Twist", twist);
			}
			// } else {
				//Debug.Log("not apply");
			// }

			if (0 < randCam) {
				this.RandCamera(randCam);
			}

			//if (0 != vortexAngle) {
				this.Vortex(vortexRadX, vortexRadY, vortexAngle);
			//}
				
			this.Ripple(ripple);

//			if (skyboxName != null) {
//				ChangeSkyBox(skyboxName);
//			}

			if (dLight !=  null) {
				SetDLight(dLight);
			}

			//Debug.Log(scale);
			AppendItem(thing, pos, move, scale * offsetScale, dur, rigid);
		};

		osc_controller.onBang += (msg) => {
			offsetScale = 2.0f;
		};
	}
	
	// Update is called once per a frame
	void Update () {
		if (1.0f < offsetScale) {
			offsetScale = offsetScale - 0.2f;
		}
	}

	void AppendItem (GameObject thing, Vector3 pos, Vector3 move, float scale, float dur, int rigid) {
		var clone = Instantiate (thing, pos, Quaternion.identity);
		if (0 < rigid) {
			clone.AddComponent<Rigidbody> (); // Add the rigidbody.
			clone.AddComponent<MeshCollider> (); // Add the rigidbody.
			MeshCollider collider = clone.GetComponent<MeshCollider> ();
			Rigidbody rigidbody = clone.GetComponent<Rigidbody> ();
			collider.convex = true;
			rigidbody.useGravity = true;
		} else {
			if ("NoMesh" == clone.tag) {
				SphereCollider collider = clone.GetComponent<SphereCollider> ();
				if (collider) {
					collider.enabled = false;
				}
			}
		}
		clone.transform.localScale = new Vector3(scale,scale,scale);

		clone.AddComponent <Move>().move = move;

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
			material.CopyPropertiesFromMaterial(renderer.sharedMaterial);

			material.SetTexture("Base (RGB)", renderer.sharedMaterial.mainTexture);
			material.SetFloat ("_Freq", val);
			renderer.material = material;
		}
		//}
	}

	void Vortex(float radX, float raadY, float angle) {
		var sp_camera = GameObject.Find("SpringCamera").GetComponent<SpringCamera>();
		if (sp_camera) {
			sp_camera.SetVortex(radX, raadY, angle);
		}
	}

	void Ripple(float radius) {
		var sp_camera = GameObject.Find("SpringCamera").GetComponent<SpringCamera>();
		if (sp_camera) {
			sp_camera.SetRipple(radius);
		}
	}

	void SetDLight(string mode) {
		if ("d" == mode) {
			this.dirLight.transform.rotation = Quaternion.Euler(-50.0f, -180.0f,  0.0f);
		} else if ("r" == mode) {
			this.dirLight.transform.rotation = Quaternion.Euler(50.0f, -180.0f,  0.0f);
		}
	}


	void ChangeSkyBox(string name) {
		if (currentSkyboxName == name) {
			return;
		}
		if ("" == name) {
			if (this.plane != null) {
				MeshRenderer m =this.plane.GetComponent<MeshRenderer>();
				m.material.SetColor("_Albedo" ,new Color(1.0f,1.0f,1.0f,1.0f));
			}
			RenderSettings.skybox = skyboxes["studio"];
			return;
		}
		var mat = skyboxes [name];

		if (mat != null) {
			RenderSettings.skybox = skyboxes [name];

			if (this.plane != null) {
				MeshRenderer m =this.plane.GetComponent<MeshRenderer>();
				m.material.SetColor("_Albedo" ,new Color(1.0f,1.0f,1.0f,0.0f));
			}
		} else {
			if (this.plane != null) {
				MeshRenderer m = this.plane.GetComponent<MeshRenderer>();
				m.material.SetColor("_Albedo" ,new Color(1.0f,1.0f,1.0f,1.0f));
			}
			RenderSettings.skybox = skyboxes["studio"];
		}
		
	}
}

using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityOSC;

public class OSCController : MonoBehaviour {
	
	static string TargetAddr = "127.0.0.1";
	static int OutGoingPort = 5001;
	static int InComingPort = 5000;

	private GameObject apple;
	private GameObject banana;
	private GameObject bottle;

	private Queue queue;

	void Start() {  
		OSCHandler.Instance.Init(TargetAddr, OutGoingPort, InComingPort);

		apple = Resources.Load("Items/Apple/Apple",typeof(GameObject))as GameObject;
		banana = Resources.Load("Items/Banana/Banana",typeof(GameObject))as GameObject;
		bottle = Resources.Load("Items/PetBottles/Pet",typeof(GameObject))as GameObject;

		queue = new Queue();
		queue = Queue.Synchronized(queue);

		var server = OSCHandler.Instance.Servers["TidalClient"].server;
		server.PacketReceivedEvent += OnPacketReceived;
	}

	void OnPacketReceived(OSCServer server, OSCPacket packet) {
		queue.Enqueue(packet);
	}

	void Update() {		
		if (queue == null) {
			return;
		}
		while (0 < queue.Count) {
			OSCPacket packet = queue.Dequeue () as OSCPacket;
			if (packet.IsBundle ()) {
				// if OSCBundl
				OSCBundle bundle = packet as OSCBundle;
				foreach (OSCMessage msg in bundle.Data) {
					
				}
			} else {
				OSCMessage msg = packet as OSCMessage;

				if ("/unity_osc" == msg.Address) {
					print (msg.Data[1].ToString());
					var thing = msg.Data [1].ToString ();
					var pos = new Vector3((float)msg.Data[2], (float)msg.Data[3], (float)msg.Data[4]);
					var dur = (float)packet.Data[5];
					print (dur); 
					AppendItem (thing, pos, dur);
				}
			}
		}
	}

	void AppendItem (String item, Vector3 pos, float dur) {
		GameObject thing = apple;
		switch (item) {
			case "apple":
				thing = apple;
				break;
			case "banana":
				thing = banana;
				break;
			case "bottle":
				thing = bottle;
				break;
		}
		if (thing == null) {
				return;
			}
		var clone = Instantiate(thing, pos, Quaternion.identity);	
		Destroy(clone, dur * 1.0f);
	}
}
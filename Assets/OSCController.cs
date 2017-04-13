using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityOSC;

public class OSCController : MonoBehaviour {
	
	[SerializeField]string _TargetAddr = "127.0.0.0";
	[SerializeField]int _OutGoingPort = 5001;
	[SerializeField]int _InComingPort = 5000;

	private Queue queue;

	public delegate void onMsgDelegate(OSCMessage msg);
	public event onMsgDelegate onMsg = null;

	void Start() {  
		OSCHandler.Instance.Init(_TargetAddr, _OutGoingPort, _InComingPort);

		queue = new Queue();
		queue = Queue.Synchronized(queue);

		var server = OSCHandler.Instance.Servers["TidalClient"].server;
		server.PacketReceivedEvent += OnPacketReceived;
	}

	void OnPacketReceived(OSCServer server, OSCPacket packet) {
		if (1 <= queue.Count) {
			return;
		} else {
			queue.Enqueue (packet);
		}
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
					if (onMsg != null) {
						onMsg (msg);
					}
				}
			}
		}
	}
}
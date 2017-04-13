using UnityEngine;
using System.Collections;

public class SpringCamera : MonoBehaviour
{
	private Camera mainCam;

	private float nextCamPosX =  0;
	private float nextCamPosY =  0;
	private float nextCamPosZ =  0;
	private float curCamPosX =   0;
	private float curCamPosY =   0;
	private float curCamPosZ =   0;
	private float speedX =       0;
	private float speedY =       0;
	private float speedZ =       0;
	private float friction =     0.4f;
	private float spring =       0.5f;

	// Use this for initialization
	void Start ()
	{
		this.mainCam = GameObject.Find("Main Camera").GetComponent<Camera>();

		this.mainCam.transform.position = this.animateCamera();
		this.NextPosition (1.0f);
	}
	
	// Update is called once per frame
	void Update ()
	{
		Vector3 next = this.animateCamera ();
		this.mainCam.transform.position = next;
		this.mainCam.transform.LookAt(new Vector3 (0, 0.5f, 0));
	}

	public void NextPosition (float speed) {
		float rand_fact_x = Random.Range (0.0f, 3.3f) +  0.7f;
		float rand_fact_z = Random.Range (0.0f, 3.3f) +  0.7f;
		this.nextCamPosX = (Random.Range(-1.0f * rand_fact_x, rand_fact_x));
		this.nextCamPosY = Random.Range (0.5f, 1.7f);
		this.nextCamPosZ = (Random.Range (-1.0f * rand_fact_z, rand_fact_z));
		this.friction = Random.Range (0.4f, 0.8f);
		this.spring = Random.Range (0.1f, 0.2f) * speed;
	}

	Vector3 animateCamera () {
		float ax, ay, az;
		ax = (this.nextCamPosX - this.curCamPosX) * this.spring;
		this.speedX += ax;
		this.speedX *= this.friction;
		this.curCamPosX += this.speedX;

		ay = (this.nextCamPosY - this.curCamPosY) * this.spring;
		this.speedY += ay;
		this.speedY *= this.friction;
		this.curCamPosY += this.speedY;

		az = (this.nextCamPosZ - this.curCamPosZ) * this.spring;
		this.speedZ += az;
		this.speedZ *= this.friction;
		this.curCamPosZ += this.speedZ;

		return new Vector3( this.curCamPosX, this.curCamPosY, this.curCamPosZ );
	}
}


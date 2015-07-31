using UnityEngine;
using System.Collections;

public class Movement : MonoBehaviour {


	private bool doubleJumping = false, wallRunning  = false, sliding = false;

	private float verticalVel = 0f;
	private float slideTime = 1.1f, currentSlide = 0, slidespeed = 1.5f;
	private float gravity = -9.8f, speed = 10f;
	public float rotSpeed = 7f;
	private float jumpPower = 3f;
	private float jumpTime = .25f, jumpGravity = 3f, currentJump = 0;
	private float zMove = 0, xMove = 0;

	private CharacterController charCon;
	private Animator anim;

	// Use this for initialization
	void Start () {
		charCon = gameObject.GetComponent<CharacterController>();
		anim = gameObject.GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {

		if (!wallRunning && !sliding) {
			float vert = Input.GetAxis ("Vertical");
			float hor = Input.GetAxis ("Horizontal");
			float yaw = Input.GetAxis ("Mouse X") * rotSpeed;



			transform.Rotate (0f, yaw, 0f);
			
			if (!charCon.isGrounded)
			{
				verticalVel += (gravity  * Time.deltaTime);
				if(Input.GetButtonDown("Jump") && !doubleJumping)
				{
					verticalVel = jumpPower;
					currentJump = 0f;
					gravity = jumpGravity;
					doubleJumping = true;
				}
				else if(Input.GetButtonDown("Slide"))
					sliding = true;

				if(Input.GetButton("Jump"))
				{
					if(currentJump >= jumpTime)
					{
						gravity = -9.8f;
					}
					currentJump += Time.deltaTime;
				}

				else
				{
					gravity = -9.8f;
				}

			}
			else if(Input.GetButtonDown("Jump"))
			{
				verticalVel = jumpPower;
				verticalVel = jumpPower;
				currentJump = 0f;
				gravity = jumpGravity;
			}
			else
			{
				verticalVel = 0;
				doubleJumping = false;
			}
	
			xMove = hor;
			if(vert >= 0)
				zMove = vert;
			else
				zMove = 0.5f*vert;

		} else if (wallRunning) {

			verticalVel = 15f;
			xMove = 0;
			zMove = 20f;

			if(Input.GetButtonDown("Jump"))
			{
				xMove = 25f;
				verticalVel = 20f;
				wallRunning = false;
			}

		} else if (sliding) {
			verticalVel = 0;
			xMove = 0;
			zMove = slidespeed;
			currentSlide+=Time.deltaTime;
			if(currentSlide >= slideTime)
			{
				currentSlide = 0;
				sliding = false;
			}
		}

		xMove = xMove * speed;
		zMove = zMove * speed;
		charCon.Move (transform.rotation * new Vector3 (xMove, verticalVel, zMove) * Time.deltaTime);
	}

	void FixedUpdate()
	{
		anim.SetBool ("Sliding", sliding);
		anim.SetBool ("Wallrunning", wallRunning);
		anim.SetFloat ("Vertical", zMove);
		anim.SetFloat ("Horizontal", xMove);
	}
}

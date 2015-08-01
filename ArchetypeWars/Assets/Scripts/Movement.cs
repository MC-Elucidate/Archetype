using UnityEngine;
using System.Collections;

public class Movement : MonoBehaviour {


	private bool doubleJumping = false, wallRunning  = false, wallrunLeft = false, wallrunRight = false, sliding = false;


	public float rotSpeed = 7f;

	private float verticalVel = 0f;
	private float slideTime = 1.1f, currentSlide = 0, slidespeed = 1.5f;
	private float gravity = -9.8f, speed = 10f;
	private float jumpPower = 3f;
	private float jumpTime = .25f, jumpGravity = 3f, currentJump = 0;
	private float zMove = 0, xMove = 0;
	private float wallrunTime = 1.5f, currentWallrun = 0;

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


			wallRunning = wallrunLeft = wallrunRight = false;

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
	
			if(vert >= 0)
			{
				xMove = hor;
				zMove = vert;
			}
			else
			{
				zMove = 0.5f*vert;
				xMove = 0.5f*hor;
			}

		} else if (wallRunning) {

			verticalVel = 3f;
			xMove = 0;
			zMove = 1f;
			currentWallrun += Time.deltaTime;

			if(currentWallrun >= wallrunTime)
			{
				verticalVel = -3f;
			}

			if(Input.GetButtonDown("Jump") && !doubleJumping)
			{
				zMove = Input.GetAxis("Vertical") * jumpPower * speed;
				xMove = Input.GetAxis("Horizontal") * jumpPower * speed;
				verticalVel = jumpPower;
				wallRunning = wallrunLeft = wallrunRight = false;
				currentWallrun = 0f;
				doubleJumping = true;
			}

			if(Input.GetButtonUp("Wallrun"))
			{
				wallRunning = wallrunLeft = wallrunRight = false;
				currentWallrun = 0f;
			}

			if(wallrunRight)
			{
				if(Physics.Raycast(transform.position, transform.right, 0.61f))
				{}
				else
				{wallrunRight = wallRunning = false;}
			}

			else
			{
				if(Physics.Raycast(transform.position, -transform.right, 0.61f))
				{}
				else
				{wallrunLeft = wallRunning = false;}
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

	void OnControllerColliderHit(ControllerColliderHit collision) {
		if (collision.gameObject.tag == "Wall" && Input.GetButton("Wallrun") && !wallRunning && zMove > 0) {
			wallRunning = true;
			RaycastHit target, target2;
			if(Physics.Raycast(transform.position, 10*transform.forward + transform.right, out target, 0.61f))
			{
				if(target.transform.gameObject.tag == "Wall")
				{
					wallrunRight = true;
					//Debug.Log("rotating");
					float angle = Vector3.Angle(transform.forward, target.normal);
					angle -= 90;
					transform.Rotate(0, -angle, 0);
				}				
				else
					wallrunLeft = true;
			}
			else if (Physics.Raycast(transform.position, transform.right, out target2, 0.61f))
			{
				if(target2.transform.gameObject.tag == "Wall")
				{
					wallrunRight = true;
					float angle = Vector3.Angle(transform.forward, target2.normal);
					angle -= 90;
					transform.Rotate(0, -angle, 0);
				}
				else
					wallrunLeft = true;
			}
			else
				wallrunLeft = true;

			if(wallrunLeft)
			{
				if (Physics.Raycast(transform.position, transform.forward - transform.right, out target, 0.61f))
				{
					float angle = Vector3.Angle(transform.forward, target.normal);
					angle -= 90;
					Debug.Log(angle);
					transform.Rotate(0, angle, 0);
				}
			}
		}
	}

}

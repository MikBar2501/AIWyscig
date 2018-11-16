using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarController : MonoBehaviour {

	public float velocity;
	public float maxVelocity;

	public float acceleration;

	public float move;
	public float rotation;

	// Use this for initialization
	void Start () {
		Physics2D.IgnoreLayerCollision(8,8);
		
	}
	
	// Update is called once per frame
	void Update () {
		move = Input.GetAxis("Vertical");
		rotation = Input.GetAxis("Horizontal");

		/* if(Input.GetKey(KeyCode.I)){
			move = setVelocity(2);
		} else if(Input.GetKey(KeyCode.K)){
			move = setVelocity(-1);
		} else {
			move = setVelocity(0);
		}
		
		if(Input.GetKey(KeyCode.L)){
			rotation = setDirection(2);
		} else if(Input.GetKey(KeyCode.J)){
			rotation = setDirection(-1);
		} else {
			rotation = setDirection(0);
		}*/

		
		
		movRotMethod(rotation, move);
		transform.Translate(Vector3.up * velocity);
		
	}

	public void movRotMethod(float dir, float vel) {
		if(vel > 0)
		{
			if(velocity < maxVelocity)
			{
				velocity += Time.deltaTime * (acceleration * 2f);
			}
			
		} else if (vel < 0) {
			if(velocity > 0) {
				velocity -= Time.deltaTime * (acceleration * 2f);
			} else {
				velocity = 0;
			}
		} else {
			if(velocity > 0) {
				velocity -= Time.deltaTime * acceleration;
			} else {
				velocity = 0;
			}
		}

		if(dir > 0) {
			transform.Rotate(Vector3.forward * 3 * -1);
		} else if (dir < 0) {
			transform.Rotate(Vector3.forward  * 3);
		}
	}

	
	public float setDirection(int dir) {
		float _direction = 0f;
		if(dir > 0) {
			_direction = 1f;
		} else if(dir < 0) {
			_direction = -1f;
		} else {
			_direction = 0f;
		}
		return _direction;
	}

	public float setVelocity(int vel) {
		float _velocity = 0f;
		if(vel > 0) {
			Debug.Log("Kurwa, działasz?");
			_velocity = 1f;
		} else if(vel < 0) {
			_velocity = -1f;
		} else {
			_velocity = 0f;
		}
		return _velocity;
	}


	
	/* public void AsetDirection(int dir) {
		if(dir > 0) {
			if(rotation < 1) {
				rotation += acceleration;
			} else {
				rotation = 1f;
			}
		} else if(dir < 0) {
			if(rotation > -1) {
				rotation -= acceleration;
			} else {
				rotation = -1f;
			}
		} else {
			if(rotation > 0) {
				rotation -= acceleration;
			} else if(rotation < 0) {
				rotation += acceleration;
			}
		}

	}

	public void AsetVelocity(int vel) {
		if(vel > 0) {
			if(move < 1) {
				move += acceleration;
			} else {
				move = 1f;
			}
		} else if(vel < 0) {
			if(move > -1) {
				move -= acceleration;
			} else {
				move = -1f;
			}
		} else {
			if(move > 0) {
				move -= acceleration;
			} else if(move < 0) {
				move += acceleration;
			}
		}
	}*/

	

}

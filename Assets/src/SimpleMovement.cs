using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Uses Character controller.move()
/// faces the direction of move for ref see https://www.youtube.com/watch?v=4HpC--2iowE 15:19min
/// </summary>
public class SimpleMovement : MonoBehaviour
{
	public CharacterController charCtrl;
	public float walkSpeed;
	public float turnSmoothTime = 0.1f;
	float turnSmoothVelocity;

    // Start is called before the first frame update
    void Start()
    {
		charCtrl = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
		handleMovement();
    }

	private void handleMovement() {
		// Get Horizontal and Vertical Input
		float horizontal = Input.GetAxis("Horizontal");
		float vertical = Input.GetAxis("Vertical");
		Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;

		if(direction.magnitude >= 0.1f) {
			float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;

			// smooth turn
			float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
			transform.rotation = Quaternion.Euler(0f, angle, 0f);


			// or hard turn
			//float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
			//transform.rotation = Quaternion.Euler(0f, targetAngle, 0f);

			charCtrl.Move(direction * walkSpeed * Time.deltaTime);
		}
	}

}

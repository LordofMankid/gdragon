using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove3D : MonoBehaviour {
    //Animator anim;
    Rigidbody rb;
    public Transform cam;

    public float speed = 6f;
    public float turnSmoothTime =0.1f;
    private float turnSmoothVelocity;

	float horiz;
	float vert;

	public bool isP1 = true;
	public bool isP2 = false;
	public bool isP3 = false;
	public bool isP4 = false;

    void Start(){
        //anim = gameObject.GetComponentInChildren<Animator>();
        rb = gameObject.GetComponent<Rigidbody>();
		if (isP1){
        	cam = GameObject.FindWithTag("Camera_P1").GetComponent<Transform>();
		}
		else if (isP2){
        	cam = GameObject.FindWithTag("Camera_P2").GetComponent<Transform>();
		}
		else if (isP3){
        	cam = GameObject.FindWithTag("Camera_P43").GetComponent<Transform>();
		}
		else if (isP4){
        	cam = GameObject.FindWithTag("Camera_P4").GetComponent<Transform>();
		}
		else {
			Debug.Log("Can't find player for camera");
		}
    }

    void FixedUpdate () {

        //float horiz = Input.GetAxisRaw("Horizontal");
        //float vert = Input.GetAxisRaw("Vertical");

		if (isP1){
			horiz = Input.GetAxisRaw("P1_Horizontal");
        	vert = Input.GetAxisRaw("P1_Vertical");
		}
		else if (isP2){
			horiz = Input.GetAxisRaw("P2_Horizontal");
        	vert = Input.GetAxisRaw("P2_Vertical");
		}
		else if (isP3){
			horiz = Input.GetAxisRaw("P3_Horizontal");
        	vert = Input.GetAxisRaw("P3_Vertical");
		}
		else if (isP4){
			horiz = Input.GetAxisRaw("P4_Horizontal");
        	vert = Input.GetAxisRaw("P4_Vertical");
		}
		else {
			Debug.Log("no player assigned for inputs");
		}


        Vector3 direct = new Vector3(horiz, 0f, vert).normalized;

        if (direct.magnitude >= 0.1f) {
            float targetAngle = Mathf.Atan2(direct.x, direct.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);
            Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            rb.MovePosition(transform.position + moveDir * speed * Time.deltaTime);
        }
		else {
			GetComponent<Rigidbody>().velocity = Vector3.zero; 
			GetComponent<Rigidbody>().angularVelocity = Vector3.zero; 
		}
    }
}
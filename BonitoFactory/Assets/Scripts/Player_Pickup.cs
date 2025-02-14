using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Pickup : MonoBehaviour
{

	public Transform PickUp_Hand;
	private Transform PickUp_Object;

	public bool hasItem = false;
	public bool canPickup = false;
	public float throwAmt = 0.5f;

    // Start is called before the first frame update
    void Update()
    {
        if ((!hasItem)&&(canPickup)&&(Input.GetKeyDown("e"))){
			PickUp();
			hasItem=true;
			canPickup = false;
		}
		else if ((hasItem)&&(Input.GetKeyDown("e"))){
			DropIt();
		}
    }

    // Update is called once per frame
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "PickUp"){
			canPickup = true;
			PickUp_Object = other.gameObject.transform;
		}
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "PickUp"){
			canPickup = false;
			if (!hasItem){
				PickUp_Object = null;
			}
		}
    }

	public void PickUp(){
		PickUp_Object.position = PickUp_Hand.position;
		PickUp_Object.parent = PickUp_Hand;
		PickUp_Object.gameObject.GetComponent<Rigidbody>().isKinematic = true;
	} 

	public void DropIt(){
		PickUp_Object.parent = null;
		hasItem=false;
		PickUp_Object.gameObject.GetComponent<Rigidbody>().isKinematic = false;
		PickUp_Object.gameObject.GetComponent<Rigidbody>().AddForce(transform.forward * throwAmt, ForceMode.Impulse);
		PickUp_Object = null;
	} 

}

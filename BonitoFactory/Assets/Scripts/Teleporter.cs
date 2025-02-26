using System.Collections.Generic;
using System.Collections;
using UnityEngine;

//This script goes onto the teleport object
//The teleport object needs a BoxCollider2D set to isTrigger and a tag called "teleport"
public class Teleporter : MonoBehaviour {

       public Vector3 newPosition;
       public GameObject[] allTeleporters;

       public void OnTriggerEnter2D(Collider2D other){
             if (other.gameObject.tag == "Player"){
                 Vector3 playerPos = other.gameObject.transform.position;
                 float DistToPlayer = Vector3.Distance(playerPos, transform.position);

           //to prevent teleporters from looping: set trigger colliders no smaller than 1x1
                 if (DistToPlayer > 0.5f){
                      GetNearest();
                      other.gameObject.transform.position = newPosition;
                 }
             }
        }

     public void GetNearest(){
           //populate array with all teleporters except self
           this.gameObject.tag = "Untagged"; //temporary tag
           allTeleporters = GameObject.FindGameObjectsWithTag("Teleport");
           this.gameObject.tag = "Teleport"; //change tag back

           //test each array item for closest
           Transform closestTarget = null;
           float closestTargetSquare = Mathf.Infinity;
           Vector3 thisPosition = transform.position;
           for (int i = 0; i < allTeleporters.Length; i++){
               Vector3 distanceToNewTarget = allTeleporters[i].transform.position - thisPosition;
               float distanceSquareToNewTarget = distanceToNewTarget.sqrMagnitude;
               if (distanceSquareToNewTarget < closestTargetSquare){
                      closestTargetSquare = distanceSquareToNewTarget;
                      closestTarget = allTeleporters[i].transform;
               }
           }
          newPosition = closestTarget.position;
      }
}

// "find nearest" adapted from:
// https://forum.unity.com/threads/clean-est-way-to-find-nearest-object-of-many-c.44315/
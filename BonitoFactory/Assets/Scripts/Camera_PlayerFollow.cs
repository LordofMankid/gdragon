using UnityEngine;

public class Camera_PlayerFollow : MonoBehaviour {
    private Transform target;     // drag intended Player object into Inspector slot
    public float smoother = 5f;
    public Vector3 offset;     // set the offset in the editor

	public string playerName = "Player1";

    void Start(){
       target = GameObject.FindWithTag(playerName).GetComponent<Transform>();
    }

    void FixedUpdate () {
       Vector3 newPos = target.position + offset;
       Vector3 smPos = Vector3.Lerp (transform.position, newPos, smoother * Time.deltaTime);
       transform.position = smPos;

       transform.LookAt (target);
    }
}
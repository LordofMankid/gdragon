using UnityEngine;

public class Camera_PlayerFollow : MonoBehaviour
{
    private Transform target;     // drag intended Player object into Inspector slot
    public float smoother = 5f;
    public Vector3 offset;     // set the offset in the editor

    public string playerName = "Player1";
    private bool isPlayer1 = false;
    private bool isPlayer2 = false;

    public string[] cache = { "X", "Z" };

    void Start()
    {
        target = GameObject.FindWithTag(playerName).GetComponent<Transform>();

        if (playerName == "Player1")
        {
            isPlayer1 = true;
        }
        else if (playerName == "Player2")
        {
            isPlayer2 = true;
        }
    }

    void FixedUpdate()
    {
        Vector3 newPos = target.position + offset;
        Vector3 smPos = Vector3.Lerp(transform.position, newPos, smoother * Time.deltaTime);
        transform.position = smPos;

        transform.LookAt(target);
    }

    void Update()
    {
        // Allow both players to shift their POV at the same time
        if (isPlayer1 && Input.GetKey(KeyCode.LeftShift))
        {
            RotateCamera(KeyCode.A, KeyCode.D);
        }

        if (isPlayer2 && Input.GetKey(KeyCode.RightShift))
        {
            RotateCamera(KeyCode.LeftArrow, KeyCode.RightArrow);
        }
    }

    // Rotate 45 degrees (left or right)
    void RotateCamera(KeyCode leftButton, KeyCode rightButton)
    {
        if (Input.GetKeyDown(leftButton))
        {
            Debug.Log("Going left");
            if (cache[1] == "X")
            {
                offset = Vector3.Scale(offset, new Vector3(1, 1, -1));
            }
            else if (cache[1] == "Z")
            {
                offset = Vector3.Scale(offset, new Vector3(-1, 1, 1));
            }
            UpdateCache();
        }

        if (Input.GetKeyDown(rightButton))
        {
            Debug.Log("Going right");
            if (cache[1] == "X")
            {
                offset = Vector3.Scale(offset, new Vector3(-1, 1, 1));
            }
            else if (cache[1] == "Z")
            {
                offset = Vector3.Scale(offset, new Vector3(1, 1, -1));
            }
            UpdateCache();
        }
    }

    void UpdateCache()
    {
        string temp = cache[0];
        cache[0] = cache[1];
        cache[1] = temp;
    }
}
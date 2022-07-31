using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using JlMetroidvaniaProject.MapManagement;

public class TPlayer : MonoBehaviour
{
    public float moveSpeed = 3.0f;
    void Start()
    {
        
    }

    public void OnDisable()
    {
        Debug.Log("Player OnDisabled.");
    }

    public void OnDestroy()
    {
        Debug.Log("Player Destroyed.");
    }

    void FixedUpdate()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.right, 1.0f, 1 << LayerMask.NameToLayer("Ground"));

        if(hit)
        {
            GroundJoint2D joint;
            bool canGetJoint = hit.collider.TryGetComponent<GroundJoint2D>(out joint);

            Debug.Log(string.Format("canGetJoint: {0}", canGetJoint));

            if(canGetJoint && joint.ledgeGrabAvailable)
            {
                Debug.Log("Ledge Grab Available.");
            }
        }

        bool left = Input.GetKey(KeyCode.LeftArrow);
        bool right = Input.GetKey(KeyCode.RightArrow);
        bool down = Input.GetKey(KeyCode.DownArrow);
        bool up = Input.GetKey(KeyCode.UpArrow);

        if(left ^ right)
        {
            transform.Translate(Vector3.right * moveSpeed * Time.fixedDeltaTime * (left ? -1.0f : 1.0f));
        }
        if(up ^ down)
        {
            transform.Translate(Vector3.up * moveSpeed * Time.fixedDeltaTime * (down ? -1.0f : 1.0f));
        }
    }
}

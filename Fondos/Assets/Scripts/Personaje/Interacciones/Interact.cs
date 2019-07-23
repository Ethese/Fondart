using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

[RequireComponent(typeof(Rigidbody))]
public class Interact : MonoBehaviour
{

    [HideInInspector]
    public Hand activeHand = null;

    public bool examined, dropped, impact, touched, picked;
    public float velocity;
    private TextMesh text;

    private void Start()
    {
        text = GetComponentInChildren<TextMesh>();
        examined = false;
        picked = false;
        if (text)
        {
            text.text = string.Empty;
        }
    }

    private void Update()
    {
        if (examined)
        {
            Examine();
        }
        else
        {
            if (text != null)
            {
                text.text = string.Empty;
            }
        }
    }

    #region COLLISIONS
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Hand" || Input.GetKeyDown("l"))
        {
            examined = true;
            touched = true;
        }

        if (examined && velocity > 0)
        {
            if (other.tag == "ENEMY")
            {
                Debug.Log("OUF");
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Hand")
        {
            examined = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Hand" || Input.GetKeyUp("l"))
        {
            examined = false;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        foreach (ContactPoint contact in collision.contacts)
        {
            impact = true;
        }
    }
    #endregion
    public void Examine()
    {
        if (text)
        {
            text.text = gameObject.name; // show object STATS
        }
    }

    public void GetVelocity(Vector3 vel)
    {
        velocity = vel.magnitude; // get velocity from hand velocity
    }
}

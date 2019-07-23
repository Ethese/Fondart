using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
using vida;

public class Player : MonoBehaviour
{
    #region Variables
    // Hands
    public Hand leftHand;
    public Hand rightHand;
    private Hand currentHand;

    // References
    private Transform parent;
    private AudioSource aud;
    public Animator an;
    public VidaJugador vida;
    public float leftSpeed;
    public float rightSpeed;

    // Movement
    public bool isMoving;
    public float markDistanceX;
    public float markDistanceY;
    public float markDistanceZ;
    public float speed;
    public float time;
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        currentHand = null;
        parent = transform.parent;
        aud = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        leftSpeed = leftHand.handSpeed;
        rightSpeed = rightHand.handSpeed;

        GetHand(); // get current hand using button

        if (isMoving)
        {
            time = 1 * Time.deltaTime;
            SoundCont();
            Move(currentHand.SetDirection());
            //an.SetBool("walking", true);
        }
        else
        {
            aud.Stop();
            //an.SetBool("walking", false);
        }

        // Stop sound if there´s no marker
        if (!currentHand)
        {
            aud.Stop();
        }

        if (currentHand)
        {
            DistanceContr();

            if (!currentHand.go)
            {
                return;
            }
            else
            {
                markDistanceX = transform.position.x - currentHand.go.transform.position.x;
                markDistanceY = transform.position.y - currentHand.go.transform.position.y;
                markDistanceZ = transform.position.z - currentHand.go.transform.position.z;
            }
        }

        Sprint();
    }

    #region Triggers
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Marker")
        {
            isMoving = false;
            Destroy(other.gameObject); // destroy marker on collision
        }

        if (other.tag == "Finish")
        {
            vida.Death();
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Marker")
        {
            isMoving = false;
            Destroy(other.gameObject); // destroy marker2 on collision
        }

        if (other.tag == "Finish")
        {
            vida.Death();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Marker")
        {
            isMoving = false;
        }
    }
    #endregion

    #region Methods
    public void Move(Vector3 target)
    {
        float moving = speed * Time.deltaTime;
        
        parent.position = Vector3.MoveTowards(new Vector3(parent.position.x, parent.position.y, parent.position.z), target, moving);
        if (parent.position == target)
        {
            isMoving = false;
        }
    }

    public void DistanceContr()
    {
        if (markDistanceX < 1 && markDistanceZ < 1 && markDistanceY > 0.4 || markDistanceX < 1 && markDistanceZ < 1 && markDistanceY < 0)
        {
            Vector3 go = currentHand.go.transform.position;
            currentHand.go.transform.position = new Vector3(go.x, transform.position.y, go.z);
        }
    }

    public void GetHand()
    {
        if (rightHand.isUsing)
        {
            currentHand = rightHand;
            leftHand.isUsing = false;
        }

        if (leftHand.isUsing)
        {
            currentHand = leftHand;
            rightHand.isUsing = false;
        }
    }

    public void Sprint()
    {
        if (rightSpeed > 5f && leftSpeed > 5f)
        {
            float extraSpeed = 3;
            speed = speed + extraSpeed;
        }
        else
        {
            speed = 3;
        }
    }

    public void SoundCont()
    {
        if (!aud.isPlaying)
        {
            aud.Play();
        }
    }
    #endregion
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rabbit : MonoBehaviour
{
    public Animator anim;
    public CharacterController controller;
    public GameObject player;
    public float timeToChangeDirection;
    public float speed;
    Quaternion desiredRotation;
    bool panicmode;
    private int life;
    private float panicspeed;
    public AudioClip hurt;
    // Start is called before the first frame update
    void Start()
    {
        speed = 3f;
        life = 2;
        panicspeed = 12f;
        player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        if (life > 0)
        {
            if (!panicmode)
            {
                if (timeToChangeDirection < 0f)
                {
                    if (Random.Range(0, 2) == 0)
                    {
                        speed = 0f;
                        anim.SetBool("run", false);
                    }
                    else
                    {
                        anim.SetBool("run", true);
                        float newyRotationValue = Random.Range(-45f, 45f);
                        desiredRotation = transform.rotation * Quaternion.Euler(Vector3.up * newyRotationValue);
                        speed = Random.Range(1f, 4f);
                    }
                    timeToChangeDirection = 4f;
                }
                transform.rotation = Quaternion.Lerp(transform.rotation, desiredRotation, 5f * Time.deltaTime);
                Vector3 movingVector = this.transform.forward;
                movingVector.y = -1f;
                controller.Move(movingVector * speed * Time.deltaTime);
                timeToChangeDirection -= Time.deltaTime;
            }
            else
            {
                Vector3 connectionPlayerRabbit = this.transform.position - player.transform.position;
                desiredRotation = Quaternion.LookRotation(connectionPlayerRabbit);
                transform.rotation = Quaternion.Lerp(transform.rotation, desiredRotation, 20f * Time.deltaTime);
                Vector3 movingVector = this.transform.forward;
                movingVector.y = -1f;
                controller.Move(movingVector * speed * Time.deltaTime);
                speed = panicspeed;
                anim.SetBool("run", true);
                anim.SetFloat("runspeed", 2f);
            }
            if ((Vector3.Distance(this.transform.position, player.transform.position) < 50f) && (Vector3.Angle(this.transform.forward, player.transform.position - this.transform.position) < 90))
            {
                panicmode = true;
                Debug.Log("panic!");
            }
            if (Vector3.Distance(this.transform.position, player.transform.position) > 100f)
            {
                panicmode = false;
            }
        }
        
    }

    public bool takeDamage(int damage)
    {
        if (life > 0)
        {
            life -= damage;
            panicspeed -= 6f;
            panicmode = true;
            AudioSource.PlayClipAtPoint(hurt, transform.position, 1f);
            if (life <= 0)
            {
                anim.SetTrigger("die");
                panicspeed -= 6f;
            }
            return true;
        }
        return false;
    }
}

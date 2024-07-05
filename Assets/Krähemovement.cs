using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Krähemovement : MonoBehaviour
{
    public Animator anim;
    public CharacterController controller;
    public GameObject player;
    private float timeToChangeDirection;
    private float speed;
    Quaternion desiredRotation;
    bool attackmode;
    bool alive;
    float attackCooldown;
    public AudioClip hurt;
    void Start()
    {
        speed = 5f;
        this.GetComponent<ParticleSystem>().Stop();
        alive = true;
        player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        if (alive && !GameController.tutorialMode)
        {
            if (!attackmode)
            {
                if (timeToChangeDirection < 0f)
                {
                    float newyRotationValue = Random.Range(-45f, 45f);
                    desiredRotation = transform.rotation * Quaternion.Euler(Vector3.up * newyRotationValue);
                    speed = Random.Range(1f, 4f);
                    timeToChangeDirection = 4f;
                }
                transform.rotation = Quaternion.Lerp(transform.rotation, desiredRotation, 5f * Time.deltaTime);
                controller.Move(this.transform.forward * speed * Time.deltaTime);
                timeToChangeDirection -= Time.deltaTime;
            }
            else
            {
                Vector3 playerPos = player.transform.position;
                playerPos.y += 4f;
                Vector3 connectionPlayerKrähe = player.transform.position - this.transform.position;
                desiredRotation = Quaternion.LookRotation(connectionPlayerKrähe);
                transform.rotation = Quaternion.Lerp(transform.rotation, desiredRotation, 20f * Time.deltaTime);
                if (Vector3.Distance(this.transform.position, player.transform.position) > 1.5f)
                {
                    controller.Move(this.transform.forward * speed * Time.deltaTime);
                }
                speed = 12f;
                attackCooldown -= Time.deltaTime;
                if (attackCooldown < 0f && Vector3.Distance(this.transform.position, player.transform.position) < 2f)
                {
                    attack();
                }
            }
            if ((Vector3.Distance(this.transform.position, player.transform.position) < 50f) && (Vector3.Angle(this.transform.forward, player.transform.position - this.transform.position) < 90))
            {
                attackmode = true;
            }
            if (Vector3.Distance(this.transform.position, player.transform.position) > 100f)
            {
                attackmode = false;
            }
        }
        else
        {
            if (!alive)
            {
                controller.Move(-Vector3.up * Time.deltaTime * 25f);
            }
        }

    }

    private void attack()
    {
            anim.SetTrigger("attack");
            Collider[] hitColliders = Physics.OverlapSphere(this.transform.position, 3f);
            foreach (var hitCollider in hitColliders)
            {
                if (hitCollider.tag == "Player")
                {
                    Debug.Log("Player hit");
                    hitCollider.GetComponent<ParticleSystem>().Play();
                    hitCollider.GetComponent<Player>().takeDamage();
                }
            }
            attackCooldown = 1f;
    }
    public void takeDamage()
    {
        anim.SetTrigger("die");
        AudioSource.PlayClipAtPoint(hurt, transform.position, 1f);
        alive = false;
    }
}

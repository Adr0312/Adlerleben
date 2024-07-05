using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.UI;


public class Player : MonoBehaviour
{

    private InputDevice leftController;
    private InputDevice rightController;
    private bool canMove;
    public CharacterController controller;
    public Animator anim;
    public Camera mainCamera;
    public float spanWidth;
    private float attackCooldown;
    private bool wingsbroken;
    public bool onGround;
    private float health;
    private float maxHealth;
    public Image healthBar;
    public Image blackImage;
    public Image waterImage;
    public Text hungerText;
    public Text ausdauerText;
    float baseSpeed;
    float hunger;
    private float maxHunger;
    float stamina;
    private float maxStamina;
    private float flapWingsCooldown;
    public Animator animWings;
    private float liftDuration;
    private bool inFreeFall;
    public GameObject leftWing;
    public GameObject rightWing;
    public GameObject eagleFeet;
    public Text deathText;
    private bool killedByTornado;
    public AudioClip hurt;
    public AudioClip treeHit;
    private float flyAgainstRebound;
    // Start is called before the first frame update
    void Start()
    {
        spanWidth = 10;
        this.GetComponent<ParticleSystem>().Stop();
        var ControllerLeft = new List<UnityEngine.XR.InputDevice>();
        var ControllerRight = new List<UnityEngine.XR.InputDevice>();
        var desiredCharacteristicsLeft = UnityEngine.XR.InputDeviceCharacteristics.HeldInHand | UnityEngine.XR.InputDeviceCharacteristics.Left | UnityEngine.XR.InputDeviceCharacteristics.Controller;
        var desiredCharacteristicsRight = UnityEngine.XR.InputDeviceCharacteristics.HeldInHand | UnityEngine.XR.InputDeviceCharacteristics.Right | UnityEngine.XR.InputDeviceCharacteristics.Controller;
        UnityEngine.XR.InputDevices.GetDevicesWithCharacteristics(desiredCharacteristicsLeft, ControllerLeft);
        UnityEngine.XR.InputDevices.GetDevicesWithCharacteristics(desiredCharacteristicsRight, ControllerRight);

        foreach (var device in ControllerLeft)
        {
            leftController = device;
            Debug.Log("left found");
        }
        foreach (var device in ControllerRight)
        {
            rightController = device;
            Debug.Log("right found");
        }
        health = 100f;
        maxHealth = 100f;
        hunger = 100f;
        maxHunger = 100f;
        stamina = 100f;
        maxStamina = 100f;
        baseSpeed = 5f;
        canMove = false;
        onGround = false;
        wingsbroken = false;
        inFreeFall = false;
        controller.enabled = true;
        blackImage.color = new Color(blackImage.color.r, blackImage.color.g, blackImage.color.b, 0f);
        flapWingsCooldown = 0.5f;
    }

    // Update is called once per frame
    void Update()
    {
        if (health > 0f)
        {
            attack();
            flapWings();
            regneration();
            checkIfUnderWater();
        }
    }

    private void FixedUpdate()
    {
            checkIfFlyAgainst();
            checkIfGrounded();
        if (leftController.isValid && rightController.isValid)
        {
            PlayerMovementVR();
        }
        else
        {
            PlayerMovementRealisticDebug();
        }
    }

    private void regneration()
    {
        if (health < 100f)
        {
            if (hunger > 50f)
            {
                if (hunger > 80f)
                {
                    updateHealth(Time.deltaTime * 3f);
                }
                else
                {
                    updateHealth(Time.deltaTime);
                }
            }
        }
        else
        {
            wingsbroken = false;
        }
    }
    private void PlayerMovementVR()
    {
        if (stamina > 0f && hunger > 0f && health > 0f)
        {
            if (canMove)
            {
                if (!wingsbroken && !onGround && !inFreeFall)
                {
                    leftController.TryGetFeatureValue(CommonUsages.devicePosition, out Vector3 positionLeft);
                    rightController.TryGetFeatureValue(CommonUsages.devicePosition, out Vector3 positionRight);
                    spanWidth = (Mathf.Abs(positionLeft.x) + Mathf.Abs(positionRight.x));
                    if (spanWidth < 0.5f)
                    {
                        eagleFeet.SetActive(true);
                    }
                    else
                    {
                        eagleFeet.SetActive(false);
                    }
                    float difference;
                    float yRotation;
                    Quaternion newRotation;
                    if (positionLeft.y < positionRight.y)
                    {
                        difference = (positionRight.y - positionLeft.y);
                        float angle = (difference / 0.7f) * 60;
                        yRotation = transform.eulerAngles.y;
                        yRotation -= Time.deltaTime * difference * 200f;
                        newRotation = Quaternion.Euler(transform.eulerAngles.x, yRotation, angle);
                        transform.rotation = Quaternion.Lerp(transform.rotation, newRotation, Time.deltaTime * 20f);
                    }
                    else
                    {
                        difference = (positionLeft.y - positionRight.y);
                        float angle = -(difference / 0.7f) * 60;
                        yRotation = transform.eulerAngles.y;
                        yRotation += Time.deltaTime * difference * 200f;
                        newRotation = Quaternion.Euler(transform.eulerAngles.x, yRotation, angle);
                        transform.rotation = Quaternion.Lerp(transform.rotation, newRotation, Time.deltaTime * 20f);
                    }
                    Vector3 moveDirection = this.transform.forward;
                    if ((1f - spanWidth) > 0)
                    {
                        moveDirection.y = -Mathf.Sqrt(1f - spanWidth);
                    }
                    if (this.transform.position.y > 400f)
                    {
                        moveDirection.y = -1f;
                    }
                    controller.Move(moveDirection * baseSpeed * (15f - (spanWidth * 7f)) * Time.deltaTime);
                }
                else if (!inFreeFall)
                {
                    if (leftController.TryGetFeatureValue(CommonUsages.primary2DAxis, out Vector2 joystickleft))
                    {
                        if (rightController.TryGetFeatureValue(CommonUsages.primary2DAxis, out Vector2 joystickRight))
                        {
                            transform.Rotate(Vector3.up, joystickRight.x * 100f * Time.deltaTime);
                        }
                        if (joystickleft.x != 0 || joystickleft.y != 0)
                        {
                            updateStamina(-Time.deltaTime);
                            Vector3 move = this.transform.forward;
                            move.y = -1f;
                            float angle = Mathf.Atan2(joystickleft.x, joystickleft.y);
                            Vector3 adaptedMovement = Quaternion.AngleAxis(angle * Mathf.Rad2Deg, Vector3.up) * move;
                            if (this.transform.position.y < -1.5f)
                            {
                                controller.Move(adaptedMovement * Time.deltaTime * baseSpeed*1.5f);
                            }
                            else
                            {
                                controller.Move(adaptedMovement * Time.deltaTime * baseSpeed * 2f);
                            }
                        }
                        else
                        {
                            if (stamina < maxStamina && !GameController.tutorialMode)
                            {
                                updateStamina(Time.deltaTime * 10f);
                            }
                        }
                    }
                }
                else
                {
                    controller.Move(-Vector3.up * Time.deltaTime * baseSpeed * 2f);
                }
            }
        }
        else
        {
            inFreeFall = true;
            blackImage.color = new Color(blackImage.color.r, blackImage.color.g, blackImage.color.b, blackImage.color.a + Time.deltaTime);
            checkForDeathReason();
        }
    }
    private void PlayerMovementRealisticDebug()
    {
        if (stamina > 0)
        {
            Cursor.lockState = CursorLockMode.Locked;
            float mouseSensitivity = 5f;
            float horizontal = Input.GetAxis("Mouse X");
            float vertical = Input.GetAxis("Mouse Y");

            transform.Rotate(0, horizontal * mouseSensitivity, 0);
            transform.Rotate(-vertical * mouseSensitivity, 0, 0);
            float forwards = Input.GetAxis("Vertical");
            if (!wingsbroken && !onGround)
            {
                Debug.Log("in air");
                if (forwards != 0)
                {
                    controller.Move(mainCamera.transform.forward * baseSpeed * 10f * forwards * Time.deltaTime);
                    updateStamina(-(15f - spanWidth) / Time.deltaTime);
                    updateHunger(-10f * Time.deltaTime);
                }
            }
            else if (onGround)
            {
                Debug.Log("on ground");
                Vector3 movement = mainCamera.transform.forward;
                movement.y = 0f;
                controller.Move(movement * baseSpeed * forwards * Time.deltaTime);
                if (forwards != 0)
                {
                    updateStamina(-(15f - spanWidth) / 200f);
                    updateHunger(-5f * Time.deltaTime);
                }
            }
        }
        else
        {
            inFreeFall = true;
            blackImage.color = new Color(blackImage.color.r, blackImage.color.g, blackImage.color.b, blackImage.color.a + 0.002f);
        }

    }

    private void attack()
    {
        if (stamina > 0f && hunger > 0f && health > 0f)
        {
            if (attackCooldown < 0f && Input.GetButton("Fire1"))
            {
                if (onGround)
                {
                    StartCoroutine(attackFromGroundRoutine());
                }
                else
                {
                    StartCoroutine(attackRoutine());
                }
            }
            attackCooldown -= Time.deltaTime;
        }
    }

    IEnumerator attackRoutine()
    {
        eagleFeet.GetComponent<Animator>().enabled = true;
        anim.SetTrigger("attack");
        attackCooldown = 1f;
        Collider[] hitColliders = Physics.OverlapSphere(eagleFeet.transform.position, 2f);
        foreach (var hitCollider in hitColliders)
        {
            if (hitCollider.tag == "Rabbit")
            {
                hitCollider.GetComponent<ParticleSystem>().Play();
                hitCollider.gameObject.GetComponent<rabbit>().takeDamage(1);
                updateHunger(20f);
            }
            if (hitCollider.tag == "Krähe")
            {
                Debug.Log("Krähe hit");
                hitCollider.GetComponent<ParticleSystem>().Play();
                hitCollider.gameObject.GetComponent<Krähemovement>().takeDamage();
            }
        }
        yield return new WaitForSeconds(1f);
        eagleFeet.GetComponent<Animator>().enabled = false;
    }

    IEnumerator attackFromGroundRoutine()
    {
        attackCooldown = 1f;
        eagleFeet.GetComponent<Animator>().enabled = true;
        anim.SetTrigger("attackFromGround");
        yield return new WaitForSeconds(0.2f);
        Collider[] hitColliders = Physics.OverlapSphere(eagleFeet.transform.position, 2f);
        foreach (var hitCollider in hitColliders)
        {
            if (hitCollider.tag == "Rabbit")
            {
                hitCollider.GetComponent<ParticleSystem>().Play();
                if (hitCollider.gameObject.GetComponent<rabbit>().takeDamage(1))
                {
                    updateHunger(20f);
                }
            }
            if (hitCollider.tag == "Krähe")
            {
                hitCollider.GetComponent<ParticleSystem>().Play();
                hitCollider.gameObject.GetComponent<Krähemovement>().takeDamage();
            }
        }
        yield return new WaitForSeconds(0.8f);
        eagleFeet.GetComponent<Animator>().enabled = false;
    }


    private void flapWings()
    {
        if (stamina > 0f && hunger > 0f && health > 0f)
        {
            leftController.TryGetFeatureValue(CommonUsages.devicePosition, out Vector3 positionLeft);
            rightController.TryGetFeatureValue(CommonUsages.devicePosition, out Vector3 positionRight);
            spanWidth = (Mathf.Abs(positionLeft.x) + Mathf.Abs(positionRight.x));
            if (flapWingsCooldown < 0f && Input.GetButton("Fire3") && health > 99f&& spanWidth>0.5f)
            {
                leftWing.SetActive(true);
                rightWing.SetActive(true);
                animWings.SetTrigger("flapwings");
                if (!GameController.tutorialMode)
                {
                    updateStamina(-1f);
                }
                liftDuration = 0.1f;
                flapWingsCooldown = 0.5f;
                if (onGround)
                {
                    onGround = false;
                    eagleFeet.SetActive(true);
                    placeFeetinAir();
                }
            }
            if (liftDuration > 0f)
            {
                controller.Move(Vector3.up * Time.deltaTime * liftDuration * 1000f);
                liftDuration -= Time.deltaTime;
            }
            flapWingsCooldown -= Time.deltaTime;
        }
    }


    private void updateHealth(float amount)
    {
        health += amount;
        float ratio = health / maxHealth;
        if (ratio < 0f)
        {
            ratio = 0f;
        }
        healthBar.color = new Color(healthBar.color.r, healthBar.color.g, healthBar.color.b, (1f - ratio)*(2/3));
    }
    private void updateStamina(float amount)
    {
        stamina += amount;
        if (stamina < 50f)
        {
            ausdauerText.text = "Du fühlst dich erschöpft";
            if (stamina < 25f)
            {
                ausdauerText.text = "Du fühlst dich sehr erschöpft";
                if (stamina < 10f)
                {
                    ausdauerText.text = "Du hast keine Kraft mehr";
                }
            }
        }
        else
        {
            ausdauerText.text = "";
        }
    }
    private void updateHunger(float amount)
    {
        hunger += amount;
        if (hunger > maxHunger)
        {
            hunger = maxHunger;
        }
        if (hunger < 50f)
        {
            hungerText.text = "Du hast Hunger";
            if (hunger < 25f)
            {
                hungerText.text = "Du hast sehr starken Hunger";
                if (hunger < 10f)
                {
                    hungerText.text = "Du bist kurz vorm verhungern";
                }
            }
        }
        else
        {
            hungerText.text = "";
        }
    }

    public void takeDamage()
    {
        updateHealth(-Random.Range(10f, 15f));
        AudioSource.PlayClipAtPoint(hurt, transform.position, 1f);
    }

    public void checkIfGrounded()
    {
        if (controller.isGrounded && this.transform.position.y<50f)
        {
            placeFeetOnGround();
            eagleFeet.SetActive(true);
            leftWing.SetActive(false);
            rightWing.SetActive(false);
            onGround = true;
            inFreeFall = false;
        }
    }

    public void checkIfFlyAgainst()
    {
        RaycastHit hit;
        // Does the ray intersect any objects excluding the player layer
        if (Physics.Raycast(this.transform.position, this.transform.forward, out hit, 2f))
        {
            Debug.Log("gameobject hit: " + hit.transform.tag);
            if (!wingsbroken && !onGround)
            {
                if (hit.transform.tag == "Terrain")
                {
                    updateHealth(-30f);
                    AudioSource.PlayClipAtPoint(treeHit, transform.position, 1f);
                    wingsbroken = true;
                    inFreeFall = true;
                    flyAgainstRebound = 0.5f;
                }
            }
        }
        if (flyAgainstRebound > 0f)
        {
            controller.Move(-this.transform.forward * flyAgainstRebound * 30f*Time.deltaTime);
            flyAgainstRebound -= Time.deltaTime;
        }
    }

    public void movePlayer(Vector3 move, float speed)
    {
        controller.Move(move * speed * Time.deltaTime);
    }

    public void rotatePlayer(Vector3 origin, float rotateSpeed)
    {
        transform.RotateAround(origin, Vector3.up, rotateSpeed * Time.deltaTime);
        blackImage.color = new Color(blackImage.color.r, blackImage.color.g, blackImage.color.b, blackImage.color.a + Time.deltaTime / 2f);
        checkForDeathReason();
        controller.Move(Vector3.up * 150f * Time.deltaTime);
        killedByTornado = true;
    }

    private void placeFeetOnGround()
    {
        eagleFeet.transform.localPosition = new Vector3(0, 0.18f, 1.55f);
        eagleFeet.transform.localEulerAngles = new Vector3(
                -90f, 0, 0
        );
    }

    private void placeFeetinAir()
    {
        eagleFeet.transform.localPosition = new Vector3(0, 0.679f, 2f);
        eagleFeet.transform.localEulerAngles = new Vector3(
                -120f, 0, 0
        );
    }

    private void checkForDeathReason()
    {
        if (blackImage.color.a > 0.98f)
        {
            if (hunger <= 0f)
            {
                deathText.text = "Du bist verhungert. \nDrücke 'A' um das Spiel neu zu starten.";
            }
            else if (stamina <= 0f)
            {
                deathText.text = "Du bist an Überanstrengung gestorben. \nDrücke 'A' um das Spiel neu zu starten.";
            }
            else if (health <= 0f)
            {
                deathText.text = "Du hast zu viele Wunden erlitten. \nDrücke 'A' um das Spiel neu zu starten.";
            } else if (killedByTornado)
            {
                deathText.text = "Du wurdest von einem Tornado erfasst. \nDrücke 'A' um das Spiel neu zu starten.";
            }
            hungerText.text = "";
            ausdauerText.text = "";
            GameController.deathState = true;
        }
    }

    public void activatePlayerMovement()
    {
        canMove = true;
    }

    private void checkIfUnderWater()
    {
        if (this.transform.position.y < -1.5f)
        {
            waterImage.color = new Color(waterImage.color.r, waterImage.color.g, waterImage.color.b,0.6f);
        }
        else
        {
            waterImage.color = new Color(waterImage.color.r, waterImage.color.g, waterImage.color.b, 0f);
        }
    }
}

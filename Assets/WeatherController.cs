using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeatherController : MonoBehaviour
{
    // Start is called before the first frame update
    bool rising;
    public ParticleSystem rain;
    float changeCooldown;
    bool raining;
    float beginRaining;
    float beginRainingExtreme;
    float stopRaining;
    public CloudSpawner cloudspawner;
    public GameObject tornadoPrefab;
    private GameObject tornado;
    private bool tornadoSpawned;
    public Transform TornadoSpawnPos1;
    public Transform TornadoSpawnPos2;
    public Transform TornadoSpawnPos3;
    public Transform TornadoSpawnPos4;
    public AudioSource audioPlayer;
    public AudioClip normalSound;
    public AudioClip windSound;
    public AudioClip rainSound;
    public AudioClip tornadoSound;
    public Player playerScript;
    private int weathermode;
    private bool playerWasOnGround;

    void Start()
    {
        rising = true;
        changeCooldown = 10f;
        rain.Stop();
        audioPlayer.clip = normalSound;
        audioPlayer.volume = 0.3f;
        weathermode = 1;
        audioPlayer.Play();
        playerWasOnGround = true;
    }

    // Update is called once per frame
    void Update()
    {
        controlRain();
        controlDayNightCycle();
        if (GameController.deathState)
        {
            audioPlayer.Pause();
        }
        if (weathermode == 1)
        {
            if (playerScript.onGround)
            {
                if (playerWasOnGround == false)
                {
                    audioPlayer.clip = normalSound;
                    audioPlayer.volume = 0.3f;
                    audioPlayer.Play();
                    playerWasOnGround = true;

                }
            }
            else
            {
                if (playerWasOnGround == true)
                {
                    audioPlayer.clip = windSound;
                    audioPlayer.volume = 1f;
                    audioPlayer.Play();
                    playerWasOnGround = false;
                }
            }
        }
    }

    public void controlRain()
    {
        changeCooldown -= Time.deltaTime;
        if (changeCooldown < 0f)
        {
            weathermode = Random.Range(0, 3);
            weathermode = 1;
            if (GameController.tutorialMode)
            {
                weathermode = 1;
            }
            if (weathermode == 0)
            {
                if (raining == false)
                {
                    beginRaining = 3f;
                    rain.Play();
                    audioPlayer.clip = rainSound;
                    audioPlayer.volume = 1f;
                    audioPlayer.Play();
                }
                raining = true;
            }
            else if(weathermode == 1)
            {
                if (raining == true)
                {
                    stopRaining = 3f;
                    rain.Stop();
                }
                raining = false;
            }
            else if (weathermode == 2)
            {
                raining = true;
                rain.Play();
                beginRainingExtreme = 2f;
                audioPlayer.clip = tornadoSound;
                audioPlayer.volume = 0.1f;
                audioPlayer.Play();
            }
            changeCooldown = 60f;
        }
        if (beginRaining > 0f)
        {
            if (RenderSettings.ambientIntensity > 0.4f)
            {
                RenderSettings.ambientIntensity -= Time.deltaTime / 10f;
            }
            beginRaining -= Time.deltaTime;
        }
        if (beginRainingExtreme > 0f)
        {
            cloudspawner.setSpawnInstantly();
            if (RenderSettings.ambientIntensity > 0.4f)
            {
                RenderSettings.ambientIntensity -= Time.deltaTime / 2f;
            }
            if (!tornadoSpawned)
            {
                int mode = Random.Range(0, 4);
                if (mode == 0)
                {
                    tornado = Instantiate(tornadoPrefab, TornadoSpawnPos1.position, Quaternion.identity);
                    tornado.GetComponent<tornadoMovement>().movingVector = new Vector3(1f, 0f, 1.5f);
                }
                else if (mode == 1)
                {
                    tornado = Instantiate(tornadoPrefab, TornadoSpawnPos2.position, Quaternion.identity);
                    tornado.GetComponent<tornadoMovement>().movingVector = new Vector3(1f, 0f, -1.5f);
                }
                else if (mode == 2)
                {
                    tornado = Instantiate(tornadoPrefab, TornadoSpawnPos3.position, Quaternion.identity);
                    tornado.GetComponent<tornadoMovement>().movingVector = new Vector3(-1f, 0f, -1.5f);
                }
                else
                {
                    tornado = Instantiate(tornadoPrefab, TornadoSpawnPos4.position, Quaternion.identity);
                    tornado.GetComponent<tornadoMovement>().movingVector = new Vector3(-1f, 0f, 1.5f);
                }
                tornadoSpawned = true;
            }
            beginRainingExtreme -= Time.deltaTime;
        }
        if (stopRaining > 0f)
        {
            Destroy(tornado);
            tornadoSpawned = false;
            if (RenderSettings.ambientIntensity < 2f && RenderSettings.ambientIntensity > 0.5f)
            {
                RenderSettings.ambientIntensity += Time.deltaTime / 10f;
            }
            stopRaining -= Time.deltaTime;
        }
    }

    public void controlDayNightCycle()
    {
        if (!GameController.tutorialMode)
        {
            if (rising)
            {
                RenderSettings.ambientIntensity += Time.deltaTime / 100f;
                if (RenderSettings.ambientIntensity > 2f)
                {
                    rising = false;
                }
            }
            else
            {
                RenderSettings.ambientIntensity -= Time.deltaTime / 100f;
                if (RenderSettings.ambientIntensity < 0.4f)
                {
                    rising = true;
                }
            }
        }
    }

  
}

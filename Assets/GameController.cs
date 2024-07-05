using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    public static bool tutorialMode;
    public static bool deathState;
    private bool selectTutorialStage;
    public Text tutorialText;
    public Transform startPos;
    public GameObject Player;
    // Start is called before the first frame update
    void Start()
    {
        selectTutorialStage = true;
        deathState = false;
        GameController.tutorialMode = true;
    }

    // Update is called once per frame
    void Update()
    {
        if(selectTutorialStage && Input.GetButton("Fire1"))
        {
            tutorialMode = true;
            selectTutorialStage = false;
            StartCoroutine(tutorial());
        } else if(selectTutorialStage && Input.GetButton("Fire3"))
        {
            tutorialMode = false;
            selectTutorialStage = false;
            tutorialText.text = "";
            Player.GetComponent<Player>().activatePlayerMovement();
        }
        else if (deathState && Input.GetButton("Fire1"))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }

    IEnumerator tutorial()
    {
        tutorialText.text = "Du wachst auf...";
        yield return new WaitForSeconds(4f);
        tutorialText.text = "Du erinnerst dich nicht mehr genau daran was passiert ist.";
        yield return new WaitForSeconds(5f);
        tutorialText.text = "Du weißt nur noch, dass du unerwartet in einen Sturm geraten bist.";
        yield return new WaitForSeconds(5f);
        tutorialText.text = "Es scheint so als wärst du in einem neuen unbekannten Gebiet.";
        yield return new WaitForSeconds(8f);
        tutorialText.text = "Versuche zu überleben.";
        yield return new WaitForSeconds(5f);
        tutorialText.text = "Du weißt nicht mehr wie man sich als Adler bewegt?";
        yield return new WaitForSeconds(5f);
        tutorialText.text = "Strecke deine Arme seitlich aus um in den Gleitmodus zu gehen.\n" +
            " Der Gleitmodus verbraucht keine Ausdauer. Nutze ihn um Beute zu erspähen.";
        yield return new WaitForSeconds(10f);
        Player.GetComponent<Player>().activatePlayerMovement();
        tutorialText.text = "Neige deine Flügel (Arme) um nach links oder rechts zu fliegen.";
        yield return new WaitForSeconds(10f);
        tutorialText.text = "Hast du deine Beute erfasst so lege die Arme an um in den Sturzflug zu gehen.";
        yield return new WaitForSeconds(10f);
        tutorialText.text = "Im Sturzflug und auf dem Boden kannst du mit 'A'\n" +
            " (rechter Controller unterer Button) mit deinen Krallen angreifen.";
        yield return new WaitForSeconds(10f);
        tutorialText.text = "Auf dem Boden kannst du dich mit dem linken Joystick bewegen.\n" +
            "Mit dem rechten Joystick kannst du dich drehen.";
        yield return new WaitForSeconds(10f);
        tutorialText.text = "Möchtest du nun wieder abheben oder bist in der Luft und möchtest höher fliegen,\n" +
            " drücke 'X' (linker Controller unterer Button) um mit den Flügeln zu schlagen.\n" +
            "Denke dabei daran deine Flügel (Arme) auszustrecken.\n" +
            " Mit den Flügeln zu schlagen ist anstrengend, raubt dir Energie und macht dich hungrig.";
        yield return new WaitForSeconds(20f);
        tutorialText.text = "Hast du keine Energie mehr, so lande kurz und bewege dich nicht um dich auszuruhen.\n" +
            " Bist du hungrig so finde Beute und erlege sie durch einen Angriff.\n";
        yield return new WaitForSeconds(10f);
        tutorialText.text = "Hast du Wunden erlitten, heilen sie automatisch,\n" +
            " falls du nicht hungrig bist.";
        yield return new WaitForSeconds(10f);
        tutorialText.text = "Zuletzt bleibt nur übrig dich zu warnen,\n" +
            " das Wetter kann manchmal sehr turbulent sein.";
        yield return new WaitForSeconds(10f);
        tutorialText.text = "";
        GameController.tutorialMode = false;
    }
}

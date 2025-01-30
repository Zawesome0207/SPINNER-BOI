using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class ui : MonoBehaviour
{
    bool uistatus;
    public GameObject player;
    public GameObject enemys;
    public GameObject sound;
    public GameObject music;
    public GameObject puasePanel;
    public GameObject startPanel;

    int dumb = 0;
    int dumbs = 0;
    public GameObject deadpan;

    private Player playerScript;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerScript = player.GetComponentInChildren<Player>();

        Time.timeScale = 0;

        playerScript.canDash = false;
        playerScript.canDodge = false;
    }

    // Update is called once per frame
    void Update()
    {
       if(Input.GetKeyDown(KeyCode.Escape))
        {
            if(uistatus)
            {
                unpause();
            }
            else
            {
                pause();
            }
        }
    }

    public void play()
    {
        Time.timeScale = 1;

        startPanel.SetActive(false);

        playerScript.canDash = true;
        playerScript.canDodge = true;
    }

    public void unpause()
    {
        Time.timeScale = 1;
        puasePanel.SetActive(false);
        uistatus = false;

        playerScript.canDash = true;
        playerScript.canDodge = true;
    }
    public void pause()
    {
        Time.timeScale = 0;
        puasePanel.SetActive(true);
        uistatus = true;

        playerScript.canDash = false;
        playerScript.canDodge = false;
    }
    public void muteP()
    {
        if( dumb%2==0)
        {
            AudioSource hi = sound.GetComponent<AudioSource>();
            hi.volume = 0;
            Debug.Log("mute");
        }
        else
        {
            AudioSource hi = sound.GetComponent<AudioSource>();
            hi.volume = 1;
            Debug.Log("unmute");
        }
        dumb++;
        
    }
    public void muteM()
    {
        if (dumbs % 2 == 0)
        {
            AudioSource hi = music.GetComponent<AudioSource>();
            hi.volume = 0;
            Debug.Log("mute");
        }
        else
        {
            AudioSource hi = music.GetComponent<AudioSource>();
            hi.volume = 1;
            Debug.Log("unmute");
        }
        dumbs++;

    }
    public void unmuteP()
    {
        
    }
    public void Quiter()
    {
        Application.Quit();
    }
    public void dead()
    {
        deadpan.SetActive(true);
    }
    public void resetart()
    {
        SceneManager.LoadScene("GameSecne");
    }

}

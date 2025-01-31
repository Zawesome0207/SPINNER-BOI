using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
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
    public GameObject instructionsPanelS;
    public GameObject instructionsPanelP;

    int dumb = 0;
    int dumbs = 0;
    public GameObject deadpan;
    public GameObject winPanel;

    private Player playerScript;
    private Enemy enemyScript;
    public GameObject enemyObject;

    public GameObject countDownParent;
    private GameObject[] countDowns;
    private int cntForDown;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerScript = player.GetComponentInChildren<Player>();
        enemyScript = enemyObject.GetComponent<Enemy>();

        List<GameObject> countDownse = GetChildren(countDownParent);

        countDowns = countDownse.ToArray();

        enemyScript.enabled = false;
        playerScript.enabled = false;
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
        startPanel.SetActive(false);

        countDowns[0].SetActive(true);

        cntForDown = 1;
        Invoke(nameof(countDown), 1);
    }

    public void playInstructionsS()
    {
        startPanel.SetActive(false);

        instructionsPanelS.SetActive(true);
    }
    public void playInstructionsP()
    {
        puasePanel.SetActive(false);

        instructionsPanelP.SetActive(true);
    }

    public void backToStart()
    {
        startPanel.SetActive(true);

        instructionsPanelS.SetActive(false); 
    }

    public void backToPause()
    {
        puasePanel.SetActive(true);

        instructionsPanelP.SetActive(false);
    }

    public void unpause()
    {
        Time.timeScale = 1;
        puasePanel.SetActive(false);
        uistatus = false;

        playerScript.enabled = true;
        enemyScript.enabled = true;
    }
    public void pause()
    {
        Time.timeScale = 0;
        puasePanel.SetActive(true);
        uistatus = true;

        playerScript.enabled = false;
        enemyScript.enabled = false;
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

    public void Win()
    {
        winPanel.SetActive(true);
    }

    public void resetart()
    {
        SceneManager.LoadScene("GameSecne");
    }

    private void countDown()
    {
        countDowns[cntForDown - 1].SetActive(false);

        if (cntForDown == countDowns.Length)
        {
            enemyScript.enabled = true;
            playerScript.enabled = true;
        }

        else
        {
            countDowns[cntForDown].SetActive(true);

            cntForDown++;
            Invoke(nameof(countDown), 1);
        }
    }

    public static List<GameObject> GetChildren(GameObject go)
    {
        List<GameObject> children = new List<GameObject>();
        foreach (Transform tran in go.transform)
        {
            children.Add(tran.gameObject);
        }
        return children;
    }
}

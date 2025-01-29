using UnityEngine;
using UnityEngine.UI;
public class ui : MonoBehaviour
{
    bool uistatus;
    public GameObject player;
    public GameObject enemys;
    public GameObject sound;
    public GameObject puasePanel;
    int dumb = 0;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
       if(Input.GetKeyDown(KeyCode.Escape))
        {
            if(uistatus)
            {
                //uistatus = false;
                unpause();
            }
            else
            {
                //uistatus = true;
                pause();
            }
        }
    }
    public void unpause()
    {
        Time.timeScale = 1;
        //Application.targetFrameRate = 60;
        puasePanel.SetActive(false);
        //player.gameObject.SetActive(true);
        //enemys.gameObject.SetActive(true);
        uistatus = false;
    }
    public void pause()
    {
        Time.timeScale = 0;
        //Application.targetFrameRate = 0;
        puasePanel.SetActive(true);
        //player.gameObject.SetActive(false);
        //enemys.gameObject.SetActive(false);
        uistatus = true;
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
    public void unmuteP()
    {
        
    }
    public void Quiter()
    {
        Application.Quit();
    }


}

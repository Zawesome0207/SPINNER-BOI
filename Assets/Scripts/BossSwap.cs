using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;

public class BossSwap : MonoBehaviour
{
    public List<GameObject> Enemies;
    private GameObject currentEnemy;
    private Enemy currentEnemyScript;
    private bool bossInPlay;

    private bool finalBossActive;
    private Enemy[] bosses;
    private bool[] hasPlayedDeathParticles;

    public GameObject normHealth;
    public GameObject finalHealth;

    [Header("Player")]
    public GameObject Player;
    private Player playerScript;
    private bool healPlayer;

    private int EnCount;
    private int finalDeathCnt;

    [Header("Ui")]
    public ui uiScript;

    private void Start()
    {
        EnCount = 0;
        finalDeathCnt = 0;

        currentEnemy = Enemies[EnCount];
        currentEnemyScript = currentEnemy.GetComponent<Enemy>();

        currentEnemy.SetActive(true);
        bossInPlay = true;

        finalBossActive = false;
        healPlayer = false;

        playerScript = Player.GetComponent<Player>();

        currentEnemyScript.dashReadyParticles.gameObject.SetActive(false);
        playerScript.dashReadyParticles.gameObject.SetActive(false);
    }

    private void Update()
    {
        if (currentEnemyScript.health <= 0 && bossInPlay)
        {
            currentEnemyScript.deathParticles.Play();
            bossInPlay = false;

            EnCount++;

            if(EnCount == Enemies.Capacity - 1)
            {
                Invoke(nameof(finalBoss), 2);
            }

            else 
            {
                Invoke(nameof(nextBoss), 2);
            }
        }

        if(!bossInPlay)
        {
            healPlayer = true;
        }

        if(healPlayer && !finalBossActive)
        {
            if(playerScript.health >= playerScript.maxhealth)
            {
                healPlayer = false;
            }

            else
            {
                playerScript.health += .5f;
            }
        }

        if(bosses != null)
        {
            int a = 0;
            foreach (Enemy E in bosses)
            {
                if(E.health <= 0 && !hasPlayedDeathParticles[a])
                {
                    PlayFinalDeathParticles(E.deathParticles, a);
                }

                a++;
            }

            if (finalDeathCnt == bosses.Length)
            {
                Win();
            }
        }
    }

    private void nextBoss()
    {
        currentEnemy.SetActive(false);

        currentEnemy = Enemies[EnCount];
        currentEnemyScript = currentEnemy.GetComponent<Enemy>();

        currentEnemy.SetActive(true);
        bossInPlay = true;
    }

    private void finalBoss()
    {
        bosses = Enemies[EnCount].GetComponentsInChildren<Enemy>();

        hasPlayedDeathParticles = new bool[bosses.Length];

        for(int a = 0; a < bosses.Length -1; a++)
        {
            hasPlayedDeathParticles[a] = false;
        }

        Enemies[EnCount].SetActive(true);
        normHealth.SetActive(false);
        finalHealth.SetActive(true);

        finalBossActive = true;
    }

    private void PlayFinalDeathParticles(ParticleSystem particles, int a)
    {
        particles.Play();

        finalDeathCnt++;
        hasPlayedDeathParticles[a] = true;
    }

    private void Win()
    {
        uiScript.Win();

        this.gameObject.SetActive(false);
    }
}

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

    [Header("Player")]
    public GameObject Player;
    private Player playerScript;
    private bool healPlayer;

    private int EnCount;

    private void Start()
    {
        EnCount = 0;

        currentEnemy = Enemies[EnCount];
        currentEnemyScript = currentEnemy.GetComponent<Enemy>();

        currentEnemy.SetActive(true);
        bossInPlay = true;

        healPlayer = false;

        playerScript = Player.GetComponent<Player>();
    }

    private void Update()
    {
        if (currentEnemyScript.health <= 0 && bossInPlay)
        {
            currentEnemyScript.deathParticles.Play();
            bossInPlay = false;

            EnCount++;

            if(EnCount > Enemies.Capacity)
            {
                Invoke(nameof(Win), 2);
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

        if(healPlayer)
        {
            playerScript.health += .3f;

            if(playerScript.health >= playerScript.maxhealth)
            {
                healPlayer = false;
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

    private void Win()
    {
        Debug.Log("You Won!");

        this.gameObject.SetActive(false);
    }
}

using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;

public class BossSwap : MonoBehaviour
{
    public List<GameObject> Enemies;
    private GameObject currentEnemy;
    private Enemy currentEnemyScript;

    [Header("Player")]
    public GameObject Player;
    private Player playerScript;

    private int EnCount;

    private void Start()
    {
        EnCount = 0;

        currentEnemy = Enemies[EnCount];
        currentEnemyScript = currentEnemy.GetComponent<Enemy>();

        currentEnemy.SetActive(true);

        playerScript = Player.GetComponent<Player>();
    }

    private void Update()
    {
        if (currentEnemyScript.health <= 0 && currentEnemy != null)
        {
            currentEnemyScript.deathParticles.Play();

            EnCount++;

            if(EnCount > Enemies.Capacity)
            {
                currentEnemy = null;

                Invoke(nameof(Win), 2);
            }

            else 
            {
                playerScript.health = playerScript.maxhealth;

                currentEnemy.SetActive(false);

                currentEnemy = Enemies[EnCount];
                currentEnemyScript = currentEnemy.GetComponent<Enemy>();

                currentEnemy.SetActive(true);
            }
        }
    }

    private void Win()
    {
        Debug.Log("You Won!");

        this.gameObject.SetActive(false);
    }
}

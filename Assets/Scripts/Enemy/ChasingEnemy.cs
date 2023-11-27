using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChasingEnemy : EnemyBase
{
    [SerializeField]
    private Transform target;

    public GameObject GameObject;

    protected override void Start()
    {
        base.Start();

        GameManager.Instance.game_winEvent.AddListener(GameWin);
    }

    protected override void Update()
    {
        base.Update();

        //update pathfinding
        moveDirection = target.position - transform.position;
        moveDirection.Normalize();

        if(currentHealth <= 0)
        {
            StartCoroutine(WinScreenWait());
        }
    }

    void GameWin()
    {
        SceneManager.LoadScene("WinScene");
    }

    IEnumerator WinScreenWait()
    {
        yield return new WaitForSeconds(2f);

        GameManager.Instance.game_winEvent.Invoke();
    }

}

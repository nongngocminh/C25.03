using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolState : IState
{
    float randomTime;
    float timer;

    public void OnEnter(Enemy enemy)
    {
        timer = 0;
        randomTime = UnityEngine.Random.Range(3f, 6f);
    }

    public void OnExecute(Enemy enemy)
    {
        timer += Time.deltaTime;

        if (enemy.Target != null)
        {
            if (enemy.IsTargetInRange())
            {
                enemy.ChangeState(new AttackState());
            }
            else
            {
                enemy.Moving();
            }

            enemy.ChangeDirection(enemy.Target.transform.position.x > enemy.transform.position.x);

            enemy.Moving();
        }
        else 
        {
            if (timer < randomTime)
            {
                enemy.Moving();
            }
            else
            {
                enemy.ChangeState(new IdleState());
            }
        }
    }

    public void OnExit(Enemy enemy)
    {

    }
}

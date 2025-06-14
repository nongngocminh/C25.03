using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Characters : MonoBehaviour
{
    private float hp;
    private string currentAnimName;
    [SerializeField] private Animator animator;

    public bool isDeath => hp <= 0;

    // Start is called before the first frame update
    void Start()
    {
        OnInit();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public virtual void OnInit()
    {
        hp = 100;
    }

    public virtual void OnDespawn()
    {

    }

    public virtual void OnDeath()
    {
        ChangeAnim("die");

        Invoke(nameof(OnDespawn), 2f);
    }

    public virtual void OnHit(float damage)
    {
        if (!isDeath)
        {
            hp -= damage;

            if (isDeath)
            {
                OnDeath();
            }
        }
    }

    protected void ChangeAnim(string animName)
    {
        if (currentAnimName != animName)
        {
            animator.ResetTrigger(animName);
            currentAnimName = animName;
            animator.SetTrigger(currentAnimName);
        }
    }
}

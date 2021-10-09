using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;
public class EnemyFly : EnemyStats
{

    public Transform attackPoint;
    public LayerMask playerLayerMask;

    // Start is called before the first frame update
    void Start()
    {
        InitilizeCharacter(OnAttack);
    }

    void OnAttack()
    {
        StartCoroutine(Delay());

        IEnumerator Delay()
        {
            yield return new WaitForSeconds(Random.Range(0.1f, 1f));

            if (dist < attackRange)
            {
                anim.SetTrigger("attack");
                rb.velocity = Vector2.zero;
            }

        }
        lastAttackTime = Time.time;
    }

    void Attack()
    {
        Collider2D player = Physics2D.OverlapCircle(attackPoint.transform.position, 0.5f, playerLayerMask);

        if(player != null && player.tag == "Player")
        {
            player.GetComponent<PlayerController>().TakeDamage(damage);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(attackPoint.position, 0.5f);
    }
}

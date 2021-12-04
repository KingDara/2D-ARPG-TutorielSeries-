using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Pathfinding;

public class EnemyStats : MonoBehaviour
{

    [Header("Stats")]
    public float speed;
    private float lastPlayerDetectTime;
    public float playerDetectRate = 0.2f;
    public float chaseRange;
    bool lookRight;
    protected int currentHealth;
    public int maxHealth;

    [Header("Attack")]
    public float attackRange;
    public float attackRate;
    protected float lastAttackTime;
    
    public int damage;

    [Header("Component")]
    protected Rigidbody2D rb;
    private PlayerController targetPlayer;
    protected Animator anim;
    public GameObject healthBar;
    public Image life;

    [Header("PathFinding")]
    public float nextWayPointDistance = 2f;
    protected Path path;
    int currentWaiPoint = 0;
    bool reachEndOfPath = false;
    protected Seeker seeker;
    protected float dist;

    [Header("Loot]")]
    public GameObject[] loot;

    public delegate void myAttack();
    public myAttack myMethode;


    public void InitilizeCharacter(myAttack _myMethode)
    {
        this.currentHealth = this.maxHealth;
        life.fillAmount = this.maxHealth;
        seeker = GetComponent<Seeker>();
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        InvokeRepeating("UpdatePath", 0f, .5f);
        myMethode = _myMethode;

    }


    private void Update()
    {
        Flip();   
    }

    public void UpdateHealthBar(int value)
    {
        life.fillAmount = (float)value / maxHealth;
    }

    void OnPathComplete(Path p)
    {
        if (!p.error)
        {
            path = p;
            currentWaiPoint = 0;
        }
    }

    private void FixedUpdate()
    {
        ReachPlayer(myMethode);
    }

    public void ReachPlayer(myAttack Attack)
    {

        if (targetPlayer != null)
        {
            dist = Vector2.Distance(transform.position, targetPlayer.transform.position);

            if (dist < attackRange && Time.time - lastAttackTime >= attackRate)
            {
                //L'attaque de notre enemy
                Debug.Log("J'attaque le joueur");
                Attack();
            }
            else if (dist > attackRange)
            {
                if (path == null)
                    return;

                if (currentWaiPoint >= path.vectorPath.Count)
                {
                    reachEndOfPath = true;
                    return;
                }
                else
                {
                    reachEndOfPath = false;
                }

                Vector2 direction = ((Vector2)path.vectorPath[currentWaiPoint] - rb.position).normalized;
                Vector2 force = direction * speed * Time.fixedDeltaTime;

                rb.velocity = force;




                float distance = Vector2.Distance(rb.position, path.vectorPath[currentWaiPoint]);

                if (distance < nextWayPointDistance) { currentWaiPoint++; }
            }
            else
            {
                rb.velocity = Vector2.zero;
            }

        }

        DetectPlayer();
    }

    private void DetectPlayer()
    {
        if (Time.time - lastPlayerDetectTime > playerDetectRate)
        {
            lastPlayerDetectTime = Time.time;


            foreach (PlayerController player in FindObjectsOfType<PlayerController>())
            {
                if (player != null)
                {
                    float dist = Vector2.Distance(transform.position, player.transform.position);

                    if (player == targetPlayer)
                    {
                        if (dist > chaseRange)
                        {
                            targetPlayer = null;
                            rb.velocity = Vector2.zero;
                            anim.SetBool("onMove", false);


                        }
                    }
                    else if (dist < chaseRange)
                    {
                        if (targetPlayer == null)
                            targetPlayer = player;
                        anim.SetBool("onMove", true);

                    }


                }
            }




        }
        // calculate distance between us and the player

    }

    void UpdatePath()
    {
        if (seeker.IsDone() && targetPlayer != null)
            seeker.StartPath(rb.position, targetPlayer.transform.position, OnPathComplete);

    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        Animator anim;
        anim = GetComponent<Animator>();
        anim.SetTrigger("Hit");
        UpdateHealthBar(currentHealth);
        healthBar.SetActive(true);
        if(currentHealth <= 0)
        {
            foreach (var item in QuestManager.instance.allQuest)
            {
                if(item.statut == QuestSO.Statut.accepter)
                {
                    if(gameObject.name == item.objectTofind)
                    {
                        item.actualAmount++;

                    }
                }
            }

            if(loot.Length > 0)
            {
                int x = Random.Range(0, 3);

                if(x == 0)
                {
                    int i = Random.Range(0, loot.Length);
                    GameObject obj1 = Instantiate(loot[Random.Range(0, loot.Length)], 
                        transform.position + new Vector3(Random.Range(0, 1), Random.Range(0,1),0), Quaternion.identity);
                    obj1.name = loot[i].name;
                }
                else if(x == 1)
                {
                    int i = Random.Range(0, loot.Length);
                    int y = Random.Range(0, loot.Length);
                    GameObject obj1 = Instantiate(loot[i], transform.position + new Vector3(Random.Range(0, 2), Random.Range(0, 2), 0), Quaternion.identity);
                    obj1.name = loot[i].name;

                    GameObject obj2 =Instantiate(loot[y], transform.position + new Vector3(Random.Range(0, 2), Random.Range(0, 2), 0), Quaternion.identity);
                    obj2.name = loot[y].name;

                }
            }
            Destroy(gameObject);
        }
    }

    void Flip()
    {
        if (rb.velocity.x > 0 && lookRight || rb.velocity.x < 0 && !lookRight)
        {
            lookRight = !lookRight;


            transform.Rotate(0, 180f, 0);
        }

    }

}

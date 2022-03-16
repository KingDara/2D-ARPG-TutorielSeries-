using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Component")]
    Rigidbody2D rb;
    Animator anim;


    [Header("Stat")]
    [SerializeField] float moveSpeed;
    public int currentHealth;
    public int maxHealth;
    public static int money;

    [Header("Attack")]
    private float attackTime;
    [SerializeField] float timeBetweenAttack;
    public bool canMove = true, canAttack =true;
    [SerializeField] Transform checkEnemy;
    public LayerMask whatIsEnemy;
    public float range;
    public int damage = 1;
    

    public static PlayerController instance;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(this);
        }
    }


    // Start is called before the first frame update
    void Start()
    {
        //canMove = true;
        currentHealth = maxHealth;
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if(canAttack)
        Attack();

        

    }

    private void Attack()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (Time.time >= attackTime)
            {
                rb.velocity = Vector2.zero;
                anim.SetTrigger("attack");

                StartCoroutine(Delay());
                IEnumerator Delay()
                {
                    canMove = false;
                    yield return new WaitForSeconds(.5f);
                    canMove = true;
                }

                attackTime = Time.time + timeBetweenAttack;
            }


        }
    }

  
    
    private void FixedUpdate()
    {
       if(canMove)
        Move();
    }

    void Move()
    {
        
        if (Input.GetAxis("Horizontal") > 0.1)
            {
                anim.SetFloat("lastInputX", 1);
                anim.SetFloat("lastInputY", 0);
        }
        else if(Input.GetAxis("Horizontal") < -0.1)
        {
            anim.SetFloat("lastInputX", -1);
            anim.SetFloat("lastInputY", 0);
        }

        if (Input.GetAxis("Vertical") > 0.1 )
        {
            anim.SetFloat("lastInputX", 0);
            anim.SetFloat("lastInputY", 1);
        }
        else if (Input.GetAxis("Vertical") < -0.1)
        {
            anim.SetFloat("lastInputX", 0);
            anim.SetFloat("lastInputY", -1);
        }

        if (Input.GetAxis("Horizontal") > 0.1)
        {
            checkEnemy.position = new Vector3(transform.position.x + range, transform.position.y,0);
        }
       else if(Input.GetAxis("Horizontal") < -0.1)
        {
            checkEnemy.position = new Vector3(transform.position.x - range, transform.position.y,0);
        }

        if (Input.GetAxis("Vertical") > 0.1)
        {
            checkEnemy.position = new Vector3(transform.position.x, transform.position.y +range,0);
        }
        else if (Input.GetAxis("Vertical") < -0.1)
        {
            checkEnemy.position = new Vector3(transform.position.x, transform.position.y -range,0);
        }

        float x = Input.GetAxis("Horizontal");
            float y = Input.GetAxis("Vertical");

            rb.velocity = new Vector2(x, y) * moveSpeed * Time.fixedDeltaTime;

            rb.velocity.Normalize();

            if (x != 0 || y != 0)
            {
                anim.SetFloat("inputX", x);
                anim.SetFloat("inputY", y);
            }
        


    }


    public void OnAttack()
    {
        Collider2D[] enemy = Physics2D.OverlapCircleAll(checkEnemy.position, 0.5f, whatIsEnemy);

        foreach (var enemy_ in enemy)
        {
            enemy_.GetComponent<EnemyFly>().TakeDamage(damage);
        }
    }

   public void TakeDamage(int damage)
    {
        currentHealth -= damage;
    }

}

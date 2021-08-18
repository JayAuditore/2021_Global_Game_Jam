using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnenyBase : MonoBehaviour, IDamageable, ITouchAttack
{
    private float timer1;//计时
    protected int faceTo;//朝右为正1

    public SpriteRenderer spriteRenderer;
    public Animator anim;
    public EnemyBaseState animState;
    public int state_num;
    [Header("  参数")]
    public float HP;
    public bool isDead;
    [Header(" 移动参数")]
    public float petrolSpeed;
    

    [Header(" 巡逻目标")]
    public Transform targetPoint;
    public Transform pointA, pointB;
    [Header("触碰伤害")]
    public float touchDamage;

    [Header("攻击")] 
    protected float attackCDLeft, skillCDLeft;//不需填
    public float attackCD, skillCD;
    public float attackRange, skillRange;
    public Transform attackTarget;

    [HideInInspector] public float attackDamage;
    [HideInInspector] public float skillDamage;

    ///---拥有的状态
    public EnemyBaseState IdleState;
    public EnemyBaseState PatrolState;
    public EnemyBaseState RunToAttackState; 
    public EnemyBaseState AttackState;
    public EnemyBaseState SkillState ;

    /// <summary>
    /// //不用管以下变量
    /// </summary>
    [Header("调试")]
    public bool isGround;
    public bool isStop;
    
    public void Awake()
    {
        Init(); //在子类中重写，方便获得在父类中获取物体的其他组件或值
      //  GameManager.GetInstance().AddEnemyList(this);
    }
    public virtual void Init() //初始化
    {

    ///---拥有的状态
      IdleState = new IdleState();
      PatrolState = new PatrolState();
      RunToAttackState = new RunToAttackState();
      AttackState = new AttackState();
      SkillState = new SkillState();

    }
    void Start()
    {
        //初始化
        spriteRenderer = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        //初始状态
        TransitionToState(IdleState);
        faceTo = 1;

       
       //spriteRenderer.color = new Color(255, 126, 126, 225);
        //GetDamage(1);
    }
    void FixedUpdate()
    {

    }


    void Update()
    {
        
        if (HP <= 0)
        {
            HP = 0;
            isDead = true;
           
        }
        anim.SetBool("isDead", isDead);
        if (isDead)
        {
            return;
        }
        AnyState();//无论处于哪个State都要进行的判断
        
                  
        anim.SetInteger("state", state_num); //改变动画状态机
        animState.UpdateState(this);

    }

   public virtual void AnyState()
    {

    }
    public virtual void MoveToTaget()
    {
       if(!attackTarget)
       {   transform.position =new Vector2(Mathf.MoveTowards(transform.position.x, targetPoint.position.x, petrolSpeed * Time.deltaTime), transform.position.y);}
       else
       {
           transform.position = new Vector2(Mathf.MoveTowards(transform.position.x, attackTarget.position.x, petrolSpeed * Time.deltaTime), transform.position.y);
        }
        FilpDirection();
    }

    public virtual void SpecialMove()
    {

    }

   
    public void FilpDirection()
    {
        if (!attackTarget)
        {
            if (transform.position.x > targetPoint.position.x)
            {
              
                transform.localRotation = transform.localRotation = Quaternion.Euler(0, 180, 0);
                faceTo = -1;
            }
            else if (transform.position.x <= targetPoint.position.x)
            {
                faceTo = 1;
                transform.localRotation = Quaternion.Euler(0, 0, 0);
            }
        }
        else
        {
            if (transform.position.x > attackTarget.position.x)
            {
               
                transform.localRotation = transform.localRotation = Quaternion.Euler(0, 180, 0);
                faceTo = -1;
            }
            else if (transform.position.x <= attackTarget.position.x)
            {
                faceTo = 1;
                transform.localRotation = Quaternion.Euler(0, 0, 0);
            }
        }

    }
    public void SwitchPoint() //选择下一个巡逻点（选择距离远的）
    {
        if (Mathf.Abs(pointA.position.x - transform.position.x) > Mathf.Abs(pointB.position.x - transform.position.x))
        {
            targetPoint = pointA;
        }
        else
        {
            targetPoint = pointB;
        }

    }

    //切换动画
    public void TransitionToState(EnemyBaseState state) //参数是EmenyBaseState类声明，
    {
        animState = state;
        animState.EnterState(this); 
    }


    
    public virtual void TouchAttack()
    {
        
        PlayerDamage.Instance.TakeDamage(touchDamage);
        //————————————使得玩家扣血touchDamage_______________________________________
    }
    public virtual void AttackAction()//普通攻击
    {
        if (Vector2.Distance(transform.position, attackTarget.position) < attackRange)
        {
            if (Time.time > attackCDLeft)
            {
               //TransitionToState(AttackState);
                attackCDLeft = Time.time + attackCD;
            }

        }
    }

    public virtual void SkillAttack()//技能攻击
    {
        if (Vector2.Distance(transform.position, attackTarget.position) <skillRange)
        {
            if (Time.time > skillCDLeft)
            {
                //TransitionToState(SkillState);
                skillCDLeft = Time.time + skillCD;

            }

        }
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))//——————————————————————待补充射线
        { attackTarget = collision.transform; }


    }

   

    /////////////________________秘籍________________________
    public void EnemyDie()//秒杀怪物
    {
        HP = 0;
    }

    public  void GetDamage(float damage)//让怪物掉血
    {
        HP -= damage;
        //StartCoroutine(imageChange());
        
    }

    IEnumerator imageChange()
    {
        float time = 90;//闪烁帧数
        float time2 = 20;//闪烁间隔
        while (time >= 0)
        {
            spriteRenderer.color = new Color(255, 126, 126,225);

            yield return 20;

            //spriteRenderer.color = new Color(255, 255, 255);
            //yield return 20;
            time -= time2 * 2;
        }



        
    }


}



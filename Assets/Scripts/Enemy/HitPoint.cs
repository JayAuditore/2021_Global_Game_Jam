using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitPoint : MonoBehaviour
{
    // Start is called before the first frame update
    Transform parentGameObject;
    void Start()
    {
        parentGameObject = transform.parent;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
   
    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player")&&gameObject.name== "touchHitPoint"&& !parentGameObject.GetComponent<EnenyBase>().anim.GetCurrentAnimatorStateInfo(0).IsName("attack")&& !parentGameObject.GetComponent<EnenyBase>().anim.GetCurrentAnimatorStateInfo(0).IsName("skill"))
           
        {
            parentGameObject.GetComponent<EnenyBase>().TouchAttack();
         
           Debug.Log("touch Attack");//在.TouchAttack();补充伤害玩家代码

        }

        else if (collision.CompareTag("Player") && gameObject.name == "attackHitPoint")
        {
            PlayerDamage.Instance.TakeDamage(parentGameObject.GetComponent<EnenyBase>().attackDamage);
            

            //在此补充代码让玩家扣血
        }
        else if (collision.CompareTag("Player") && gameObject.name == "skillHitPoint")
        {
            PlayerDamage.Instance.TakeDamage(parentGameObject.GetComponent<EnenyBase>().skillDamage);

           // Debug.Log("skill Attack");
            //在此补充代码让玩家扣血

        }

    }   

}

using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class EnemyScript : MonoBehaviour 
{

    public int maxHealth;
    protected int currentHealth;
    public int resources;
    public GameObject scrapPrefab;
    protected new Rigidbody rigidbody;

    [SerializeField]protected Image hpBar;


    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Bullet")
        {
            currentHealth -= collision.gameObject.GetComponent<BulletScript>().damage;
            if (currentHealth <= 0)
            {
                GameObject scrap = (GameObject)Instantiate(scrapPrefab, transform.position, Quaternion.identity);
                ResourcesScript rs = scrap.GetComponent<ResourcesScript>();
                rs.resources = resources;
                Destroy(gameObject);
            }
            hpBar.fillAmount = (float)currentHealth / (float)maxHealth;
        }
    }

    
}

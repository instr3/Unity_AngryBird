using UnityEngine;
using System.Collections;

public class Plank_Script : MonoBehaviour {
    public GameObject particlePrefab;
    public float maxDamage = 300.0f;
    private float damage = 0;
    private bool destroyFlag = false;
    IEnumerator DestroyMF()
    {
        GameObject particleMF = GameObject.Instantiate(particlePrefab, transform.position, transform.rotation) as GameObject;
        Destroy(GetComponent<Rigidbody2D>());
        yield return new WaitForSeconds(0.2f);
        Destroy(particleMF);
        Destroy(gameObject);

        
    }
    void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log(collision.relativeVelocity);
        damage += collision.relativeVelocity.sqrMagnitude;
        if(damage > maxDamage && !destroyFlag)
        {
            destroyFlag = true;
            Debug.Log(damage);
            StartCoroutine(DestroyMF());
        }
    }
	// Use this for initialization
	void Start () {
	    
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}

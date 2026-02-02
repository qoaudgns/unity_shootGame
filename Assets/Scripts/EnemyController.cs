using UnityEngine;

public class EnemyController : MonoBehaviour
{
    enum State
    {
        Spawning,
        Moving,
        Dying,
    }
    
    public float speed = 2;
    
    public Material flashMaterial;
    public Material defaultMaterial;
    
    GameObject target;

    private State state;

    public AudioClip hitSound;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
    
    }

    public void Spawn(GameObject target)
    {
        this.target = target;
        state = State.Spawning;

        GetComponent<Character>().Initialize();
        GetComponent<Animator>().SetTrigger("Spawn");
        Invoke("StartMoving", 1f);
        GetComponent<Collider2D>().enabled = false;
    }

    void StartMoving()
    {
        GetComponent<Collider2D>().enabled = true;
        state = State.Moving;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        if (state == State.Moving)
        {
            Vector2 direction = target.transform.position - transform.position;
        
            transform.Translate(direction.normalized * speed * Time.deltaTime);

            if (direction.x < 0)
            {
                GetComponent<SpriteRenderer>().flipX = true;
            }

            if (direction.x > 0)
            {
                GetComponent<SpriteRenderer>().flipX = false;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Bullet")
        {
            
            float d = other.gameObject.GetComponent<Bullet>().damage;

            if (GetComponent<Character>().Hit(d))
            {
                GetComponent<AudioSource>().PlayOneShot(hitSound);
                Flash();
            }
            else
            {
                GetComponent<AudioSource>().PlayOneShot(hitSound);
                Die();
            }
            
            
        }
    }

    void Flash()
    {
        GetComponent<SpriteRenderer>().material = flashMaterial;
        Invoke("AfterFlash", 0.5f);
    }

    void AfterFlash()
    {
        GetComponent<SpriteRenderer>().material = defaultMaterial;
    }

    void Die()
    {
        state = State.Dying;
        GetComponent<Animator>().SetTrigger("Die");
        Invoke("AfterDying", 1.4f);
    }

    void AfterDying()
    {
        gameObject.SetActive(false);
    }
    
    
}

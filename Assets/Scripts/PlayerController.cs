using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    public float speed = 8;
    public GameObject bulletPrefab;
    
    public Material flashMaterial;
    public Material defaultMaterial;

    public AudioClip shotSound;
    public AudioClip hitSound;
    
    private Vector3 move;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        move = Vector3.zero;
        
        if (Keyboard.current.leftArrowKey.isPressed || Keyboard.current.aKey.isPressed)
        {
            move += new Vector3(-1f, 0f, 0f);
        }
        
        if (Keyboard.current.rightArrowKey.isPressed || Keyboard.current.dKey.isPressed)
        {
            move += new Vector3(1f, 0f, 0f);
        }
        
        if (Keyboard.current.upArrowKey.isPressed || Keyboard.current.wKey.isPressed)
        {
            move += new Vector3(0f, 1f, 0f);
        }
        
        if (Keyboard.current.downArrowKey.isPressed || Keyboard.current.sKey.isPressed)
        {
            move += new Vector3(0f, -1f, 0f);
        }
        
        move = move.normalized;

        if (move.x < 0)
        {
            GetComponent<SpriteRenderer>().flipX = true;
        }

        if (move.x > 0)
        {
            GetComponent<SpriteRenderer>().flipX = false;
        }

        if (move.magnitude > 0) 
        {
            GetComponent<Animator>().SetTrigger("Move");
        }
        else
        {
            GetComponent<Animator>().SetTrigger("Stop");
        }

        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            Shoot();
        }
    }

    void Shoot()
    {
        GetComponent<AudioSource>().PlayOneShot(shotSound);
        
        Vector3 worldPosition = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        Debug.Log(worldPosition);
        worldPosition.z = 0f;
        worldPosition -= (transform.position - new Vector3(0, -0.5f, 0));
        
        GameObject newBullet = GetComponent<ObjectPool>().Get();
        if (newBullet != null)
        {
            newBullet.transform.position = transform.position + new Vector3(0, -0.5f);
            newBullet.GetComponent<Bullet>().Direction = worldPosition;
        }
    
    }

    private void FixedUpdate()
    {
        transform.Translate(move * speed * Time.fixedDeltaTime);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            if (GetComponent<Character>().Hit(1))
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
        GetComponent<Animator>().SetTrigger("Die");
        Invoke("AfterDying", 0.875f);
    }
    
    void AfterDying()
    {
        SceneManager.LoadScene("GameOverScene");
    }
}

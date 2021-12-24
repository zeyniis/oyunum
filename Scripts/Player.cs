using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    private Rigidbody2D myRigidbody;
    private Animator myAnimator;
    

    [SerializeField]
    private float movementspeed;
    
    private bool facingRight;

    private bool jump;

    [SerializeField]
    private Transform[] groundPoints;

    [SerializeField]
    private float groundRadius;

    private bool Spikes;

    [SerializeField]
    private LayerMask whatIsGround;

    [SerializeField]
    private float jumpforce;

    private bool isGrounded;

    [SerializeField]
    private AudioSource jumpSoundEffect;

    [SerializeField]
    private AudioSource dyingSoundEffect;
    void Start()
    {
        facingRight = true;
        myRigidbody = GetComponent<Rigidbody2D>();
        myAnimator = GetComponent<Animator>();
    }

    void Update(){
        if(Input.GetKeyDown(KeyCode.Space))
        {
            jump = true;
        }
    }
    
    void FixedUpdate()
    {
        float horizontal = Input.GetAxis("Horizontal");
        isGrounded = IsGrounded();
        
        Hareket(horizontal);
        Donus(horizontal);
        Ziplama(horizontal);
        HandleLayers();
        ResetValues();

    }


    private void Ziplama(float horizontal){

        if(myRigidbody.velocity.y < 0){
            myAnimator.SetBool("land",true);
        }
        if(isGrounded && jump) 
        {
            jumpSoundEffect.Play();
            isGrounded = false;
            myRigidbody.AddForce(new Vector2(0,jumpforce));
            myAnimator.SetTrigger("jump");

        }
    }

    private void Hareket(float horizontal){

        myRigidbody.velocity = new Vector2(horizontal*movementspeed,myRigidbody.velocity.y);
        myAnimator.SetFloat("speed",Mathf.Abs(horizontal));
    }

    private void Donus(float horizontal)
    {
        if(horizontal > 0 && !facingRight || horizontal < 0 && facingRight){
            facingRight = !facingRight;
            Vector3 theScale = transform.localScale;
            theScale.x *= -1;
            transform.localScale = theScale;
        }
    }
    
    private void ResetValues(){
        jump=false;
    }

     private bool IsGrounded(){
        if(myRigidbody.velocity.y <=0)
        {
            foreach (Transform point in groundPoints)
            {
                Collider2D[] colliders = Physics2D.OverlapCircleAll(point.position,groundRadius,whatIsGround);
                for(int i=0;i<colliders.Length;i++){
                    if(colliders[i].gameObject != gameObject){
                        myAnimator.ResetTrigger("jump");
                        myAnimator.SetBool("land",false);
                        return true;
                    }
                }
            }
        }
        return false;
    }

    
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("Trap"))
        {
         Olum();
        }
        if(collision.gameObject.CompareTag("underground"))
        {
         Olum();
        }
    }

    private void Olum()
    {
        myRigidbody.bodyType= RigidbodyType2D.Static;
        dyingSoundEffect.Play();
        myAnimator.SetTrigger("die");

    
    }

    private void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    private void HandleLayers()
    {
        if(!isGrounded){
            myAnimator.SetLayerWeight(1,1);
        }
        else{
            myAnimator.SetLayerWeight(1,0);
        }
    }


}


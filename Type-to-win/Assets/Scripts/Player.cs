using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public GameObject typingManager;
    public GameObject typingEventCube;
    public CharacterController characterController;
    public bool startMoving = false;
    public bool isCollide = false;
    private float speed = 6f;

    private Animator animator;
    public GameObject model;

    float smooth = 5.0f;

    public int maxhealth = 100;
    public int currentHealth;

    public HealthBar healthBar;
    // Start is called before the first frame update

    IEnumerator ExampleCoroutine()
    {

        //yield on a new YieldInstruction that waits for 5 seconds.
        yield return new WaitForSeconds(5);
        startMoving = true;

    }


    void Start()
    {
        model = transform.Find("model").gameObject;
        animator = model.GetComponent<Animator>();
        //setMovenmetnAnimation(true, false, false);
        /*animator.SetBool("isIdel", false);
        animator.SetBool("isWalking", true);
        animator.SetBool("isRunning", false);*/
        StartCoroutine(ExampleCoroutine());
        currentHealth = maxhealth;
    }


    // Update is called once per frame
    void Update()
    {
        if (startMoving)
        {
            if (isCollide == true)
            {
                animator.SetBool("isRunning", false);
            }
            else {
                animator.SetBool("isRunning", true);
                // Rotate the cube by converting the angles into a quaternion.
                Quaternion target = Quaternion.Euler(0, -70, 0);

                // Dampen towards the target rotation
                transform.rotation = Quaternion.Slerp(transform.rotation, target, Time.deltaTime * smooth);

                characterController.Move(transform.forward * Time.deltaTime * speed);
                //transform.Translate(Vector3.forward * Time.deltaTime * speed);
            }
        }
        


        /*if (isMoving && isCollide == false)
        {
            isMoving = false;
            Debug.Log("Stopped moving");
            typingManager.GetComponent<TypingManager>().SetTypingEvent(
                typingEventCube.GetComponent<TypingEvent>());
        }*/
    }

    public void takeDamage(int damage) {
        currentHealth -= damage;
        healthBar.setHealth(currentHealth);
    }
}

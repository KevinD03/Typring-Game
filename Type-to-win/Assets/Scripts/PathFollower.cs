using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathFollower : MonoBehaviour
{
    public GameObject path;
    public float movementSpeed;
    public float defaultMovementSpeed;
    public bool autoStartPathTraversal;
    public float autoStartPathDelaySeconds;

    //animator 
    public bool level_indicator;
    private Animator FPSChara_Gun_animator;
    private Animator FPSChara_Arm_animator;
    public GameObject FPSChara_Gun;
    public GameObject FPSChara_Arm;

    // ui health bar
    public int maxhealth = 100;
    public int currentHealth;
    public HealthBar healthBar;

    private PathNode[] nodes;
    private int nodeIndex;
    public bool traversing;
    private float pathStepTimer;
    private float pathStepTotalTime;
    private Vector3 startPosition;
    private Vector3 endPosition;


    // Start is called before the first frame update
    void Start()
    {
        if (level_indicator) {
            FPSChara_Gun_animator = FPSChara_Gun.GetComponent<Animator>();
            FPSChara_Arm_animator = FPSChara_Arm.GetComponent<Animator>();
        }
        


        if (autoStartPathTraversal) {
            StartCoroutine(AutoStartCoroutine());
        }
    }

    IEnumerator AutoStartCoroutine()
    {
        yield return new WaitForSeconds(autoStartPathDelaySeconds);
        SetPath(path);
    }

    // Update is called once per frame
    void Update()
    {
        if (traversing)
        {
            if (transform.position == endPosition)
            {
                // Activate node's on arrival callback
                nodes[nodeIndex].OnArrival();

                nodeIndex++;

                if (nodeIndex < nodes.Length)
                {
                    changePathStep(nodes[nodeIndex]);
                }
                else
                {
                    // End of path reached
                    traversing = false;
                    //animator.SetBool("isRunning", false);

                    return;
                }
            }

            pathStepTimer += Time.deltaTime;
            transform.position =
                Vector3.Lerp(startPosition, endPosition, pathStepTimer / pathStepTotalTime);
        }
    }

    public void SetPath(GameObject path)
    {
        this.path = path;
        nodes =  path.GetComponentsInChildren<PathNode>();
        nodeIndex = 0;
        traversing = true;

        changePathStep(nodes[nodeIndex]);
        //animator.SetBool("isRunning", true);
    }

    public void SetTraversing(bool traversing)
    {
        this.traversing = traversing;
        if (traversing) {
            if (level_indicator)
            {
                FPSChara_Gun_animator.SetBool("isMoveGun", true);
                FPSChara_Arm_animator.SetBool("isMoveChara", true);
            }
        } else {
            if (level_indicator)
            {
                FPSChara_Gun_animator.SetBool("isMoveGun", false);
                FPSChara_Arm_animator.SetBool("isMoveChara", false);
            }
        }
    }

    public void ResetMovementSpeed()
    {
        movementSpeed = defaultMovementSpeed;
        changePathStep(nodes[nodeIndex]);
    }


    private void changePathStep(PathNode nextNode)
    {
        pathStepTimer = 0.0f;
        startPosition = transform.position;
        endPosition = nextNode.GetTransform().position;

        float distance = Vector3.Distance(startPosition, endPosition);
        pathStepTotalTime = distance / movementSpeed;
    }
    
    // ui health bar, player takes damage
    public void takeDamage(int damage)
    {
        currentHealth -= damage;
        healthBar.setHealth(currentHealth);
    }
}


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Used to make sure that the canvas under the TypingManager is always
// turned towards the main camera.
public class ObjectTracker : MonoBehaviour
{
	public GameObject target;
    public bool autoStartTracking;
    public bool rotateYOnly;
    public float targetTransitionSpeed = 1.0f;


    private enum State { Disabled, Tracking, TransIn, TransOut };
    private State currentState = State.Disabled;

	private Quaternion originalRotation;
    private Quaternion transitionTargetRotation;



    void Start()
    {
        originalRotation = transform.rotation;

        if (autoStartTracking) {
            SetTracking(true);
        }
    }

    void Update()
    {
        if (currentState == State.Tracking) {
            if (rotateYOnly) {
                transform.LookAt(
                    new Vector3(
                        target.transform.position.x,
                        transform.position.y,
                        target.transform.position.z
                    )
                );
            } else {
                transform.LookAt(target.transform);
            }
        }

        if (currentState == State.TransIn) {
            transitionToState(State.Tracking);
        }

        if (currentState == State.TransOut) {
            transitionToState(State.Disabled);
        }
    }

    public void SetTarget(GameObject target)
    {
        this.target = target;
    }

    public void SetTracking(bool tracking)
    {
        if (tracking) {
            currentState = State.TransIn;

            Vector3 targetPosition = new Vector3(
                target.transform.position.x,
                transform.position.y,
                target.transform.position.z
            );

            Vector3 targetDirection;

            if (rotateYOnly) {
                targetDirection =
                    targetPosition - transform.position;
            } else {
                targetDirection =
                    target.transform.position - transform.position;
            }

            transitionTargetRotation =
                Quaternion.LookRotation(targetDirection);
        } else {
            if (currentState != State.Disabled) {
                currentState = State.TransOut;
                transitionTargetRotation = originalRotation;
            }
        }
    }


    private void transitionToState(State nextState)
    {
        // Follow future target through transition
        if (nextState == State.Tracking) {
            Vector3 targetDirection =
                target.transform.position - transform.position;

            transitionTargetRotation =
                Quaternion.LookRotation(targetDirection);
        }

        transform.rotation = Quaternion.Slerp(
            transform.rotation,
            transitionTargetRotation,
            Time.deltaTime * targetTransitionSpeed 
        );

        if (transform.rotation == transitionTargetRotation) {
            currentState = nextState;
        }
    }
}


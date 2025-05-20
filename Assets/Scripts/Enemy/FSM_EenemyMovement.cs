using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FSM_EnemyMovement : MonoBehaviour
{
    public Transform[] waypoints;
    public float speed = 3f;
    public float waitTime = 2f;
    public float arrivalThreshold = 0.2f;

    private int currentIndex = 0;
    private float waitTimer = 0f;

    public enum State { Idle, Move, Wait }
    public State currentState = State.Idle;

    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        if (waypoints.Length > 0)
        {
            currentState = State.Move;
        }
    }

    void Update()
    {
        switch (currentState)
        {
            case State.Idle:
                HandleIdle();
                break;
            case State.Move:
                HandleMove();
                break;
            case State.Wait:
                HandleWait();
                break;
        }
    }

    void HandleIdle()
    {
        waitTimer += Time.deltaTime;
        if (waitTimer >= waitTime)
        {
            waitTimer = 0f;
            currentState = State.Move;
        }
    }

    void HandleMove()
    {
        if (waypoints.Length == 0) return;

        Vector3 target = waypoints[currentIndex].position;
        Vector3 direction = (target - transform.position).normalized;

        rb.MovePosition(transform.position + direction * speed * Time.deltaTime);

        if (Vector3.Distance(transform.position, target) <= arrivalThreshold)
        {
            currentState = State.Wait;
        }
    }

    void HandleWait()
    {
        waitTimer += Time.deltaTime;
        if (waitTimer >= waitTime)
        {
            waitTimer = 0f;
            currentIndex = (currentIndex + 1) % waypoints.Length;
            currentState = State.Move;
        }
    }
}

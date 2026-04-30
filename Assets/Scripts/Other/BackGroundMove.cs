using UnityEngine;
using System.Collections;

public class BackGroundMove : MonoBehaviour
{
    public float speed = 1f;
    public float delay = 3f;
    public float targetY = 0f;

    private bool canMove = false;
    private bool finished = false;

    void Start()
    {
        StartCoroutine(StartMove());
    }

    IEnumerator StartMove()
    {
        yield return new WaitForSeconds(delay);
        canMove = true;
    }

    void Update()
    {
        if (canMove)
        {
            transform.position += Vector3.up * speed * Time.deltaTime;

            if (transform.position.y >= targetY)
            {
                transform.position = new Vector3(
                    transform.position.x,
                    targetY,
                    transform.position.z
                );

                canMove = false;
                finished = true; 
            }
        }
    }


    public bool IsFinished()
    {
        return finished;
    }
}
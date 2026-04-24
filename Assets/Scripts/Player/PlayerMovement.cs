using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 5f;

    private Animator ani;
    private Vector2 direction;

    void Start()
    {
        ani = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        direction.x = Input.GetAxisRaw("Horizontal");
        direction.y = Input.GetAxisRaw("Vertical");

        Move();
        UpdateAnimation();
    }

    void Move()
    {
        transform.Translate(direction.normalized * speed * Time.deltaTime);
    }

    void UpdateAnimation()
    {
        if (direction != Vector2.zero) {
            ani.SetFloat("x", direction.x);
            ani.SetFloat("y", direction.y);
        }
        
        ani.SetBool("isRun", (direction != Vector2.zero));
    }
}

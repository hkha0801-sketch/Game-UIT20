// using UnityEngine;

// public class PlayerMovement : MonoBehaviour
// {
//     public float speed = 5f;
//     public Vector2 direction;

//     private Animator ani;

//     void Start()
//     {
//         ani = GetComponent<Animator>();
//     }

//     // Update is called once per frame
//     void Update()
//     {
//         direction.x = Input.GetAxisRaw("Horizontal");
//         direction.y = Input.GetAxisRaw("Vertical");

//         Move();
//         UpdateAnimation();
//     }

//     void Move()
//     {
//         transform.Translate(direction.normalized * speed * Time.deltaTime);
//     }

//     void UpdateAnimation()
//     {
//         if (direction != Vector2.zero) {
//             ani.SetFloat("x", direction.x);
//             ani.SetFloat("y", direction.y);
//         }
        
//         ani.SetBool("isRun", (direction != Vector2.zero));
//     }
// }


using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 5f;
    public Vector2 direction;
    private Animator ani;

    void Start()
    {
        ani = GetComponent<Animator>();
    }

    void Update()
    {
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveY = Input.GetAxisRaw("Vertical");

        // Cập nhật hướng nhìn cuối cùng khi có input
        if (moveX != 0 || moveY != 0)
        {
            direction.x = moveX;
            direction.y = moveY;
        }

        Move();
        UpdateAnimation(moveX != 0 || moveY != 0);
    }

    void Move()
    {
        Vector2 input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        transform.Translate(input.normalized * speed * Time.deltaTime);
    }

    void UpdateAnimation(bool isRunning)
    {
        if (direction != Vector2.zero) {
            ani.SetFloat("x", direction.x);
            ani.SetFloat("y", direction.y);
        }
        ani.SetBool("isRun", isRunning);
    }

    // Khôi phục hướng nhìn từ MinigameManager
    public void SetFacingDirection(Vector2 dir)
    {
        direction = dir;
        if (ani == null) ani = GetComponent<Animator>();
        ani.SetFloat("x", dir.x);
        ani.SetFloat("y", dir.y);
        ani.SetBool("isRun", false);
    }
}
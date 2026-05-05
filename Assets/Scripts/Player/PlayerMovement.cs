using UnityEngine;
using DialogueEditor;

public class PlayerMovement : MonoBehaviour
{
    public float speedX = 5f;
    public float speedY = 5f;
    [HideInInspector] public float speedMultiplier = 1f;
    public Vector2 direction;
    private Animator ani;

    void Start()
    {
        ani = GetComponent<Animator>();
    }

    void Update()
    {
        if (ConversationManager.Instance != null && ConversationManager.Instance.IsConversationActive)
        {
            direction = Vector2.zero; 
            UpdateAnimation(false); 
            return;
        }

        float moveX = Input.GetAxisRaw("Horizontal");
        float moveY = Input.GetAxisRaw("Vertical");

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
          float currentScale = Mathf.Abs(transform.localScale.y);
        
        Vector2 inputDir = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")).normalized;

        float velocityX = inputDir.x * speedX;
        float velocityY = inputDir.y * speedY;

        Vector3 movement = new Vector3(velocityX, velocityY, 0);

        transform.Translate(movement * currentScale * speedMultiplier * Time.deltaTime);
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
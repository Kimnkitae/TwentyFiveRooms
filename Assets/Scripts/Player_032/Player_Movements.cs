using UnityEngine;

public class Player_Movements : MonoBehaviour
{
    public Sprite Idle_player_032;
    public Sprite Right_player_032;
    public Sprite Left_player_032;
    public Sprite Behind_player_032;
    private SpriteRenderer spriteRenderer;

    void Start()
    {
        
        spriteRenderer = GetComponent<SpriteRenderer>();

        if (Idle_player_032 != null)
        {
            spriteRenderer.sprite = Idle_player_032;
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.W))
        {
            if (Behind_player_032 != null)
            {
                spriteRenderer.sprite = Behind_player_032;
            }
        }

        else if (Input.GetKeyUp(KeyCode.W))
        {
            if (Idle_player_032 != null)
            {
                spriteRenderer.sprite = Idle_player_032;
            }
        }

        if (Input.GetKeyDown(KeyCode.A))
        {
            if (Left_player_032 != null)
            {
                spriteRenderer.sprite = Left_player_032;
            }
        }

        else if (Input.GetKeyUp(KeyCode.A))
        {
            if (Idle_player_032 != null)
            {
                spriteRenderer.sprite = Idle_player_032;
            }
        }

        if (Input.GetKeyDown(KeyCode.S))
        {
            if (Idle_player_032 != null)
            {
                spriteRenderer.sprite = Idle_player_032;
            }
        }

        else if (Input.GetKeyUp(KeyCode.S))
        {
            if (Idle_player_032 != null)
            {
                spriteRenderer.sprite = Idle_player_032;
            }
        }

        if (Input.GetKeyDown(KeyCode.D))
        {
            if (Right_player_032 != null)
            {
                spriteRenderer.sprite = Right_player_032;
            }
        }

        else if (Input.GetKeyUp(KeyCode.D))
        {
            if (Idle_player_032 != null)
            {
                spriteRenderer.sprite = Idle_player_032;
            }
        }
        


        
    }
}



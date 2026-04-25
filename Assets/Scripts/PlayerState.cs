using UnityEngine;

public enum State
{
    Good,
    Normal,
    Bad
}

public class PlayerState
{
    public State state = State.Good;
    public float regenStamina = 1f;
    public float stamina = 100f;
    public float maxStamina = 100f;
    public bool isGround = true;
    public bool isRunning = false;
    public bool isWalking = false;

    public void Stamina()
    {
        if (isWalking)
        {
            stamina -= 1 * Time.deltaTime;
        }
        else if (isRunning)
        {
            stamina -= 3 * Time.deltaTime;
        }
        else if (!isGround)
        {
            stamina -= 10 * Time.deltaTime;
        }
        else if (stamina < maxStamina && stamina >= 0)
        {
            stamina += regenStamina * Time.deltaTime;
        }
    }

    public void ChangeState()
    {
        if (stamina >= 60)
        {
            state = State.Good;
        }
        else if (stamina >= 30)
        {
            state = State.Normal;
        }
        else
        {
            state = State.Bad;
        }
    }
}

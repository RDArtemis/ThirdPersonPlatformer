using UnityEngine;
using UnityEngine.Events;

public class InputManager : MonoBehaviour
{
    public UnityEvent<Vector2> OnMove = new UnityEvent<Vector2>(); //move around
    public UnityEvent OnJump = new UnityEvent(); //jump
    public UnityEvent OnDash = new UnityEvent(); //dash 

    
    void Update()
    {
        //move direction controls
        Vector2 inputVector = Vector2.zero;
        if (Input.GetKey(KeyCode.W))
        {
            inputVector += Vector2.up;  //forward
        }
        if (Input.GetKey(KeyCode.S))
        {
            inputVector += Vector2.down;  //backward
        }
        if (Input.GetKey(KeyCode.A))
        {
            inputVector += Vector2.left;  //left
        }
        if (Input.GetKey(KeyCode.D))
        {
            inputVector += Vector2.right;  //right
        }


        OnMove?.Invoke(inputVector);

        
        if (Input.GetKey(KeyCode.Space))
        {
            OnJump?.Invoke(); //jump time!
        }

    }
}

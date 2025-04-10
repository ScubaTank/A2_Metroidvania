using UnityEngine;

public class PlayerStateMachine : MonoBehaviour
{
    public enum PlayerState
    {
        Idle,
        Walking,
        JumpUp,
        JumpMid,
        JumpDown
    }

    private PlayerState _currentState;

    [SerializeField] private Animator PlayerAnimator;
    [SerializeField] private PlayerController playerController;
    
    void Start()
    {
        // at the start of the game, the player is in the idle state 
        ChangeState(PlayerState.Idle);
    }

    
    void Update()
    {
        UpdateState(_currentState);
    }

    private void EnterState(PlayerState targetState)
    {
        switch (targetState)
        {
            case PlayerState.Idle:
                PlayerAnimator.Play("Idle");
                break;
            case PlayerState.Walking:
                PlayerAnimator.Play("Walk");
                break;
            case PlayerState.JumpUp:
                PlayerAnimator.Play("JumpRise");
                break;
            case PlayerState.JumpDown:
                PlayerAnimator.Play("JumpFall");
                break;
            case PlayerState.JumpMid:
                PlayerAnimator.Play("JumpMid");
                break;
            default:
                
                break;
        }
        
    }

    private void ExitState(PlayerState exitingState)
    {
        
    }

    private void UpdateState(PlayerState currentState)
    {
        switch (currentState)
        {
            case PlayerState.Idle:
                
                // if the player velocity is > 0 then change state to walking
                if (playerController.isWalking)
                {
                    ChangeState(PlayerState.Walking);
                }

                if (playerController.yVelocity > 0)
                {
                    ChangeState(PlayerState.JumpUp);
                }
                
                if (playerController.yVelocity < 0)
                {
                    ChangeState(PlayerState.JumpDown);
                }

                if (playerController.yVelocity == 0 && !playerController._isGround)
                {
                    ChangeState(PlayerState.JumpMid);
                }
                
                break;
            case PlayerState.Walking:
                
                // if the player velocity is 0 then change state to idle
                if (!playerController.isWalking)
                {
                    ChangeState(PlayerState.Idle);
                }
                
                if (playerController.yVelocity > 0)
                {
                    ChangeState(PlayerState.JumpUp);
                }
                
                if (playerController.yVelocity < 0)
                {
                    ChangeState(PlayerState.JumpDown);
                }

                if (playerController.yVelocity == 0 && !playerController._isGround)
                {
                    ChangeState(PlayerState.JumpMid);
                }
                
                break;
            case PlayerState.JumpUp:
                
                if (playerController.yVelocity < 0)
                {
                    ChangeState(PlayerState.JumpDown);
                }

                if (playerController.yVelocity == 0 && !playerController._isGround)
                {
                    ChangeState(PlayerState.JumpMid);
                }

                if (playerController._isGround)
                {
                    ChangeState(playerController.isWalking ? PlayerState.Walking : PlayerState.Idle);
                }
                
                break;
            case PlayerState.JumpDown:

                if (playerController._isGround)
                {
                    ChangeState(playerController.isWalking ? PlayerState.Walking : PlayerState.Idle);
                }
                break;
            case PlayerState.JumpMid:
                
                if (playerController._isGround)
                {
                    ChangeState(playerController.isWalking ? PlayerState.Walking : PlayerState.Idle);
                }
                
                if (playerController.yVelocity < 0)
                {
                    ChangeState(PlayerState.JumpDown);
                }
                
                break;
            default:
                
                break;
        }
    }

    public void ChangeState(PlayerState targetState)
    {
        // exit the current state
        ExitState(_currentState);
        
        // set the current state
        _currentState = targetState;
        
        // enter the new state
        EnterState(_currentState);
    }
    
}

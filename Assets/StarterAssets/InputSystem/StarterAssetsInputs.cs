using UnityEngine;
#if ENABLE_INPUT_SYSTEM
using Unity.VisualScripting;
using UnityEngine.InputSystem;
#endif

namespace StarterAssets
{
	public class StarterAssetsInputs : MonoBehaviour
	{
		[Header("Character Input Values")]
		public Vector2 move;
		public Vector2 look;
		public bool jump;
		public bool sprint;
		
		[Header("Character Interaction")]
		public bool interact;
		public bool cancel;
		public bool submit;
		public bool paused;

		[Header("Movement Settings")]
		public bool analogMovement;

		[Header("Mouse Cursor Settings")]
		public bool cursorLocked = true;
		public bool cursorInputForLook = true;

#if ENABLE_INPUT_SYSTEM
		public void OnMove(InputValue value)
		{
			MoveInput(value.Get<Vector2>());
		}

		public void OnLook(InputValue value)
		{
			if(cursorInputForLook)
			{
				LookInput(value.Get<Vector2>());
			}
		}

		public void OnJump(InputValue value)
		{
			JumpInput(value.isPressed);
		}

		public void OnSprint(InputValue value)
		{
			SprintInput(value.isPressed);
		}

		public void OnInteract(InputValue value)
		{
			InteractInput(value.isPressed);
		}

		public void Oncancel(InputValue value)
		{
			CancelInput(value.isPressed);
		}
		public void OnSubmit(InputValue value)
		{
			SubmitInput(value.isPressed);
		}

		public void OnPause(InputValue value)
		{
			PauseInput(value.isPressed);
		}


#endif


		public void MoveInput(Vector2 newMoveDirection)
		{
			move = newMoveDirection;
		} 

		public void LookInput(Vector2 newLookDirection)
		{
			look = newLookDirection;
		}

		public void JumpInput(bool newJumpState)
		{
			jump = newJumpState;
		}

		public void SprintInput(bool newSprintState)
		{
			sprint = newSprintState;
		}
		private void InteractInput(bool newInteractState)
		{
			interact = newInteractState;
		}
		
		public void CancelInput(bool newCancelState)
		{
			cancel = newCancelState;
		}
		
		public void SubmitInput(bool newSubmitState)
		{
			submit = newSubmitState;		
		}
		
		/*private void OnApplicationFocus(bool hasFocus)
		{
			SetCursorState(cursorLocked);
		}*/

		private void SetCursorState(bool newState)
		{
			Cursor.lockState = newState ? CursorLockMode.Locked : CursorLockMode.None;
		}
		
		private void PauseInput(bool newPauseState)
		{
			paused = newPauseState;
		}
	}
	
}

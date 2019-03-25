using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCrouch : MonoBehaviour {

    private bool isCrouching = false;

    private InputController playerInput;
    private Animator animator;

	void Start () {
        playerInput = GameManager.Instance.InputController;
        animator = GetComponentInChildren<Animator>();
	}
	
	void Update () {
        if (playerInput.Crouch > 0 && !isCrouching) {
            isCrouching = true;
            animator.SetBool("Crouch_b", isCrouching);
        } else if (playerInput.Crouch == 0f && isCrouching) {
            isCrouching = false;
            animator.SetBool("Crouch_b", isCrouching);
        }
	}
}

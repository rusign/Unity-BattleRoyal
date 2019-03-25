using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

[RequireComponent(typeof(MoveController))]
public class PlayerController : NetworkBehaviour {

    [System.Serializable]
    public struct MouseInput {
        public Vector2 Damping;
        public Vector2 Sensitivity;
    }

    [SerializeField] private float speed;
    [SerializeField] private float jumpForce;
    [SerializeField] private MouseInput mouseControl;
    [SerializeField] private Animator animator;
    [SerializeField] private GameObject postProcess;

    private Rigidbody rig;

    private MoveController _moveController;
    public MoveController MoveController {
        get {
            if (_moveController == null) {
                _moveController = GetComponent<MoveController>();
            }
            return _moveController;
        }
    }

    private InputController playerInput;
    private Vector2 mouseInput;

	void Start () {
        if (!isLocalPlayer)
        {
            enabled = false;
            return;
        }
        GetComponent<NetworkAnimator>().SetParameterAutoSend(0, true);
        rig = GetComponent<Rigidbody>();
        animator = GetComponentInChildren<Animator>();
        playerInput = GameManager.Instance.InputController;
        GameManager.Instance.LocalPlayer = this;

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
	}
	
	void Update () {
        if (Input.GetKeyDown(KeyCode.LeftShift))
            speed *= 1.5f;
        if (Input.GetKeyUp(KeyCode.LeftShift))
            speed /= 1.5f;

        if (Input.GetKeyDown(KeyCode.Escape)) {
            if (Cursor.visible)
            {
                Cursor.visible = false;
                Cursor.lockState = CursorLockMode.Locked;
            }
            else
            {
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;
            }
        }

        bool grounded = Grounded();

        //animator.SetBool("Grounded", grounded);
        //animator.SetBool("Jump_b", !grounded);

        if (Input.GetKeyDown(KeyCode.Space) && grounded)
            rig.AddForce(transform.up * jumpForce);

        animator.SetFloat("Speed_f", playerInput.Vertical * speed);
        
        Vector2 direction = new Vector2(playerInput.Vertical * speed, playerInput.Horizontal * speed);
        MoveController.Move(direction);

        if (isLocalPlayer)
            if (GameObject.FindWithTag("PartyManager").GetComponent<PartyManager>().state == PartyManager.StateFSM.End)
                GameObject.Find("Canvas").transform.GetChild(2).gameObject.SetActive(true);
	}

    bool Grounded()
    {
        return Physics.Raycast(transform.position, Vector3.down, 0.1f);
    }

    public void ActivePostProcess()
    {
        postProcess.SetActive(true);
    }

    public void DisablePostProcess()
    {
        postProcess.SetActive(false);
    }
}

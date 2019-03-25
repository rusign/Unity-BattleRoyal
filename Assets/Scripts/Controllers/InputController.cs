using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputController : MonoBehaviour {

    public float Vertical;
    public float Horizontal;
    public float Jump;
    public float Crouch;
    public float Sprint;
    public Vector2 MouseInput;
    public bool Fire1;
    public bool Fire2;
    public bool Reload;
    public bool Num1;
    public bool Num2;
    public bool Num3;
    public bool Num4;
    public bool Num5;
    public bool Grenade;

	void Update () {
        Vertical = Input.GetAxis("Vertical");
        Horizontal = Input.GetAxis("Horizontal");
        Jump = Input.GetAxis("Jump");
        //Crouch = Input.GetAxis("Crouch");
        //Sprint = Input.GetAxis("Sprint");
        MouseInput = new Vector2(Input.GetAxisRaw("Mouse X"), Input.GetAxisRaw("Mouse Y"));
        Fire1 = Input.GetButton("Fire1");
        Fire2 = Input.GetButton("Fire2");
        Reload = Input.GetKeyDown(KeyCode.R);
        Num1 = Input.GetKeyDown(KeyCode.Ampersand);
        Num2 = Input.GetKeyDown(KeyCode.Alpha2);
        Num3 = Input.GetKeyDown(KeyCode.DoubleQuote);
        Num4 = Input.GetKeyDown(KeyCode.Quote);
        Num5 = Input.GetKeyDown(KeyCode.Alpha5);
        Grenade = Input.GetKeyDown(KeyCode.G);

        //foreach (KeyCode kcode in System.Enum.GetValues(typeof(KeyCode)))
        //{
        //    if (Input.GetKeyDown(kcode))
        //        Debug.Log("KeyCode down: " + kcode);
        //}
	}
}

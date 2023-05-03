using UnityEngine;
using Unity.Netcode;

public class CharacterMovement : NetworkBehaviour
{
    //public Animator characterAnimator;
    public Transform characterTr;
    public Rigidbody characterBody;
    //public Joystick joystick;

    public float moveSpeed;
    public float rotateSpeed;

    private Vector3 _InputVector;

    private void FixedUpdate()
    {
        CharacterMove();
        CharacterRotate();
    }

    private void CharacterMove()
    {
        //if (SystemInfo.deviceType == DeviceType.Handheld)
        //{
            //_InputVector.x = joystick.Horizontal;
            //_InputVector.z = joystick.Vertical;
        //}
        //else
        //{
            _InputVector.x = Input.GetAxis("Horizontal");
            _InputVector.z = Input.GetAxis("Vertical");
        //}

        characterBody.velocity = _InputVector * moveSpeed;
        //characterAnimator.SetBool("IsRun", _InputVector != Vector3.zero);
    }

    private void CharacterRotate()
    {
        if (_InputVector == Vector3.zero) return;

        characterTr.rotation = Quaternion.Lerp(characterTr.rotation,
            Quaternion.LookRotation(new Vector3(_InputVector.x, 0, _InputVector.z)), rotateSpeed);
    }
}
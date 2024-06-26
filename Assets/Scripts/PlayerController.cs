using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices.ComTypes;
using UnityEngine;
using UnityEngine.Scripting.APIUpdating;

public class PlayerController : MonoBehaviour
{
    //���ǵ�, ���� ���� ����
    [SerializeField]
    private float walkSpeed;
    [SerializeField]
    private float runSpeed;
    private float applySpeed;   //���� ����Ǵ� ���ǵ�.

    [SerializeField]
    private float jumpForce;

    private bool isRun = false;    //�޸��� ����
    private bool isGround = true;  //���� ��Ҵ��� ����
    
    //�ɱ� ���� ����
    [SerializeField]
    private float crouchSpeed;
    private float crouchPosY;   //���� ���� ���� ����
    private float originPosY;   //�ɱ� �� ���� ���� ����
    private float applyCrouchPosY;  //���� ���� ���� ����
    private bool isCrouch = false; //�ɱ� ����


    //ī�޶� ���� ����
    [SerializeField]
    private float lookSensitivity;                  //�ΰ���
    [SerializeField]
    private float cameraRotationLimit;              //ī�޶� ȸ�� ���� ����.
    private float currentCameraRotationX = 0.0f;    //���� ī�޶� x�� ����.
    [SerializeField]
    private Camera theCamera;  //ī�޶� ������Ʈ

    //�� �� ������Ʈ
    private Rigidbody myRigid;
    private CapsuleCollider capsuleCollider;


    void Start()
    {
        capsuleCollider = GetComponent<CapsuleCollider>();
        myRigid = GetComponent<Rigidbody>();
        applySpeed = walkSpeed;
        originPosY = theCamera.transform.localPosition.y;    //ĳ���͸� ���������� ���� ��������. ��� ī�޶� ������. ���� �׳� position�ϸ� world������ �� �ʹ� �������Ƿ� localPosition�������.
        applyCrouchPosY = originPosY;
    }

    void Update()
    {
        IsGround();
        TryJump();
        TryRun();
        TryCrouch();
        Move();
        CameraRotation();       //ī�޶� ����ȸ��
        CharacterRotation();    //ĳ����,ī�޶� �¿� ȸ��.
    }

    //���� ���� �Լ���
    private void IsGround()
    {
        isGround = Physics.Raycast(transform.position, Vector3.down, capsuleCollider.bounds.extents.y + 0.1f);
    }

    private void TryJump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && isGround)
        {
            Jump();
        }
    }

    private void Jump()
    {
        if (isCrouch) Crouch();                         //���� ���¼� ���� �� �������� ����
        myRigid.velocity = transform.up * jumpForce;    //���������� �ӵ��� �ٲ�. �̰ɷ� ���� ����
    }

    //�޸��� ���� �Լ���
    private void TryRun()
    {
        if (Input.GetKey(KeyCode.LeftShift))
        {
            Running();
        }
        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            RunningCancle();
        }
    }

    private void Running()
    {
        if (isCrouch) Crouch();  //���� ���¿��� �޸��� �� ���� ���� ����

        isRun = true;
        applySpeed = runSpeed;
    }

    private void RunningCancle()
    {
        isRun = false;
        applySpeed = walkSpeed;
    }

    //�ɱ� ���� �Լ�.
    private void TryCrouch()
    {
        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            Crouch();
        }
    }

    private void Crouch()
    {
        isCrouch = !isCrouch;   //���� ���� ��ȯ

        if (isCrouch)
        {
            applySpeed = crouchSpeed;
            applyCrouchPosY = crouchPosY;
        }
        else
        {
            applySpeed = walkSpeed;
            applyCrouchPosY = originPosY;
        }

        StartCoroutine(CrouchCoroutine());
    }

    IEnumerator CrouchCoroutine()
    {
        float posY = theCamera.transform.localPosition.y;
        int frameCount = 0;

        while(posY != applyCrouchPosY)
        {
            frameCount++;
            posY = Mathf.Lerp(posY, applyCrouchPosY, 0.3f);
            theCamera.transform.localPosition = new Vector3(0, posY, 0);
            if(frameCount > 15) break;      //�ִ� 15�����ӱ��� �ݺ���
            yield return null;              //1 ������ ���
        }

        theCamera.transform.localPosition = new Vector3(0, applyCrouchPosY, 0);
    }

    //�̵� �Լ�
    private void Move()
    {
        float moveDirX = Input.GetAxisRaw("Horizontal");    //�¿��̵�
        float moveDirZ = Input.GetAxisRaw("Vertical");      //�����̵�

        Vector3 moveHorizontal = transform.right * moveDirX;
        Vector3 moveVertical = transform.forward * moveDirZ;

        Vector3 velocity = (moveHorizontal + moveVertical).normalized * applySpeed;

        myRigid.MovePosition(transform.position + velocity * Time.deltaTime);
    }

    private void CameraRotation()   //ī�޶� ���� ȸ��
    {
        float xRotation = Input.GetAxisRaw("Mouse Y");
        float cameraRotationX = xRotation * lookSensitivity;
        currentCameraRotationX -= cameraRotationX;

        currentCameraRotationX
            = Mathf.Clamp(currentCameraRotationX, -cameraRotationLimit, cameraRotationLimit);    //ī�޶� ȸ������

        theCamera.transform.localEulerAngles = new Vector3(currentCameraRotationX, 0.0f, 0.0f);
    }

    private void CharacterRotation()    //ĳ���� �¿� ȸ��
    {
        float yRotation = Input.GetAxisRaw("Mouse X");
        Vector3 characterRotationY = new Vector3(0.0f, yRotation, 0.0f) * lookSensitivity;

        myRigid.MoveRotation(myRigid.rotation * Quaternion.Euler(characterRotationY));
    }
}

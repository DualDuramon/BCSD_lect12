using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices.ComTypes;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.Scripting.APIUpdating;

public class PlayerController : MonoBehaviour
{
    //���ǵ�, ���� ���� ����
    [SerializeField]
    private float walkSpeed;
    [SerializeField]
    private float runSpeed;
    [SerializeField]
    private float applySpeed;   //���� ����Ǵ� ���ǵ�.

    [SerializeField]
    private float jumpForce;

    private bool isWalk = false;    //�ȱ� ����
    private bool isRun = false;    //�޸��� ����
    private bool isGround = true;  //���� ��Ҵ��� ����

    //������ üũ ����
    private Vector3 lastPos;    //�� �������� �÷��̾� ��ġ

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
    private GunController theGunController;
    private CrossHair theCrossHair;
    private StatusController theStatusController;

    void Start()
    {
        capsuleCollider = GetComponent<CapsuleCollider>();
        myRigid = GetComponent<Rigidbody>();
        theGunController = FindObjectOfType<GunController>();
        theCrossHair = FindObjectOfType<CrossHair>();
        theStatusController = FindObjectOfType<StatusController>();

        applySpeed = walkSpeed;
        originPosY = theCamera.transform.localPosition.y;    //ĳ���͸� ���������� ���� ��������. ��� ī�޶� ������. ���� �׳� position�ϸ� world������ �� �ʹ� �������Ƿ� localPosition�������.
        applyCrouchPosY = originPosY;
    }

    private void FixedUpdate()
    {
        MoveCheck();    //������ ������Ʈ�������� lastPos�� �ſ� ������ ȣ���.
    }
    void Update()
    {
        IsGround();
        TryJump();
        TryRun();
        TryCrouch();
        Move();
        if (!Inventory.inventoryActivated)
        {
            CameraRotation();       //ī�޶� ����ȸ��
            CharacterRotation();    //ĳ����,ī�޶� �¿� ȸ��.
        }
    }

    //���� ���� �Լ���
    private void IsGround()
    {
        isGround = Physics.Raycast(transform.position, Vector3.down, capsuleCollider.bounds.extents.y + 0.1f);
        theCrossHair.JumpingAnimation(!isGround);   //���� ���� ������ ũ�ν���� �޸��⶧�� ���� ������
    }

    private void TryJump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && isGround && theStatusController.GetCurrentSP() > 0)
        {
            Jump();
        }
    }

    private void Jump()
    {
        if (isCrouch) Crouch();                         //���� ���¼� ���� �� �������� ����
        theStatusController.DecreaseStamina(100);
        myRigid.velocity = transform.up * jumpForce;    //���������� �ӵ��� �ٲ�. �̰ɷ� ���� ����

    }

    //�޸��� ���� �Լ���
    private void TryRun()
    {
        if (Input.GetKey(KeyCode.LeftShift) && theStatusController.GetCurrentSP() > 0)
        {
            Running();
        }
        if (Input.GetKeyUp(KeyCode.LeftShift) || theStatusController.GetCurrentSP() <= 0)
        {
            RunningCancle();
        }
    }

    private void Running()
    {
        if (isCrouch) Crouch();  //���� ���¿��� �޸��� �� ���� ���� ����

        theGunController.CancleFineSight(); //�޸��� �� ������ ����

        isRun = true;
        theCrossHair.RunningAnimation(isRun);
        theStatusController.DecreaseStamina(10);
        applySpeed = runSpeed;
    }

    private void RunningCancle()
    {
        isRun = false;
        theCrossHair.RunningAnimation(isRun);
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
        theCrossHair.CrouchingAnimation(isCrouch);

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

    //�̵� ���� �Լ�
    private void Move()
    {
        float moveDirX = Input.GetAxisRaw("Horizontal");    //�¿��̵�
        float moveDirZ = Input.GetAxisRaw("Vertical");      //�����̵�

        Vector3 moveHorizontal = transform.right * moveDirX;
        Vector3 moveVertical = transform.forward * moveDirZ;

        Vector3 velocity = (moveHorizontal + moveVertical).normalized * applySpeed;

        myRigid.MovePosition(transform.position + velocity * Time.deltaTime);
    }

    private void MoveCheck()
    {
        if (!isRun && !isCrouch && isGround)
        {
            if (Vector3.Distance(lastPos, transform.position) >= 0.01f)
            {
                isWalk = true;
            }
            else
            {
                isWalk = false;
            }
            theCrossHair.WalkingAnimation(isWalk);
            lastPos = transform.position;
        }
    }

    //ī�޶� ���� �Լ�
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

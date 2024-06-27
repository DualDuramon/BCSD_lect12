using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices.ComTypes;
using UnityEngine;
using UnityEngine.Scripting.APIUpdating;

public class PlayerController : MonoBehaviour
{
    //스피드, 점프 관련 변수
    [SerializeField]
    private float walkSpeed;
    [SerializeField]
    private float runSpeed;
    private float applySpeed;   //현재 적용되는 스피드.

    [SerializeField]
    private float jumpForce;

    private bool isRun = false;    //달리기 여부
    private bool isGround = true;  //땅에 닿았는지 여부
    
    //앉기 관련 변수
    [SerializeField]
    private float crouchSpeed;
    private float crouchPosY;   //앉을 때의 높이 변수
    private float originPosY;   //앉기 전 원래 높이 변수
    private float applyCrouchPosY;  //현재 앉은 높이 변수
    private bool isCrouch = false; //앉기 여부


    //카메라 관련 변수
    [SerializeField]
    private float lookSensitivity;                  //민감도
    [SerializeField]
    private float cameraRotationLimit;              //카메라 회전 제한 각도.
    private float currentCameraRotationX = 0.0f;    //현제 카메라 x축 각도.
    [SerializeField]
    private Camera theCamera;  //카메라 컴포넌트

    //그 외 컴포넌트
    private Rigidbody myRigid;
    private CapsuleCollider capsuleCollider;


    void Start()
    {
        capsuleCollider = GetComponent<CapsuleCollider>();
        myRigid = GetComponent<Rigidbody>();
        applySpeed = walkSpeed;
        originPosY = theCamera.transform.localPosition.y;    //캐릭터를 내려버리면 땅에 박혀버림. 고로 카메라 내리기. 또한 그냥 position하면 world기준이 라 너무 내려가므로 localPosition해줘야함.
        applyCrouchPosY = originPosY;
    }

    void Update()
    {
        IsGround();
        TryJump();
        TryRun();
        TryCrouch();
        Move();
        CameraRotation();       //카메라 상하회전
        CharacterRotation();    //캐릭터,카메라 좌우 회전.
    }

    //점프 관련 함수들
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
        if (isCrouch) Crouch();                         //앉은 상태서 점프 시 앉은상태 해제
        myRigid.velocity = transform.up * jumpForce;    //순간적으로 속도를 바꿈. 이걸로 점프 구현
    }

    //달리기 관련 함수들
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
        if (isCrouch) Crouch();  //앉은 상태에서 달리기 시 앉은 상태 해제

        isRun = true;
        applySpeed = runSpeed;
    }

    private void RunningCancle()
    {
        isRun = false;
        applySpeed = walkSpeed;
    }

    //앉기 관련 함수.
    private void TryCrouch()
    {
        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            Crouch();
        }
    }

    private void Crouch()
    {
        isCrouch = !isCrouch;   //앉은 상태 전환

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
            if(frameCount > 15) break;      //최대 15프레임까지 반복함
            yield return null;              //1 프레임 대기
        }

        theCamera.transform.localPosition = new Vector3(0, applyCrouchPosY, 0);
    }

    //이동 함수
    private void Move()
    {
        float moveDirX = Input.GetAxisRaw("Horizontal");    //좌우이동
        float moveDirZ = Input.GetAxisRaw("Vertical");      //전후이동

        Vector3 moveHorizontal = transform.right * moveDirX;
        Vector3 moveVertical = transform.forward * moveDirZ;

        Vector3 velocity = (moveHorizontal + moveVertical).normalized * applySpeed;

        myRigid.MovePosition(transform.position + velocity * Time.deltaTime);
    }

    private void CameraRotation()   //카메라 상하 회전
    {
        float xRotation = Input.GetAxisRaw("Mouse Y");
        float cameraRotationX = xRotation * lookSensitivity;
        currentCameraRotationX -= cameraRotationX;

        currentCameraRotationX
            = Mathf.Clamp(currentCameraRotationX, -cameraRotationLimit, cameraRotationLimit);    //카메라 회전제한

        theCamera.transform.localEulerAngles = new Vector3(currentCameraRotationX, 0.0f, 0.0f);
    }

    private void CharacterRotation()    //캐릭터 좌우 회전
    {
        float yRotation = Input.GetAxisRaw("Mouse X");
        Vector3 characterRotationY = new Vector3(0.0f, yRotation, 0.0f) * lookSensitivity;

        myRigid.MoveRotation(myRigid.rotation * Quaternion.Euler(characterRotationY));
    }
}

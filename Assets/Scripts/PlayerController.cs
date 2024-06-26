using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Scripting.APIUpdating;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private float walkSpeed;

    [SerializeField]
    private float lookSensitivity;
    [SerializeField]
    private float cameraRotationLimit;              //카메라 회전 제한 각도.
    private float currentCameraRotationX = 0.0f;    //현제 카메라 x축 각도.
    [SerializeField]
    private Camera theCamera;  //카메라 컴포넌트

    private Rigidbody myRigid;


    // Start is called before the first frame update
    void Start()
    {
        myRigid = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        Move();
        CameraRotation();       //카메라 상하회전
        CharacterRotation();    //캐릭터,카메라 좌우 회전.
    }

    private void Move()
    {
        float moveDirX = Input.GetAxisRaw("Horizontal");    //좌우이동
        float moveDirZ = Input.GetAxisRaw("Vertical");      //전후이동

        Vector3 moveHorizontal = transform.right * moveDirX;
        Vector3 moveVertical = transform.forward * moveDirZ;

        Vector3 velocity = (moveHorizontal + moveVertical).normalized * walkSpeed;

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

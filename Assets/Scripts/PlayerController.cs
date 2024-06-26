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
    private float cameraRotationLimit;              //ī�޶� ȸ�� ���� ����.
    private float currentCameraRotationX = 0.0f;    //���� ī�޶� x�� ����.
    [SerializeField]
    private Camera theCamera;  //ī�޶� ������Ʈ

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
        CameraRotation();       //ī�޶� ����ȸ��
        CharacterRotation();    //ĳ����,ī�޶� �¿� ȸ��.
    }

    private void Move()
    {
        float moveDirX = Input.GetAxisRaw("Horizontal");    //�¿��̵�
        float moveDirZ = Input.GetAxisRaw("Vertical");      //�����̵�

        Vector3 moveHorizontal = transform.right * moveDirX;
        Vector3 moveVertical = transform.forward * moveDirZ;

        Vector3 velocity = (moveHorizontal + moveVertical).normalized * walkSpeed;

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

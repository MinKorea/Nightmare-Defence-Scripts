using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMoveController : MonoBehaviour
{
    Rigidbody rb;
    Animator anit;
    [SerializeField]
    float speed = 5;
    [SerializeField]
    float maxSpeed = 10;
    Vector3 movement;

    int floorMask;
    int speedLv;
    Text speedUI;



    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        anit = GetComponent<Animator>();
        floorMask = LayerMask.GetMask("Floor");
        speedUI = GameObject.Find("SpdUI").GetComponentInChildren<Text>();
    }

    private void FixedUpdate()
    {

        if (!GameManager.gm.START || GameManager.gm.END) return;

        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");

        Move(h, v);
        Turning();
        Animation(h, v);

    }

    void Move(float h, float v)
    {
        movement.Set(h, 0, v);

        movement = movement.normalized * speed * Time.deltaTime;

        rb.MovePosition(transform.position + movement);

    }

    void Turning()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);        // ī�޶󿡼� ���� �Ű������� ��ġ�� ���.

        RaycastHit hit; // ������ ���� ��ü�� ��Ƶ� ����

        if(Physics.Raycast(ray, out hit, Mathf.Infinity,floorMask))         // ������ mask�� �ش��ϴ� ��ü�� ������ true �ƴϸ� false
                                                                            // Physics.Raycast(����, ���� ��ü, ������ �Ÿ�, ����ũ)      
        {
            Vector3 dir = hit.point - transform.position;                   // �ٶ� ������ ���Ѵ�.

            dir.y = 0;

            Quaternion rot = Quaternion.LookRotation(dir);                  // ȸ������ ������ ����

            rb.MoveRotation(rot);
        }
    }

    void Animation(float h, float v)
    {
        bool isMove = (h != 0f || v != 0f);

        anit.SetBool("isMove", isMove);
    }

    public void GetSpeedUpItem()
    {
        if (speedLv < 6) speedLv++;
        else return;

        speed = DataManager.dm.GetDataSpeed(speedLv - 1);
        speedLv++;
        speedUI.text = "Lv." + speedLv.ToString();
        if (speed >= maxSpeed)
        { 
            speed = maxSpeed;
            speedUI.text = "Lv.MAX";
        }
    }
}

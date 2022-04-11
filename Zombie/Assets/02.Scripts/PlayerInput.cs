using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    public string moveAxisName = "Vertical"; // �յ� �������� ���� �Է���
    public string rotateAxisName = "Horizontal"; // �¿� ȸ���� ���� �Է���
    public string fireButtonName = "Fire1"; // �߻縦 ���� �Է� ��ư
    public string reloadButtonName = "Reload"; // �������� ���� �Է� ��ư

    // property : �� �Ҵ��� ���ο����� �����ϰ�
    public float move { get; private set; } // ������ ��
    public float rotate { get; private set; } // ȸ�� ��
    public bool fire { get; private set; } // �߻� �Է� ��
    public bool reload { get; private set; } // ������ �Է� ��

    private void Update()
    {
        // ���ӿ��� ���¿����� �Է°��� X
        if (GameManager.instance != null && GameManager.instance.isGameover)
        {
            move = 0;
            rotate = 0;
            fire = false;
            reload = false;
            return;
        }

        // �� ���¿� ���� �Է� ����
        move = Input.GetAxis(moveAxisName); 
        rotate = Input.GetAxis(rotateAxisName);
        fire = Input.GetButton(fireButtonName);
        reload = Input.GetButtonDown(reloadButtonName);
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    public string moveAxisName = "Vertical"; // 앞뒤 움직임을 위한 입력축
    public string rotateAxisName = "Horizontal"; // 좌우 회전을 위한 입력축
    public string fireButtonName = "Fire1"; // 발사를 위한 입력 버튼
    public string reloadButtonName = "Reload"; // 재장전을 위한 입력 버튼

    // property : 값 할당은 내부에서만 가능하게
    public float move { get; private set; } // 움직임 값
    public float rotate { get; private set; } // 회전 값
    public bool fire { get; private set; } // 발사 입력 값
    public bool reload { get; private set; } // 재장전 입력 값

    private void Update()
    {
        // 게임오버 상태에서는 입력감지 X
        if (GameManager.instance != null && GameManager.instance.isGameover)
        {
            move = 0;
            rotate = 0;
            fire = false;
            reload = false;
            return;
        }

        // 각 상태에 따른 입력 감지
        move = Input.GetAxis(moveAxisName); 
        rotate = Input.GetAxis(rotateAxisName);
        fire = Input.GetButton(fireButtonName);
        reload = Input.GetButton(reloadButtonName);
    }
}

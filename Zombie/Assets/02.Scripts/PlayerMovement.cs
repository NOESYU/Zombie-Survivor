using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f; // 앞 뒤 움직임 속도
    public float rotateSpeed = 180f; // 좌 우 회전 속도

    private PlayerInput playerInput; // 플레이어 입력을 알려주는 컴포넌트

    [SerializeField] private Rigidbody playerRb;
    [SerializeField]private Animator playerAni;

    private void Start()
    {
        playerInput = GetComponent<PlayerInput>();
        playerRb = GetComponent<Rigidbody>();
        playerAni = GetComponent<Animator>();
    }

    // FixedUpdate() : 물리 갱신 주기에 맞춰서 실행됨
    private void FixedUpdate()
    {
        // 물리정보갱신 주기마다 실행됨 (0.02초)

        Rotate();
        Move();

        // 입력값에 따라 애니메이터 Move 파라미터 값 변경
        playerAni.SetFloat("Move", playerInput.move);
    }

    private void Move()
    {
        // 상대적으로 이동할 거리 계산
        Vector3 moveDistance = playerInput.move * transform.forward * moveSpeed * Time.deltaTime;
        // rb를 이용하여 게임 오브젝트 위치 변경
        playerRb.MovePosition(playerRb.position + moveDistance);
    }

    private void Rotate()
    {
        // 상대적으로 회전할 수치 계산
        float turn = playerInput.rotate * rotateSpeed * Time.deltaTime;
        // rb를 이용하여 게임 오브젝트 회전 변경
        playerRb.rotation = playerRb.rotation * Quaternion.Euler(0, turn, 0f);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f; // 앞 뒤 움직임 속도
    public float rotateSpeed = 180f; // 좌 우 회전 속도

    private PlayerInput playerInput; // 플레이어 입력을 알려주는 컴포넌트

    [SerializeField] private Rigidbody playerRb;
    private Animator playerAni;

    private void Start()
    {
        playerInput = GetComponent<PlayerInput>();
        playerRb = GetComponent<Rigidbody>();
        playerAni = GetComponent<Animator>();
    }

    // FixedUpdate() : 물리 갱신 주기에 맞춰서 실행됨
    private void FixedUpdate()
    {
           
    }

    private void Move()
    {

    }

    private void Rotate()
    {

    }
}

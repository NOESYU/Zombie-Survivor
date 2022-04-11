using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f; // �� �� ������ �ӵ�
    public float rotateSpeed = 180f; // �� �� ȸ�� �ӵ�

    private PlayerInput playerInput; // �÷��̾� �Է��� �˷��ִ� ������Ʈ

    [SerializeField] private Rigidbody playerRb;
    private Animator playerAni;

    private void Start()
    {
        playerInput = GetComponent<PlayerInput>();
        playerRb = GetComponent<Rigidbody>();
        playerAni = GetComponent<Animator>();
    }

    // FixedUpdate() : ���� ���� �ֱ⿡ ���缭 �����
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

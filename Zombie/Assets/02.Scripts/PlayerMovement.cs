using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f; // �� �� ������ �ӵ�
    public float rotateSpeed = 180f; // �� �� ȸ�� �ӵ�

    private PlayerInput playerInput; // �÷��̾� �Է��� �˷��ִ� ������Ʈ

    [SerializeField] private Rigidbody playerRb;
    [SerializeField]private Animator playerAni;

    private void Start()
    {
        playerInput = GetComponent<PlayerInput>();
        playerRb = GetComponent<Rigidbody>();
        playerAni = GetComponent<Animator>();
    }

    // FixedUpdate() : ���� ���� �ֱ⿡ ���缭 �����
    private void FixedUpdate()
    {
        // ������������ �ֱ⸶�� ����� (0.02��)

        Rotate();
        Move();

        // �Է°��� ���� �ִϸ����� Move �Ķ���� �� ����
        playerAni.SetFloat("Move", playerInput.move);
    }

    private void Move()
    {
        // ��������� �̵��� �Ÿ� ���
        Vector3 moveDistance = playerInput.move * transform.forward * moveSpeed * Time.deltaTime;
        // rb�� �̿��Ͽ� ���� ������Ʈ ��ġ ����
        playerRb.MovePosition(playerRb.position + moveDistance);
    }

    private void Rotate()
    {
        // ��������� ȸ���� ��ġ ���
        float turn = playerInput.rotate * rotateSpeed * Time.deltaTime;
        // rb�� �̿��Ͽ� ���� ������Ʈ ȸ�� ����
        playerRb.rotation = playerRb.rotation * Quaternion.Euler(0, turn, 0f);
    }
}

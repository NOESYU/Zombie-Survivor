using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    // �� ���� ǥ���ϴµ� ����� Ÿ�� ����
    public enum State
    {
        Ready,
        Empty,
        Reloadiing
    }

    public State state { get; private set; }

    public Transform fireTransform; // ź���� �߻�� ��ġ

    // �ѱ� ȭ��, ź�� ���� ȿ�� ��ƼŬ �ý���
    public ParticleSystem muzzleFlashEffect;
    public ParticleSystem shellEjectEffect;

    private LineRenderer bulletLineRenderer;  // ź�� ������ �׸������� ������

    private AudioSource gunAudioPlayer;

    public GunData gunData; // ���� ���� ������

    private float fireDistance = 50f; // �����Ÿ�

    public int ammoRemain = 100; // ���� ��ü ź��
    public int magAmmo; // ���� źâ�� ���� �ִ� ź��

    private float lastFireTime; //���� ���������� �߻��� ����

    private void Awake()
    {
        // ����� ������Ʈ �ҷ�����
    }

    private void OnEnable()
    {
        // �� ���� �ʱ�ȭ
    }

    public void Fire()
    {
        // �߻� �õ�
    }

    private void Shot()
    {
        // ���� �߻� ó��
    }

    // �߻� ����Ʈ�� �Ҹ��� ����ϰ� ź�� ������ �׸�
    private IEnumerator ShotEffect(Vector3 hitPosition)
    {
        bulletLineRenderer.enabled = true; // ���η����� Ȱ��ȭ�Ͽ� ź�� ���� �׸�
        yield return new WaitForSeconds(0.03f);

        bulletLineRenderer.enabled = false; // ź�� ���� ��Ȱ��ȭ
    }

    public bool Reload()
    {
        return false;
    }


    // ���� ������ ó���� ����
    private IEnumerator ReloadRoutine()
    {
        state = State.Reloadiing; // ���� ���� reloading���� ����

        yield return new WaitForSeconds(gunData.reloadTime);

        state = State.Ready; // ���� ���� ���� ready�� ����
    } 
}


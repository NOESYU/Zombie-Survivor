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
        bulletLineRenderer = GetComponent<LineRenderer>();
        gunAudioPlayer = GetComponent<AudioSource>();

        // ���η��������� ����� �� ����(position size) ����
        bulletLineRenderer.positionCount = 2;
        // ���η����� ��Ȱ��ȭ
        bulletLineRenderer.enabled = false;
    }

    private void OnEnable()
    {
        // ��ü ���� ź�� ���� �ʱ�ȭ - ��ũ���ͺ� ������Ʈ
        ammoRemain = gunData.startAmmoRemain;
        // ���� źâ�� ���� ä���
        magAmmo = gunData.magCapacity;

        // ���� ������¸� ���� �� �غ� �� ���·� ����
        state = State.Ready;
        // ���������� �� �� ���� �ʱ�ȭ
        lastFireTime = 0;
    }

    public void Fire()
    {
        
    }

    private void Shot()
    {
        // ���� �߻� ó��
    }

    // �߻� ����Ʈ�� �Ҹ��� ����ϰ� ź�� ������ �׸�
    private IEnumerator ShotEffect(Vector3 hitPosition)
    {
        // �ѱ� ȭ��, ź�� ���� ȿ�� ���
        muzzleFlashEffect.Play();
        shellEjectEffect.Play();

        // �Ѱ� �Ҹ� ���
        gunAudioPlayer.PlayOneShot(gunData.shotClip);

        // �� ������-�ѱ� ��ġ, �� ����-�浹��ġ
        bulletLineRenderer.SetPosition(0, fireTransform.position);
        bulletLineRenderer.SetPosition(1, hitPosition);
        // ���η������� Ȱ��ȭ �Ͽ� ź�� �����׸�
        bulletLineRenderer.enabled = true;

        // ���ó�� - �ڷ�ƾ
        yield return new WaitForSeconds(0.03f);

        // ���η����� ��Ȱ��ȭ
        bulletLineRenderer.enabled = false;
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


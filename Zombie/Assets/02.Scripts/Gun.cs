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
        Reloading
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
        // ������°� �߻� ������ ����
        // && ������ �߻� �������� timeBetFire �̻��� �ð��� ������
        if (state == State.Ready && Time.time >= lastFireTime + gunData.timeBetFire)
        {
            // ������ �߻� ���� ����
            lastFireTime = Time.time;
            // ���� �߻� ó�� ����
            Shot();
        }
    }

    private void Shot()
    {
        // ����ĳ��Ʈ�� ���� �浹 ���� ����
        RaycastHit hit;
        // ź���� ���� ���� ����
        Vector3 hitPosition = Vector3.zero;

        // ����ĳ��Ʈ - ��������, ����, �浹 ���� �����̳�, �����Ÿ�
        // ���̿� ������ �浹�� ���
        if(Physics.Raycast(fireTransform.position, fireTransform.forward, out hit, fireDistance))
        {
            // �浹�� �������κ��� IDamageable ������Ʈ ��������
            IDamageable target = hit.collider.GetComponent<IDamageable>();

            if (target != null)
            {
                // IDamageable�� �����Դٸ� ������ OnDamage �Լ��� ������� ���濡�� ������ �ֱ�
                target.OnDamage(gunData.damage, hit.point, hit.normal);
            }

            // ���̰� �浹�� ��ġ ����
            hitPosition = hit.point;
        }   
        // �浹���� ���� ���
        else
        {
            // ź���� �ִ� �����Ÿ����� ���ư��� ���� ��ġ�� �浹 ��ġ�� ���
            hitPosition = fireTransform.position + fireTransform.forward * fireDistance;
        }

        // �߻� ����Ʈ ��� ����
        StartCoroutine(ShotEffect(hitPosition));

        magAmmo--; // ź�� �� -1
        if (magAmmo <= 0)
        {
            state = State.Empty; // ź�� ���� ������ ���� ���� empty�� ����
        }
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
        if (state == State.Reloading || ammoRemain <= 0 || magAmmo >= gunData.magCapacity)
        {
            // �̹� ������(Reloading) �����̰ų� ���� ź���� ���ų�
            // źâ�� ź���� �̹� ������ ��� false ��ȯ
            return false;
        }

        // ������ ó�� ����
        StartCoroutine(ReloadRoutine());
        return true;
    }


    // ���� ������ ó���� ����
    private IEnumerator ReloadRoutine()
    {
        state = State.Reloading; // ���� ���� reloading���� ����
        gunAudioPlayer.PlayOneShot(gunData.reloadClip); // ������ �Ҹ� ���

        yield return new WaitForSeconds(gunData.reloadTime);

        // źâ�� ä�� ź�� ���
        int ammoToFill = gunData.magCapacity - magAmmo;

        // źâ�� ä������ ź���� ���� ź�˺��� ������
        // ä���� �� ź�� ���� ���� ź�� ���� ���缭 ����
        if (ammoRemain < ammoToFill)
        {
            ammoToFill = ammoRemain;
        }

        // źâ�� ä��
        magAmmo += ammoToFill;
        // ���� ź�˿��� źâ�� ä�ŭ ź���� ��
        ammoRemain -= ammoToFill;

        state = State.Ready; // ���� ���� ���� ready�� ����
    } 
}


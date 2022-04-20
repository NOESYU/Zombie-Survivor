using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    // 총 상태 표현하는데 사용할 타입 선언
    public enum State
    {
        Ready,
        Empty,
        Reloading
    }

    public State state { get; private set; }

    public Transform fireTransform; // 탄알이 발사될 위치

    // 총구 화염, 탄피 배출 효과 파티클 시스템
    public ParticleSystem muzzleFlashEffect;
    public ParticleSystem shellEjectEffect;

    private LineRenderer bulletLineRenderer;  // 탄알 궤적을 그리기위한 렌더러

    private AudioSource gunAudioPlayer;

    public GunData gunData; // 총의 현재 데이터

    private float fireDistance = 50f; // 사정거리

    public int ammoRemain = 100; // 남은 전체 탄알
    public int magAmmo; // 현재 탄창에 남아 있는 탄알

    private float lastFireTime; //총을 마지막으로 발사한 시점

    private void Awake()
    {
        // 사용할 컴포넌트 불러오기
        bulletLineRenderer = GetComponent<LineRenderer>();
        gunAudioPlayer = GetComponent<AudioSource>();

        // 라인렌더러에서 사용할 점 개수(position size) 설정
        bulletLineRenderer.positionCount = 2;
        // 라인렌더러 비활성화
        bulletLineRenderer.enabled = false;
    }

    private void OnEnable()
    {
        // 전체 예비 탄알 양을 초기화 - 스크립터블 오브젝트
        ammoRemain = gunData.startAmmoRemain;
        // 현재 탄창을 가득 채우기
        magAmmo = gunData.magCapacity;

        // 총의 현재상태를 총을 쏠 준비가 된 상태로 변경
        state = State.Ready;
        // 마지막으로 총 쏜 시점 초기화
        lastFireTime = 0;
    }

    public void Fire()
    {
        // 현재상태가 발사 가능한 상태
        // && 마지막 발사 시점에서 timeBetFire 이상의 시간이 지났음
        if (state == State.Ready && Time.time >= lastFireTime + gunData.timeBetFire)
        {
            // 마지막 발사 시점 갱신
            lastFireTime = Time.time;
            // 실제 발사 처리 실행
            Shot();
        }
    }

    private void Shot()
    {
        // 레이캐스트에 의한 충돌 정보 저장
        RaycastHit hit;
        // 탄알이 맞은 곳을 저장
        Vector3 hitPosition = Vector3.zero;

        // 레이캐스트 - 시작지점, 방향, 충돌 정보 컨테이너, 사정거리
        // 레이와 상대방이 충돌한 경우
        if(Physics.Raycast(fireTransform.position, fireTransform.forward, out hit, fireDistance))
        {
            // 충돌한 상대방으로부터 IDamageable 오브젝트 가져오기
            IDamageable target = hit.collider.GetComponent<IDamageable>();

            if (target != null)
            {
                // IDamageable을 가져왔다면 상대방의 OnDamage 함수를 실행시켜 상대방에게 데미지 주기
                target.OnDamage(gunData.damage, hit.point, hit.normal);
            }

            // 레이가 충돌한 위치 저장
            hitPosition = hit.point;
        }   
        // 충돌하지 않은 경우
        else
        {
            // 탄알이 최대 사정거리까지 날아갔을 때의 위치를 충돌 위치로 사용
            hitPosition = fireTransform.position + fireTransform.forward * fireDistance;
        }

        // 발사 이펙트 재생 시작
        StartCoroutine(ShotEffect(hitPosition));

        magAmmo--; // 탄알 수 -1
        if (magAmmo <= 0)
        {
            state = State.Empty; // 탄알 수가 없으면 총의 상태 empty로 갱신
        }
    }

    // 발사 이펙트와 소리를 재생하고 탄알 궤적을 그림
    private IEnumerator ShotEffect(Vector3 hitPosition)
    {
        // 총구 화염, 탄피 배출 효과 재생
        muzzleFlashEffect.Play();
        shellEjectEffect.Play();

        // 총격 소리 재생
        gunAudioPlayer.PlayOneShot(gunData.shotClip);

        // 선 시작점-총구 위치, 선 끝점-충돌위치
        bulletLineRenderer.SetPosition(0, fireTransform.position);
        bulletLineRenderer.SetPosition(1, hitPosition);
        // 라인렌더러를 활성화 하여 탄알 궤적그림
        bulletLineRenderer.enabled = true;

        // 대기처리 - 코루틴
        yield return new WaitForSeconds(0.03f);

        // 라인렌더러 비활성화
        bulletLineRenderer.enabled = false;
    }

    public bool Reload()
    {
        if (state == State.Reloading || ammoRemain <= 0 || magAmmo >= gunData.magCapacity)
        {
            // 이미 재장전(Reloading) 상태이거나 남은 탄알이 없거나
            // 탄창에 탄알이 이미 가득찬 경우 false 반환
            return false;
        }

        // 재장전 처리 시작
        StartCoroutine(ReloadRoutine());
        return true;
    }


    // 실제 재장전 처리를 실행
    private IEnumerator ReloadRoutine()
    {
        state = State.Reloading; // 현재 상태 reloading으로 변경
        gunAudioPlayer.PlayOneShot(gunData.reloadClip); // 재장전 소리 재생

        yield return new WaitForSeconds(gunData.reloadTime);

        // 탄창에 채울 탄알 계산
        int ammoToFill = gunData.magCapacity - magAmmo;

        // 탄창에 채워야할 탄알이 남은 탄알보다 많으면
        // 채워야 할 탄알 수를 남은 탄알 수에 맞춰서 줄임
        if (ammoRemain < ammoToFill)
        {
            ammoToFill = ammoRemain;
        }

        // 탄창을 채움
        magAmmo += ammoToFill;
        // 남은 탄알에서 탄창에 채운만큼 탄알을 뺌
        ammoRemain -= ammoToFill;

        state = State.Ready; // 총의 현재 상태 ready로 변경
    } 
}


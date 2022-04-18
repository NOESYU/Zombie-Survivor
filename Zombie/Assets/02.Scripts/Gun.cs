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
        Reloadiing
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
        
    }

    private void Shot()
    {
        // 실제 발사 처리
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
        return false;
    }


    // 실제 재장전 처리를 실행
    private IEnumerator ReloadRoutine()
    {
        state = State.Reloadiing; // 현재 상태 reloading으로 변경

        yield return new WaitForSeconds(gunData.reloadTime);

        state = State.Ready; // 총의 현재 상태 ready로 변경
    } 
}


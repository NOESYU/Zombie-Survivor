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
    }

    private void OnEnable()
    {
        // 총 상태 초기화
    }

    public void Fire()
    {
        // 발사 시도
    }

    private void Shot()
    {
        // 실제 발사 처리
    }

    // 발사 이펙트와 소리를 재생하고 탄알 궤적을 그림
    private IEnumerator ShotEffect(Vector3 hitPosition)
    {
        bulletLineRenderer.enabled = true; // 라인렌더러 활성화하여 탄알 궤적 그림
        yield return new WaitForSeconds(0.03f);

        bulletLineRenderer.enabled = false; // 탄알 궤적 비활성화
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


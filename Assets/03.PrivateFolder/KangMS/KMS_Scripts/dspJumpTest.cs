using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class dspJumpTest : MonoBehaviour
{
    public float jumpHeight = 2f;          // 점프 높이
    public float jumpDuration = 0.5f;      // 점프 총 시간
    public float gravity = -9.8f;          // 중력 가속도

    private bool isJumping = false;        // 점프 상태
    private double jumpStartTime;          // 점프 시작 시간
    private Vector3 startPos;              // 시작 위치
    private Vector3 targetPos;             // 목표 위치

    void Update()
    {
        double currentDspTime = AudioSettings.dspTime;

        if (Input.GetKeyDown(KeyCode.Space) && !isJumping)
        {
            StartJump(currentDspTime);
        }

        // 점프 중일 때 위치 업데이트
        if (isJumping)
        {
            double elapsed = currentDspTime - jumpStartTime;
            float t = (float)(elapsed / jumpDuration);

            if (t >= 1f)
            {
                EndJump();
            }
            else
            {
                float height = jumpHeight * Mathf.Sin(Mathf.PI * t); // 포물선 이동
                transform.position = new Vector3(startPos.x, startPos.y + height, startPos.z);
            }
        }
    }

    private void StartJump(double currentDspTime)
    {
        isJumping = true;
        jumpStartTime = currentDspTime;
        startPos = transform.position;
        targetPos = startPos + Vector3.up * jumpHeight;
    }

    private void EndJump()
    {
        isJumping = false;
        transform.position = startPos; // 원래 위치로 돌아가기
    }
}

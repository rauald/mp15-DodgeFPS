using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;

public class CoroutineTest : MonoBehaviour
{
    /*
    private float _elapsedTime;
    private float _time = 2f;

    private void Start() => StartCoroutine(MyRoutine());

    private IEnumerator MyRoutine()
    {
        while(true)
        {
            yield return new WaitForSeconds(_time);
            Debug.Log("지정 시간 경과");
        }
    }

    private void update()
    {
        _elapsedtime += time.deltatime;

        if (_elapsedtime >= _time)
        {
            _elapsedtime = 0f;
            debug.log("지정 시간 경과");
        }
    }*/

    [SerializeField] private float _delay;
    private bool _isBool;
    private WaitForSeconds _wait;
    private Coroutine _routine;

    private void Awake()
    {
        // YieldInstruction 반복적으로 사용될거라면 캐싱해두기.
        _wait = new WaitForSeconds(_delay);
    }

    private void Start()
    {
        Debug.Log("Start 시작");

        // 시작 : StartCoroutine();
        // X : MyRoutine();
        StartCoroutine(MyRoutine());
        // 멈출 때 : StopCoroutine();

        Debug.Log("Start 종료");
    }

    private void Run()
    {
        if (_routine != null) return;

        _routine = StartCoroutine(MyRoutine());
    }

    private void Stop()
    {
        if (_routine == null) return;

        StopCoroutine(_routine);
        _routine = null;
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            _isBool = !_isBool;
        }
        if(Input.GetKeyDown(KeyCode.Alpha1))
        {
            Run();
        }
        if(Input.GetKeyDown(KeyCode.Alpha2))
        {
            Stop();
        }
    }

    // 함수의 반환형은 IEnumerator
    private IEnumerator MyRoutine()
    {
        while(true)
        {
            // WaitUntil(bool) bool 값이 참이 될 때까지 대기한다.
            yield return new WaitUntil(() => _isBool);
            Debug.Log("Coroutine");
            _isBool = false;
        }

        //Debug.Log("Coroutine 1");
        // 반환할 때는 'yield return'
        // yield return 000 : 000이(가) 충족되는 상황까지 함수를 일시정지 하고 대기할 것.
        //yield return _wait;
        //Debug.Log("Coroutine 2");

        // 루틴을 아예 멈출 때
        // yield break;
    }
}
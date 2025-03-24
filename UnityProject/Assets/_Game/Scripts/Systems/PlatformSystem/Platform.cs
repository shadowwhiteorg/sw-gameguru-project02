using System.Collections;
using System.Collections.Generic;
using _Game.Systems.MeshSystem;
using _Game.Systems.MovementSystem;
using UnityEngine;
using UnityEngine.Serialization;

namespace _Game.Systems.PlatformSystem
{
    public class Platform : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private float _fallSpeed = 2f;
    [SerializeField] private float _destroyHeight = -5f;
    
    private GameObject _mainPart;
    private GameObject _slicedPart;
    private MeshHandler _meshHandler;
    private Coroutine _moveCoroutine;

    public GameObject MainPart => _mainPart;
    public Vector3 MainPartCenterPosition => _mainPart.GetComponent<MeshRenderer>().bounds.center;
    public Vector3 MainPartSize => _mainPart.GetComponent<MeshRenderer>().bounds.size;
    public Vector3 MainPartPivot => new Vector3(
        MainPartCenterPosition.x - MainPartSize.x / 2,
        MainPartCenterPosition.y - MainPartSize.y / 2, 
        MainPartCenterPosition.z - MainPartSize.z / 2
    );

    public void Initialize(GameObject mainPart, MeshHandler meshHandler)
    {
        _mainPart = mainPart;
        _meshHandler = meshHandler;
    }

    public void MoveMainPart()
    {
        _moveCoroutine = StartCoroutine(MoveToX());
    }

    private IEnumerator MoveToX()
    {
        float duration = 1f;
        Vector3 startPos = _mainPart.transform.localPosition;
        Vector3 endPos = new Vector3(startPos.x + _meshHandler.RelativeSpawnPositionX*2, startPos.y, startPos.z);

        while (true)
        {
            yield return MoveBetweenPoints(startPos, endPos, duration);
            yield return MoveBetweenPoints(endPos, startPos, duration);
        }
    }

    private IEnumerator MoveBetweenPoints(Vector3 start, Vector3 end, float duration)
    {
        float elapsed = 0;
        while (elapsed < duration)
        {
            _mainPart.transform.localPosition = 
                Vector3.Lerp(start, end, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }
        _mainPart.transform.localPosition = end;
    }

    public void SetMainPart(GameObject mainPart) => _mainPart = mainPart;
    public void SetSlicedPart(GameObject slicedPart) => _slicedPart = slicedPart;

    public void StopMoving()
    {
        if (_moveCoroutine != null)
            StopCoroutine(_moveCoroutine);
    }

    public void StartFalling()
    {
        StartCoroutine(FallDown());
    }

    private IEnumerator FallDown()
    {
        while (_slicedPart.transform.position.y > _destroyHeight)
        {
            _slicedPart.transform.position += 
                Vector3.down * (_fallSpeed * Time.deltaTime);
            yield return null;
        }
        Destroy(_slicedPart);
    }
}
}
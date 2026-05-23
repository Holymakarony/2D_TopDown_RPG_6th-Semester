using System.Collections;
using UnityEngine;

public class GrapeProjectile : MonoBehaviour
{
    [SerializeField] private float duration = 1f;
    [SerializeField] private AnimationCurve animCurve;
    [SerializeField] private float heightY = 3f;
    [SerializeField] private GameObject grapeProjectileShadowPrefab;
    [SerializeField] private GameObject grapeSplatterPrefab;
    
    private void Start()
    {
        GameObject grapeShadow = Instantiate(grapeProjectileShadowPrefab, transform.position + new Vector3(0, -0.3f, 0), Quaternion.identity);
        
        Vector3 playerPos = PlayerController.Instance.transform.position;
        Vector3 grapeShadowStartPosition = grapeShadow.transform.position;
        
        StartCoroutine(ProjectileCurveRoutine(transform.position, playerPos));
        StartCoroutine(MoveGrapeShadowRoutine(grapeShadow, grapeShadowStartPosition, playerPos));
    }

    private IEnumerator ProjectileCurveRoutine(Vector3 startPosition, Vector3 endPosition)
    {
        float timePassed = 0f;

        while (timePassed < duration)
        {
            timePassed += Time.deltaTime;
            float linearT = timePassed / duration;
            float heightT = animCurve.Evaluate(linearT);
            float height = Mathf.Lerp(0f, heightY, heightT);

            transform.position = Vector2.Lerp(startPosition, endPosition, linearT) + new Vector2(0f, height);

            yield return null;
        }

        Instantiate(grapeSplatterPrefab, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }

    private IEnumerator MoveGrapeShadowRoutine(GameObject grapeShadow, Vector3 startPosition, Vector3 endPosition)
    {
        float timePassed = 0f;
        float shadowSizeOnTop = 0.5f;

        while (timePassed < duration)
        {
            timePassed += Time.deltaTime;
            
            float linearT = timePassed / duration;
            
            float scale = timePassed < duration / 2 ? Mathf.Lerp(1, shadowSizeOnTop, linearT) : Mathf.Lerp(shadowSizeOnTop, 1, linearT);

            grapeShadow.transform.localScale = new Vector3(scale, scale, 0);

            grapeShadow.transform.position = Vector2.Lerp(startPosition, endPosition, linearT);
            yield return null;
        }

        Destroy(grapeShadow);
    }
}

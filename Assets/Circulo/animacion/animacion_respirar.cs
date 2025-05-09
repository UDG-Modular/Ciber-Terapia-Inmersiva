using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class animacion_respirar : MonoBehaviour
{
    public Sprite[] Background;
    public GameObject respiracion;
    private float timer = 0.01f;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(TimeWithinFrames());
    }

    // Update is called once per frame
    void Update()
    {

    }

    IEnumerator TimeWithinFrames()
    {
        int totalFrames = Background.Length - 1;
        float duration = 2f; // duracion de la inhalacion o exhalacion
        int cycles = 3;

        for (int j = 0; j < cycles; j++)
        {
            // exhalar: curva que desacelera al final y pausa al final
            yield return StartCoroutine(SmoothAnimation(1, 0, totalFrames, duration, true));

            // mantener el ultimo frame de exhalacion un momento
            respiracion.GetComponent<Image>().sprite = Background[0];
            yield return new WaitForSeconds(1f); // pausa extra al final de exhalar

            // inhalar: curva suave tipo sen(t)
            yield return StartCoroutine(SmoothAnimation(0, 1, totalFrames, duration, false));

            // mantener el ultimo frame de exhalacion un momento
            respiracion.GetComponent<Image>().sprite = Background[90];
            yield return new WaitForSeconds(1f); // pausa extra al final de exhalar
        }
    }


    IEnumerator SmoothAnimation(float start, float end, int maxFrame, float duration, bool easeOut)
    {
        float elapsed = 0f;
        while (elapsed < duration)
        {
            float t = elapsed / duration;

            // easing basado en sen(x) o smoothstep
            float easedT = easeOut
                ? Mathf.Sin(t * Mathf.PI * 0.5f)              // exhalar: lento al final
                : 0.5f - 0.5f * Mathf.Cos(t * Mathf.PI);      // inhalar: sen(t)

            float interpolated = Mathf.Lerp(start, end, easedT);
            int frameIndex = Mathf.Clamp(Mathf.RoundToInt(interpolated * maxFrame), 0, maxFrame);

            respiracion.GetComponent<Image>().sprite = Background[frameIndex];
            elapsed += Time.deltaTime;
            yield return null;
        }
    }


}

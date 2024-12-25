using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class IntroController : MonoBehaviour
{
    [Header("Intro Main")]
    [SerializeField] Image back;
    [SerializeField] Image fullTitle;
    [SerializeField] GameObject pieceContain;
    [SerializeField] RectTransform[] titlePiece;
    [SerializeField] float timeDelayStart = 0f;
    [SerializeField] float timeDisplayIntro = 3f;
    private Vector3 defaultFullTitleScale;


    [Space(5)]
    [Header("Test Mode")]
    [Tooltip("Loop all effect")]
    [SerializeField] bool isTestMode = true;

    [Space(5)]
    [Header("CHANGE BACK COLOR")]
    [SerializeField] bool isChangeBack = true;
    [SerializeField] float timeChangeBack = 2;
    [SerializeField] Color startColorBack = Color.gray;
    [SerializeField] Color endColorBack = Color.black;


    [Space(5)]
    [Header("TITLE EFFECT")]
    [SerializeField] bool isEffectTitle = true;
    [SerializeField] bool randomEffectType = false;
    [Tooltip("1 - Fade, 2 - Fade2, 3 - Scale, 4 - Scale2, 5 - PieceFade, 6 - PieceMove, 7 - PieceMove2")]
    [Range(1, 7)]
    [SerializeField] int effectType = 1;
    [SerializeField] float effectTime = 2f;

    [Space(5)]
    [Header("ADVANCED SETTINGS")]
    [Header("Effect 2 - Fade2")]
    [SerializeField] Color startColorTitle = Color.black;
    [SerializeField] Color endColorTitle = Color.red;
    [Header("Effect 6,7")]
    [SerializeField] float speedMove = 5f;

    private void Start()
    {
        if (GameManager.instance.introDisplayed)
        {
            gameObject.SetActive(false);
            return;
        }

        back.color = startColorBack;
        defaultFullTitleScale = fullTitle.transform.localScale;
        StartCoroutine(StartIntro());
    }

    private IEnumerator StartIntro()
    {
        yield return new WaitForSeconds(timeDelayStart);

        if (isChangeBack)
        {
            StartCoroutine(ChangeColorToBlack());
        }
        if (randomEffectType)
        {
            effectType = Random.Range(1, 8);
        }
        if (isEffectTitle)
        {
            switch (effectType)
            {
                case 1:
                    StartCoroutine(EffectFade());
                    break;
                case 2:
                    StartCoroutine(EffectFade2());
                    break;
                case 3:
                    StartCoroutine(EffectScale());
                    break;
                case 4:
                    StartCoroutine(EffectScale2());
                    break;
                case 5:
                    StartCoroutine(EffectPieceFade());
                    break;
                case 6:
                    StartCoroutine(EffectPieceMove());
                    break;
                case 7:
                    StartCoroutine(EffectPieceMove2());
                    break;
            }
        }

        StartCoroutine(IntroDone());
        Debug.Log("Effect Type: " + effectType);
    }

    private IEnumerator ChangeColorToBlack()
    {
        float elapsedTime = 0f;

        while (elapsedTime < timeChangeBack)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / timeChangeBack;
            back.color = Color.Lerp(startColorBack, endColorBack, t);
            yield return null;
        }
    }

    private IEnumerator EffectFade()
    {
        ChangeTitle();

        float elapsedTime = 0f;

        while (elapsedTime < effectTime)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / effectTime;
            fullTitle.color = Color.Lerp(new Color(1, 1, 1, 0), Color.white, t);
            yield return null;
        }
    }

    private IEnumerator EffectFade2()
    {
        ChangeTitle();

        float elapsedTime = 0f;

        while (elapsedTime < effectTime)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / effectTime;
            fullTitle.color = Color.Lerp(startColorTitle, endColorTitle, t);
            yield return null;
        }
    }

    private IEnumerator EffectScale()
    {
        ChangeTitle();

        float elapsedTime = 0f;

        while (elapsedTime < effectTime)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / effectTime;
            fullTitle.transform.localScale = defaultFullTitleScale * t;
            yield return null;
        }
    }

    private IEnumerator EffectScale2()
    {
        ChangeTitle();

        float elapsedTime = 0f;

        while (elapsedTime < effectTime)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / effectTime;
            fullTitle.transform.localScale = defaultFullTitleScale * (effectTime - t);
            yield return null;
        }
    }

    private IEnumerator EffectPieceFade()
    {
        ChangeTitle(true);

        foreach (var item in titlePiece)
        {
            item.GetComponent<Image>().color = new Color(1, 1, 1, 0);
        }

        float elapsedTime = 0f;

        while (elapsedTime < effectTime)
        {
            elapsedTime += Time.deltaTime;
            float t0 = elapsedTime / (effectTime / 4);
            titlePiece[0].GetComponent<Image>().color = Color.Lerp(new Color(1, 1, 1, 0), Color.white, t0);
            float t1 = elapsedTime / (effectTime * 2 / 4);
            titlePiece[1].GetComponent<Image>().color = Color.Lerp(new Color(1, 1, 1, 0), Color.white, t1);
            float t2 = elapsedTime / (effectTime * 3 / 4);
            titlePiece[2].GetComponent<Image>().color = Color.Lerp(new Color(1, 1, 1, 0), Color.white, t2);
            float t3 = elapsedTime / (effectTime * 4 / 4);
            titlePiece[3].GetComponent<Image>().color = Color.Lerp(new Color(1, 1, 1, 0), Color.white, t3);
            yield return null;

        }
    }

    private IEnumerator EffectPieceMove()
    {
        ChangeTitle(true);
        Vector3[] defaultPos = new Vector3[4];
        for (int i = 0; i < titlePiece.Length; i++)
        {
            defaultPos[i] = titlePiece[i].anchoredPosition;
        }

        float elapsedTime = 0f;

        while (elapsedTime < effectTime)
        {
            float angle = elapsedTime * speedMove;
            float radius = Mathf.Lerp(1440, 0, elapsedTime / effectTime);

            float x = Mathf.Cos(angle) * radius;
            float y = Mathf.Sin(angle) * radius;

            titlePiece[0].anchoredPosition = defaultPos[0] + new Vector3(x, y, 0);
            titlePiece[1].anchoredPosition = defaultPos[1] + new Vector3(-x, y, 0);
            titlePiece[2].anchoredPosition = defaultPos[2] + new Vector3(x, -y, 0);
            titlePiece[3].anchoredPosition = defaultPos[3] + new Vector3(-x, -y, 0);

            elapsedTime += Time.deltaTime;
            yield return null;
        }
    }

    private IEnumerator EffectPieceMove2()
    {
        ChangeTitle(true);
        Vector3[] defaultPos = new Vector3[4];
        for (int i = 0; i < titlePiece.Length; i++)
        {
            defaultPos[i] = titlePiece[i].anchoredPosition;
        }

        float elapsedTime = 0f;

        while (elapsedTime < effectTime)
        {
            float t = elapsedTime / effectTime;
            titlePiece[0].anchoredPosition = Vector2.Lerp(new Vector3(1500, defaultPos[0].y, 0), defaultPos[0], t);
            titlePiece[1].anchoredPosition = Vector2.Lerp(new Vector3(defaultPos[0].x, 1500, 0), defaultPos[0], t);
            titlePiece[2].anchoredPosition = Vector2.Lerp(new Vector3(defaultPos[0].x, -1500, 0), defaultPos[0], t);
            titlePiece[3].anchoredPosition = Vector2.Lerp(new Vector3(-1500, defaultPos[0].y, 0), defaultPos[0], t);

            elapsedTime += Time.deltaTime;
            yield return null;
        }
    }

    private void ChangeTitle(bool isPiece = false)
    {
        if (isPiece)
        {
            fullTitle.gameObject.SetActive(false);
            pieceContain.SetActive(true);
        }
        else
        {
            fullTitle.gameObject.SetActive(true);
            pieceContain.SetActive(false);
        }
    }

    private IEnumerator IntroDone()
    {
        yield return new WaitForSeconds(timeDisplayIntro);
       
        if (isTestMode)
        {
            StartCoroutine(StartIntro());
        }
        else
        {
            GameManager.instance.introDisplayed = true;
            gameObject.SetActive(false);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using DG.Tweening;

public class LevelLoadSystem : MonoBehaviour
{
    public Image pizarraSprite = default, loadImage = default;
    [SerializeField] string[] dialogo = default;
    [SerializeField] Sprite[] sprite = default;
    int indexMssg = 0;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        loadImage.DOFade(0, 1.0f);
    }

    public void nextTexto()
    {
        indexMssg++;
        if (indexMssg >= dialogo.Length)
            indexMssg = 0;
        
        ShowLevelMessage(indexMssg);
    }

    public void prevTexto()
    {
        indexMssg--;
        if (indexMssg < 0)
            indexMssg = dialogo.Length-1;

        ShowLevelMessage(indexMssg);
    }

    public void RevealCurrent()
    {
        pizarraSprite.sprite = sprite[indexMssg];
        TextRevealer.txtInstance.MostrarTexto(dialogo[indexMssg]);
    }

    public void ShowLevelMessage(int ind)
    {
        pizarraSprite.sprite = sprite[ind];
        TextRevealer.txtInstance.MostrarTexto(dialogo[ind]);
    }

    public void LoadCurrentLevel()
    {
        indexMssg++;
        loadImage.DOFade(1, 1.0f).OnComplete(Startlevelcoroutine);
    }

    public void LoadALevel(int sceneIndex)
    {
        indexMssg = sceneIndex;
        loadImage.DOFade(1, 1.0f).OnComplete(Startlevelcoroutine);
    }

    IEnumerator CargarEscena(int _sceneInd)
    {
        AsyncOperation asyncOp = SceneManager.LoadSceneAsync(_sceneInd);
        while (!asyncOp.isDone)
        {
            yield return null;
        }
        loadImage.DOFade(0, 1.0f).OnComplete(DestroyMyself);
    }

    void Startlevelcoroutine()
    {
        StartCoroutine(CargarEscena(indexMssg));
    }

    public void DestroyMyself()
    {
        Destroy(gameObject);
    }
}

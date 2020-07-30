using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelLoadSystem : MonoBehaviour
{
    public Image pizarraSprite = default;
    [SerializeField] string[] dialogo = default;
    [SerializeField] Sprite[] sprite = default;
    int indexMssg = 0;

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

    public void LoadALevel(int sceneIndex)
    {
        StartCoroutine(CargarEscena(sceneIndex));
    }

    IEnumerator CargarEscena(int _sceneInd)
    {
        AsyncOperation asyncOp = SceneManager.LoadSceneAsync(_sceneInd);
        while (!asyncOp.isDone)
        {
            yield return null;
        }
    }
}

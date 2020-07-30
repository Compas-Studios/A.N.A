using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TextRevealer : MonoBehaviour
{
    public GameObject UserInputs = default;
    public GameObject DialogInputs = default;
    public LevelLoadSystem levLoad = default;
    public static TextRevealer txtInstance = default;
    [SerializeField] TextMeshProUGUI textDisplayer = default;
    public float timePerChar = 0.01f;
    [HideInInspector]public bool terminado = false, terminadosMensajes = true;
    WaitForSecondsRealtime secondsPerChar;
    bool continuar = false;

    //bufferMensajes
    string[] bufferString = default;
    int nMensajes = 0;
    int indexMssg = 0;
    IInteractuable _activador;

    private void Awake()
    {
        if (txtInstance == null)
            txtInstance = this;
        else
            Destroy(this);

        secondsPerChar = new WaitForSecondsRealtime(timePerChar);
    }

    private void Update()
    {
        ReproducirBuffer();
    }

    private void ReproducirBuffer()
    {
        if (!terminadosMensajes)
        {
            if (terminado && continuar)
            {
                continuar = false;
                indexMssg++;
                if (indexMssg >= nMensajes)
                {
                    terminadosMensajes = true; //si el indice de mensajes es igual al numero de mensajes ya termino
                    textDisplayer.text = "";
                    _activador.FinInteraccion();
                }
                else
                    StartCoroutine(RutinaTexto(bufferString[indexMssg])); //muestra el primer mensaje
            }
        }
    }

    public void CONTINUE()
    {
        continuar = true;
    }

    public void EXIT()
    {
        _activador.FinInteraccion();
        MostrarInputs(true);
    }

    public void SetActivator(IInteractuable _acti)
    {
        _activador = _acti;
    }

    IEnumerator RutinaTexto(string mensaje)
    {
        terminado = false;
        textDisplayer.text = mensaje;
        textDisplayer.maxVisibleCharacters = 0;
        yield return new WaitForSecondsRealtime(0.01f);
        int caracTotales = textDisplayer.textInfo.characterCount; //cuantos caracteres deberia mostrar TMPro
        int contador = 0;
        while (contador <= caracTotales)
        {   
            textDisplayer.maxVisibleCharacters = contador;
            yield return secondsPerChar;
            contador++;
        }
        yield return new WaitForSecondsRealtime(2f);
        //Aqui lo que sigue una vez terminado
        terminado = true;
    }

    public void MostrarTexto(string mensaje = "default")
    {
        StopAllCoroutines();
        StartCoroutine(RutinaTexto(mensaje));
    }

    public void MostrarMensajes(string[] mensajes ,IInteractuable activador = null)
    {
        bufferString = mensajes; //copiando todos los mensajes
        nMensajes = bufferString.Length; //obteniendo longitud mensajes
        indexMssg = 0;
        terminadosMensajes = false;
        StartCoroutine(RutinaTexto(bufferString[indexMssg])); //muestra el primer mensaje
        _activador = activador ?? null;
    }

    public void MostrarInputs(bool mostrar)
    {
        UserInputs.SetActive(mostrar);
        DialogInputs.SetActive(!mostrar);
    }

    public void mostrarCurrentLevel()
    {
        if (levLoad != null)
            levLoad.RevealCurrent();
    }
}

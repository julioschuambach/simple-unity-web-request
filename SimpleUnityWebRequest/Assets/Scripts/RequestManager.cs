using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class RequestManager : MonoBehaviour
{
    [SerializeField] private Text url;
    [SerializeField] private Text console;
    [SerializeField] private Text result;
    [SerializeField] private string statusCodeColor;

    private void Start()
    {
        statusCodeColor = "#4B4B4B";
    }

    public void Get() => StartCoroutine(nameof(GetData));

    private IEnumerator GetData()
    {
        ClearTextFields();

        using (UnityWebRequest request = UnityWebRequest.Get(url.text))
        {
            yield return request.SendWebRequest();

            string response = request.downloadHandler.text;
            string resultText = "<color={0}>{1}</color> {2}";

            switch (request.responseCode)
            {
                case 0:
                    SetStatusCodeColor("#D7D700");
                    SetConsoleText("Connection error!" + Environment.NewLine + "Check your internet connection and try again.");
                    break;

                case 200:
                    SetStatusCodeColor("#00D700");
                    SetConsoleText(response);
                    break;

                case 404:
                    SetStatusCodeColor("#960096");
                    SetConsoleText("Resource not found!");
                    break;

                case 500:
                    SetStatusCodeColor("#D70000");
                    SetConsoleText("Internal server error!");
                    break;

                default:
                    SetStatusCodeColor("#FF0000");
                    SetConsoleText("Error not specified.");
                    break;
            }

            SetResultText(string.Format(resultText, statusCodeColor, request.responseCode, request.result));
        }
    }

    private void ClearTextFields()
    {
        SetConsoleText(string.Empty);
        SetResultText(string.Empty);
    }

    private void SetConsoleText(string text)
        => console.text = text;

    private void SetStatusCodeColor(string hexadecimalColor)
        => statusCodeColor = hexadecimalColor;

    private void SetResultText(string text)
        => result.text = text;
}

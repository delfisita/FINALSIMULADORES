using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class FirebaseREST : MonoBehaviour
{
    private string databaseURL = "https://finalsimuladores-54bba-default-rtdb.firebaseio.com/" +
        ""; // Cambia por el URL de tu base de datos.

    public void SendData(string path, object data)
    {
        StartCoroutine(PostDataCoroutine(path, data));
    }

    private IEnumerator PostDataCoroutine(string path, object data)
    {
        string jsonData = JsonUtility.ToJson(data);
        string fullPath = databaseURL + path + ".json";

        using (UnityWebRequest request = UnityWebRequest.Put(fullPath, jsonData))
        {
            request.method = "PUT";
            request.SetRequestHeader("Content-Type", "application/json");

            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                Debug.Log("Datos enviados correctamente.");
            }
            else
            {
                Debug.LogError("Error al enviar datos: " + request.error);
            }
        }
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine.Networking;

namespace Namify;

public static class Data
{
    private const string DataPath = "namify_names.json";
    public static List<string> Names = new();
    private static readonly COTLDataReadWriter<List<string>> NameReadWriter = new();

    internal static void LoadData()
    {
        NameReadWriter.OnReadCompleted += delegate(List<string> names)
        {
            Names = names;
            Plugin.Log.LogWarning(Names.Count > 0 ? $"Loaded saved names. Count: {Names.Count}" : "No saved names exist.");

            foreach (var name in Names)
            {
                Follower.Followers.RemoveAll(a => a.Brain.Info.Name == name);
            }
        };

        NameReadWriter.OnReadError += delegate { Plugin.Log.LogWarning("Failed to load saved names!"); };

        NameReadWriter.Read(DataPath);
    }


    internal static void SaveData()
    {
        NameReadWriter.OnWriteCompleted += delegate { Plugin.Log.LogWarning($"Saved generated names! {Names.Count}"); };

        NameReadWriter.OnWriteError += delegate(MMReadWriteError error) { Plugin.Log.LogWarning($"There was an issue saving generated names: {error.Message}"); };

        NameReadWriter.Write(Names, DataPath, true, false);
    }

    internal static void GetNames()
    {
        var primaryError = false;
        if (Names is {Count: > 0})
        {
            Plugin.Log.LogWarning("Names already loaded!");
            return;
        }

        GameManager.GetInstance().StartCoroutine(GetRequest("https://randommer.io/api/Name?nameType=fullname&quantity=1000", true, req =>
        {
            if (req.isNetworkError || req.isHttpError)
            {
                Plugin.Log.LogError($"{req.error}: {req.downloadHandler.text}");
                NotificationCentre.Instance.PlayGenericNotification("There was an error retrieving names for Namify! Trying back-up source...");
                primaryError = true;
            }
            else
            {
                var nameText = JsonConvert.DeserializeObject<string[]>(req.downloadHandler.text);
                foreach (var name in nameText)
                {
                    Names.AddRange(name.Split());
                }

                SaveData();
            }
        }));

        if (!primaryError) return;


        GameManager.GetInstance().StartCoroutine(BackupRequest());
    }

    private static IEnumerator BackupRequest()
    {
        for (var i = 0; i < 10; i++)
        {
            yield return GameManager.GetInstance().StartCoroutine(GetRequest("https://namey.muffinlabs.com/name.json?count=10&with_surname=true&frequency=all", false, req =>
            {
                if (req.isNetworkError || req.isHttpError)
                {
                    Plugin.Log.LogError($"{req.error}: {req.downloadHandler.text}");
                    NotificationCentre.Instance.PlayGenericNotification("There was an error retrieving names for Namify from the backup source!");
                }
                else
                {
                    var nameText = JsonConvert.DeserializeObject<string[]>(req.downloadHandler.text);
                    foreach (var name in nameText)
                    {
                        Names.AddRange(name.Split());
                        Plugin.Log.LogWarning($"Backup Name: {name}");
                    }

                    SaveData();
                }
            }));
        }
    }

    private static IEnumerator GetRequest(string endpoint, bool apiKey, Action<UnityWebRequest> callback)
    {
        using var request = UnityWebRequest.Get(endpoint);
        if (apiKey)
        {
            request.SetRequestHeader("X-Api-Key", Plugin.PersonalApiKey.Value);
        }

        yield return request.SendWebRequest();
        callback(request);
    }
}
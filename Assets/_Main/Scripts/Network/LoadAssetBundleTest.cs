using System;
using System.Collections;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.Networking;
using Newtonsoft.Json;

namespace StarterAssets {
    public class LoadAssetBundleTest : MonoBehaviour {
        private const string worldGameObjectName = "world";

        private void Start() {
            // StartCoroutine(LoadWorld());

            StartCoroutine(Post(
                               "https://rpc.testnet.near.org/",
                               "{\n\t\"method\": \"query\",\n\t\"params\": {\n\t\t\"request_type\": \"call_function\",\n\t\t\"account_id\": \"dev-1663077383336-80416121270463\",\n\t\t\"method_name\": \"get_asset_bundles\",\n\t\t\"args_base64\": \"e30=\",\n\t\t\"finality\": \"optimistic\"\n\t},\n\t\"id\": 132,\n\t\"jsonrpc\": \"2.0\"\n}",
                               (json) => {
                                   var urls = ParseAssetBundleUrl(json);
                                   StartCoroutine(LoadWorlds(urls));
                               }));
        }

        private IEnumerator LoadWorlds(string[] urls) {
            foreach (var url in urls) {
                StartCoroutine(LoadWorld(url));
            }

            return null;
        }


        IEnumerator LoadWorld(string url) {
            Debug.Log("Loading remote AssetBundle: " + url);
            UnityWebRequest www = UnityWebRequestAssetBundle.GetAssetBundle(url);
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success) {
                Debug.Log(www.error);
            }
            else {
                AssetBundle bundle = DownloadHandlerAssetBundle.GetContent(www);
                GameObject world = bundle.LoadAsset<GameObject>(worldGameObjectName);
                Debug.Log("Instantiating remote AssetBundle: " + url
                );
                Instantiate(world);
            }
        }

        string[] ParseAssetBundleUrl(string json) {
            var response = JsonConvert.DeserializeObject<RpcResponse>(json);
            var text = "";
            foreach (var ch in response.result.result) {
                text += (char)ch;
            }

            var pattern = "\\\"(.*?)\\\"";
            var rg = new Regex(pattern);
            var urls = rg.Matches(text).Select(m => m.Groups[1].Value).ToArray();
            foreach (var url in urls) {
                Debug.Log("url = " + url);
            }

            Debug.Log("Parsed AssetBundle URL: " + text);
            return urls;
        }

        IEnumerator Post(string url, string bodyJsonString, Action<string> onSuccess) {
            var request = new UnityWebRequest(url, "POST");
            byte[] bodyRaw = Encoding.UTF8.GetBytes(bodyJsonString);
            request.uploadHandler = (UploadHandler)new UploadHandlerRaw(bodyRaw);
            request.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
            request.SetRequestHeader("Content-Type", "application/json");
            yield return request.SendWebRequest();
            Debug.Log("Status Code: " + request.responseCode);
            onSuccess(request.downloadHandler.text);
        }
    }

    internal class RpcResponse {
        public RpcResponseResult result;
    }

    internal class RpcResponseResult {
        public int[] result;
    }
}
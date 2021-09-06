using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

namespace LAO.Generic {

    /// <summary>
    /// How unity communicates to other server
    /// </summary>
    public class NetService : MonoBehaviour {

        private string serverURL = "https://mydomain.com/getData.php";
        string jsonStr { get; set; }
        List<string> data;

        void Start() {
            data = new List<string> { "19532", "19531", "19525", "19572" }; //actual test data
        }

        IEnumerator getDataFromServer(string id) {

            //use the wwwForm for post
            // set as many key,values you want into wwwform
            //server will use post to retrieve the stored info
            WWWForm form = new WWWForm();
            form.AddField("cid", id);

            //use www to send the form to the server
            WWW w = new WWW(serverURL, form);

            yield return w;

            if (!string.IsNullOrEmpty(w.error)) {
                Debug.Log(w.error);
            } else {
                //pass 
                this.GetComponent<Text>().text = w.text;
                jsonStr = w.text;
                displayItem();
            }

        }

        private void displayItem() {
            //JObject jObj = JObject.Parse(jsonStr);
        }

        ///If you want to toggle between a list of test data
        /// ---------------------------- to test toggle between avaialble data
        private int currentData;

        public void nextData() {
            if (currentData < data.Count - 1) {
                currentData++;
            } else {
                currentData = 0;
            }

            StartCoroutine(getDataFromServer(data[currentData]));
        }

        public void previousData() {
            if (currentData > 0) {
                currentData--;
            } else {
                currentData = data.Count - 1;
            }

            StartCoroutine(getDataFromServer(data[currentData]));
        }
    }
}
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using Newtonsoft.Json;


namespace LAO.Generic {


	/// <summary>
	/// For rail API make sure to put
	/// protect_from_forgery
	/// under the controller handling post request
	/// </summary>
	public class WebService : MonoBehaviour {

		public Text textOutput_gui;

		//test call
		public void testServerCall() {
			//string query = "http://xailao.com/games/poplopoly/retreive.php?query=top5";

			//RUBY
			string api = "localhost:3000/welcome/login";
			//string query = "http://jsonplaceholder.typicode.com/posts";
			//string query = "https://knife-example-api1.herokuapp.com/customers";
			//string query = "www.xailao.com/game/popop/retrieve.php";
			//string query = "http://demo.app/items/unityItems";
			StartCoroutine(sendQuery(api));
		}

		//the return data is forwarded to the gameManager state
		IEnumerator sendQuery(string query) {
			/*
	 	url = "www.xailao.com/game/popop/update.php?" +
	 	"username" + username +
	 	"password" + password +
	 	"score" + level;
	 	*/
			//string url = "http://xailao.com/games/poplopoly/retreive.php?query=top10";
			string url = query;

			//json data - works with only UnityWebRequest
			string jstr = "{\'name\':\'john\'}";

			//form data - works with both UnityWebRequest and WWW
			WWWForm f = new WWWForm();
			f.AddField("name", "Kimmy");
			f.AddField("gameid", "1001001");
			f.AddField("user", "222-0001");

			//Hash data - works with only UnityWebRequest
			Dictionary<string, string> hash = new Dictionary<string, string>();
			hash.Add("name", "john");

			//show this text while loading
			//go_scoreTxt.gameObject.GetComponent<Text>().text = "Loading . . .";


			//old www get method - working
			//################################
			/*
            WWW www = new WWW(url);
            yield return www;
            

            if (!string.IsNullOrEmpty(www.error)) {
                Debug.Log(www.error);
            } else {
                Debug.Log("Must click on debug lines below in console to see the whole json string");
                Debug.Log(www.text);

                //convert deserialized json str arrays, into tru objects
                string jstr = www.text;
                List<Foo> foos = JsonConvert.DeserializeObject<List<Foo>>(jstr);

                Debug.Log("converting json into tru objects >> now testing read from customer 1");
                Foo f = foos[0];
                Debug.Log("#Name: " + f.full_name 
                    + " #id: " + f.id
                    + " #email: " + f.email
                    + " #phone: " + f.phone
                    );

            }
			*/
			//############################### END of old www 


			//WWW POST
			//###################################
			//You have to use loops to populate the form
			WWW www = new WWW(url, f);
			yield return www;
			if (!string.IsNullOrEmpty(www.error)) {
				Debug.Log(www.error);
			} else {
				Debug.Log(www.text);
			}
			//################################## end of www post



			//Post way using unityWebRequest
			//######################################
			//This new method allows you to pass in json format str as a argument into the
			//the only downside is that rails cannot parse this properly on rails server
			/*	



			//string jstr = "{\"customer\":[{\"full_name\":\"tom\",\"email\":\"tom@hotmail.com\",\"phone\":\"333-3333\"}]}";


			//f.AddField("created_at", System.DateTime.Now);
			//f.AddField("updated_at", System.DateTime.Now);
			//Debug.Log(System.DateTime.Now);

			//2016-02-19 03:03:33

			var www = UnityWebRequest.Post(url, hash);
			//UnityWebRequest www = UnityWebRequest.Get(url);
			www.SetRequestHeader("Content-Type", "application/json");
			//www.SetRequestHeader("Content-Type", "text/json");

            www.downloadHandler = new DownloadHandlerBuffer();

            yield return www.Send();
            if (www.isError) {
                Debug.Log(www.error);
            } else {
				textOutput_gui.text = "login failed!";
                Debug.Log("Form uploaded complete");
                Debug.Log(www.downloadHandler.text);
            }
			*/
			//------- UNITY WEB REQUEST  end-------------



			//the php file is called and whatever it echose back will be put into www.text
			//go_scoreTxt.gameObject.GetComponent<Text>().text = www.text;



			//use somethign to capture the data
			//GameService.setWebReturnedData(www.text);
			//GameService.setWebCallStatus(true);	//web is done calling
		}

		void OnMouseUp() {
			Debug.Log("I was clicked");
			testServerCall();
		}

	}
}
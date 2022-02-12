using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using UnityEngine;
using UnityEngine.Networking;

namespace Potterblatt.Utils {
	public static class UnityWebRequestExtensions {
		/// <summary>
		/// Get plain text from the <see cref="UnityWebRequest"/>
		/// </summary>
		/// <param name="request">The request being processed</param>
		/// <returns>Plain text of the response or null if there is an error</returns>
		public static string GetTextResponse(this UnityWebRequest request) {
			return request.result == UnityWebRequest.Result.Success ? request.downloadHandler.text : null;
		}

		/// <summary>
		/// Get <see cref="AssetBundle"/> from the <see cref="UnityWebRequest"/>
		/// </summary>
		/// <param name="request">The request being processed</param>
		/// <returns>Bundle of the response or null if there is an error</returns>
		public static AssetBundle GetAssetBundleResponse(this UnityWebRequest request) {
			return request.result == UnityWebRequest.Result.Success ? 
				DownloadHandlerAssetBundle.GetContent(request) : null;
		}

		/// <summary>
		/// Get bytes from the <see cref="UnityWebRequest"/>
		/// </summary>
		/// <param name="request">The request being processed</param>
		/// <returns>bytes of the response or null if there is an error</returns>
		public static byte[] GetBytesResponse(this UnityWebRequest request) {
			return request.result == UnityWebRequest.Result.Success ? request.downloadHandler.data : null;
		}

		/// <summary>
		/// Get <see cref="Texture2D"/> from the request
		/// </summary>
		/// <param name="request">the current request to get the texture from</param>
		/// <returns>The texture</returns>
		public static Texture2D GetTextureResponse(this UnityWebRequest request) {
			return request.result == UnityWebRequest.Result.Success ? 
				DownloadHandlerTexture.GetContent(request) : null;
		}

		/// <summary>
		/// Set the request headers of a request with a dictionary, each key value pair will be added
		/// </summary>
		/// <param name="request">The current request to add headers to</param>
		/// <param name="headers">the header pairs to add</param>
		public static void SetRequestHeaders(this UnityWebRequest request, IEnumerable<KeyValuePair<string, string>> headers) {
			foreach(var headerEntry in headers) {
				request.SetRequestHeader(headerEntry.Key, headerEntry.Value);
			}
		}
		
		/// <summary>
		/// Helper function to get a json response. This will take a text response and parse it into json.
		/// </summary>
		/// <typeparam name="T">The type of json element to return</typeparam>
		/// <returns>the text of the response converted to json</returns>
		public static T GetJsonResponse<T>(this UnityWebRequest request) where T : JToken {
			//get the text
			var jsonText = request.GetTextResponse();

			if(jsonText == null) {
				return null;
			}

			//parse the json
			return (T) JToken.Parse(jsonText);
		}
	}
}
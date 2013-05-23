using System.IO;
using System.Net;
using System.Web;

namespace EddPorter.RacingSuite.Data {

  internal class Internet : IInternet {

    public string Get(string uri) {
      var request = (HttpWebRequest)WebRequest.Create(uri);
      var response = (HttpWebResponse)request.GetResponse();
      using (var sr = new StreamReader(response.GetResponseStream())) {
        return sr.ReadToEnd();
      }
    }

    public string Post(string uri, string post) {
      post = HttpUtility.UrlEncode(post);

      var request = (HttpWebRequest)WebRequest.Create(uri);
      request.Method = "POST";
      request.ContentLength = post.Length;
      request.ContentType = "application/x-www-form-urlencoded";

      using (var writer = new StreamWriter(request.GetRequestStream())) {
        writer.Write(post);
      }

      var response = (HttpWebResponse)request.GetResponse();
      using (var sr = new StreamReader(response.GetResponseStream())) {
        return sr.ReadToEnd();
      }
    }
  }
}
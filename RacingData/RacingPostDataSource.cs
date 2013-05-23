using System;
using System.Xml;

namespace EddPorter.RacingSuite.Data {

  /// <summary>
  /// Interface for accessing racing data from the Racing Post web site.
  /// </summary>
  public class RacingPostDataSource : IDataSource {
    private IInternet internet;

    /// <summary>
    /// Creates a new data source connected to RacingPost.com.
    /// </summary>
    /// <param name="internet">An object this class can use to connect to the internet, e.g. `Internet`.</param>
    public RacingPostDataSource(IInternet internet) {
      this.internet = internet;
    }

    /// <summary>
    /// Locates a horse by its name.
    /// </summary>
    /// <param name="name">The name of the horse to find.</param>
    /// <returns>Information about the horse if an exact match is found or null if not.</returns>
    public Horse FindHorse(string name) {
      if (string.IsNullOrWhiteSpace(name)) {
        throw new ArgumentException("name", "A valid horse name must be specified.");
      }

      var result = ExecuteHorseSearch(name);
      var id = ExtractHorseIdFromSearchResults(name, result);
      var horsePage = GetHorsePage(id);

      return null;
    }

    private static string ExtractHorseIdFromSearchResults(string name, string result) {
      var doc = new XmlDocument();
      doc.LoadXml(result);
      var node = doc.SelectSingleNode(string.Format(@"//item[NAME='{0}']/ID", name));
      return node.InnerText;
    }

    private string ExecuteHorseSearch(string name) {
      var uri = @"http://www.racingpost.com/public_gateway/db_search_interface.sd";
      var post = string.Format(@"search={0}&edition=4&category=2", name);
      return internet.Post(uri, post);
    }

    private string GetHorsePage(string id) {
      var horseUri = string.Format("http://www.racingpost.com/horses/horse_home.sd?horse_id={0}", id);
      return internet.Get(horseUri);
    }
  }
}
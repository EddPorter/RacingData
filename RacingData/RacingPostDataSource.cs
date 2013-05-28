using System;
using System.Text.RegularExpressions;
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

      var document = new HtmlAgilityPack.HtmlDocument();
      var horse = new Horse();

      // <div class="popUp"><div class="popUpHead clearfix"><div class="leftCol"><h1>Academy General (IRE)  <span>Race record</span></h1>
      document.LoadHtml(horsePage);
      var nameNode = document.DocumentNode.SelectSingleNode("//div[@class='popUp']/div[@class='popUpHead clearfix']/div[@class='leftCol']/h1");
      var horseName = nameNode.ChildNodes[0].InnerText.Trim();

      Regex horseNameRegex = new Regex(@"(.*) \((.*)\)");
      var horseNameMatch = horseNameRegex.Match(horseName);
      horse.Name = horseNameMatch.Groups[1].Value;
      horse.CountryOfBirth = horseNameMatch.Groups[2].Value;

      /* ==Date of Birth, Mother, Father==
       * <ul id="detailedInfo">
       * <li>
       * <b>
       * 7-y-o (17Apr06 b g)
       * </b>
       * </li>
       * <li>
       * <b>
       * <a href="http://bloodstock.racingpost.com/stallionbook/stallion.sd?horse_id=75837&amp;popup=1" class="White" onclick='scorecards.send("stallion_name");return Html.popup(this, {width:750, height:800})' title="Full details about this STALLION">Beneficial </a> </b>
       * &nbsp;(14.3f) —
       * <b>
       * <a href="http://bloodstock.racingpost.com/dam/dam_home.sd?horse_id=503103" class="White" onclick='scorecards.send("dam_name");return Html.popup(this, {width:695, height:800})' title="Full details about this DAM ">Discerning Air </a> </b>
       * (<b><a href="http://bloodstock.racingpost.com/stallionbook/stallion.sd?horse_id=69582&amp;popup=1" class="White" onclick='scorecards.send("stallion_name");return Html.popup(this, {width:750, height:800})' title="Full details about this STALLION">Ezzoud (IRE)</a></b>
       * &nbsp;<span>(11.5f)</span>)
       * </li>
       */
      var dobNode = document.DocumentNode.SelectSingleNode("//ul[@id='detailedInfo']/li/b");
      var dobText = dobNode.InnerText.Trim();

      var dobRegex = new Regex(@"\((\d+)([A-Za-z]+)(\d+) .*\)");
      var dobMatch = dobRegex.Match(dobText);
      horse.DateOfBirth = DateTime.Parse(string.Format("{0}-{1}-{2}", dobMatch.Groups[1], dobMatch.Groups[2], dobMatch.Groups[3]));

      // Breed => Load Pedigree tab: http://www.racingpost.com/horses/horse_pedigree.sd?horse_id={id}


      return horse;
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
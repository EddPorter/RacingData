using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.IO;
using System.Web;

namespace EddPorter.RacingSuite.Data.Test {

  [TestClass]
  public class RacingPostDataSourceTest {

    [TestMethod]
    [ExpectedException(typeof(ArgumentException))]
    public void FindHorse_given_empty_horse_name_throws_exception() {
      string name = string.Empty;
      var source = CreateDataSource();
      source.FindHorse(name);
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentException))]
    public void FindHorse_given_null_horse_name_throws_exception() {
      string name = null;
      var source = CreateDataSource();
      source.FindHorse(name);
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentException))]
    public void FindHorse_given_whitespace_horse_name_throws_exception() {
      string name = "\t  \n\r  \t";
      var source = CreateDataSource();
      source.FindHorse(name);
    }

    [TestMethod]
    public void FindHorse_with_valid_name_gets_horse_page() {
      var internet = CreateInternetMock();
      var source = CreateDataSource(internet);

      source.FindHorse("Academy General");

      internet.Verify(i => i.Get(It.IsRegex("horse_id=AcademyGeneral")));
    }

    [TestMethod]
    public void FindHorse_with_valid_name_posts_horse_search() {
      var internet = CreateInternetMock();
      var source = CreateDataSource(internet);

      source.FindHorse("Academy General");

      internet.Verify(i => i.Post(It.IsAny<string>(), It.IsRegex(@"Academy\+General")));
    }

    [TestMethod]
    public void FindHorse_with_valid_name_returns_correct_birthdate() {
      var internet = CreateInternetMock();
      var source = CreateDataSource(internet);

      var horse = source.FindHorse("Academy General");

      Assert.AreEqual<DateTime>(new DateTime(2006, 4, 17), horse.DateOfBirth);
    }

    [TestMethod]
    public void FindHorse_with_valid_name_returns_correct_name() {
      var internet = CreateInternetMock();
      var source = CreateDataSource(internet);

      var horse = source.FindHorse("Academy General");

      Assert.AreEqual<string>("Academy General", horse.Name);
    }

    [TestMethod]
    public void FindHorse_with_valid_name_returns_correct_father_name_on_access() {
      var internet = CreateInternetMock();
      var source = CreateDataSource(internet);

      var horse = source.FindHorse("Academy General");

      Assert.AreEqual<string>("Beneficial", horse.Father.Name);
    }


    [TestMethod]
    public void FindHorse_with_valid_name_delay_loads_father() {
      var internet = CreateInternetMock();
      var source = CreateDataSource(internet);

      var horse = source.FindHorse("Academy General");

      Assert.IsFalse(horse.father.IsValueCreated);
    }

    [TestMethod]
    public void FindHorse_with_valid_name_delay_loads_mother() {
      var internet = CreateInternetMock();
      var source = CreateDataSource(internet);

      var horse = source.FindHorse("Academy General");

      Assert.IsFalse(horse.mother.IsValueCreated);
    }

    [TestMethod]
    public void FindHorse_with_valid_name_returns_correct_mother_name_on_access() {
      var internet = CreateInternetMock();
      var source = CreateDataSource(internet);

      var horse = source.FindHorse("Academy General");

      Assert.AreEqual<string>("Discerning Air", horse.Mother.Name);
    }

    [TestMethod]
    public void FindHorse_with_valid_name_returns_correct_nationality() {
      var internet = CreateInternetMock();
      var source = CreateDataSource(internet);

      var horse = source.FindHorse("Academy General");

      Assert.AreEqual<string>("IRE", horse.CountryOfBirth);
    }

    [TestMethod]
    public void FindHorse_with_valid_name_returns_non_null_horse_data() {
      var internet = CreateInternetMock();
      var source = CreateDataSource(internet);

      var horse = source.FindHorse("Academy General");

      Assert.IsNotNull(horse);
    }

    private static RacingPostDataSource CreateDataSource() {
      var internetMock = new Mock<IInternet>();
      return CreateDataSource(internetMock);
    }

    private static RacingPostDataSource CreateDataSource(Mock<IInternet> internetMock) {
      return new RacingPostDataSource(internetMock.Object);
    }

    private static Mock<IInternet> CreateInternetMock() {
      var internet = new Mock<IInternet>();
      internet.Setup(i => i.Post(It.IsAny<string>(), It.IsAny<string>())).Returns<string, string>((url, name) => {
        string horse = name.Substring(name.IndexOf("=") + 1);
        horse = horse.Substring(0, horse.IndexOf("&"));
        horse = HttpUtility.UrlDecode(horse);
        return "<results><item><NAME>" + horse + "</NAME><ID>" + horse.Replace(" ", "") + "</ID></item></results>";
      });
      internet.Setup(i => i.Get(It.IsAny<string>())).Returns<string>(name => new StreamReader("Horse_" + name.Substring(name.IndexOf("=") + 1) + ".htm").ReadToEnd());
      return internet;
    }
  }
}
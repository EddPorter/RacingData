using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;

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
    public void FindHorse_with_valid_name_calls_internet_post() {
      string name = "Horsey";
      var internet = new Mock<IInternet>();
      internet.Setup(i => i.Post(It.IsAny<string>(), It.IsAny<string>())).Returns("<results><item><NAME>" + name + "</NAME><ID>4</ID></item></results>");
      var source = CreateDataSource(internet);

      source.FindHorse(name);

      internet.Verify(i => i.Post(It.IsAny<string>(), It.IsRegex(name)));
    }

    private static RacingPostDataSource CreateDataSource() {
      var internetMock = new Mock<IInternet>();
      return CreateDataSource(internetMock);
    }

    private static RacingPostDataSource CreateDataSource(Mock<IInternet> internetMock) {
      return new RacingPostDataSource(internetMock.Object);
    }
  }
}
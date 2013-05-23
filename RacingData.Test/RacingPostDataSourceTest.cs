using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace EddPorter.RacingSuite.Data.Test {
  [TestClass]
  public class RacingPostDataSourceTest {
    [TestMethod]
    public void FindHorse_() {
      var source = new RacingPostDataSource(new Internet());
      var horse = source.FindHorse("Red Rum");
    }
  }
}

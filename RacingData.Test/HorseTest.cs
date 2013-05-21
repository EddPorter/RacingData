using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace EddPorter.RacingSuite.Data.Test {

  [TestClass]
  public class HorseTest {
    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public void Horse_ActualAge_given_no_dob_throws_exception() {
      var horse = new Horse();
      var age = horse.ActualAge;
    }
    [TestMethod]
    [ExpectedException(typeof(InvalidOperationException))]
    public void Horse_OfficialAge_given_no_dob_throws_exception() {
      var horse = new Horse();
      var age = horse.OfficialAge;
    }
    [TestMethod]
    public void Horse_ActualAge_given_known_dob_returns_horses_actual_age() {
      var dateOfBirth = new DateTime(1984, 2, 1);
      var horse = new Horse();
      horse.DateOfBirth = dateOfBirth;
      int age = horse.ActualAge;

      var now = DateTime.Now;
      int expectedAge = now.Year - dateOfBirth.Year;

      var backDate = new DateTime(dateOfBirth.Year, now.Month, now.Day);
      if (dateOfBirth - backDate > TimeSpan.Zero) {
        --expectedAge;
      }
      Assert.AreEqual(expectedAge, age);
    }

    [TestMethod]
    public void Horse_ActualAge_given_known_dob_today_returns_horses_actual_age() {
      var now = DateTime.Now;
      var dateOfBirth = new DateTime(1984, now.Month, now.Day);
      var horse = new Horse();
      horse.DateOfBirth = dateOfBirth;
      int age = horse.ActualAge;

      int expectedAge = now.Year - dateOfBirth.Year;
      Assert.AreEqual(expectedAge, age);
    }

    [TestMethod]
    public void Horse_OfficialAge_given_known_dob_returns_horses_official_age() {
      var dateOfBirth = new DateTime(1984, 2, 1);
      var horse = new Horse();
      horse.DateOfBirth = dateOfBirth;
      int age = horse.OfficialAge;

      var now = DateTime.Now;
      int expectedAge = now.Year - dateOfBirth.Year;

      Assert.AreEqual(expectedAge, age);
    }
  }
}
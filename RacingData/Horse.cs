using System;

namespace EddPorter.RacingSuite.Data {

  /// <summary>
  /// Represents information pertaining to a specific horse.
  /// </summary>
  public class Horse {

    /// <summary>
    /// Gets the actual of the horse based on their true date of birth.
    /// </summary>
    public int ActualAge {
      get {
        if (DateOfBirth == default(DateTime)) {
          throw new InvalidOperationException("The horse's actual age could not be computed since no date of birth is known.");
        }

        var now = DateTime.Now;
        int age = now.Year - DateOfBirth.Year;

        var backDate = new DateTime(DateOfBirth.Year, now.Month, now.Day);
        if (DateOfBirth - backDate > TimeSpan.Zero) {
          --age;
        }
        return age;
      }
    }

    /// <summary>
    /// Gets the official breed of the horse.
    /// </summary>
    public string Breed {
      get;
      set;
    }

    /// <summary>
    /// Gets the horse's registered country of birth.
    /// </summary>
    public string CountryOfBirth {
      get;
      set;
    }

    /// <summary>
    /// Gets the horse's actual date of birth. Their official date of birth is 1st January in the same year.
    /// </summary>
    public DateTime DateOfBirth {
      get;
      set;
    }

    /// <summary>
    /// Gets this horse's father, if known.
    /// </summary>
    public virtual Horse Father {
      get;
      set;
    }

    /// <summary>
    /// Gets the gender of the horse.
    /// </summary>
    public Gender Gender {
      get;
      set;
    }

    /// <summary>
    /// Gets this horse's mother, if known.
    /// </summary>
    public virtual Horse Mother {
      get;
      set;
    }

    /// <summary>
    /// Gets the registed name of the horse.
    /// </summary>
    public string Name {
      get;
      set;
    }

    /// <summary>
    /// Gets the official age of the horse based on their birthday being on 1st January of the year of their birth.
    /// </summary>
    public int OfficialAge {
      get {
        if (DateOfBirth == default(DateTime)) {
          throw new InvalidOperationException("The horse's official age could not be computed since no date of birth is known.");
        }

        return DateTime.Now.Year - DateOfBirth.Year;
      }
    }

    // form
    // races
  }
}
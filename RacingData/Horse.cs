using System;

namespace RacingData {

  public class Horse {

    public int Age {
      get;
      set;
    }

    public string Breed {
      get;
      set;
    }

    public string CountryOfBirth {
      get;
      set;
    }

    public DateTime DateOfBirth {
      get;
      set;
    }

    public virtual Horse Father {
      get;
      set;
    }

    public virtual Horse Mother {
      get;
      set;
    }

    public string Name {
      get;
      set;
    }

    // form
    // races
  }
}
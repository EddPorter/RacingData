using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EddPorter.RacingSuite.Data {
  /// <summary>
  /// Interface for accessing racing data.
  /// </summary>
  public interface IDataSource {
    /// <summary>
    /// Locates a horse by its name.
    /// </summary>
    /// <param name="name">The name of the horse to find.</param>
    /// <returns>Information about the horse if an exact match is found or null if not.</returns>
    Horse FindHorse(string name);
  }
}

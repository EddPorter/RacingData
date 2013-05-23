namespace EddPorter.RacingSuite.Data {

  /// <summary>
  /// Provides abstracted methods that can be used by other classes for accessing the internet.
  /// </summary>
  public interface IInternet {

    /// <summary>
    /// Sends an HTTP GET request to the specified page and returns the contents.
    /// </summary>
    /// <param name="uri">The page to send the request to.</param>
    /// <returns>The contents of the request response.</returns>
    string Get(string uri);

    /// <summary>
    /// Sends an HTTP POST request to the specified page along with the given request body and returns the contents.
    /// </summary>
    /// <param name="uri">The page to send the request to.</param>
    /// <param name="post">The body to include in the request.</param>
    /// <returns>The contents of the request response.</returns>
    string Post(string uri, string post);
  }
}
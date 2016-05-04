using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace Stannieman.HttpQueries
{
    /// <summary>
    /// Class representing a http query.
    /// </summary>
    public class Query
    {
        // Dictionary containing the query parameters
        private IDictionary<string, object> _params;

        /// <summary>
        /// Default constructor.
        /// </summary>
        public Query()
        {
            _params = new Dictionary<string, object>();
        }

        /// <summary>
        /// Constructor that takes 1 parameter.
        /// </summary>
        public Query(string key, object value) : this ()
        {
            AddParameter(key, value);
        }

        /// <summary>
        /// Returns a valid url encoded parameter string.
        /// </summary>
        public string QueryString
        {
            get
            {
                // Create new StringBuilder instance
                StringBuilder strBuilder = new StringBuilder();

                // Check if there are parameters.
                if (_params.Count > 0)
                {
                    // Create enumerator for keys
                    var enumerator = _params.Keys.GetEnumerator();
                    enumerator.MoveNext();

                    string key;

                    // For all but the last param add them to the string, followed by an '&'
                    for (int i = 0; i < _params.Count - 1; i++)
                    {
                        key = enumerator.Current;
                        enumerator.MoveNext();
                        // Url encode the keys and value.
                        // UrlEncode encodes spaces as '+', so we also replace that with %20.
                        strBuilder.Append(WebUtility.UrlEncode(key).Replace("+", "%20")).Append("=").Append(WebUtility.UrlEncode(_params[key].ToString()).Replace("+", "%20")).Append("&");
                    }

                    // Add the last param without trailing '&'. First check if there is at least 1 param.
                    key = enumerator.Current;

                    // Url encode the keys and value.
                    // UrlEncode encodes spaces as '+', so we also replace that with %20.
                    strBuilder.Append(WebUtility.UrlEncode(key).Replace("+", "%20")).Append("=").Append(WebUtility.UrlEncode(_params[key].ToString()).Replace("+", "%20"));
                }

                // Return the result
                return strBuilder.ToString();
            }
        }

        /// <summary>
        /// Returns the dictionary that contains all the parameters.
        /// </summary>
        public IDictionary<string, object> Parameters
        {
            get
            {
                return _params;
            }
        }

        /// <summary>
        /// Adds a parameter to the query. If a parameter with the given key already exists it's value is replaced.
        /// </summary>
        /// <param name="key">Key of new parameter.</param>
        /// <param name="value">Value of new parameter.</param>
        public void AddParameter(string key, object value)
        {
            // If the key doesn't exist add it, otherwise overwrite
            if (!ContainsKey(key))
                _params.Add(key, value);
            else
                _params[key] = value;
        }

        /// <summary>
        /// Returns whether the query contains a parameter with the given key.
        /// </summary>
        /// <param name="key">Key to check for.</param>
        /// <returns>Whether the query contains the parameter.</returns>
        public bool ContainsKey(string key)
        {
            return _params.ContainsKey(key);
        }

        /// <summary>
        /// Returns whether the query contains a parameter with the given value.
        /// </summary>
        /// <param name="value">Value to check for.</param>
        /// <returns>Whether the query contains the parameter.</returns>
        public bool ContainsValue(object value)
        {
            return _params.Values.Contains(value);
        }

        /// <summary>
        /// Removes a parameter with the given key from the query.
        /// </summary>
        /// <param name="key">Key of the parameter to remove.</param>
        public void RemoveParameter(string key)
        {
            _params.Remove(key);
        }

        /// <summary>
        /// Returns the value of the parameter who's key is given.
        /// </summary>
        /// <param name="key">Key of parameter to return value for.</param>
        /// <returns>Value of parameter.</returns>
        public object GetParameter(string key)
        {
            return _params[key];
        }

        /// <summary>
        /// Creates a query instance that contains the parameters of a given query string.
        /// </summary>
        /// <param name="queryString">Query string to parse.</param>
        /// <returns>The query instance created from the given query string.</returns>
        public static Query Parse(string queryString)
        {
            // Create query object to populate with parameters
            Query returnQuery = new Query();

            // Remove leading ? if present. This is "robust" as multiple question marks don't cause errors.
            var trimmedQueryString = queryString.TrimStart('?');

            // Get individual parameters. This is "robust" as multiple ampersants next to each other don't cause errors.
            string[] keyValuePairs = trimmedQueryString.Split('&');

            foreach (string keyValuePair in keyValuePairs)
            {
                // Check if a keyValuePair is empty (could be the case if the query string isn't well formed).
                if (keyValuePair != String.Empty)
                {
                    // Split the parameter in it's key and value
                    string[] keyValue = keyValuePair.Split('=');
                    // Add parameter to query if both the key and value are present and not empty, otherwise throw exception
                    if (keyValue.Length == 2 && keyValue[0] != String.Empty && keyValue[1] != String.Empty)
                        returnQuery.AddParameter(WebUtility.UrlDecode(keyValue[0]), WebUtility.UrlDecode(keyValue[1]));
                    else
                        throw new FormatException($"{keyValuePair} is not a well formed parameter.");
                }
            }

            // Return query instance
            return returnQuery;
        }
    }
}

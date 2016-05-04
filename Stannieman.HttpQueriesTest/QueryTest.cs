using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Stannieman.HttpQueries;
using System.Collections.Generic;

namespace Stannieman.HttpQueriesTest
{
    [TestClass]
    public class QueryTest
    {
        [TestMethod]
        public void ConstructorWithParameterAddsParameterCorrectly()
        {
            string testKey = "testKey";
            string testValue = "testValue";

            Query query = new Query(testKey, testValue);

            Assert.AreEqual(testValue, query.Parameters[testKey]);
        }

        [TestMethod]
        public void AddParameterAddsParameterCorrectly()
        {
            string testKey = "testKey";
            string testValue = "testValue";

            Query query = new Query();
            query.AddParameter(testKey, testValue);

            Assert.AreEqual(testValue, query.Parameters[testKey]);
        }

        [TestMethod]
        public void AddParameterReplacesValueIfKeyExists()
        {
            string testKey = "testKey";
            string testValue1 = "testValue1";
            string testValue2 = "testValue2";

            Query query = new Query();
            query.AddParameter(testKey, testValue1);
            query.AddParameter(testKey, testValue2);

            Assert.AreEqual(testValue2, query.Parameters[testKey]);
        }

        [TestMethod]
        public void ContainsKeyReturnsTrueWhenKeyExists()
        {
            string testKey = "testKey";
            string testValue = "testValue";

            Query query = new Query();
            query.AddParameter(testKey, testValue);

            Assert.IsTrue(query.ContainsKey(testKey));
        }

        [TestMethod]
        public void ContainsKeyReturnsFalseWhenKeyNotExists()
        {
            string testKey = "testKey";

            Query query = new Query();

            Assert.IsFalse(query.ContainsKey(testKey));
        }

        [TestMethod]
        public void ContainsValueReturnsTrueWhenValueExists()
        {
            string testKey = "testKey";
            string testValue = "testValue";

            Query query = new Query();
            query.AddParameter(testKey, testValue);

            Assert.IsTrue(query.ContainsValue(testValue));
        }

        [TestMethod]
        public void ContainsValueReturnsFalseWhenValueNotExists()
        {
            string testValue = "testValue";

            Query query = new Query();

            Assert.IsFalse(query.ContainsValue(testValue));
        }

        [TestMethod]
        public void RemoveParameterCorrectlyRemovesParameter()
        {
            string testKey = "testKey";
            string testValue = "testValue";

            Query query = new Query();
            query.AddParameter(testKey, testValue);
            query.RemoveParameter(testKey);

            Assert.IsFalse(query.Parameters.ContainsKey(testKey));
        }

        [TestMethod]
        public void GetParameterCorrectlyReturnsParameter()
        {
            string testKey = "testKey";
            string testValue = "testValue";

            Query query = new Query();
            query.AddParameter(testKey, testValue);

            Assert.AreEqual(testValue, query.GetParameter(testKey));
        }

        [TestMethod]
        public void QueryStringReturnsCorrectQueryWith0Parameters()
        {
            Query query = new Query();

            Assert.AreEqual(String.Empty, query.QueryString);
        }

        [TestMethod]
        public void QueryStringReturnsCorrectQueryWith1Parameter()
        {
            // User characters that are not allowed in the string to test if they are encoded properly
            string testKey1 = "testKey &?1";
            string testValue1 = "testValue &?1";

            // Expected encoded query string
            string expectedQuery = "testKey%20%26%3F1=testValue%20%26%3F1";

            Query query = new Query();
            query.AddParameter(testKey1, testValue1);

            Assert.AreEqual(expectedQuery, query.QueryString);
        }

        [TestMethod]
        public void QueryStringReturnsCorrectQueryWithMoreParameters()
        {
            // User characters that are not allowed in the string to test if they are encoded properly
            string testKey1 = "testKey &?1";
            string testValue1 = "testValue &?1";
            string encodedParameterKeyValue1 = "testKey%20%26%3F1=testValue%20%26%3F1";
            string testKey2 = "testKey &?2";
            string testValue2 = "testValue &?2";
            string encodedParameterKeyValue2 = "testKey%20%26%3F2=testValue%20%26%3F2";
            string testKey3 = "testKey &?3";
            string testValue3 = "testValue &?3";
            string encodedParameterKeyValue3 = "testKey%20%26%3F3=testValue%20%26%3F3";

            Query query = new Query();
            query.AddParameter(testKey1, testValue1);
            query.AddParameter(testKey2, testValue2);
            query.AddParameter(testKey3, testValue3);

            // We don't know the order of the params in the string, so we must validate it manually as we can't just compare the string.
            string queryString = query.QueryString;

            // Check if there are exactly 2 occurrences of '&'
            Assert.IsTrue((queryString.Length - queryString.Replace("&", String.Empty).Length) == 2);

            // Extract individual parameters as list
            var parameters = new List<string>(queryString.Split('&'));

            // Check if there are exactly 3 parameters
            Assert.IsTrue(parameters.Count == 3);

            // Check if all parameters are present
            Assert.IsTrue(parameters.Contains(encodedParameterKeyValue1) && parameters.Contains(encodedParameterKeyValue2) && parameters.Contains(encodedParameterKeyValue3));
        }

        [TestMethod]
        public void ParseMethodReturnsCorrectQueryInstanceWithValidQueryStringWithNoParameters()
        {
            // Query to parse with url encoded characters
            string testQueryString = String.Empty;

            Query query = Query.Parse(testQueryString);

            // Check if there are no parameters in the returned query object
            Assert.IsTrue(query.Parameters.Count == 0);
        }

        [TestMethod]
        public void ParseMethodReturnsCorrectQueryInstanceWithValidQueryStringWith1Parameter()
        {
            // User characters that are not allowed in the string to test if they are decoded properly
            string testKey1 = "testKey &?1";
            string testValue1 = "testValue &?1";

            // Query to parse with url encoded characters
            string testQueryString = "testKey%20%26%3F1=testValue%20%26%3F1";

            Query query = Query.Parse(testQueryString);

            // Check if there are no parameters in the returned query object
            Assert.AreEqual(testValue1, query.GetParameter(testKey1));
        }

        [TestMethod]
        public void ParseMethodReturnsCorrectQueryInstanceWithValidQueryStringWithMoreParameters()
        {
            // User characters that are not allowed in the string to test if they are decoded properly
            string testKey1 = "testKey &?1";
            string testValue1 = "testValue &?1";
            string testKey2 = "testKey &?2";
            string testValue2 = "testValue &?2";
            string testKey3 = "testKey &?3";
            string testValue3 = "testValue &?3";

            // Query to parse with url encoded characters
            string testQueryString = "testKey%20%26%3F1=testValue%20%26%3F1&testKey%20%26%3F2=testValue%20%26%3F2&testKey%20%26%3F3=testValue%20%26%3F3";

            Query query = Query.Parse(testQueryString);

            Assert.AreEqual(testValue1, query.GetParameter(testKey1));
            Assert.AreEqual(testValue2, query.GetParameter(testKey2));
            Assert.AreEqual(testValue3, query.GetParameter(testKey3));
        }

        [TestMethod]
        public void ParseMethodReturnsCorrectQueryInstanceWithValidQueryStringWithPrecedingQuestionMark()
        {
            // User characters that are not allowed in the string to test if they are decoded properly
            string testKey1 = "testKey &?1";
            string testValue1 = "testValue &?1";

            // Query to parse with url encoded characters
            string testQueryString = "?testKey%20%26%3F1=testValue%20%26%3F1";

            Query query = Query.Parse(testQueryString);

            // Check if there are no parameters in the returned query object
            Assert.AreEqual(testValue1, query.GetParameter(testKey1));
        }

        [TestMethod]
        public void ParseMethodReturnsCorrectQueryInstanceWithQueryStringWithTooManyAmpersants()
        {
            // User characters that are not allowed in the string to test if they are decoded properly
            string testKey1 = "testKey &?1";
            string testValue1 = "testValue &?1";
            string testKey2 = "testKey &?2";
            string testValue2 = "testValue &?2";
            string testKey3 = "testKey &?3";
            string testValue3 = "testValue &?3";

            // Query to parse with url encoded characters
            string testQueryString = "testKey%20%26%3F1=testValue%20%26%3F1&&&testKey%20%26%3F2=testValue%20%26%3F2&testKey%20%26%3F3=testValue%20%26%3F3";

            Query query = Query.Parse(testQueryString);

            Assert.AreEqual(testValue1, query.GetParameter(testKey1));
            Assert.AreEqual(testValue2, query.GetParameter(testKey2));
            Assert.AreEqual(testValue3, query.GetParameter(testKey3));
        }

        [TestMethod]
        [ExpectedException(typeof(FormatException))]
        public void ParseMethodThrowsFormatExceptionWithWithQueryStringWithParameterWithoutKey()
        {
            // Query to parse with url encoded characters
            string testQueryString = "=testValue%20%26%3F1";

            Query query = Query.Parse(testQueryString);
        }

        [TestMethod]
        [ExpectedException(typeof(FormatException))]
        public void ParseMethodThrowsFormatExceptionWithWithQueryStringWithParameterWithoutValue()
        {
            // Query to parse with url encoded characters
            string testQueryString = "testKey%20%26%3F1=";

            Query query = Query.Parse(testQueryString);
        }
    }
}

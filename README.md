# Http Queries
A .NET PCL to help construct and parse http query strings.

# Documentation
## Stannieman.HttpQueries ##
# T:Stannieman.HttpQueries.Query
Class representing a http query.
---
##### M:Stannieman.HttpQueries.Query.#ctor
Default constructor.
 
---
##### M:Stannieman.HttpQueries.Query.#ctor(System.String,System.Object)
Constructor that takes 1 parameter.

---
##### P:Stannieman.HttpQueries.Query.QueryString
Returns a valid url encoded parameter string.

---
##### P:Stannieman.HttpQueries.Query.Parameters
Returns the dictionary that contains all the parameters.

---
##### M:Stannieman.HttpQueries.Query.AddParameter(System.String,System.Object)
Adds a parameter to the query. If a parameter with the given key already exists it's value is replaced.

|Name | Description |
|-----|------|
|key: |Key of new parameter.|
|value: |Value of new parameter.|

---
##### M:Stannieman.HttpQueries.Query.ContainsKey(System.String)
Returns whether the query contains a parameter with the given key.

|Name | Description |
|-----|------|
|key: |Key to check for.|
Returns: Whether the query contains the parameter.

---
##### M:Stannieman.HttpQueries.Query.ContainsValue(System.Object)
Returns whether the query contains a parameter with the given value.

|Name | Description |
|-----|------|
|value: |Value to check for.|
Returns: Whether the query contains the parameter.

---
##### M:Stannieman.HttpQueries.Query.RemoveParameter(System.String)
Removes a parameter with the given key from the query.

|Name | Description |
|-----|------|
|key: |Key of the parameter to remove.|

---
##### M:Stannieman.HttpQueries.Query.GetParameter(System.String)
Returns the value of the parameter who's key is given.

|Name | Description |
|-----|------|
|key: |Key of parameter to return value for.|
Returns: Value of parameter.

---
##### M:Stannieman.HttpQueries.Query.Parse(System.String)
Creates a query instance that contains the parameters of a given query string.

|Name | Description |
|-----|------|
|queryString: |Query string to parse.|
Returns: The query instance created from the given query string.

---
Pokemon Management API

This Pokemon Management API is a REST API for managing Pokemon, Owners, and their Reviews. Pokemon have several components such as their Category/Type, a relationship to Owners, and Reviews of them. Owners own pokemon and are also linked to a specific Country. Reviewers can conduct reviews on pokemon.

Dotnet core and other important dependencies like Entity Framework were used to construct this API. SQLServer is used for the database, managed with the help of Sql Server Management Studio (SSMS).

---

## Backend Documentation - API Endpoints

### `GET /api/Category`
Retrieves a list of all categories present in the database.

#### Response
``` javascript
['CategoryDto']
```

### `GET /api/Category/{categoryId}`
Retrieves a specific category based on Id. Responds with Bad Request if no id is found.

#### Response
``` javascript
'CategoryDto'
```

### `GET /api/Category/pokemon/{categoryId}`
Retrieves all pokemon linked to a specific category. Responds with Bad Request if no id is found.

#### Response
``` javascript
'[PokemonDto]'
```

### `POST api/Category`
Adds a new category to the database.

#### Request
``` javascript
'CategoryDto'
```

### `PUT /api/Category/{categoryId}`
Updates a specific category based on Id. Responds with Bad Request if no id is found.

#### Request
``` javascript
'CategoryDto'
```

### `DELETE /api/Category/{categoryId}`
Deletes a specific category based on Id. Responds with Bad Request if no id is found.

#### Request
``` javascript
'categoryId'
```



### `GET /api/Country`
Retrieves a list of all countries present in the database.

#### Response
``` javascript
['CountryDto']
```

### `GET /api/Country/{countryId}`
Retrieves a specific country based on Id. Responds with Bad Request if no id is found.

#### Response
``` javascript
'CountryDto'
```

### `GET /api/Country/owners/{ownerId}`
Retrieves the country of a specified owner. Responds with Bad Request if no id is found.

#### Response
``` javascript
'CountryDto'
```

### `POST api/Country`
Adds a new country to the database.

#### Request
``` javascript
'CountryDto'
```

### `PUT /api/Country/{countryId}`
Updates a specific country based on Id. Responds with Bad Request if no id is found.

#### Request
``` javascript
'CountryDto'
```

### `DELETE /api/Country/{countryId}`
Deletes a specific country based on Id. Responds with Bad Request if no id is found.

#### Request
``` javascript
'countryId'
```



### `GET /api/Owner`
Retrieves a list of all owners present in the database.

#### Response
``` javascript
['OwnerDto']
```

### `GET /api/Owner/{ownerId}`
Retrieves a specific owner based on Id. Responds with Bad Request if no id is found.

#### Response
``` javascript
'OwnerDto'
```

### `GET /api/Owner/{ownerId}/pokemon`
Retrieves all pokemon owned by a specific owner. Responds with Bad Request if no id is found.

#### Response
``` javascript
'[PokemonDto]'
```

### `POST api/Owner`
Adds a new owner to the database.

#### Request
``` javascript
'OwnerDto'
```

### `PUT /api/Owner/{ownerId}`
Updates a specific owner based on Id. Responds with Bad Request if no id is found.

#### Request
``` javascript
'OwnerDto'
```

### `DELETE /api/Owner/{ownerId}`
Deletes a specific owner based on Id. Responds with Bad Request if no id is found.

#### Request
``` javascript
'ownerId'
```



### `GET /api/Pokemon`
Retrieves a list of all pokemon present in the database.

#### Response
``` javascript
['PokemonDto']
```

### `GET /api/Pokemon/{pokeId}`
Retrieves a specific pokemon based on Id. Responds with Bad Request if no id is found.

#### Response
``` javascript
'PokemonDto'
```

### `GET /api/Pokemon/{pokeId}/rating`
Retrieves the average rating of a pokemon based on all of its reviews. Responds with Bad Request if no id is found.

#### Response
``` javascript
'rating (1-5)'
```

### `POST api/Pokemon`
Adds a new Pokemon to the database.

#### Request
``` javascript
'PokemonDto'
```

### `PUT /api/Pokemon/{pokeId}`
Updates a specific pokemon based on Id. Responds with Bad Request if no id is found.

#### Request
``` javascript
'PokemonDto'
```

### `DELETE /api/Pokemon/{pokemonId}`
Deletes a specific pokemon based on Id. Responds with Bad Request if no id is found.

#### Request
``` javascript
'pokemonId'
```



### `GET /api/Review`
Retrieves a list of all reviews present in the database.

#### Response
``` javascript
['ReviewDto']
```

### `GET /api/Review/{reviewId}`
Retrieves a specific review based on Id. Responds with Bad Request if no id is found.

#### Response
``` javascript
'ReviewDto'
```

### `GET /api/Review/pokemon/{pokeId}`
Retrieves all reviews of a specific pokemon. Responds with Bad Request if no id is found.

#### Response
``` javascript
'[ReviewDto]'
```

### `POST api/Review`
Adds a new review to the database.

#### Request
``` javascript
'ReviewDto'
```

### `PUT /api/Review/{reviewId}`
Updates a specific review based on Id. Responds with Bad Request if no id is found.

#### Request
``` javascript
'OwnerDto'
```

### `DELETE /api/Review/{reviewId}`
Deletes a specific review based on Id. Responds with Bad Request if no id is found.

#### Request
``` javascript
'reviewId'
```



### `GET /api/Reviewer`
Retrieves a list of all reviewers present in the database.

#### Response
``` javascript
['ReviewerDto']
```

### `GET /api/Reviewer/{reviewerId}`
Retrieves a specific reviewer based on Id. Responds with Bad Request if no id is found.

#### Response
``` javascript
'ReviewerDto'
```

### `GET /api/Review/{reviewerId}/reviews`
Retrieves all reviews made by a specific reviewer. Responds with Bad Request if no id is found.

#### Response
``` javascript
'[ReviewDto]'
```

### `POST api/Reviewer`
Adds a new reviewer to the database.

#### Request
``` javascript
'ReviewerDto'
```

### `PUT /api/Reviewer/{reviewerId}`
Updates a specific reviewer based on Id. Responds with Bad Request if no id is found.

#### Request
``` javascript
'OwnerDto'
```

### `DELETE /api/Reviewer/{reviewerId}`
Deletes a specific reviewer based on Id. Responds with Bad Request if no id is found.

#### Request
``` javascript
'reviewerId'
```

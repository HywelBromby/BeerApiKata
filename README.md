# BeerApiKata
A Basic Kata For Creating a .net API.

Two separate  approaches have been used:
1. A bare minimum all in the controller approach.
2. A fully abstracted approach.

The persistence is done in a simple in memory object - obviously this only persists the lifetime the service is running - it has been abstracted away behind a generic repository interface so can easily be swapped out for a different persistence layer.

Rather than go for a relational DB, or an event sourced system, I decided to use a simple flat file approach.

The majority of the testing is functional testing, in the abstracted version this would allow the whole implementation to be replaced and still be covered, the non abstracted one is a little more brittle. only the validators have any unit testing around them

Exception handling is done in the middleware

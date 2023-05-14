# BeerApiKata
A Basic Kata For Creating a .net API.

2 seperate approaches have been used:
1. a bare minimum all in the controller approach
2. a full abstracted approach

The persistance is doen in a simple in memory object - obvioulsy this only persists the lifetime the service is running - it has been abstracted away behind a generic repoository interface so can easily be swapped out for a differnt persistance layer.

Rather than go for a relational DB approach, or an event sourced system, I decided to use a simple flat file aproach.

The majority of the testing is functional testing, in the abstracted version this would allow the whole implmentation to be replaced and still be covered, the non abstracted one is a little more brittle.


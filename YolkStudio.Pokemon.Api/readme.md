## Implementation notes
- Sorting is supported only by a single property.
- Links could also be added to Paged response.
- When a client sorts by unsupported property, a different type of response is returned, which is by decision.
- AsNoTracking could be added to basically every query. I would not bother with it unless faced with some performance issues during benchmark.
- The fuzzy/substring search is done in memory. This works but does not scale for a large dataset. Better to do it on a DB level or leverage ElasticSearch.

## Design choices
The solution has three logical layers and is an implementation of a "pragmatic clean architecture."
For an app this size, it still provides a somewhat clean separation of concerns, while not adding a lot of unnecessary complexity.

The code itself is organized in vertical slices, as I find this approach more readable and logical.
## Changes to the model
- Changed Trainer's property Age to Birthdate.
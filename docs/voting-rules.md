# Voting Domain Model and Business Rules

## Domain Models

### VotingItem

A voteable item that users can select when casting their vote.

| Field       | Type     | Required | Description                              |
|-------------|----------|----------|------------------------------------------|
| Id          | int      | Yes      | Unique identifier for the item           |
| Name        | string   | Yes      | Display name shown to voters             |
| Description | string?  | No       | Optional context about the item          |

### VoteResult

Aggregated vote total for a single voting item.

| Field     | Type       | Required | Description                              |
|-----------|------------|----------|------------------------------------------|
| Item      | VotingItem | Yes      | The voting item this result belongs to   |
| VoteCount | int        | Yes      | Total number of votes cast for this item |

## Business Rules

1. **Multiple-choice voting** — Users may vote for more than one item. Each item is an independent choice, so a user can cast one vote per item rather than one vote across the entire poll.

2. **One vote per user per item** (enforced in the authentication phase) — Once authentication is added, each authenticated user may vote at most once for any given item. Before authentication is implemented, the MVP does not restrict repeat votes.

3. **Anonymous voting in MVP** — The MVP allows anyone to vote without signing in. Vote restriction enforcement is deferred to the authentication phase.

4. **Vote totals only** — The MVP surfaces aggregate vote counts, not a list of individual votes. The `VoteResult` model carries the total count per item.

5. **Static seed data** — Voting items are predefined seed data in the MVP. There is no admin interface for adding or removing items.

6. **In-memory storage first** — The initial implementation uses in-memory storage behind the `IVoteService` interface so the model can be validated quickly before adding database persistence.

## Service Interface

The `IVoteService` interface defines the contract for all voting operations:

- `GetItemsAsync()` — List all voting items.
- `VoteAsync(itemId)` — Record a vote for an item. Returns false if the item id is invalid.
- `GetResultsAsync()` — Return aggregated vote results for all items.

Implementations can swap between in-memory, SQLite/EF Core, or any future storage without changing the UI or page handlers.

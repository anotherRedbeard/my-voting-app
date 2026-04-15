# Dotnet Voting App Implementation Plan

## Overview

Build a small ASP.NET Core Razor Pages voting app in phases so the first release delivers the core user flow quickly: view items, cast a vote, then see current totals. Start with a single web app and in-memory storage to keep the MVP small, then add SQLite and EF Core persistence, authentication, and optional real-time updates as follow-on issues.

This document is the repo-tracked version of the implementation plan and should stay aligned with the GitHub issue roadmap for `anotherRedbeard/my-voting-app`.

## Decisions

- Use ASP.NET Core Razor Pages for the initial app because it is the lowest-friction .NET web option for this scope.
- Treat database persistence as a post-MVP feature; use an in-memory implementation first to keep the first release narrow.
- Treat authentication as a post-MVP feature.
- Prefer SQLite plus EF Core for persistence rather than Firebase, because it fits the .NET stack cleanly and lowers operational complexity. Firebase remains optional only if cross-platform real-time sync becomes a hard requirement.
- Recommended vote restriction for the auth phase: one vote per user per item. If the product instead requires only one total vote across the whole poll, that rule should be fixed before implementation.

## Phased Plan

### Phase 1: MVP

1. ~~Repository and solution setup. Create a .NET solution with one ASP.NET Core Razor Pages app and one test project. Add baseline documentation, `.gitignore`, and configuration structure for feature flags. This blocks all later work.~~ ✅ Done
2. Define the core domain model. Introduce voting item and vote concepts, decide item fields, and document the vote lifecycle. Keep the initial model minimal: item id, display text, optional description, and aggregated vote count or vote records depending on storage design. This depends on step 1.
3. Implement an in-memory vote service or repository for the MVP. It should support listing voting items, submitting a vote for a selected item, and returning updated totals. This depends on step 2.
4. Build the Razor Pages UI for the MVP flow. The home page should list all voteable items, render one vote button per item, submit a vote, and then show current totals immediately after voting. This depends on step 3 and can proceed in parallel with step 5 once the contract is agreed.
5. Add HTTP endpoints or page handlers for listing items and recording votes. Keep the boundary thin and reuse the vote service so the app can later swap in a database-backed implementation. This depends on step 3.
6. Add MVP tests. Cover vote counting rules, invalid item handling, and the post-vote result flow. This depends on steps 3 through 5.

### Phase 2: Persistence

7. Add persistence with SQLite and Entity Framework Core. Introduce `DbContext`, entity mappings, migrations, and a seed path for initial voting items. Swap the in-memory repository behind the existing interface so the UI and handlers stay stable. This depends on Phase 1.
8. Add persistence-focused integration tests. Validate migrations, item retrieval, vote writes, and result reads against a test database or EF-backed test configuration. This depends on step 7.

### Phase 3: Authentication

9. Add authentication and vote restrictions. Introduce ASP.NET Core Identity or another built-in ASP.NET auth approach, require sign-in for vote submission, and enforce the chosen rule for whether a user may vote once total or once per item. This depends on Phase 2 if vote history must persist per user; otherwise it can begin after Phase 1, but persistence is recommended first.
10. Update the UI and application flow for authenticated voting. Add login and register pages or external auth, show sign-in state, and surface clear messaging when a user cannot vote again. This depends on step 9.

### Phase 4: Optional Polish

11. Optional real-time updates and polish. If live vote totals are desired, add SignalR for server-pushed updates rather than introducing Firebase into the .NET app by default. Also add responsive styling, admin seeding or management improvements, and deployment and CI documentation. This depends on earlier phases.

## Relevant Files

- `/src/MyVotingApp.Web/Program.cs` for service registration, persistence, auth, and routing.
- `/src/MyVotingApp.Web/Pages/Index.cshtml` for the main voting UI.
- `/src/MyVotingApp.Web/Pages/Index.cshtml.cs` for loading items and handling vote submissions.
- `/src/MyVotingApp.Web/Models/VotingItem.cs` for the core item model.
- `/src/MyVotingApp.Web/Models/Vote.cs` for persisted vote records once database and auth phases are added.
- `/src/MyVotingApp.Web/Services/IVoteService.cs` for the voting abstraction.
- `/src/MyVotingApp.Web/Services/InMemoryVoteService.cs` for the MVP implementation.
- `/src/MyVotingApp.Web/Data/AppDbContext.cs` for EF Core persistence.
- `/src/MyVotingApp.Tests/` for unit and integration tests.
- `/README.md` for setup, architecture summary, and roadmap.

## Verification

1. Confirm the MVP user story manually: the home page lists items, each item has a vote button, and after voting the page shows current totals.
2. Run automated tests for the vote service and page handlers using `dotnet test`.
3. Verify invalid item ids and repeat submissions follow the intended rule and produce clear responses.
4. After adding persistence, run EF migrations locally and confirm votes survive application restarts.
5. After adding authentication, verify anonymous users cannot vote and authenticated users follow the configured vote limit rule.
6. If real-time updates are added, open two browser sessions and verify totals update across both without page refresh.

## Further Considerations

1. Decide whether users may vote on multiple different items or only once total across the whole poll; the current recommendation is one vote per item because the prompt says users can give multiple choices.
2. Decide whether voting items are static seed data or managed in-app by an admin. Seeding is sufficient for MVP.
3. Decide whether "see all the votes" means only aggregate totals or a detailed list of individual votes. The recommended MVP is aggregate totals only.

## GitHub Issue Roadmap

Follow these issues in numeric order. GitHub will usually show them newest first, but the intended execution order is ascending by issue number.

1. [#1](https://github.com/anotherRedbeard/my-voting-app/issues/1) Set up the .NET solution and baseline project structure
2. [#2](https://github.com/anotherRedbeard/my-voting-app/issues/2) Define the voting domain model and business rules
3. [#3](https://github.com/anotherRedbeard/my-voting-app/issues/3) Implement the in-memory vote service for the MVP
4. [#4](https://github.com/anotherRedbeard/my-voting-app/issues/4) Build the Razor Pages voting UI
5. [#5](https://github.com/anotherRedbeard/my-voting-app/issues/5) Implement vote submission and post-vote result display
6. [#6](https://github.com/anotherRedbeard/my-voting-app/issues/6) Add MVP automated tests
7. [#7](https://github.com/anotherRedbeard/my-voting-app/issues/7) Add SQLite and EF Core persistence
8. [#8](https://github.com/anotherRedbeard/my-voting-app/issues/8) Add seed data and database migrations
9. [#9](https://github.com/anotherRedbeard/my-voting-app/issues/9) Add persistence integration tests
10. [#10](https://github.com/anotherRedbeard/my-voting-app/issues/10) Add authentication for voting
11. [#11](https://github.com/anotherRedbeard/my-voting-app/issues/11) Enforce one vote per user per item
12. [#12](https://github.com/anotherRedbeard/my-voting-app/issues/12) Update the UI for authenticated voting states
13. [#13](https://github.com/anotherRedbeard/my-voting-app/issues/13) Add optional real-time vote updates
14. [#14](https://github.com/anotherRedbeard/my-voting-app/issues/14) Add deployment and project documentation

## Maintenance Notes

- Update this document when issue titles, sequencing, or scope changes.
- Keep the GitHub issue roadmap section aligned with the open issues in the repository.
- If a new issue is added between phases, insert it here in the correct execution order instead of appending blindly.
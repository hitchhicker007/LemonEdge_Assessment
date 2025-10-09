# Contributing to LemonEdge Assessment

Thanks for your interest in contributing! This guide explains how to propose changes and get them merged smoothly.

## Getting Started

1. Fork the repository
2. Clone your fork and set up the upstream remote
3. Create a feature branch from `master`

```bash
git checkout -b feature/your-change
```

## Development

- Project: C# console app targeting .NET 8.0
- Entry: `LemonEdgeAssessment/Program.cs`
- Build: `dotnet build`
- Run: `dotnet run --project LemonEdgeAssessment`

### CLI

Use `--` to pass arguments to the app:

```bash
# single length
dotnet run --project LemonEdgeAssessment -- -l 5
# range
dotnet run --project LemonEdgeAssessment -- -r 3-7
# choose algorithm (dp|dfs)
dotnet run --project LemonEdgeAssessment -- -l 6 -a dp
```

## Coding Guidelines

- Prefer clear, descriptive names over abbreviations
- Avoid unnecessary try/catch; use guard clauses
- Keep changes focused; avoid unrelated refactors
- Match existing formatting and indentation

## Commit Messages

- Use imperative mood: "Add DP counting", "Fix CLI parsing"
- Reference issues where applicable: `Fixes #12`

## Pull Requests

- Describe the motivation and approach
- Include before/after behavior if user-visible
- Update `README.md` for new features or flags
- Ensure the project builds and runs without warnings

## Areas to Contribute

- Additional chess piece movement modes (knight, bishop, king)
- Unit tests and benchmarking
- Export counts/results to file formats (CSV/JSON)
- Performance improvements (memoization, parallelism)
- CI setup (GitHub Actions) for build and style checks

## License

By contributing, you agree that your contributions will be licensed as part of this repository under its existing license terms.



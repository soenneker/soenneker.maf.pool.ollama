[![](https://img.shields.io/nuget/v/soenneker.maf.pool.ollama.svg?style=for-the-badge)](https://www.nuget.org/packages/soenneker.maf.pool.ollama/)
[![](https://img.shields.io/github/actions/workflow/status/soenneker/soenneker.maf.pool.ollama/publish-package.yml?style=for-the-badge)](https://github.com/soenneker/soenneker.maf.pool.ollama/actions/workflows/publish-package.yml)
[![](https://img.shields.io/nuget/dt/soenneker.maf.pool.ollama.svg?style=for-the-badge)](https://www.nuget.org/packages/soenneker.maf.pool.ollama/)
[![](https://img.shields.io/github/actions/workflow/status/soenneker/soenneker.maf.pool.ollama/codeql.yml?label=CodeQL&style=for-the-badge)](https://github.com/soenneker/soenneker.maf.pool.ollama/actions/workflows/codeql.yml)

# Soenneker.Maf.Pool.Ollama

Provides Ollama-specific registration extensions for `IMafPool`, enabling integration with local LLMs via Microsoft Agent Framework.

## Install

```bash
dotnet add package Soenneker.Maf.Pool.Ollama
```

## Quick start

```csharp
using Soenneker.Maf.Pool.Ollama;

IMafPool pool = /* obtain from your application */;
await pool.AddOllama("value", "value", "value", "value", default);
```

Registers an Ollama model in the agent pool with optional rate/token limits.

## What you get

- `MafPoolOllamaExtension` — Provides Ollama-specific registration extensions for `IMafPool`, enabling integration with local LLMs via Microsoft Agent Framework.

## API at a glance

| API | What it does | Result / important behavior |
| --- | --- | --- |
| `MafPoolOllamaExtension.AddOllama(pool, poolId, key, modelId, endpoint, rps, rpm, rpd, tokensPerDay, instructions, cancellationToken)` | Registers an Ollama model in the agent pool with optional rate/token limits. | A task that completes when the ollama addition is complete. |
| `MafPoolOllamaExtension.RemoveOllama(pool, poolId, key, cancellationToken)` | Unregisters an Ollama model from the agent pool and removes the associated cache entry. | True if the entry existed and was removed; false if it was not present. |

## Practical notes

- Cancellation stops pending work; it does not undo work that has already completed.

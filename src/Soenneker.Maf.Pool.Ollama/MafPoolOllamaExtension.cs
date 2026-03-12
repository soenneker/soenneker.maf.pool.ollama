using Microsoft.Agents.AI;
using Microsoft.Extensions.AI;
using Soenneker.Maf.Dtos.Options;
using Soenneker.Maf.Pool.Abstract;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Soenneker.Maf.Pool.Ollama;

/// <summary>
/// Provides Ollama-specific registration extensions for <see cref="IMafPool"/>, enabling integration with local LLMs via Microsoft Agent Framework.
/// </summary>
public static class MafPoolOllamaExtension
{
    /// <summary>
    /// Registers an Ollama model in the agent pool with optional rate/token limits.
    /// </summary>
    /// <param name="pool">The MAF pool to register with.</param>
    /// <param name="poolId">Identifier for the sub-pool.</param>
    /// <param name="key">Unique key for this agent entry.</param>
    /// <param name="modelId">Ollama model id (e.g. llama3.2).</param>
    /// <param name="endpoint">Ollama endpoint (e.g. http://localhost:11434).</param>
    /// <param name="rps">Optional requests per second limit.</param>
    /// <param name="rpm">Optional requests per minute limit.</param>
    /// <param name="rpd">Optional requests per day limit.</param>
    /// <param name="tokensPerDay">Optional tokens per day limit.</param>
    /// <param name="instructions">Optional system instructions for the agent.</param>
    /// <param name="cancellationToken">Token to cancel the operation.</param>
    public static ValueTask AddOllama(this IMafPool pool, string poolId, string key, string modelId, string endpoint,
        int? rps = null, int? rpm = null, int? rpd = null, int? tokensPerDay = null, string? instructions = null,
        CancellationToken cancellationToken = default)
    {
        var options = new MafOptions
        {
            ModelId = modelId,
            Endpoint = endpoint,
            RequestsPerSecond = rps,
            RequestsPerMinute = rpm,
            RequestsPerDay = rpd,
            TokensPerDay = tokensPerDay,
            AgentFactory = (opts, _) =>
            {
                Uri uri = string.IsNullOrEmpty(opts.Endpoint)
                    ? new Uri("http://localhost:11434", UriKind.Absolute)
                    : new Uri(opts.Endpoint!, UriKind.Absolute);
                var chatClient = new OllamaChatClient(uri, opts.ModelId!);
                AIAgent agent = chatClient.AsAIAgent(instructions: instructions ?? "You are a helpful assistant running locally via Ollama.");
                return new ValueTask<AIAgent>(agent);
            }
        };

        return pool.Add(poolId, key, options, cancellationToken);
    }

    /// <summary>
    /// Unregisters an Ollama model from the agent pool and removes the associated cache entry.
    /// </summary>
    /// <returns>True if the entry existed and was removed; false if it was not present.</returns>
    public static ValueTask<bool> RemoveOllama(this IMafPool pool, string poolId, string key, CancellationToken cancellationToken = default)
    {
        return pool.Remove(poolId, key, cancellationToken);
    }
}

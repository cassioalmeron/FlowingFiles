using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Pgvector;

namespace FlowingFiles.Core.Configurations;

// pgvector's `Vector` type is only ever seen here, at the Postgres mapping boundary — the domain
// model keeps `Embedding` as a plain `float[]` on every provider (Design Decision 3 of plan 003).
internal class NpgsqlEmbeddingColumnStrategy : IEmbeddingColumnStrategy
{
    public void Configure(PropertyBuilder<float[]> embedding, ValueComparer<float[]> comparer) =>
        embedding.HasColumnType("vector(1024)").HasConversion(
            new ValueConverter<float[], Vector>(v => new Vector(v), v => v.ToArray()), comparer);
}

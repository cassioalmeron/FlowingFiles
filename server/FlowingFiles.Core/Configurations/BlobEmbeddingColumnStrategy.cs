using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace FlowingFiles.Core.Configurations;

// No pgvector equivalent outside Postgres (Design Decision 3 of plan 003) — used for both real
// SQLite and the EF Core InMemory provider FlowingFiles.Tests runs against, so it is named after
// the storage shape (BLOB / byte[]) rather than a specific provider.
internal class BlobEmbeddingColumnStrategy : IEmbeddingColumnStrategy
{
    public void Configure(PropertyBuilder<float[]> embedding, ValueComparer<float[]> comparer) =>
        embedding.HasConversion(
            new ValueConverter<float[], byte[]>(v => FloatsToBytes(v), v => BytesToFloats(v)), comparer);

    private static byte[] FloatsToBytes(float[] floats)
    {
        var bytes = new byte[floats.Length * sizeof(float)];
        Buffer.BlockCopy(floats, 0, bytes, 0, bytes.Length);
        return bytes;
    }

    private static float[] BytesToFloats(byte[] bytes)
    {
        var floats = new float[bytes.Length / sizeof(float)];
        Buffer.BlockCopy(bytes, 0, floats, 0, bytes.Length);
        return floats;
    }
}

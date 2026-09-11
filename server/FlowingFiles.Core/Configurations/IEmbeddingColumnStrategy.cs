using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FlowingFiles.Core.Configurations;

internal interface IEmbeddingColumnStrategy
{
    void Configure(PropertyBuilder<float[]> embedding, ValueComparer<float[]> comparer);
}

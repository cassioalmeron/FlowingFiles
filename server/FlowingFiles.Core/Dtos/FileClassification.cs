namespace FlowingFiles.Core.Dtos;

public record FileClassification(string Label, ClassificationMethod Method, IReadOnlyList<NeighbourScore>? Neighbours);

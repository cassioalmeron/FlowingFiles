namespace FlowingFiles.Core.Dtos;

public record ClassificationResult(string Label, double Confidence, IReadOnlyList<NeighbourScore> Neighbours);

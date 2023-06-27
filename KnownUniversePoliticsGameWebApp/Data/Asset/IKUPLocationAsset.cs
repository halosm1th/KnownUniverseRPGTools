namespace KnownUniversePoliticsGameWebApp.Data;

public interface IKUPLocationAsset : IKUPAsset
{
    KUPPointsOfInterest POI { get; }
}
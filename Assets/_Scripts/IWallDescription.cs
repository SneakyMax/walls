using System.Collections.Generic;
using Assets._Scripts.Helpers;

namespace Assets._Scripts
{
    public interface IWallDescription
    {
        IEnumerable<MeshInfo> GenerateSolidMeshes(float thickness);

        string DescriptionName { get; }
    }
}
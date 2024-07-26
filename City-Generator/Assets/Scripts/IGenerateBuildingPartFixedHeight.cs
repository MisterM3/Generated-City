using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IGenerateBuildingPartFixedHeight
{
    float BuildingHeight { get; }
    public virtual GameObject GenerateBuildingPart() => throw new System.NotImplementedException();
}

// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace VgEAPClient.Common.CNC.ATP.Models;

[Serializable]
public class MagazineDTO
{
    public int magazineId { get; set; }
    public int toolId { get; set; }
    public float toolDiameter { get; set; }
    public string toolState { get; set; }//used new expired
    public int toolLife { get; set; }//N
    public int toolUseLife { get; set; }//B

    public MagazineDTO()
    {
    }

    public void ClearMagazData()
    {
        magazineId = 0;
        toolId = 0;
        toolDiameter = 0;
        toolState = "";
        toolLife = 0;
        toolUseLife = 0;
    }

    public MagazineDTO Clone()
    {
        MagazineDTO magazine = new MagazineDTO();
        magazine.magazineId = this.magazineId;
        magazine.toolId = this.toolId;
        magazine.toolDiameter = this.toolDiameter;
        magazine.toolState = this.toolState;
        magazine.toolLife = this.toolLife;
        magazine.toolUseLife = this.toolUseLife;
        return magazine;
    }
}

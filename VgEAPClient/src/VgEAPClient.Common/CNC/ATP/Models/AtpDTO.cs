// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

namespace VgEAPClient.Common.CNC.ATP.Models;

public class AtpDTO
{
    public List<ToolToleranceTableDTO> toolToleranceTableDTOs = new List<ToolToleranceTableDTO>();
    public List<PeckDrillingValuesDTO> peckDrillingValuesDTOs = new List<PeckDrillingValuesDTO>();
    public List<ShortSlotNibblingDTO> shortSlotNibblingDTOs = new List<ShortSlotNibblingDTO>();
    public List<LongSlotNibblingDTO> longSlotNibblingDTOs = new List<LongSlotNibblingDTO>();
    public List<MagazineDTO> magazineDTOs = new List<MagazineDTO>();
    public List<ToolDTO> toolDTOs = new List<ToolDTO>();

    public int ToolDTOCount = 999;
    public int MagazineDTOCount = 2000;
    public int LongSlotNibCount = 35;
    public int ShortSlotNibCount = 35;
    public int PeckDrlDTOCount = 999;
    public int ToolTolCount = 101;

    public void InitAtpDTO(int nToolDTOCnt = 999, int nMagDTOCnt = 2000, int nToolTolCnt = 101, int nPeckDrlCnt = 999, int nShortSlotNIbCnt = 35, int nLongSlotNibCnt = 35)
    {
        this.toolDTOs = Enumerable.Range(0, nToolDTOCnt + 1).Select(i => new ToolDTO() { toolId = i }).ToList();
        this.magazineDTOs = Enumerable.Range(0, nMagDTOCnt + 1).Select(i => new MagazineDTO() { magazineId = i }).ToList();
        this.toolToleranceTableDTOs = Enumerable.Range(0, nToolTolCnt + 1).Select(i => new ToolToleranceTableDTO() { tableId = i }).ToList();
        this.peckDrillingValuesDTOs = Enumerable.Range(0, nPeckDrlCnt + 1).Select(i => new PeckDrillingValuesDTO() { toolNumber = i }).ToList();
        this.shortSlotNibblingDTOs = Enumerable.Range(0, nShortSlotNIbCnt + 1).Select(i => new ShortSlotNibblingDTO() { toolId = i }).ToList();
        this.longSlotNibblingDTOs = Enumerable.Range(0, nLongSlotNibCnt + 1).Select(i => new LongSlotNibblingDTO() { toolId = i }).ToList();
        ToolDTOCount = nToolDTOCnt;
        MagazineDTOCount = nMagDTOCnt;
        LongSlotNibCount = nLongSlotNibCnt;
        ShortSlotNibCount = nShortSlotNIbCnt;
        PeckDrlDTOCount = nPeckDrlCnt;
        ToolTolCount = nToolTolCnt;
    }
}

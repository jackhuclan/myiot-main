namespace VgDeviceGateway.Devices.Drill.Other.Dto
{
    public class AtpDTO
    {
        public List<ToolToleranceTableDTO> toolToleranceTableDTOs = new List<ToolToleranceTableDTO>();
        public List<PeckDrillingValuesDTO> peckDrillingValuesDTOs = new List<PeckDrillingValuesDTO>();
        public List<ShortSlotNibblingDTO> shortSlotNibblingDTOs = new List<ShortSlotNibblingDTO>();
        public List<LongSlotNibblingDTO> longSlotNibblingDTOs = new List<LongSlotNibblingDTO>();
        public List<MagazineDTO> magazineDTOs = new List<MagazineDTO>();
        public List<ToolDTO> toolDTOs = new List<ToolDTO>();
    }
}

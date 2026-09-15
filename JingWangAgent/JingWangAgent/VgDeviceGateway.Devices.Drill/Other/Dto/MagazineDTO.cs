namespace VgDeviceGateway.Devices.Drill.Other.Dto
{
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
    }
}

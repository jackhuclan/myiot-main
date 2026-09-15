namespace VgAutoDrill.Admin.Model.ViewModels.Mes.CutterConfigDetail
{
    public class CutterConfigDetailDto : BaseDto
    {
        /// <summary>
        /// 
        /// </summary>
        public virtual int? MasterId { get; set; }

        /// <summary>
        /// D直径MM
        /// </summary>
        public virtual decimal? D { get; set; }
        /// <summary>
        /// S转速KRPM
        /// </summary>
        public virtual decimal? S { get; set; }
        /// <summary>
        /// F(进刀速)M/MIN
        /// </summary>
        public virtual decimal? F { get; set; }
        /// <summary>
        /// R(退刀速)M/MIN
        /// </summary>
        public virtual decimal? R { get; set; }
        /// <summary>
        /// Z(深度补偿)MM
        /// </summary>
        public virtual decimal? Z { get; set; }
        /// <summary>
        /// 寿命
        /// </summary>
        public virtual int? Age { get; set; }
    }
}

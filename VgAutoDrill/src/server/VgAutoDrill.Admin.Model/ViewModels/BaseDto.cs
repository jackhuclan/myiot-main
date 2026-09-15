namespace VgAutoDrill.Admin.Model.ViewModels
{
    public abstract class BaseDto : GenearlBaseDto
    {
        /// <summary>
        /// id
        /// </summary>
        public override sealed long Id { set; get; }

        /// <summary>
        /// 是否已删除 
        /// 默认值: 0
        ///</summary>
        public override sealed bool IsDeleted { get; set; }

        /// <summary>
        /// 状态(1:启用;0禁用) 
        /// 默认值: 1
        ///</summary>
        public override sealed int Status { get; set; } = 1;

        /// <summary>
        /// 创建人Id 
        ///</summary>
        public override sealed int? CreatorId { get; set; }       
        /// <summary>
        /// 创建时间 
        ///</summary>
        public override sealed DateTime CreateTime { get; set; }        
        /// <summary>
        /// 修改时间 
        ///</summary>
        public override sealed DateTime? ModifyTime { get; set; }     
        /// <summary>
        /// 修改人Id 
        ///</summary>
        public override sealed int? ModifierId { get; set; }
    }

    public abstract class GenearlBaseDto
    {
        /// <summary>
        /// id
        /// </summary>
        public virtual long Id { set; get; }

        /// <summary>
        /// 是否已删除 
        /// 默认值: 0
        ///</summary>
        public virtual bool IsDeleted { get; set; }

        /// <summary>
        /// 状态(1:启用;0禁用) 
        /// 默认值: 1
        ///</summary>
        public abstract int Status { get; set; }

        /// <summary>
        /// 创建人Id 
        ///</summary>
        public abstract int? CreatorId { get; set; }
        /// <summary>
        /// 创建时间 
        ///</summary>
        public abstract DateTime CreateTime { get; set; }
        /// <summary>
        /// 修改时间 
        ///</summary>
        public abstract DateTime? ModifyTime { get; set; }
        /// <summary>
        /// 修改人Id 
        ///</summary>
        public abstract int? ModifierId { get; set; }
    }

    public interface IDtoOnlyId
    {
        /// <summary>
        /// id
        /// </summary>
        public long Id { set; get; }
    }
}

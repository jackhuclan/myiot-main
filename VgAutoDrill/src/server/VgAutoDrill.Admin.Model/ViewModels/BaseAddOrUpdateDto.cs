namespace VgAutoDrill.Admin.Model.ViewModels
{
    public abstract class BaseAddOrUpdateDto : GeneralBaseAddOrUpdateDto, IDtoOnlyId
    {
        /// <summary>
        /// id
        /// </summary>
        public override sealed long Id { set; get; }

        public override sealed int Status { get; set; }
    }

    public abstract class GeneralBaseAddOrUpdateDto
    {
        public abstract long Id { set; get; }

        public abstract int Status { get; set; }
    }
}

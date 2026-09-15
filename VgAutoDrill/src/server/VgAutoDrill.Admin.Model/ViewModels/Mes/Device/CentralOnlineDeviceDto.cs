using VgAutoDrill.Admin.Common.Extensions;
using VgAutoDrill.Fundation.Iot;
using VgAutoDrill.Fundation.Iot.Configuration;
using VgAutoDrill.Fundation.Iot.Models;

namespace VgAutoDrill.Admin.Model.ViewModels.Mes.Device
{
    public class CentralOnlineDeviceDto
    {
        /// <summary>
        /// 工艺路线编号
        /// </summary>
        public virtual string? RouteCode { get; set; }

        /// <summary>
        /// 工艺路线名称
        /// </summary>
        public virtual string? RouteName { get; set; }
        public virtual string? ProductId { get; set; }

        public virtual string? DeviceId { get; set; }

        public virtual string? TargetDevice { get; set; }

        public virtual string? Status { get; set; }

        public virtual string? RoutingKey { get; set; }

        public virtual bool? NotActive { get; set; }

        public virtual DateTime? LoginTime { get; set; }
        public virtual DateTime? DeviceStandbyTime { get; set; }
        /// <summary>
        /// 设备状态
        /// </summary>
        public virtual DeviceStatus? DeviceStatus { get; set; }
        /// <summary>
        /// 钻机状态，WORK  STOP  ALAM  WAIT
        /// </summary>
        public virtual string? DrillState { get; set; }


        public virtual DeviceDescriptor? Descriptor { get; set; }

        public virtual Dictionary<string, object>? Properties { get; set; } = new Dictionary<string, object>();

        public virtual Dictionary<string, object?> XianJinParams { get; set; } = new();

        public virtual int Percentage
        {
            get
            {
                return Properties != null && Properties.ContainsKey("Drill_Percentage") ? Properties["Drill_Percentage"].ToInt() : 0;
            }
        }
        public virtual bool ExistRawPanel
        {
            get
            {
                return Properties != null && Properties.ContainsKey("Buffer_RawMaterialLayerBoardStatus") &&
                   Properties["Buffer_RawMaterialLayerBoardStatus"].ToInt() != 0 ? true : false;
            }
        }
        public virtual bool ExistClinkerPanel
        {
            get
            {
                return Properties != null && Properties.ContainsKey("Buffer_ClinkerLayerBoardStatus") &&
                    Properties["Buffer_ClinkerLayerBoardStatus"].ToInt() != 0 ? true : false;
            }
        }

        public virtual bool IsAllVerifyOK
        {
            get
            {
                return XianJinParams != null && XianJinParams.ContainsKey("IsAllVerifyOK") ? XianJinParams["IsAllVerifyOK"].ToBool() : false;
            }
        }

        public virtual bool IsCompleteUpperPanel
        {
            get
            {
                return XianJinParams != null && XianJinParams.ContainsKey("IsCompleteUpperPanel") ? XianJinParams["IsCompleteUpperPanel"].ToBool() : false;
            }
        }

        public virtual List<string> VerifyFailShafts
        {
            get
            {
                try
                {
                    if (XianJinParams != null && XianJinParams.ContainsKey("VerifyFailShafts"))
                    {
                        return System.Text.Json.JsonSerializer.Deserialize<List<string>>(XianJinParams["VerifyFailShafts"].ToString());
                    }
                    else { return new List<string>(); }
                }
                catch
                {
                    return new List<string>();
                }
            }
        }

        public virtual string? UpperPanelProgress
        {
            get
            {
                return XianJinParams != null && XianJinParams.ContainsKey("UpperPanelProgress") ? XianJinParams["UpperPanelProgress"].ToString() : "0";
            }
        }

        public virtual string? LoadingTask
        {
            get
            {
                return XianJinParams != null && XianJinParams.ContainsKey("LoadingTask") ? XianJinParams["LoadingTask"].ToString() : "";
            }
        }

        public virtual List<Fundation.Iot.Models.Panel>? PayloadPanels { get; set; }
        public virtual List<CutterTray>? PayloadCutterTrays { get; set; }
    }
}

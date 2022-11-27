using System.ComponentModel;

namespace CarRepairServiceCode.Helper
{
    public enum ActionName
    {
        [Description("create")]
        CreateEntity,

        [Description("get")]
        ReadEntity,

        [Description("update")]
        UpdateEntity,

        [Description("delete")]
        DeleteEntity
    }
}

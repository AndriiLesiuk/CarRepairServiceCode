using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace CarRepairServiceCode.Helper
{
    public enum DbEntitiesName
    {
        [Description("car")]
        Car,

        [Description("car_order")]
        CarOrder,

        [Description("car_order_details")]
        CarOrderDetails,

        [Description("client")]
        Client,

        [Description("emp_position")]
        EmpPosition,

        [Description("employee")]
        Employee,

        [Description("permissions")]
        Permissions,

        [Description("task_catalog")]
        TaskCatalog
    }
}

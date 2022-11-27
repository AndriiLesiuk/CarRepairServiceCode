namespace CarRepairServiceCode.RequestModels.CarOrderDetail
{
    public class CarOrderDetailRequest
    {
        public int? OrderDetailsId { get; set; }
        public int? TaskId { get; set; }
        public int? EmployeeId { get; set; }
    }
}

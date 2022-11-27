namespace CarRepairServiceCode.RequestModels.TaskCatalog
{
    public class TaskCatalogRequest
    {
        public string TaskName { get; set; }
        public string TaskDescription { get; set; }
        public decimal? TaskPrice { get; set; }
    }
}
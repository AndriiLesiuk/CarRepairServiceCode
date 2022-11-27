namespace CarRepairServiceCode.RequestModels.TaskCatalog
{
    public class TaskCatalogQuery
    {
        public string TaskName { get; set; }
        public string TaskDescription { get; set; }
        public decimal? TaskPrice { get; set; }
    }
}
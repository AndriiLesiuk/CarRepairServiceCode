namespace CarRepairServiceCode.RequestModels.TaskCatalog
{
    public class TaskCatalogView
    {
        public string TaskName { get; set; }
        public string TaskDescription { get; set; }
        public decimal? TaskPrice { get; set; }
    }
}
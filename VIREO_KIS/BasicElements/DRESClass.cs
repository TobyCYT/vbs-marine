using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace VIREO_KIS
{
    public class Competition
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public string Status { get; set; }
        public string TemplateId { get; set; }
        public string TemplateDescription { get; set; }
        public List<string> Teams { get; set; }
        public List<TaskTemplate> TaskTemplates { get; set; }
    }

    public class TaskTemplate
    {
        public string Name { get; set; }
        public string TaskGroup { get; set; }
        public string TaskType { get; set; }
        public long Duration { get; set; }
    }
}

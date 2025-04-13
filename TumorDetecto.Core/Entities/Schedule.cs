using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TumorDetector.Core.Entities
{
    public class Schedule
    {
        [Key]
        public string SessionId { get; set; }

        public string PublishKey { get; set; }

        public string Url { get; set; }
    }
}

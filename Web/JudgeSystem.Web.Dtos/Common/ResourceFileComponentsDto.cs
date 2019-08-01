using System;
using System.Collections.Generic;
using System.Text;

namespace JudgeSystem.Web.Dtos.Common
{
    public class ResourceFileComponentsDto
    {
        public string ContainerName { get; set; }

        public string FileName { get; set; }

        public string FilePath => $"{ContainerName}/{FileName}";
    }
}

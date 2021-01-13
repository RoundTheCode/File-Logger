using System;
using System.Collections.Generic;
using System.Text;

namespace RoundTheCode.FileLogger.Data
{
    public interface IBase
    {
        int Id { get; set; }

        DateTimeOffset Created { get; set; }

        DateTimeOffset? LastUpdated { get; set; }

        DateTimeOffset? Deleted { get; set; }
    }
}

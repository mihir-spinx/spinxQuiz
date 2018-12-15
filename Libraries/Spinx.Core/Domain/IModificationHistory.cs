using System;

namespace Spinx.Core.Domain
{
    public interface IModificationHistory
    {
        DateTime CreatedAt { get; set; }
        DateTime? UpdatedAt { get; set; }
    }
}
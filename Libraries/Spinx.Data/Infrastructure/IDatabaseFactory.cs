using System;

namespace Spinx.Data.Infrastructure
{
    public interface IDatabaseFactory : IDisposable
    {
        SqlContext Get();
    }
}
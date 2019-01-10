using Spinx.Data.Infrastructure;
using Spinx.Domain.GeneralSettings;

namespace Spinx.Data.Repository.GeneralSettings
{
    public interface IGeneralSettingRepository : IRepository<GeneralSetting>
    {
        
    }

    public class GeneralSettingRepository : Repository<GeneralSetting>, IGeneralSettingRepository
    {
        public GeneralSettingRepository(IDatabaseFactory databaseFactory) : base(databaseFactory)
        {
            
        }
    }
}
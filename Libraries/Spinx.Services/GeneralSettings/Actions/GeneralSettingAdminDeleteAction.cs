using Spinx.Core;
using Spinx.Data.Infrastructure;
using Spinx.Services.Content;
using Spinx.Services.Infrastructure;
using System.Collections.Generic;
using System.Linq;
using Spinx.Data.Repository.GeneralSettings;

namespace Spinx.Services.GeneralSettings.Actions
{
    public class GeneralSettingAdminDeleteAction : BaseAction
    {
        private readonly IGeneralSettingRepository _GeneralSettingRepository;
        private readonly IUnitOfWork _unitOfWork;

        public GeneralSettingAdminDeleteAction(
                    IGeneralSettingRepository GeneralSettingRepository,
                    IUnitOfWork unitOfWork)
        {
            _GeneralSettingRepository = GeneralSettingRepository;
            _unitOfWork = unitOfWork;
        }

        public override Result Apply(IEnumerable<int> ids)
        {
            var query = _GeneralSettingRepository.AsNoTracking.Where(q => ids.Contains(q.Id));
            var result = new Result().SetSuccess(string.Format(Messages.RecordDelete, query.Count()));

            foreach (var entity in query)
                _GeneralSettingRepository.Delete(entity);

            _unitOfWork.Commit();
            GeneralSettingCacheManager.ClearCache();

            return result;
        }
    }
}

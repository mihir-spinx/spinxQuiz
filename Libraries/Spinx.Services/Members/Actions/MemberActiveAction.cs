using Spinx.Core;
using Spinx.Data.Infrastructure;
using Spinx.Data.Repository.Member;
using Spinx.Services.Content;
using Spinx.Services.Infrastructure;
using System.Collections.Generic;
using System.Linq;

namespace Spinx.Services.Members.Actions
{
    public class MemberActiveAction : BaseAction
    {
        private readonly IMemberRepository _memberRepository;
        private readonly IUnitOfWork _unitOfWork;

        public MemberActiveAction(
                    IMemberRepository memberRepository, 
                    IUnitOfWork unitOfWork)
        {
            _memberRepository = memberRepository;
            _unitOfWork = unitOfWork;
        }

        public override Result Apply(IEnumerable<int> ids)
        {
            var query = _memberRepository.AsNoTracking.Where(q => ids.Contains(q.Id));
            var result = new Result().SetSuccess(string.Format(Messages.RecordActivate, query.Count()));

            foreach (var entity in query)
            {
                entity.IsActive = true;
                _memberRepository.Update(entity);
            }

            _unitOfWork.Commit();
            MemberCacheManager.ClearCache();

            return result;
        }
    }
}
using Spinx.Domain.Members;
using Spinx.Services.Infrastructure;
using Spinx.Services.Members.DTOs;
using System.Linq;

namespace Spinx.Services.Members.Filters
{
    public class MemberResultListFilter : BaseFilter<MemberResult, MemberResultListDto>
    {

        public MemberResultListFilter(IQueryable<MemberResult> query, MemberResultListDto dto) : base(query, dto)
        {
        }

        internal void Name()
        {
            Query = Query.Where(w => w.Member.Name.Contains(Dto.Name));
        }
        internal void Email()
        {
            Query = Query.Where(w => w.Member.Email.Contains(Dto.Email));
        }
        internal void College()
        {
            Query = Query.Where(w => w.Member.College.Contains(Dto.College));
        }

        internal void QuizTitle()
        {
            Query = Query.Where(w => w.Quiz.Title.Contains(Dto.QuizTitle));
        }

        internal void QuizCategoryName()
        {
            Query = Query.Where(w => w.Quiz.QuizCategory.Name.Contains(Dto.QuizCategoryName));
        }
      

        internal void StartTime()
        {
            Query = Query.Where(w => w.StartTime.Equals(Dto.StartTime));
        }

        internal void EndTime()
        {
            Query = Query.Where(w => w.EndTime.Equals(Dto.EndTime));
        }

        internal void CreatedSource()
        {
            Query = Query.Where(w => w.Member.CreatedSource >= Dto.CreatedSource);
        }

        internal void FromCreatedAt()
        {
            Query = Query.Where(w => w.CreatedAt >= Dto.FromCreatedAt);
        }
        internal void ToCreatedAt()
        {
            Query = Query.Where(w => w.CreatedAt <= Dto.ToCreatedAt);
        }

    }
}
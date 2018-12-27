using Humanizer;
using Spinx.Core;
using Spinx.Core.Extensions;
using Spinx.Data.Infrastructure;
using Spinx.Data.Repository.Member;
using Spinx.Domain.Members;
using Spinx.Services.Members.Actions;
using Spinx.Services.Members.DTOs;
using Spinx.Services.Members.Filters;
using Spinx.Services.Members.ListOrders;
using System;
using System.Data.Entity;
using System.Linq;

namespace Spinx.Services.Members
{
    public interface IMemberResultService
    {
        Result DetailList(MemberResultListDto dto);
    }

    public class MemberResultService : IMemberResultService
    {

        private readonly IMemberResultRepository _memberResultRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly MemberActionFactory _actionFactory;

        public MemberResultService(
            IMemberResultRepository memberResultRepository,
            IUnitOfWork unitOfWork, MemberActionFactory actionFactory
           )
        {
            _unitOfWork = unitOfWork;
            _actionFactory = actionFactory;
            _memberResultRepository = memberResultRepository;
        }

        public Result DetailList(MemberResultListDto dto)
        {
            var result = _actionFactory.Action(dto.Action)?.Apply(dto.Ids) ?? new Result();

            if (!result.Success)
                return result;

            var query = _memberResultRepository.AsNoTracking;
            query = query.Include(i => i.Member);
            query = query.Include(i => i.Quiz);

            query = new MemberResultListFilter(query, dto).FilteredQuery();
            query = new MemberResultListOrder(query, dto).OrderByQuery();

            result.SetPaging(dto?.Page ?? 1, dto?.Size ?? 10, query.Count());

            result.Data = query.Select(s => new MemberResultListDto
            {
                Name = s.Member.Name,
                Email = s.Member.Email,
                CreatedSource = s.Member.CreatedSource,
                CreatedAt = s.CreatedAt,
                Score = s.Score,
                Percentage = s.Percentage,
                AttempedQues = s.AttempedQues,
                StartTime = s.StartTime,
                EndTime = s.EndTime,
                QuizQuestions = s.Quiz.QuizQuestions.Count
            })
            .ToPaged(result.Paging.Page, result.Paging.Size)
            .ToList()
            .Select(s => new
            {
                s.Name,
                s.Email,
                s.CreatedAt,
                s.CreatedSource,
                CreatedSourceName = Enum.GetName(typeof(MemberCreatedSource), s.CreatedSource).Humanize(LetterCasing.Title),
                s.Score,
                s.Percentage,
                s.AttempedQues,
                s.StartTime,
                s.EndTime,
                TimeDuration = s.EndTime - s.StartTime,
                s.QuizQuestions
                //s.TestStartTime
            });

            return result;
        }


    }
}
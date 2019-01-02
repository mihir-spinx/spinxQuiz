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

            if (dto.MemberId != 0)
            {
                query = query.Where(w => w.MemberId == dto.MemberId);
            }

            if (dto.FromScore != null && dto.FromScore != 0)
            {
                query = query.Where(w => w.Score >= dto.FromScore);
            }

            if (dto.ToScore != null && dto.ToScore != 0)
            {
                query = query.Where(w => w.Score <= dto.ToScore);
            }

            if (dto.FromPercentage != null && dto.FromPercentage != 0)
            {
                query = query.Where(w => w.Percentage >= dto.FromPercentage);
            }

            if (dto.ToPercentage != null && dto.ToPercentage != 0)
            {
                query = query.Where(w => w.Percentage <= dto.ToPercentage);
            }

            if (dto.FromAttempedQues != null && dto.FromAttempedQues != 0)
            {
                query = query.Where(w => w.AttempedQues >= dto.FromAttempedQues);
            }

            if (dto.ToAttempedQues != null && dto.ToAttempedQues != 0)
            {
                query = query.Where(w => w.AttempedQues <= dto.ToAttempedQues);
            }

            if (dto.CreatedSource != 0)
            {
                query = query.Where(w => w.Member.CreatedSource == dto.CreatedSource);
            }

            query = new MemberResultListOrder(query, dto).OrderByQuery();

            result.SetPaging(dto?.Page ?? 1, dto?.Size ?? 10, query.Count());



            if (dto.SortColumn != "timeDuration")
            {
                result.Data = query.Select(s => new MemberResultListDto
                {
                    Name = s.Member.Name,
                    Email = s.Member.Email,
                    College = s.Member.College,
                    CreatedSource = s.Member.CreatedSource,
                    CreatedAt = s.CreatedAt,
                    Score = s.Score,
                    Percentage = s.Percentage,
                    AttempedQues = s.AttempedQues,
                    StartTime = s.StartTime,
                    EndTime = s.EndTime,
                    QuizQuestions = s.Quiz.QuizQuestions.Count,
                    QuizTitle = s.Quiz.Title,
                    QuizCategoryName = s.Quiz.QuizCategory.Name
                })
                    .ToPaged(result.Paging.Page, result.Paging.Size)
                    .ToList()
                    .Select(s => new
                    {
                        s.Name,
                        s.Email,
                        s.College,
                        s.CreatedAt,
                        s.CreatedSource,
                        CreatedSourceName = Enum.GetName(typeof(MemberCreatedSource), s.CreatedSource)
                            .Humanize(LetterCasing.Title),
                        s.Score,
                        s.Percentage,
                        s.AttempedQues,
                        s.StartTime,
                        StartTimeDisplay = s.StartTime.Value.ToString("MM/dd/yyyy hh:mm:ss"),
                        EndTimeDisplay = s.EndTime.Value.ToString("MM/dd/yyyy hh:mm:ss"),
                        s.EndTime,
                        TimeDuration = ((DateTime)s.EndTime - (DateTime)s.StartTime).Hours + ":" +
                                       ((DateTime)s.EndTime - (DateTime)s.StartTime).Minutes + ":" +
                                       ((DateTime)s.EndTime - (DateTime)s.StartTime).Seconds,
                        s.QuizQuestions,
                        s.QuizTitle,
                        s.QuizCategoryName
                    });
            }
            else
            {
                if (dto.SortType == "desc")
                {
                    result.Data = query.Select(s => new MemberResultListDto
                    {
                        Name = s.Member.Name,
                        Email = s.Member.Email,
                        College = s.Member.College,
                        CreatedSource = s.Member.CreatedSource,
                        CreatedAt = s.CreatedAt,
                        Score = s.Score,
                        Percentage = s.Percentage,
                        AttempedQues = s.AttempedQues,
                        StartTime = s.StartTime,
                        EndTime = s.EndTime,
                        QuizQuestions = s.Quiz.QuizQuestions.Count,
                        QuizTitle = s.Quiz.Title,
                        QuizCategoryName = s.Quiz.QuizCategory.Name
                    })
                    .ToPaged(result.Paging.Page, result.Paging.Size)
                    .ToList()
                    .Select(s => new
                    {
                        s.Name,
                        s.Email,
                        s.College,
                        s.CreatedAt,
                        s.CreatedSource,
                        CreatedSourceName = Enum.GetName(typeof(MemberCreatedSource), s.CreatedSource)
                            .Humanize(LetterCasing.Title),
                        s.Score,
                        s.Percentage,
                        s.AttempedQues,
                        s.StartTime,
                        StartTimeDisplay = s.StartTime.Value.ToString("MM/dd/yyyy hh:mm:ss"),
                        EndTimeDisplay = s.EndTime.Value.ToString("MM/dd/yyyy hh:mm:ss"),
                        s.EndTime,
                        TimeDuration = ((DateTime)s.EndTime - (DateTime)s.StartTime).Hours + ":" +
                                       ((DateTime)s.EndTime - (DateTime)s.StartTime).Minutes + ":" +
                                       ((DateTime)s.EndTime - (DateTime)s.StartTime).Seconds,
                        s.QuizQuestions,
                        s.QuizTitle,
                        s.QuizCategoryName
                    })
                    .OrderByDescending(o => o.TimeDuration);
                }
                else
                {
                    result.Data = query.Select(s => new MemberResultListDto
                    {
                        Name = s.Member.Name,
                        Email = s.Member.Email,
                        College = s.Member.College,
                        CreatedSource = s.Member.CreatedSource,
                        CreatedAt = s.CreatedAt,
                        Score = s.Score,
                        Percentage = s.Percentage,
                        AttempedQues = s.AttempedQues,
                        StartTime = s.StartTime,
                        EndTime = s.EndTime,
                        QuizQuestions = s.Quiz.QuizQuestions.Count,
                        QuizTitle = s.Quiz.Title,
                        QuizCategoryName = s.Quiz.QuizCategory.Name
                    })
                    .ToPaged(result.Paging.Page, result.Paging.Size)
                    .ToList()
                    .Select(s => new
                    {
                        s.Name,
                        s.Email,
                        s.College,
                        s.CreatedAt,
                        s.CreatedSource,
                        CreatedSourceName = Enum.GetName(typeof(MemberCreatedSource), s.CreatedSource)
                            .Humanize(LetterCasing.Title),
                        s.Score,
                        s.Percentage,
                        s.AttempedQues,
                        s.StartTime,
                        StartTimeDisplay = s.StartTime.Value.ToString("MM/dd/yyyy hh:mm:ss"),
                        EndTimeDisplay = s.EndTime.Value.ToString("MM/dd/yyyy hh:mm:ss"),
                        s.EndTime,
                        TimeDuration = ((DateTime)s.EndTime - (DateTime)s.StartTime).Hours + ":" +
                                       ((DateTime)s.EndTime - (DateTime)s.StartTime).Minutes + ":" +
                                       ((DateTime)s.EndTime - (DateTime)s.StartTime).Seconds,
                        s.QuizQuestions,
                        s.QuizTitle,
                        s.QuizCategoryName
                    })
                    .OrderBy(o => o.TimeDuration);
                }

            }
            return result;
        }


    }
}
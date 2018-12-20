using Omu.ValueInjecter;
using Spinx.Core;
using Spinx.Data.Infrastructure;
using Spinx.Data.Repository.Member;
using Spinx.Data.Repository.QuizQuestions;
using Spinx.Data.Repository.Quizs;
using Spinx.Domain.Members;
using Spinx.Services.AdminUsers.DTOs;
using Spinx.Services.Content;
using Spinx.Services.Infrastructure;
using Spinx.Services.Members.Validators;
using System;
using System.Linq;

namespace Spinx.Services.Members
{
    public interface IMemberQuizService
    {        
        Result SaveMemberQuizInit(int userId,string slug);
    }

    public class MemberQuizService : IMemberQuizService
    {
        private readonly IMemberRepository _memberRepository;
        private readonly IQuizRepository _quizRepository;
        private readonly IMemberResultRepository _memberResultRepository;
        private readonly IMemberQuizAnswerRepository _memberQuizAnswerRepository;
        private readonly IQuizQuestionRepository _quizQuestionRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly MemberEditProfileValidator _memberEditProfileValidator;

        public MemberQuizService(
            IMemberRepository memberRepository,
            IMemberResultRepository memberResultRepository,
            IMemberQuizAnswerRepository memberQuizAnswerRepository,
            IQuizQuestionRepository quizQuestionRepository,
            IUnitOfWork unitOfWork,
            IQuizRepository quizRepository,
            MemberEditProfileValidator memberEditProfileValidator)
        {
            _memberRepository = memberRepository;
            _quizRepository = quizRepository;
            _memberQuizAnswerRepository = memberQuizAnswerRepository;
            _memberResultRepository = memberResultRepository;
            _quizQuestionRepository = quizQuestionRepository;
            _unitOfWork = unitOfWork;
            _memberEditProfileValidator = memberEditProfileValidator;
        }

       

        public Result SaveMemberQuizInit(int userId, string slug)
        {
            var result = new Result();

           var quiz = _quizRepository.AsNoTracking.FirstOrDefault(w => w.Slug == slug && w.IsActive);

            if(quiz == null)
            {
                return result.SetError("Quiz not found");
            }

            var entity = new MemberResult()
            {
                MemberId = userId,
                QuizId = quiz.Id,
                CreatedAt = DateTime.Now,
                StartTime = DateTime.Now                
            };

            _memberResultRepository.Insert(entity);
            _unitOfWork.Commit();

            var questionList = _quizQuestionRepository.AsNoTracking.Where(x => x.QuizId == entity.QuizId).Select(s => s.Id).OrderBy(o => Guid.NewGuid());
            var i = 1;
            foreach (var questionId in questionList)
            {
                var memberQuizEntity = new MemberQuizAnswer()
                {
                   MemberResultId = entity.Id,
                   QuizQuestionId = questionId,
                   CreatedAt = DateTime.Now,
                   SortOrder = i
                };

                _memberQuizAnswerRepository.Insert(memberQuizEntity);
                i++;
            }
            _unitOfWork.Commit();
            result.Id = entity.Id;
            result.SetSuccess("Quiz created successfully.");          

            return result;
        }

       
    }
}
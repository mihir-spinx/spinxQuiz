using Omu.ValueInjecter;
using Spinx.Core;
using Spinx.Data.Infrastructure;
using Spinx.Data.Repository.EmailTemplates;
using Spinx.Domain.EmailTemplates;
using Spinx.Services.Content;
using Spinx.Services.EmailTemplates.Actions;
using Spinx.Services.EmailTemplates.DTOs;
using Spinx.Services.EmailTemplates.Filters;
using Spinx.Services.EmailTemplates.ListOrders;
using Spinx.Services.EmailTemplates.Mappers;
using Spinx.Services.EmailTemplates.Validators;
using Spinx.Services.Infrastructure;
using System.Linq;

namespace Spinx.Services.EmailTemplates
{
    public interface IEmailTemplateAdminService
    {
        Result List(EmailTemplateAdminFilterDto dto);
        Result Create(EmailTemplateCreateAdminDto dto);
        EmailTemplateEditAdminDto GetById(int id);
        Result Edit(int id, EmailTemplateEditAdminDto dto);
    }

    public class EmailTemplateAdminService : IEmailTemplateAdminService
    {
        private readonly IEmailTemplateRepository _emailTemplateRepository;
        private readonly EmailTemplateAdminActionFactory _actionFactory;
        private readonly EmailTemplateCreateAdminValidator _validatorCreate;
        private readonly EmailTemplateEditAdminValidator _validatorEdit;
        private readonly IUnitOfWork _unitOfWork;

        public EmailTemplateAdminService(
            IEmailTemplateRepository emailTemplateRepository,
            EmailTemplateAdminActionFactory actionFactory,
            EmailTemplateCreateAdminValidator validatorCreate,
            EmailTemplateEditAdminValidator validatorEdit,
            IUnitOfWork unitOfWork)
        {
            _emailTemplateRepository = emailTemplateRepository;
            _actionFactory = actionFactory;
            _validatorCreate = validatorCreate;
            _validatorEdit = validatorEdit;
            _unitOfWork = unitOfWork;
            EmailTemplateAdminMapper.Init();
        }

        public Result List(EmailTemplateAdminFilterDto dto)
        {
            var result = _actionFactory.Action(dto.Action)?.Apply(dto.Ids) ?? new Result();
            if (!result.Success) return result;

            var query = _emailTemplateRepository.AsNoTracking;
            query = new EmailTemplateAdminFilter(query, dto).FilteredQuery();
            query = new EmailTemplateAdminListOrder(query, dto).OrderByQuery();
            result.SetPaging(dto?.Page ?? 1, dto?.Size ?? 10, query.Count());

            result.Data = query
                .Select(s => new
                {
                    s.Id,
                    s.Name,
                    s.Slug,
                })
                .Skip((result.Paging.Page - 1) * result.Paging.Size)
                .Take(result.Paging.Size);

            return result;
        }

        public Result Create(EmailTemplateCreateAdminDto dto)
        {
            var result = _validatorCreate.ValidateResult(dto);
            if (!result.Success) return result;

            var entity = Mapper.Map<EmailTemplate>(dto);
            _emailTemplateRepository.Insert(entity);
            _unitOfWork.Commit();

            EmailTemplateCacheManager.ClearCache();

            result.Id = entity.Id;

            return result.SetSuccess(Messages.RecordSaved);
        }

        public EmailTemplateEditAdminDto GetById(int id)
        {
            var entity = _emailTemplateRepository.AsNoTracking
                .FirstOrDefault(w => w.Id == id);
            return entity == null ? null : Mapper.Map<EmailTemplateEditAdminDto>(entity);
        }

        public Result Edit(int id, EmailTemplateEditAdminDto dto)
        {
            dto.Id = id;
            var result = _validatorEdit.ValidateResult(dto);
            if (!result.Success) return result;

            if (dto.Id > 0)
            {
                var entity = _emailTemplateRepository.AsNoTracking.FirstOrDefault(w => w.Id == dto.Id);

                if (entity == null)
                    return result.SetError("There are error for update record. Please try again with refresh email template.");

                Mapper.Map<EmailTemplate>(dto, entity);
                _emailTemplateRepository.Update(entity);
                _unitOfWork.Commit();

                result.Id = entity.Id;
            }
            else
            {
                var entity = Mapper.Map<EmailTemplate>(dto);
                _emailTemplateRepository.Insert(entity);
                _unitOfWork.Commit();

                result.Id = entity.Id;
            }

            EmailTemplateCacheManager.ClearCache();

            return result.SetSuccess(Messages.RecordSaved);
        }
    }
}
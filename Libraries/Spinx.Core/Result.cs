using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace Spinx.Core
{
    public class Result
    {
        public Result()
        {
            Success = true;
        }

        public int? Id { get; set; }

        public bool Success;

        public string Message;

        public MessageType MessageType = MessageType.Success;

        public IDictionary<string, string> Errors = new ConcurrentDictionary<string, string>();

        private bool? _isRedirect;
        public bool IsRedirect
        {
            get => _isRedirect ?? !string.IsNullOrEmpty(Redirect);
            set => _isRedirect = value;
        }

        public string Redirect;

        public bool ClearForm;

        public Paging Paging = new Paging();

        #region Helpers

        public Result SetError(string message)
        {
            Success = false;
            Message = message;
            MessageType = MessageType.Error;

            return this;
        }

        public Result SetBlankRedirect()
        {
            IsRedirect = true;

            return this;
        }

        public Result SetRedirect(string url)
        {
            Redirect = url;

            return this;
        }

        public Result SetSuccess(string message = null)
        {
            Success = true;

            if (!string.IsNullOrEmpty(message))
                Message = message;

            MessageType = MessageType.Success;

            return this;
        }

        public Result Clear()
        {
            ClearForm = true;

            return this;
        }

        public Result SetPaging(int page, int size, int total)
        {
            Paging.Size = size == 0 ? 10 : size;
            Paging.Page = page == 0 ? 1 : page;
            Paging.Total = total;

            return this;
        }

        #endregion

        public dynamic Data;
    }

    public enum MessageType
    {
        Error = 0,
        Success = 1, 
        Info = 2,
        Warning = 3
    }

    public class Paging
    {
        public int Page { get; set; }
        public int LastPage { get
        {
            if (Page == 0) return 0; 
            
            var lastPage = Total / (decimal)Size; 
            return (int)Math.Ceiling(lastPage); }
        }
        public int Size { get; set; }
        public int Total { get; set; }
    }
}
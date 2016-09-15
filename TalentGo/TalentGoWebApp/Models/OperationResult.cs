namespace TalentGo.Recruitment
{
    /// <summary>
    /// 表示一个操作结果的ViewModel
    /// </summary>
    public class OperationResult
	{
		public OperationResult(ResultStatus Status, string Message, string ReturnUrl, int RedirectInterval)
		{
			this.Result = Status;
			this.Message = Message;
			this.ReturnUrl = ReturnUrl;
			this.RedirectInterval = RedirectInterval;
		}

		public OperationResult(ResultStatus Status, string Message, string ReturnUrl)
			:this(Status, Message, ReturnUrl, -1)
		{ }

		public OperationResult(ResultStatus Status, string Message)
			: this(Status, Message, null)
		{ }

		public OperationResult(ResultStatus Status)
			: this(Status, null)
		{ }

		public ResultStatus Result { get; set; }

		public string Message { get; set; }

		public string ReturnUrl { get; set; }

		public int RedirectInterval { get; set; }
	}

	public enum ResultStatus
	{
		Success = 0,
		Warning = 1,
		Failure = 2
	}
}

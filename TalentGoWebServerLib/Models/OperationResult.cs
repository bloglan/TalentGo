namespace TalentGo.Models
{
    /// <summary>
    /// 表示一个操作结果的ViewModel
    /// </summary>
    public class OperationResult
	{
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Status"></param>
        /// <param name="Message"></param>
        /// <param name="ReturnUrl"></param>
        /// <param name="RedirectInterval"></param>
		public OperationResult(ResultStatus Status, string Message, string ReturnUrl, int RedirectInterval)
		{
			this.Result = Status;
			this.Message = Message;
			this.ReturnUrl = ReturnUrl;
			this.RedirectInterval = RedirectInterval;
		}

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Status"></param>
        /// <param name="Message"></param>
        /// <param name="ReturnUrl"></param>
		public OperationResult(ResultStatus Status, string Message, string ReturnUrl)
			:this(Status, Message, ReturnUrl, -1)
		{ }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Status"></param>
        /// <param name="Message"></param>
		public OperationResult(ResultStatus Status, string Message)
			: this(Status, Message, null)
		{ }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Status"></param>
		public OperationResult(ResultStatus Status)
			: this(Status, null)
		{ }

        /// <summary>
        /// 
        /// </summary>
		public ResultStatus Result { get; set; }

        /// <summary>
        /// 
        /// </summary>
		public string Message { get; set; }

        /// <summary>
        /// 
        /// </summary>
		public string ReturnUrl { get; set; }

        /// <summary>
        /// 
        /// </summary>
		public int RedirectInterval { get; set; }
	}

    /// <summary>
    /// 
    /// </summary>
	public enum ResultStatus
	{
        /// <summary>
        /// 
        /// </summary>
		Success = 0,
        /// <summary>
        /// 
        /// </summary>
		Warning = 1,
        /// <summary>
        /// 
        /// </summary>
		Failure = 2
	}
}

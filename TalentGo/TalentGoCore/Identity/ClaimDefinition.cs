namespace TalentGo.Identity
{
    public static class ClaimDefinition
	{
		/// <summary>
		/// 注册代理
		/// </summary>
		public static readonly string RegisterationDelegateType = "http://schemas.qjyc.cn/nonstd/2016/01/registerationdelegate";

		/// <summary>
		/// 代理信息。
		/// </summary>
		public static readonly string DelegateInfo = "http://schemas.qjyc.cn/nonstd/2016/01/delegateinfo";
		/// <summary>
		/// 角色
		/// </summary>
		public static readonly string RoleClaimType = "http://schemas.microsoft.com/ws/2008/06/identity/claims/role";
		/// <summary>
		/// 用户ID标识符
		/// </summary>
		public static readonly string UserIdClaimType = "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier";
		/// <summary>
		/// 用户名
		/// </summary>
		public static readonly string UserNameClaimType = "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name";
		/// <summary>
		/// 安全戳
		/// </summary>
		public static readonly string SecurityStampClaimType = "AspNet.Identity.SecurityStamp";

		/// <summary>
		/// 字符串类型的类型声明。
		/// </summary>
		public static readonly string TypeofString = "http://www.w3.org/2001/XMLSchema#string";
	}
}

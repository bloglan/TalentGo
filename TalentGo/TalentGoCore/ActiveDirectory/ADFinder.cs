using System;
using System.Collections.Generic;
using System.DirectoryServices;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace TalentGo.ActiveDirectory
{
	public class ADFinder : IDisposable
	{
		DirectoryEntry root = new DirectoryEntry("LDAP://DC=qjyc,DC=cn");
		DirectorySearcher searcher;

		public ADFinder()
		{
			this.searcher = new DirectorySearcher(this.root);
		}

		public DirectoryEntry FindUserBySID(SecurityIdentifier SID)
		{
			byte[] binForm = new byte[SID.BinaryLength];
			this.searcher.Filter = string.Format("(objectSid={0})", this.ConvertBytesToHexString(binForm));
			SearchResult result = this.searcher.FindOne();
			if (result == null)
				return null;

			return result.GetDirectoryEntry();
		}

		public DirectoryEntry FindUserBySID(string SDDLFormt)
		{
			SecurityIdentifier sid = new SecurityIdentifier(SDDLFormt);
			return this.FindUserBySID(sid);
		}


		string ConvertBytesToHexString(byte[] Input)
		{
			StringBuilder sb = new StringBuilder();
			foreach (byte b in Input)
			{
				sb.Append("\\" + b.ToString("X2"));
			}
			return sb.ToString();
		}

		#region IDisposable Support
		private bool disposedValue = false; // 要检测冗余调用

		protected virtual void Dispose(bool disposing)
		{
			if (!disposedValue)
			{
				if (disposing)
				{
					// TODO: 释放托管状态(托管对象)。
					this.root.Dispose();
					this.searcher.Dispose();
				}

				// TODO: 释放未托管的资源(未托管的对象)并在以下内容中替代终结器。
				// TODO: 将大型字段设置为 null。

				disposedValue = true;
			}
		}


		// TODO: 仅当以上 Dispose(bool disposing) 拥有用于释放未托管资源的代码时才替代终结器。
		// ~ADFinder() {
		//   // 请勿更改此代码。将清理代码放入以上 Dispose(bool disposing) 中。
		//   Dispose(false);
		// }

		// 添加此代码以正确实现可处置模式。
		public void Dispose()
		{
			// 请勿更改此代码。将清理代码放入以上 Dispose(bool disposing) 中。
			Dispose(true);
			// TODO: 如果在以上内容中替代了终结器，则取消注释以下行。
			// GC.SuppressFinalize(this);
		}
		#endregion
	}
}

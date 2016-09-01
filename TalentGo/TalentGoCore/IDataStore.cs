using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TalentGo
{
	public interface IDataStore<TData, TKey>
	{
		DbContext DbContext { get; }

		Task<TData> FindByKeyAsync(TKey Key);

		Task CreateAsync(TData Data);

		Task UpdateAsync(TData Data);

		Task DeleteAsync(TData Data);
	}
}

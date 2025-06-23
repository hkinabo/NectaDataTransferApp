using System.Data;

namespace DataTransferShared.Services
{
 //   public class UsajiliService : IUsajiliService
	//{
	//	private readonly psle_usajiliContext _context;
	//	public UsajiliService(psle_usajiliContext context)
	//	{
	//		_context = context;
	//		//	_entities = context.Set<>();
	//	}
	//	public async Task<List<Tblschool>> GetSchool()
	//	{
	//		return await _context.Tblschools.ToListAsync().ConfigureAwait(true);
	//	}
	//	public async Task<List<Dbname>> GetDatabaseName()
	//	{
	//		List<Dbname> dbNames = new();
	//		await Task.Delay(100);
	//		string dd = _context.Database.GetDbConnection().Database.ToString();
	//		if (dd != null)
	//		{
	//			Dbname dname = new()
	//			{
	//				Databasename = dd
	//			};

	//			dbNames.Add(dname);
	//		}
	//		return dbNames;
	//	}
	//	public async Task<bool> IsConnected()
	//	{
	//		Task<bool> conn = _context.Database.CanConnectAsync();

	//		return await conn.ConfigureAwait(false);
	//	}
	//	public async Task<int> CandidateDelete(string regionNo)
	//	{
	//		int s = await _context.Tblmarksdetails.Where(x => x.RegionNo == regionNo).ExecuteDeleteAsync().ConfigureAwait(true);
	//		return s;

	//	}
	//	public async Task<int> SchoolDelete()
	//	{
	//		int d = await _context.Tblschools.ExecuteDeleteAsync().ConfigureAwait(true);
	//		return d;
	//	}
	//	public async Task<int> DistrictDelete()
	//	{
	//		int d = await _context.Tbldistricts.ExecuteDeleteAsync().ConfigureAwait(true);
	//		return d;
	//	}
	//	public async Task<int> RegionDelete()
	//	{
	//		int d = await _context.Tblregions.ExecuteDeleteAsync().ConfigureAwait(true);
	//		return d;
	//	}
	//	public async Task<int> CandidateAdd(List<Tblmarksdetail> candlist)
	//	{
	//		try
	//		{
	//			await _context.Tblmarksdetails.AddRangeAsync(candlist);

	//			int cc = await _context.SaveChangesAsync();
	//			foreach (Microsoft.EntityFrameworkCore.ChangeTracking.EntityEntry entity in _context.ChangeTracker.Entries())
	//			{
	//				entity.State = EntityState.Detached;
	//			}

	//			return cc;
	//		}
	//		catch (DbUpdateException)
	//		{
	//			foreach (Microsoft.EntityFrameworkCore.ChangeTracking.EntityEntry entity in _context.ChangeTracker.Entries())
	//			{
	//				entity.State = EntityState.Detached;
	//			}
	//			return -1;
	//		}
	//		catch (InvalidOperationException)
	//		{
	//			foreach (Microsoft.EntityFrameworkCore.ChangeTracking.EntityEntry entity in _context.ChangeTracker.Entries())
	//			{
	//				entity.State = EntityState.Detached;
	//			}
	//			return -1;
	//		}
	//		catch (Exception)
	//		{
	//			foreach (Microsoft.EntityFrameworkCore.ChangeTracking.EntityEntry entity in _context.ChangeTracker.Entries())
	//			{
	//				entity.State = EntityState.Detached;
	//			}
	//			return -1;
	//		}

	//	}
	//	public async Task<int> RegionAdd(List<Tblregion> regionlist)
	//	{
	//		try
	//		{
	//			await _context.Tblregions.AddRangeAsync(regionlist);
	//			int rr = await _context.SaveChangesAsync();
	//			foreach (Microsoft.EntityFrameworkCore.ChangeTracking.EntityEntry entity in _context.ChangeTracker.Entries())
	//			{
	//				entity.State = EntityState.Detached;
	//			}
	//			return rr;

	//		}
	//		catch (DbUpdateException)
	//		{
	//			foreach (Microsoft.EntityFrameworkCore.ChangeTracking.EntityEntry entity in _context.ChangeTracker.Entries())
	//			{
	//				entity.State = EntityState.Detached;
	//			}
	//			return -1;
	//		}
	//		catch (InvalidOperationException)
	//		{
	//			foreach (Microsoft.EntityFrameworkCore.ChangeTracking.EntityEntry entity in _context.ChangeTracker.Entries())
	//			{
	//				entity.State = EntityState.Detached;
	//			}
	//			return -1;
	//		}
	//		catch (Exception)
	//		{
	//			foreach (Microsoft.EntityFrameworkCore.ChangeTracking.EntityEntry entity in _context.ChangeTracker.Entries())
	//			{
	//				entity.State = EntityState.Detached;
	//			}
	//			return -1;
	//		}
	//	}
	//	public async Task<int> DistrictAdd(List<Tbldistrict> districtlist)
	//	{
	//		await _context.Tbldistricts.AddRangeAsync(districtlist);
	//		return await _context.SaveChangesAsync();
	//	}
	//	public async Task<int> SchoolAdd(List<Tblschool> schoollist)
	//	{
	//		try
	//		{
	//			await _context.Tblschools.AddRangeAsync(schoollist);
	//			int ss = await _context.SaveChangesAsync();
	//			foreach (Microsoft.EntityFrameworkCore.ChangeTracking.EntityEntry entity in _context.ChangeTracker.Entries())
	//			{
	//				entity.State = EntityState.Detached;
	//			}
	//			return ss;
	//		}
	//		catch (DbUpdateException)
	//		{
	//			foreach (Microsoft.EntityFrameworkCore.ChangeTracking.EntityEntry entity in _context.ChangeTracker.Entries())
	//			{
	//				entity.State = EntityState.Detached;
	//			}
	//			return -1;
	//		}
	//		catch (InvalidOperationException)
	//		{
	//			foreach (Microsoft.EntityFrameworkCore.ChangeTracking.EntityEntry entity in _context.ChangeTracker.Entries())
	//			{
	//				entity.State = EntityState.Detached;
	//			}
	//			return -1;
	//		}
	//		catch (Exception)
	//		{
	//			foreach (Microsoft.EntityFrameworkCore.ChangeTracking.EntityEntry entity in _context.ChangeTracker.Entries())
	//			{
	//				entity.State = EntityState.Detached;
	//			}
	//			return -1;
	//		}
	//	}

	//	public async Task<int> SchoolAddIfNotExist(Tblschool schoolModel)
	//	{
	//		try
	//		{
	//			_ = await _context.Tblschools.AddAsync(schoolModel);
	//			int ss = await _context.SaveChangesAsync();

	//			foreach (Microsoft.EntityFrameworkCore.ChangeTracking.EntityEntry entity in _context.ChangeTracker.Entries())
	//			{
	//				entity.State = EntityState.Detached;
	//			}
	//			return ss;
	//		}
	//		catch (DbUpdateException)
	//		{
	//			foreach (Microsoft.EntityFrameworkCore.ChangeTracking.EntityEntry entity in _context.ChangeTracker.Entries())
	//			{
	//				entity.State = EntityState.Detached;
	//			}
	//			return -1;
	//		}
	//		catch (InvalidOperationException)
	//		{
	//			foreach (Microsoft.EntityFrameworkCore.ChangeTracking.EntityEntry entity in _context.ChangeTracker.Entries())
	//			{
	//				entity.State = EntityState.Detached;
	//			}
	//			return -1;
	//		}
	//		catch (Exception)
	//		{
	//			foreach (Microsoft.EntityFrameworkCore.ChangeTracking.EntityEntry entity in _context.ChangeTracker.Entries())
	//			{
	//				entity.State = EntityState.Detached;
	//			}
	//			return -1;
	//		}

	//	}

	//	public async Task<int> CanSeeNdioUpdate(string regionNo)
	//	{
	//		return await _context.Tblmarksdetails
	//	.Where(p => (p.RegionNo == regionNo) && (p.Cansee == "1" || p.Cansee == "2"))
	//	.ExecuteUpdateAsync(p => p.SetProperty(b => b.Cansee, b => "N"));

	//	}
	//	public async Task<int> SchoolCanSeeNdioUpdate(string schoolNo)
	//	{
	//		return await _context.Tblmarksdetails
	//	.Where(p => (p.SchoolNo == schoolNo) && (p.Cansee == "1" || p.Cansee == "2"))
	//	.ExecuteUpdateAsync(p => p.SetProperty(b => b.Cansee, b => "N"));

	//	}

	//	public async Task<int> CanSeeHapanaUpdate(string regionNo)
	//	{
	//		return await _context.Tblmarksdetails
	//	.Where(p => (p.RegionNo == regionNo) && p.Cansee == "3")
	//	.ExecuteUpdateAsync(p => p.SetProperty(b => b.Cansee, b => "H"));
	//	}

	//	public async Task<int> SchoolCanSeeHapanaUpdate(string schoolNo)
	//	{
	//		return await _context.Tblmarksdetails
	//	.Where(p => (p.SchoolNo == schoolNo) && p.Cansee == "3")
	//	.ExecuteUpdateAsync(p => p.SetProperty(b => b.Cansee, b => "H"));
	//	}

	//	public async Task<int> MaMakubwaNdioUpdate(string regionNo)
	//	{
	//		return await _context.Tblmarksdetails
	//	.Where(p => (p.RegionNo == regionNo) && p.MaMakubwa == "2")
	//	.ExecuteUpdateAsync(p => p.SetProperty(b => b.MaMakubwa, b => "N"));
	//	}

	//	public async Task<int> SchoolMaMakubwaNdioUpdate(string schoolNo)
	//	{
	//		return await _context.Tblmarksdetails
	//	.Where(p => (p.SchoolNo == schoolNo) && p.MaMakubwa == "2")
	//	.ExecuteUpdateAsync(p => p.SetProperty(b => b.MaMakubwa, b => "N"));
	//	}

	//	public async Task<int> MaMakubwaHapanaUpdate(string regionNo)
	//	{
	//		return await _context.Tblmarksdetails
	//	.Where(p => (p.RegionNo == regionNo) && (p.MaMakubwa == "1" || p.MaMakubwa == "3"))
	//	.ExecuteUpdateAsync(p => p.SetProperty(b => b.MaMakubwa, b => "H"));
	//	}
	//	public async Task<int> SchoolMaMakubwaHapanaUpdate(string schoolNo)
	//	{
	//		return await _context.Tblmarksdetails
	//	.Where(p => (p.SchoolNo == schoolNo) && (p.MaMakubwa == "1" || p.MaMakubwa == "3"))
	//	.ExecuteUpdateAsync(p => p.SetProperty(b => b.MaMakubwa, b => "H"));
	//	}

	//	public async Task<int> IsEnglishNdioUpdate(string regionNo)
	//	{
	//		return await _context.Tblschools
	//	.Where(p => (p.RegionNo == regionNo) && p.EnglishMedium == "1")
	//	.ExecuteUpdateAsync(p => p.SetProperty(b => b.EnglishMedium, b => "N"));
	//	}
	//	public async Task<int> SchoolIsEnglishNdioUpdate(string schoolNo)
	//	{
	//		return await _context.Tblschools
	//	.Where(p => (p.SchoolNo == schoolNo) && p.EnglishMedium == "1")
	//	.ExecuteUpdateAsync(p => p.SetProperty(b => b.EnglishMedium, b => "N"));
	//	}

	//	public async Task<int> IsEnglishHapanaUpdate(string regionNo)
	//	{
	//		return await _context.Tblschools
	//	.Where(p => (p.RegionNo == regionNo) && p.EnglishMedium == "0")
	//	.ExecuteUpdateAsync(p => p.SetProperty(b => b.EnglishMedium, b => "H"));
	//	}

	//	public async Task<int> SchoolIsEnglishHapanaUpdate(string schoolNo)
	//	{
	//		return await _context.Tblschools
	//	.Where(p => (p.SchoolNo == schoolNo) && p.EnglishMedium == "0")
	//	.ExecuteUpdateAsync(p => p.SetProperty(b => b.EnglishMedium, b => "H"));
	//	}
	//	public async Task<List<string>> GetRegionsFromCandidates()
	//	{
	//		return await _context.Tblmarksdetails.Select(m => m.RegionNo).AsNoTracking().Distinct().ToListAsync();
	//	}

	//	public async Task<int> CandidateDeleteBySchool(string schoolNo)
	//	{
	//		return await _context.Tblmarksdetails.Where(x => x.SchoolNo == schoolNo).ExecuteDeleteAsync();
	//	}
	//	public async Task<int> CandidateAddBySchool(List<Tblmarksdetail> candlist)
	//	{
	//		try
	//		{

	//			await _context.Tblmarksdetails.AddRangeAsync(candlist);

	//			int cc = await _context.SaveChangesAsync();
	//			foreach (Microsoft.EntityFrameworkCore.ChangeTracking.EntityEntry entity in _context.ChangeTracker.Entries())
	//			{
	//				entity.State = EntityState.Detached;
	//			}

	//			return cc;
	//		}
	//		catch (DbUpdateException)
	//		{
	//			foreach (Microsoft.EntityFrameworkCore.ChangeTracking.EntityEntry entity in _context.ChangeTracker.Entries())
	//			{
	//				entity.State = EntityState.Detached;
	//			}
	//			return -1;
	//		}
	//		catch (InvalidOperationException)
	//		{
	//			foreach (Microsoft.EntityFrameworkCore.ChangeTracking.EntityEntry entity in _context.ChangeTracker.Entries())
	//			{
	//				entity.State = EntityState.Detached;
	//			}
	//			return -1;
	//		}
	//		catch (Exception)
	//		{
	//			foreach (Microsoft.EntityFrameworkCore.ChangeTracking.EntityEntry entity in _context.ChangeTracker.Entries())
	//			{
	//				entity.State = EntityState.Detached;
	//			}
	//			return -1;
	//		}

	//	}
	//}

}

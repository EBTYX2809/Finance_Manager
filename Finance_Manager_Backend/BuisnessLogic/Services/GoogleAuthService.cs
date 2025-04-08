using Finance_Manager_Backend.DataBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Finance_Manager_Backend.BuisnessLogic.Services;

public class GoogleAuthService
{
	private readonly AppDbContext _appDbContext;
	public GoogleAuthService(AppDbContext dbContext)
	{
		_appDbContext = dbContext;
	}


}

using Finance_Manager.DataBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Finance_Manager.Models;

public class GoogleAuthServise
{
	private readonly AppDbContext _appDbContext;
	public GoogleAuthServise(AppDbContext dbContext)
	{
		_appDbContext = dbContext;
	}


}

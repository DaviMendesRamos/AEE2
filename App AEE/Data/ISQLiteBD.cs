using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App_AEE.Data
{
	internal interface ISQLiteBD
	{
		string SQLiteLocalPath(string bancoDados);
	}
}

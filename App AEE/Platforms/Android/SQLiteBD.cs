using App_AEE.Data;
using App_AEE.Platforms.Android;
using Java.Sql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
[assembly:Xamarin.Forms.Dependency(typeof(SQLiteBD))]
namespace App_AEE.Platforms.Android
{
	
	public class SQLiteBD : ISQLiteBD
	{
		public string SQLiteLocalPath(string bancoDados)
		{
			var path = System.Environment
				.GetFolderPath(System.Environment.SpecialFolder.Personal);
			return Path.Combine(path, bancoDados);
		}
	}
}

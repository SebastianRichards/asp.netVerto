using Microsoft.EntityFrameworkCore;
using System;
namespace VertoTest.Models
{
	public class ContentContext : DbContext
	{
		
		public DbSet<ContentModel> Content { get; set; }
		
		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) => optionsBuilder.UseSqlite(@"Data Source=data");
		
	}
}


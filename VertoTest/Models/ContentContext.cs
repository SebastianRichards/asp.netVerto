using Microsoft.EntityFrameworkCore;
using System;
namespace VertoTest.Models
{
	public class ContentContext : DbContext
	{
		public ContentContext()
		{
		}

		public ContentContext(DbContextOptions<ContentContext> options) : base(options)
		{
		}

		public DbSet<ContentModel> Content { get; set; }
		
	}
}


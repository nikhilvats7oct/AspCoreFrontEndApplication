using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace FinancialPortal.Web.Configuration
{
    public class DatabaseConfigurationProvider : ConfigurationProvider
    {
        private readonly Action<DbContextOptionsBuilder> _options;

        public DatabaseConfigurationProvider(Action<DbContextOptionsBuilder> options)
        {
            _options = options;
        }

        public override void Load()
        {
            var builder = new DbContextOptionsBuilder<DbContext>();
            _options(builder);

            using (var context = new DbContext(builder.Options))
            {
                var items = context.Set<ApplicationConfiguration>()
                    .AsNoTracking()
                    .ToList();

                foreach (var item in items)
                {
                    Data.Add(item.Title, item.Value);
                }
            }
        }

        private class ApplicationConfiguration
        {
            public long Id { get; set; }
            public string Title { get; set; }
            public string Value { get; set; }
        }
    }
}
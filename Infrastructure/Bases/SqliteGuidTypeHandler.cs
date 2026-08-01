using Dapper;
using System;
using System.Data;

namespace Infrastructure.Bases
{
    public class SqliteGuidTypeHandler : SqlMapper.TypeHandler<Guid>
    {
        // التصحيح: Parameter نوعه IDbDataParameter وليس IDbConnection
        public override void SetValue(IDbDataParameter parameter, Guid value)
        {
            parameter.Value = value.ToString().ToUpper();
        }

        public override Guid Parse(object value)
        {
            // SQLite أحياناً بيرجع القيمة كـ string
            return Guid.Parse((string)value);
        }
    }
}
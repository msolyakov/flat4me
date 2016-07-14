using Dapper;
using Flat4me.Core.Data.Objects;
using Flat4me.Core.Data;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace Flat4me.Core.Data.MsSql
{
    public class CityRepository : BaseRepository, ICityRepository
    {
        public async Task<List<CityDTO>> Find(string name)
        {
            IEnumerable<CityDTO> items = null;

            using (var con = new SqlConnection(base.ConnectionString))
            {
                var spName = "cmn.sp_City_Find";
                var p = new DynamicParameters();
                p.Add("@Name", string.IsNullOrEmpty(name) ? (string)null : name);

                await con.OpenAsync();                
                items = await con.QueryAsync<CityDTO>(
                    sql: spName,
                    param: p,
                    commandType: CommandType.StoredProcedure);
            }

            return items.ToList();
        }


        public async Task<CityDTO> GetByUrl(string url)
        {
            using (var con = new SqlConnection(base.ConnectionString))
            {
                var spName = "cmn.sp_City_GetByUrl";
                var p = new DynamicParameters();
                p.Add("@Url", url);

                await con.OpenAsync();
                return (await con.QueryAsync<CityDTO>(
                    sql: spName,
                    param: p,
                    commandType: CommandType.StoredProcedure)).FirstOrDefault();
            }
        }

        public async Task<IEnumerable<CityDTO>> GetAll()
        {
            IEnumerable<CityDTO> items = null;

            using (var con = new SqlConnection(base.ConnectionString))
            {
                var spName = "cmn.sp_City_GetAll";

                await con.OpenAsync();
                items = await con.QueryAsync<CityDTO>(
                    sql: spName,
                    commandType: CommandType.StoredProcedure);
            }

            return items;
        }
    }
}

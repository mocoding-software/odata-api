using Mocoding.AspNetCore.ODataApi.Core;
using Mocoding.AspNetCore.ODataApi.DataAccess;

namespace WebApp
{
    /// <summary>
    /// Example of custom controller that overrides CrudController.
    /// </summary>
    public class CustomController : CrudController<KeyValuePair>
    {
        public CustomController(ICrudRepository<KeyValuePair> repository)
            : base(repository)
        {
        }
    }
}

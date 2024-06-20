using CarWorkshopSystem.Core.Enums;
using CarWorkshopSystem.WebAPI.ViewModel.General;
using Microsoft.AspNetCore.Mvc;

namespace CarWorkshopSystem.WebAPI.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class TypesController : ControllerBase
    {
        public TypesController()
        {

        }

        [HttpGet]
        public ActionResult<List<KeyValue<string>>> StatusTypeList()
        {
            var response = GeneralProcessStatusTypeList.Map().Select(item => new KeyValue<string>
            {
                Id = item.Key,
                Name = item.Value
            });

            return Ok(response);
        }

        [HttpGet]
        public ActionResult<List<KeyValue<string>>> RoleTypeList()
        {
            var response = UserRoleTypeList.Map().Select(item => new KeyValue<string>
            {
                Id = item.Key,
                Name = item.Value
            });

            return Ok(response);
        }
    }
}
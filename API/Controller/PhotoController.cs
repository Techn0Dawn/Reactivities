using Application.Photos;
using Microsoft.AspNetCore.Mvc;

namespace API.Controller
{
    public class PhotosController : BaseApiController
    {
        [HttpPost] // /api/photos
        public async Task<IActionResult> Add([FromForm] Add.Command command)
        {
            return HandleResult(await Mediator.Send(command));
        }

        [HttpDelete("{id}")] // /api/photos
        public async Task<IActionResult> Delete(string id)
        {
            return HandleResult(await Mediator.Send(new Delete.Command { Id = id }));
        }

        [HttpPost("{id}/setMain")] // /api/photos/{id}/setMain
        public async Task<IActionResult> SetMain(string id)
        {
            return HandleResult(await Mediator.Send(new SetMain.Command { Id = id}));
        }
    }
}

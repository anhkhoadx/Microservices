using Catalog.DataLayer.Commands;
using Catalog.DataLayer.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using Shared.DataLayer.Attributes;

namespace Catalog.API.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	[ValidationFilter]
	public class CatalogController : ControllerBase
	{
		private readonly IMediator _mediator;

		public CatalogController(IMediator mediator)
		{
			_mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
		}

		[HttpGet("items")]
		public async Task<ActionResult> GetAllCatalogs()
		{
			var result = await _mediator.Send(new FindAllCatalogsQuery());

			return new JsonResult(result);
		}

		[HttpPost]
		public async Task<ActionResult> AddCatalog([FromBody] AddCatalogCommand request)
		{
			var result = await _mediator.Send(request);

			return new JsonResult(result);
		}
	}
}

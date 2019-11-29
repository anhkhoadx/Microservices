using Baseline;
using Catalog.DataLayer.Commands;
using Catalog.DataLayer.Events;
using Catalog.DataLayer.Queries;
using Marten;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Shared.DataLayer.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Catalog.API.Controllers
{
	public class Quest
	{
		public Guid Id { get; set; }
	}
	public class ArrivedAtLocation
	{
		public int Day { get; set; }

		public string Location { get; set; }

		public override string ToString()
		{
			return $"Arrived at {Location} on Day {Day}";
		}
	}

	public class MembersJoined
	{
		public MembersJoined()
		{
		}

		public MembersJoined(int day, string location, params string[] members)
		{
			Day = day;
			Location = location;
			Members = members;
		}

		public Guid QuestId { get; set; }

		public int Day { get; set; }

		public string Location { get; set; }

		public string[] Members { get; set; }

		//public override string ToString()
		//{
		//	return $"Members {Members.Join(", ")} joined at {Location} on Day {Day}";
		//}
	}

	public class QuestStarted
	{
		public string Name { get; set; }
		public Guid Id { get; set; }

		public override string ToString()
		{
			return $"Quest {Name} started";
		}
	}

	public class QuestEnded
	{
		public string Name { get; set; }
		public Guid Id { get; set; }

		public override string ToString()
		{
			return $"Quest {Name} ended";
		}
	}

	public class MembersDeparted
	{
		public Guid Id { get; set; }

		public Guid QuestId { get; set; }

		public int Day { get; set; }

		public string Location { get; set; }

		public string[] Members { get; set; }

		//public override string ToString()
		//{
		//	return $"Members {Members.Join(", ")} departed at {Location} on Day {Day}";
		//}
	}

	public class MembersEscaped
	{
		public Guid Id { get; set; }

		public Guid QuestId { get; set; }

		public string Location { get; set; }

		public string[] Members { get; set; }

		//public override string ToString()
		//{
		//	return $"Members {Members.Join(", ")} escaped from {Location}";
		//}
	}

	public class QuestParty
	{
		protected readonly IList<string> _members = new List<string>();

		public string[] Members
		{
			get
			{
				return _members.ToArray();
			}
			set
			{
				_members.Clear();
				_members.AddRange(value);
			}
		}

		public IList<string> Slayed { get; } = new List<string>();

		public void Apply(MembersJoined joined)
		{
			_members.Fill(joined.Members);
		}

		public void Apply(MembersDeparted departed)
		{
			_members.RemoveAll(x => departed.Members.Contains(x));
		}

		public void Apply(QuestStarted started)
		{
			Name = started.Name;
		}

		public string Key { get; set; }

		public string Name { get; set; }

		public Guid Id { get; set; }

		public override string ToString()
		{
			return $"Quest party '{Name}' is {Members.Join(", ")}";
		}
	}
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

		private void TestEventSourcing(IDocumentStore store)
		{
			var userId = Guid.NewGuid();
			var streamId = Guid.NewGuid();
			var catalogId1 = Guid.NewGuid();
			var catalogId2 = Guid.NewGuid();
			var orderId = Guid.NewGuid();

			using var session = store.OpenSession();

			var catalogAdded1 = new CatalogAdded
			{
				CatalogId = catalogId1,
				Quantity = 5,
				UserId = userId
			};

			var catalogAdded2 = new CatalogAdded
			{
				CatalogId = catalogId2,
				Quantity = 3,
				UserId = userId
			};

			// Assume user adds 2 catalog first
			session.Events.StartStream(streamId, catalogAdded1, catalogAdded2);
			session.SaveChanges();

			var catalogRemoved = new CatalogRemoved
			{
				CatalogId = catalogId1,
				Quantity = 2,
				UserId = userId
			};

			var orderCreated = new OrderCreated
			{
				UserId = userId,
				OrderId = orderId,
				TotalMoney = 10000000
			};

			var orderChangeStatus = new OrderStatusChangeToPaid { OrderId = orderId };
			var orderConfirmed = new OrderConfirmed { OrderId = orderId };
			var orderShipped = new OrderShipped { OrderId = orderId };
			var orderCompleted = new OrderCompleted { OrderId = orderId };
			session.Events.Append(streamId, orderCreated, catalogRemoved, orderChangeStatus, orderConfirmed, orderShipped, orderCompleted);
			session.SaveChanges();
		}
	}
}

using FluentAssertions;
using Ganaz.Backend.Library.DataStructures;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Ganaz.Backend.Library.Tests
{
	public class GanazQueueTests : IDisposable
	{
		private GanazQueue _sut;
		private MessageDto _item;
		private List<MessageDto> _listResult;

		public GanazQueueTests()
		{
			_sut = new GanazQueue();
		}

		public void Dispose()
		{

			_sut = null;
			_listResult = null;
			_item = null;
		}

		[Fact]
		public void GivenEmptyQueue_WhenInsertingItem_ThenItemIsInserted()
		{
			GivenAnEmptyQueue();
			GivenValidItemToInsert();
			WhenAddInvoked();
			ThenItemIsAdded();
		}

		[Fact]
		public void GivenAQueueWithItems_WhenInsertingItem_ThenItemIsInserted()
		{
			GivenAQueueHasItems();
			GivenValidItemToInsert();
			WhenAddInvoked();
			ThenItemIsAdded();
			ThenItemAddedHasHighestPriority();
		}

		[Fact]
		public void GivenAqueueWithItems_WhenRemovedInvoked_ThenItemIsRemoved()
		{
			GivenAQueueHasItems();
			WhenRemoveInvoked();
			ThenItemIsRemoved();
			ThenRemovedItemIsMovedToProcessedMessagesQueue();	
		}

		[Fact]
		public void GivenAqueueWithItems_WhenGetProcessedMessagesInvoked_ThenMessagesReturned()
		{
			GivenAQueueHasItems();
			WhenRemoveInvoked();
			WhenGetProcessedMessagesInvoked();
			ThenMessagesReturned();
		}

		#region Givens
		private void GivenValidItemToInsert() => _item = new MessageDto
		{
			City = "City",
			FirstName ="FirstName",
			LastName = "LastName",
			PhoneNumber = "PhoneNumber",
			Priority = 10,
			Sip ="Sip",
			State = "State",
			TimeStamp = "TimeStamp"			
		};

		private void GivenAnEmptyQueue() => _sut.Clear();

		private void GivenAQueueHasItems()
		{
			for (int i = 0; i < 3; i++)
			{
				_sut.Add(new MessageDto
				{
					City = "City",
					FirstName = "FirstName",
					LastName = "LastName",
					PhoneNumber = "PhoneNumber",
					Priority = new Random().Next(1, 9),
					Sip = "Sip",
					State = "State",
					TimeStamp = "TimeStamp"
				});
			}
		}
			

		#endregion

		#region Whens
		private void WhenAddInvoked() => _sut.Add(_item);

		private void WhenRemoveInvoked() => _sut.Remove();

		private void WhenGetProcessedMessagesInvoked() => _listResult = _sut.GetProcessedMessages();

		private void WhenGetQueueMessagesInvoked() => _listResult = _sut.GetQueueMessages();
		#endregion

		#region Thens

		private void ThenItemIsAdded()
		{
			_sut.Count.Should().BeGreaterThan(0);
			_sut.Contains(_item).Should().BeTrue();
		}

		private void ThenItemAddedHasHighestPriority()
		{
			var items = _sut.GetQueueMessages();

			var maxPriorityItem = items.OrderByDescending(x => x.Priority).Take(1).FirstOrDefault();

			maxPriorityItem.Priority.Should().Be(_item.Priority);
		}

		private void ThenRemovedItemIsMovedToProcessedMessagesQueue() =>	
			_sut.GetProcessedMessages().Count.Should().BeGreaterThan(0);

		private void ThenMessagesReturned()
		{

			_listResult.Should().NotBeNull();
			_listResult.Count().Should().BeGreaterThan(0);
		}

		private void ThenItemIsRemoved()
		{
			var removedItem = _sut.GetProcessedMessages().FirstOrDefault();

			var itemsInQueue = _sut.GetQueueMessages();

			itemsInQueue.Contains(removedItem).Should().BeFalse();
		}


		#endregion
	}
}

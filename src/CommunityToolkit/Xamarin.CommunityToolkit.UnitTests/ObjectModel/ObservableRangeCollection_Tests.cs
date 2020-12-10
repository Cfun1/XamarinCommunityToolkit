﻿using System.Collections.Generic;
using System.Collections.Specialized;
using Xamarin.CommunityToolkit.ObjectModel;
using NUnit.Framework;

namespace Xamarin.CommunityToolkit.UnitTests.ObjectModel
{
	public class ObservableRangeCollection_Tests
	{
		[Test]
		public void AddRange()
		{
			var collection = new ObservableRangeCollection<int>();
			var toAdd = new[] { 3, 1, 4, 1, 5, 9, 2, 6, 5, 3, 5, 8, 9, 7, 9, 3, 2, 3 };

			collection.CollectionChanged += (s, e) =>
			{
				Assert.AreEqual(NotifyCollectionChangedAction.Add, e.Action);
				Assert.Null(e.OldItems);
				Assert.AreEqual(toAdd.Length, e.NewItems.Count);

				for (var i = 0; i < toAdd.Length; i++)
				{
					Assert.AreEqual(toAdd[i], (int)e.NewItems[i]);
				}
			};
			collection.AddRange(toAdd);
		}

		[Test]
		public void AddRangeEmpty()
		{
			var collection = new ObservableRangeCollection<int>();
			var toAdd = new int[0];

			collection.CollectionChanged += (s, e) =>
			{
				throw new AssertionException("The event is raised.");
			};
			collection.AddRange(toAdd);
		}

		[Test]
		public void ReplaceRange()
		{
			var collection = new ObservableRangeCollection<int>();
			var toAdd = new[] { 3, 1, 4, 1, 5, 9, 2, 6, 5, 3, 5, 8, 9, 7, 9, 3, 2, 3 };
			var toRemove = new[] { 1, 1, 2, 2, 3, 3, 4, 4, 5, 5, 6, 6, 7, 7, 8, 8, 9, 9, 0, 0 };
			collection.AddRange(toRemove);
			collection.CollectionChanged += (s, e) =>
			{
				Assert.AreEqual(NotifyCollectionChangedAction.Reset, e.Action);

				Assert.Null(e.OldItems);
				Assert.Null(e.NewItems);

				Assert.AreEqual(collection.Count, toAdd.Length);

				for (var i = 0; i < toAdd.Length; i++)
				{
					if (collection[i] != (int)toAdd[i])
						throw new AssertionException("Expected and actual items don't match.");
				}
			};
			collection.ReplaceRange(toAdd);
		}

		[Test]
		public void ReplaceRange_on_non_empty_collection_should_always_raise_collection_changes()
		{
			var collection = new ObservableRangeCollection<int>(new[] { 1 });
			var toAdd = new int[0];
			var eventRaised = false;

			collection.CollectionChanged += (s, e) =>
			{
				eventRaised = true;
			};

			collection.ReplaceRange(toAdd);
			Assert.True(eventRaised, "Collection Reset should be raised.");
		}

		[Test]
		public void ReplaceRange_on_empty_collection_should_NOT_raise_collection_changes_when_empty()
		{
			var collection = new ObservableRangeCollection<int>();
			var toAdd = new int[0];

			collection.CollectionChanged += (s, e) =>
			{
				throw new AssertionException("Collection changes should NOT be raised.");
			};

			collection.ReplaceRange(toAdd);
		}

		[Test]
		public void ReplaceRange_should_NOT_mutate_source()
		{
			var sourceData = new List<int>(new[] { 1, 2, 3 });
			var collection = new ObservableRangeCollection<int>(new[] { 1, 2, 3, 4, 5, 6 });

			collection.ReplaceRange(sourceData);

			Assert.AreEqual(3, sourceData.Count);
		}

		[Test]
		public void RemoveRangeRemoveFact()
		{
			var collection = new ObservableRangeCollection<int>();
			var toAdd = new[] { 3, 1, 4, 1, 5, 9, 2, 6, 5, 3, 5, 8, 9, 7, 9, 3, 2, 3 };
			var toRemove = new[] { 1, 1, 2, 2, 3, 3, 4, 4, 5, 5, 6, 6, 7, 7, 8, 8, 9, 9, 0, 0 };
			collection.AddRange(toAdd);
			collection.CollectionChanged += (s, e) =>
			{
				if (e.Action != NotifyCollectionChangedAction.Remove)
					throw new AssertionException("RemoveRange didn't use Remove like requested.");
				if (e.OldItems == null)
					throw new AssertionException("OldItems should not be null.");
				var expected = new int[] { 1, 1, 2, 2, 3, 3, 4, 5, 5, 6, 7, 8, 9, 9 };
				if (expected.Length != e.OldItems.Count)
					throw new AssertionException("Expected and actual OldItems don't match.");
				for (var i = 0; i < expected.Length; i++)
				{
					if (expected[i] != (int)e.OldItems[i])
						throw new AssertionException("Expected and actual OldItems don't match.");
				}
			};
			collection.RemoveRange(toRemove, NotifyCollectionChangedAction.Remove);

		}

		[Test]
		public void RemoveRangeEmpty()
		{
			var collection = new ObservableRangeCollection<int>();
			var toAdd = new[] { 3, 1, 4, 1, 5, 9, 2, 6, 5, 3, 5, 8, 9, 7, 9, 3, 2, 3 };
			var toRemove = new int[0];
			collection.AddRange(toAdd);
			collection.CollectionChanged += (s, e) =>
			{
				throw new AssertionException("The event is raised.");
			};
			collection.RemoveRange(toRemove, NotifyCollectionChangedAction.Remove);
		}

		[Test]
		public void RemoveRange_should_NOT_mutate_source_when_source_data_is_not_present()
		{
			var sourceData = new List<int>(new[] { 1, 2, 3 });
			var collection = new ObservableRangeCollection<int>(new[] { 4, 5, 6 });

			collection.RemoveRange(sourceData, NotifyCollectionChangedAction.Remove);

			Assert.AreEqual(3, sourceData.Count);
		}

		[Test]
		public void RemoveRange_should_NOT_mutate_source_when_source_data_is_present()
		{
			var sourceData = new List<int>(new[] { 1, 2, 3 });
			var collection = new ObservableRangeCollection<int>(new[] { 1, 2, 3, 4, 5, 6 });

			collection.RemoveRange(sourceData, NotifyCollectionChangedAction.Remove);

			Assert.AreEqual(3, sourceData.Count);
		}

		[Test]
		public void RemoveRange_should_NOT_mutate_collection_when_source_data_is_not_present()
		{
			var sourceData = new List<int>(new[] { 1, 2, 3 });
			var collection = new ObservableRangeCollection<int>(new[] { 4, 5, 6, 7, 8, 9 });

			collection.RemoveRange(sourceData, NotifyCollectionChangedAction.Remove);

			// the collection should not be modified if the source items are not found
			Assert.AreEqual(6, collection.Count);
		}
	}
}
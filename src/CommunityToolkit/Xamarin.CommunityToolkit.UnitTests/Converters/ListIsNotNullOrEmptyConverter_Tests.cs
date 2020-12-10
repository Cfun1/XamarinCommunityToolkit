﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Xamarin.CommunityToolkit.Converters;
using NUnit.Framework;

namespace Xamarin.CommunityToolkit.UnitTests.Converters
{
	public class ListIsNotNullOrEmptyConverter_Tests
	{
		public static IEnumerable<object[]> GetData()
		{
			return new List<object[]>
			{
				new object[] { new List<string>(), false},
				new object[] { new List<string>() { "TestValue"}, true},
				new object[] { null, false},
				new object[] { Enumerable.Range(1, 3), true},
			};
		}

		[Theory]
		[TestCaseSource(nameof(GetData))]
		public void ListIsNotNullOrEmptyConverter(object value, bool expectedResult)
		{
			var listIsNotNullOrEmptyConverter = new ListIsNotNullOrEmptyConverter();

			var result = listIsNotNullOrEmptyConverter.Convert(value, typeof(ListIsNotNullOrEmptyConverter), null, CultureInfo.CurrentCulture);

			Assert.AreEqual(result, expectedResult);
		}

		[Theory]
		[TestCase(0)]
		public void InValidConverterValuesThrowArgumenException(object value)
		{
			var listIsNotNullOrEmptyConverter = new ListIsNotNullOrEmptyConverter();

			Assert.Throws<ArgumentException>(() => listIsNotNullOrEmptyConverter.Convert(value, typeof(ListIsNotNullOrEmptyConverter), null, CultureInfo.CurrentCulture));
		}
	}
}